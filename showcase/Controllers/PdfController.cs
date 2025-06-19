using DocToPdf.Services;
using Microsoft.AspNetCore.Mvc;

namespace showcase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IDocumentToPdfService _pdfService;
    private readonly ILogger<PdfController> _logger;

    public PdfController(IDocumentToPdfService pdfService, ILogger<PdfController> logger)
    {
        _pdfService = pdfService;
        _logger = logger;
    }

    [HttpPost("convert/html")]
    public async Task<IActionResult> ConvertHtml([FromBody] ConvertHtmlRequest request)
    {
        try
        {
            _logger.LogInformation("Converting HTML to PDF: {Title}", request.Title);
              var pdfBytes = await _pdfService.ConvertHtmlToPdfAsync(
                request.HtmlContent, 
                request.Title ?? "Generated PDF",
                null // basePath
            );

            var fileName = $"{request.Title?.Replace(" ", "_") ?? "document"}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert HTML to PDF");
            return BadRequest($"Conversion failed: {ex.Message}");
        }
    }

    [HttpPost("convert/markdown")]
    public async Task<IActionResult> ConvertMarkdown([FromBody] ConvertMarkdownRequest request)
    {
        try
        {
            _logger.LogInformation("Converting Markdown to PDF: {Title}", request.Title);
              var pdfBytes = await _pdfService.ConvertMarkdownToPdfAsync(
                request.MarkdownContent, 
                request.Title ?? "Generated PDF",
                null // basePath
            );

            var fileName = $"{request.Title?.Replace(" ", "_") ?? "document"}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert Markdown to PDF");
            return BadRequest($"Conversion failed: {ex.Message}");
        }
    }

    [HttpPost("convert/text")]
    public async Task<IActionResult> ConvertText([FromBody] ConvertTextRequest request)
    {
        try
        {
            _logger.LogInformation("Converting Text to PDF: {Title}", request.Title);
            
            var pdfBytes = await _pdfService.ConvertTextToPdfAsync(
                request.TextContent, 
                request.Title ?? "Generated PDF"
            );

            var fileName = $"{request.Title?.Replace(" ", "_") ?? "document"}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert Text to PDF");
            return BadRequest($"Conversion failed: {ex.Message}");
        }
    }

    [HttpGet("examples")]
    public IActionResult GetExamples()
    {
        return Ok(new
        {
            Markdown = new
            {
                Title = "Markdown Example",
                Content = @"# My Document

This is a **bold** statement and this is *italic*.

## Features

- ✅ Bullet points
- ✅ **Bold** and *italic* text
- ✅ Links: [GitHub](https://github.com)

### Code Block

```csharp
public class Example
{
    public string Name { get; set; } = ""Hello World"";
}
```

### Mermaid Diagram

```mermaid
graph TD
    A[Start] --> B{Decision}
    B -->|Yes| C[Action 1]
    B -->|No| D[Action 2]
    C --> E[End]
    D --> E
```

### Table

| Feature | Status |
|---------|--------|
| HTML | ✅ |
| Markdown | ✅ |
| Mermaid | ✅ |"
            },
            Html = new
            {
                Title = "HTML Example",
                Content = @"<!DOCTYPE html>
<html>
<head>
    <title>HTML Example</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        .header { color: #2c3e50; border-bottom: 2px solid #3498db; }
        .content { margin: 20px 0; }
        .highlight { background-color: #f39c12; color: white; padding: 5px; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
        th { background-color: #f2f2f2; }
    </style>
</head>
<body>
    <h1 class=""header"">Professional HTML Document</h1>
    
    <div class=""content"">
        <h2>Introduction</h2>
        <p>This is a <span class=""highlight"">professionally styled</span> HTML document that demonstrates various formatting options.</p>
        
        <h3>Features</h3>
        <ul>
            <li>Custom CSS styling</li>
            <li>Professional layout</li>
            <li>Tables and lists</li>
            <li>Rich formatting</li>
        </ul>
        
        <h3>Data Table</h3>
        <table>
            <thead>
                <tr>
                    <th>Feature</th>
                    <th>Description</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>HTML Conversion</td>
                    <td>Convert styled HTML to PDF</td>
                    <td>✅ Complete</td>
                </tr>
                <tr>
                    <td>CSS Support</td>
                    <td>Full CSS styling support</td>
                    <td>✅ Complete</td>
                </tr>
                <tr>
                    <td>Responsive Design</td>
                    <td>PDF-optimized layouts</td>
                    <td>✅ Complete</td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
</html>"
            },
            Text = new
            {
                Title = "Plain Text Example",
                Content = @"PLAIN TEXT DOCUMENT EXAMPLE

This is a simple plain text document that will be converted to PDF.

Key Features:
- Simple formatting
- No HTML tags
- Clean and readable
- Professional appearance

Section 1: Introduction
=======================

Plain text conversion is useful for:
* Log files
* Configuration files  
* Simple documentation
* Reports and summaries

Section 2: Technical Details
============================

The DocToPdf library automatically:
1. Formats plain text with proper fonts
2. Handles line breaks correctly
3. Maintains text structure
4. Creates professional-looking PDFs

Section 3: Conclusion
=====================

This demonstrates how easy it is to convert plain text to PDF using the DocToPdf library with dependency injection in ASP.NET Core."
            }
        });
    }
}

public record ConvertHtmlRequest(string HtmlContent, string? Title = null);
public record ConvertMarkdownRequest(string MarkdownContent, string? Title = null);
public record ConvertTextRequest(string TextContent, string? Title = null);
