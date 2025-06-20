using DocToPdf.Core.Services;
using DocToPdf.Customization.Services;
using DocToPdf.Customization.Models;
using Microsoft.AspNetCore.Mvc;

namespace showcase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IDocumentToPdfService _pdfService;
    private readonly IPdfCustomizationService _customizationService;
    private readonly ILogger<PdfController> _logger;

    public PdfController(
        IDocumentToPdfService pdfService,
        IPdfCustomizationService customizationService,
        ILogger<PdfController> logger)
    {
        _pdfService = pdfService;
        _customizationService = customizationService;
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

    [HttpPost("convert/markdown-with-watermark")]
    public async Task<IActionResult> ConvertMarkdownWithWatermark([FromBody] ConvertMarkdownWithWatermarkRequest request)
    {
        try
        {
            _logger.LogInformation("Converting Markdown to PDF with watermark: {WatermarkText}", request.WatermarkText);            // First convert to PDF
            var pdfBytes = await _pdfService.ConvertMarkdownToPdfAsync(
                request.MarkdownContent, 
                request.Title ?? "Generated PDF",
                null  // basePath
            );
            
            // Apply watermark
            var watermarkOptions = new WatermarkOptions
            {
                Text = request.WatermarkText ?? "DRAFT",
                Opacity = request.Opacity ?? 0.3f,
                Position = request.Position ?? WatermarkPosition.Center,
                Rotation = request.Rotation ?? 45f,
                FontSize = request.FontSize ?? 48f,
                Color = request.Color ?? "#CCCCCC"
            };
            
            var customizedPdf = await _customizationService.AddWatermarkAsync(pdfBytes, watermarkOptions);
            
            var fileName = $"{request.Title?.Replace(" ", "_") ?? "document"}_watermarked.pdf";
            return File(customizedPdf, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting Markdown to PDF with watermark");
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpPost("convert/html-with-customization")]
    public async Task<IActionResult> ConvertHtmlWithCustomization([FromBody] ConvertHtmlWithCustomizationRequest request)
    {
        try
        {
            _logger.LogInformation("Converting HTML to PDF with full customization");            // First convert to PDF
            var pdfBytes = await _pdfService.ConvertHtmlToPdfAsync(
                request.HtmlContent, 
                request.Title ?? "Generated PDF",
                null  // basePath
            );
            
            // Build customization options
            var customizationOptions = new PdfCustomizationOptions();
            
            if (!string.IsNullOrWhiteSpace(request.WatermarkText))
            {
                customizationOptions.Watermark = new WatermarkOptions
                {
                    Text = request.WatermarkText,
                    Opacity = request.WatermarkOpacity ?? 0.3f,
                    Position = request.WatermarkPosition ?? WatermarkPosition.Center,
                    Rotation = 45f
                };
            }
            
            if (!string.IsNullOrWhiteSpace(request.HeaderTemplate))
            {
                customizationOptions.Header = new HeaderFooterOptions
                {
                    Template = request.HeaderTemplate,
                    Alignment = HorizontalAlignment.Center,
                    FontSize = 10f
                };
            }
            
            if (!string.IsNullOrWhiteSpace(request.FooterTemplate))
            {
                customizationOptions.Footer = new HeaderFooterOptions
                {
                    Template = request.FooterTemplate,
                    Alignment = HorizontalAlignment.Center,
                    FontSize = 10f
                };
            }
            
            if (request.Metadata != null)
            {
                customizationOptions.Metadata = new PdfMetadata
                {
                    Title = request.Metadata.Title,
                    Author = request.Metadata.Author,
                    Subject = request.Metadata.Subject,
                    Keywords = request.Metadata.Keywords,
                    CreationDate = DateTime.UtcNow
                };
            }
            
            // Apply all customizations
            var customizedPdf = await _customizationService.ApplyCustomizationAsync(pdfBytes, customizationOptions);
            
            var fileName = $"{request.Title?.Replace(" ", "_") ?? "document"}_customized.pdf";
            return File(customizedPdf, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting HTML to PDF with customization");
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpPost("add-watermark")]
    public async Task<IActionResult> AddWatermarkToExistingPdf(IFormFile pdfFile, [FromForm] AddWatermarkRequest request)
    {
        try
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return BadRequest("No PDF file provided");

            _logger.LogInformation("Adding watermark to existing PDF: {FileName}", pdfFile.FileName);
            
            // Read the uploaded PDF
            using var memoryStream = new MemoryStream();
            await pdfFile.CopyToAsync(memoryStream);
            var pdfBytes = memoryStream.ToArray();
            
            // Apply watermark
            var watermarkOptions = new WatermarkOptions
            {
                Text = request.WatermarkText ?? "CONFIDENTIAL",
                Opacity = request.Opacity ?? 0.3f,
                Position = request.Position ?? WatermarkPosition.Center,
                Rotation = request.Rotation ?? 45f,
                FontSize = request.FontSize ?? 48f,
                Color = request.Color ?? "#CCCCCC"
            };
            
            var watermarkedPdf = await _customizationService.AddWatermarkAsync(pdfBytes, watermarkOptions);
            
            var fileName = Path.GetFileNameWithoutExtension(pdfFile.FileName) + "_watermarked.pdf";
            return File(watermarkedPdf, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding watermark to PDF");
            return BadRequest($"Error: {ex.Message}");
        }
    }
}

public record ConvertHtmlRequest(string HtmlContent, string? Title = null);
public record ConvertMarkdownRequest(string MarkdownContent, string? Title = null);
public record ConvertTextRequest(string TextContent, string? Title = null);
public record ConvertMarkdownWithWatermarkRequest(
    string MarkdownContent, 
    string? Title = null,
    string? WatermarkText = null,
    float? Opacity = null,
    WatermarkPosition? Position = null,
    float? Rotation = null,
    float? FontSize = null,
    string? Color = null);
public record ConvertHtmlWithCustomizationRequest(
    string HtmlContent,
    string? Title = null,
    string? WatermarkText = null,
    float? WatermarkOpacity = null,
    WatermarkPosition? WatermarkPosition = null,
    string? HeaderTemplate = null,
    string? FooterTemplate = null,
    PdfMetadataRequest? Metadata = null);
public record PdfMetadataRequest(
    string? Title = null,
    string? Author = null,
    string? Subject = null,
    string? Keywords = null);
public record AddWatermarkRequest(
    string? WatermarkText = null,
    float? Opacity = null,
    WatermarkPosition? Position = null,
    float? Rotation = null,
    float? FontSize = null,
    string? Color = null);
