# DocToPdf Library Usage Examples

## 🚀 ASP.NET Core Integration

### Startup Configuration

```csharp
using DocToPdf.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add DocToPdf services to DI container
builder.Services.AddDocToPdf();

// Or with custom configuration
builder.Services.AddDocToPdf(options =>
{
    options.DefaultOutputDirectory = "pdfs";
    options.EnableMermaidDiagrams = true;
    options.DefaultPageSize = "A4";
    options.DefaultMargin = 40;
});

var app = builder.Build();
```

### Controller Usage

```csharp
using DocToPdf.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IDocumentToPdfService _pdfService;

    public PdfController(IDocumentToPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpPost("html-to-pdf")]
    public async Task<IActionResult> ConvertHtmlToPdf([FromBody] HtmlToPdfRequest request)
    {
        try
        {
            var pdfBytes = await _pdfService.ConvertHtmlToPdfAsync(
                request.HtmlContent, 
                request.Title ?? "Generated PDF"
            );

            return File(pdfBytes, "application/pdf", $"{request.Title ?? "document"}.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error converting to PDF: {ex.Message}");
        }
    }

    [HttpPost("markdown-to-pdf")]
    public async Task<IActionResult> ConvertMarkdownToPdf([FromBody] MarkdownToPdfRequest request)
    {
        try
        {
            var pdfBytes = await _pdfService.ConvertMarkdownToPdfAsync(
                request.MarkdownContent, 
                request.Title ?? "Generated PDF"
            );

            return File(pdfBytes, "application/pdf", $"{request.Title ?? "document"}.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error converting to PDF: {ex.Message}");
        }
    }

    [HttpPost("text-to-pdf")]
    public async Task<IActionResult> ConvertTextToPdf([FromBody] TextToPdfRequest request)
    {
        try
        {
            var pdfBytes = await _pdfService.ConvertTextToPdfAsync(
                request.TextContent, 
                request.Title ?? "Generated PDF"
            );

            return File(pdfBytes, "application/pdf", $"{request.Title ?? "document"}.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error converting to PDF: {ex.Message}");
        }
    }
}

public record HtmlToPdfRequest(string HtmlContent, string? Title = null);
public record MarkdownToPdfRequest(string MarkdownContent, string? Title = null);
public record TextToPdfRequest(string TextContent, string? Title = null);
```

## 🔧 Dependency Injection in Console Apps

```csharp
using DocToPdf.Extensions;
using DocToPdf.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDocToPdf();
        services.AddLogging();
    })
    .Build();

var pdfService = host.Services.GetRequiredService<IDocumentToPdfService>();

// Convert HTML to PDF
var htmlContent = "<h1>Hello World</h1><p>This is a test document.</p>";
var pdfBytes = await pdfService.ConvertHtmlToPdfAsync(htmlContent, "Test Document");
await File.WriteAllBytesAsync("test.pdf", pdfBytes);

// Convert Markdown to PDF
var markdownContent = @"
# My Document
This is **bold** text and *italic* text.

## Features
- Markdown support
- HTML rendering
- Professional PDF output
";
await pdfService.ConvertMarkdownToPdfAsync(markdownContent, "markdown-output.pdf", "My Markdown Document");
```

## 🎯 Blazor Server/WASM Integration

```csharp
// Program.cs (Blazor Server)
using DocToPdf.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDocToPdf();

var app = builder.Build();
```

```razor
@* PdfGenerator.razor *@
@page "/pdf-generator"
@inject IDocumentToPdfService PdfService
@inject IJSRuntime JSRuntime

<h3>PDF Generator</h3>

<div class="mb-3">
    <label class="form-label">Content Type:</label>
    <select class="form-select" @bind="contentType">
        <option value="html">HTML</option>
        <option value="markdown">Markdown</option>
        <option value="text">Plain Text</option>
    </select>
</div>

<div class="mb-3">
    <label class="form-label">Document Title:</label>
    <input class="form-control" @bind="documentTitle" placeholder="Enter document title" />
</div>

<div class="mb-3">
    <label class="form-label">Content:</label>
    <textarea class="form-control" rows="10" @bind="content" placeholder="Enter your content here..."></textarea>
</div>

<button class="btn btn-primary" @onclick="GeneratePdf" disabled="@isGenerating">
    @if (isGenerating)
    {
        <span class="spinner-border spinner-border-sm me-2"></span>
    }
    Generate PDF
</button>

@code {
    private string contentType = "html";
    private string documentTitle = "Generated Document";
    private string content = "";
    private bool isGenerating = false;

    private async Task GeneratePdf()
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            await JSRuntime.InvokeVoidAsync("alert", "Please enter some content");
            return;
        }

        isGenerating = true;
        StateHasChanged();

        try
        {
            byte[] pdfBytes = contentType switch
            {
                "html" => await PdfService.ConvertHtmlToPdfAsync(content, documentTitle),
                "markdown" => await PdfService.ConvertMarkdownToPdfAsync(content, documentTitle),
                "text" => await PdfService.ConvertTextToPdfAsync(content, documentTitle),
                _ => throw new InvalidOperationException("Unknown content type")
            };

            var fileName = $"{documentTitle.Replace(" ", "_")}.pdf";
            var base64 = Convert.ToBase64String(pdfBytes);
            var dataUrl = $"data:application/pdf;base64,{base64}";

            await JSRuntime.InvokeVoidAsync("downloadFile", dataUrl, fileName);
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Error generating PDF: {ex.Message}");
        }
        finally
        {
            isGenerating = false;
            StateHasChanged();
        }
    }
}
```

## 🔄 Background Service Example

```csharp
using DocToPdf.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class PdfGenerationBackgroundService : BackgroundService
{
    private readonly IDocumentToPdfService _pdfService;
    private readonly ILogger<PdfGenerationBackgroundService> _logger;

    public PdfGenerationBackgroundService(
        IDocumentToPdfService pdfService,
        ILogger<PdfGenerationBackgroundService> logger)
    {
        _pdfService = pdfService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Process documents from a queue, database, or file system
                await ProcessPendingDocuments(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PDF generation background service");
            }
        }
    }

    private async Task ProcessPendingDocuments(CancellationToken cancellationToken)
    {
        // Example: Process markdown files from input directory
        var inputDir = "input";
        var outputDir = "output";

        if (!Directory.Exists(inputDir)) return;

        var markdownFiles = Directory.GetFiles(inputDir, "*.md");
        
        foreach (var file in markdownFiles)
        {
            try
            {
                var content = await File.ReadAllTextAsync(file, cancellationToken);
                var fileName = Path.GetFileNameWithoutExtension(file);
                var outputPath = Path.Combine(outputDir, $"{fileName}.pdf");

                await _pdfService.ConvertMarkdownToPdfAsync(content, outputPath, fileName);
                
                _logger.LogInformation("Generated PDF: {OutputPath}", outputPath);
                
                // Optionally move processed file
                File.Move(file, Path.Combine("processed", Path.GetFileName(file)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process file: {File}", file);
            }
        }
    }
}
```

## 📦 NuGet Package Installation

```bash
# Install from NuGet (when published)
dotnet add package DocToPdf

# Or install local package
dotnet pack
dotnet add package DocToPdf --source ./bin/Debug/
```

## 🎛️ Advanced Configuration

```csharp
builder.Services.AddDocToPdf(options =>
{
    options.DefaultOutputDirectory = "generated-pdfs";
    options.DefaultInputDirectory = "documents";
    options.EnableMermaidDiagrams = true;
    options.MaxImageSizeBytes = 20 * 1024 * 1024; // 20MB
    options.DefaultPageSize = "Letter";
    options.DefaultMargin = 60;
});

// Custom logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
});
```

## ⚡ Performance Tips

1. **Reuse Service Instance**: The service is registered as Scoped, perfect for request-scoped usage
2. **Async Operations**: All methods are async for better performance
3. **Memory Management**: Byte arrays are returned for in-memory processing
4. **Logging**: Built-in logging for monitoring and debugging
5. **Error Handling**: Comprehensive exception handling with meaningful messages
