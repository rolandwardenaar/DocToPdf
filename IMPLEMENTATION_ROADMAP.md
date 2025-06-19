# 🚀 Implementation Roadmap - Step-by-Step Guide

## 📋 Quick Start: Begin met Feature 1 (DocToPdf.Customization)

Deze roadmap toont de **exacte stappen** om te beginnen met de eerste feature package. Gebruik dit als template voor alle andere features.

---

## 🎯 Phase 1: DocToPdf.Customization Implementation

### Step 1: Project Setup (5 minutes)

```powershell
# Navigate to workspace root
cd d:\source\temp\DocToPfd

# Create the customization package directory
mkdir DocToPdf.Customization
cd DocToPdf.Customization

# Initialize project
dotnet new classlib -f net8.0 --name DocToPdf.Customization

# Add package dependencies
dotnet add package DocToPdf --version 1.1.0
dotnet add package SkiaSharp --version 3.119.0
dotnet add package System.Drawing.Common --version 8.0.0
dotnet add package Microsoft.Extensions.DependencyInjection.Abstractions --version 8.0.0
dotnet add package Microsoft.Extensions.Logging.Abstractions --version 8.0.0
dotnet add package System.ComponentModel.Annotations --version 5.0.0

# Create directory structure
mkdir Models, Services, Extensions, Themes, Utils
```

### Step 2: Core Models (15 minutes)

Create these files in exact order:

#### 1. `Models/WatermarkOptions.cs`
```csharp
using System.ComponentModel.DataAnnotations;

namespace DocToPdf.Customization.Models;

public class WatermarkOptions
{
    [Required]
    public string Text { get; set; } = string.Empty;
    
    public string? ImagePath { get; set; }
    
    [Range(0.1, 1.0)]
    public float Opacity { get; set; } = 0.3f;
    
    public WatermarkPosition Position { get; set; } = WatermarkPosition.Center;
    
    [Range(-180, 180)]
    public float Rotation { get; set; } = 45f;
    
    public string FontName { get; set; } = "Arial";
    public float FontSize { get; set; } = 48f;
    public string Color { get; set; } = "#CCCCCC";
}

public enum WatermarkPosition
{
    TopLeft,
    TopCenter,
    TopRight,
    MiddleLeft,
    Center,
    MiddleRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}
```

#### 2. `Models/HeaderFooterOptions.cs`
```csharp
using System.ComponentModel.DataAnnotations;

namespace DocToPdf.Customization.Models;

public class HeaderFooterOptions
{
    [Required]
    public string Template { get; set; } = string.Empty; // "Page {page} of {totalPages}"
    
    public string? FontName { get; set; }
    public float FontSize { get; set; } = 10f;
    public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;
    public string Color { get; set; } = "#000000";
    public float MarginTop { get; set; } = 20f;
    public float MarginBottom { get; set; } = 20f;
}

public enum HorizontalAlignment
{
    Left,
    Center,
    Right
}
```

#### 3. `Models/PdfTheme.cs`
```csharp
namespace DocToPdf.Customization.Models;

public enum PdfTheme
{
    Default,
    Corporate,
    Modern,
    Minimal,
    Academic,
    Creative
}

public class ThemeDefinition
{
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#000000";
    public string SecondaryColor { get; set; } = "#666666";
    public string AccentColor { get; set; } = "#0066CC";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string FontFamily { get; set; } = "Arial";
    public float DefaultFontSize { get; set; } = 12f;
    public float HeaderFontSize { get; set; } = 16f;
    public float LineSpacing { get; set; } = 1.2f;
    public float Margins { get; set; } = 25f;
}
```

#### 4. `Models/PdfMetadata.cs`
```csharp
namespace DocToPdf.Customization.Models;

public class PdfMetadata
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Subject { get; set; }
    public string? Keywords { get; set; }
    public string? Creator { get; set; } = "DocToPdf.Customization";
    public DateTime? CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }
}
```

#### 5. `Models/PdfCustomizationOptions.cs`
```csharp
namespace DocToPdf.Customization.Models;

public class PdfCustomizationOptions
{
    public WatermarkOptions? Watermark { get; set; }
    public HeaderFooterOptions? Header { get; set; }
    public HeaderFooterOptions? Footer { get; set; }
    public PdfTheme Theme { get; set; } = PdfTheme.Default;
    public PdfMetadata? Metadata { get; set; }
    public FontOptions? Fonts { get; set; }
}

public class FontOptions
{
    public string DefaultFont { get; set; } = "Arial";
    public Dictionary<string, string> FontMappings { get; set; } = new();
    public List<string> EmbeddedFonts { get; set; } = new();
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}
```

### Step 3: Service Interface (5 minutes)

#### `Services/IPdfCustomizationService.cs`
```csharp
using DocToPdf.Customization.Models;

namespace DocToPdf.Customization.Services;

public interface IPdfCustomizationService
{
    /// <summary>
    /// Apply comprehensive customization to a PDF
    /// </summary>
    Task<byte[]> ApplyCustomizationAsync(byte[] pdfBytes, PdfCustomizationOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Add watermark to PDF
    /// </summary>
    Task<byte[]> AddWatermarkAsync(byte[] pdfBytes, WatermarkOptions watermark, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Add headers and footers to PDF
    /// </summary>
    Task<byte[]> AddHeaderFooterAsync(byte[] pdfBytes, HeaderFooterOptions? header, HeaderFooterOptions? footer, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Apply theme to PDF
    /// </summary>
    Task<byte[]> ApplyThemeAsync(byte[] pdfBytes, PdfTheme theme, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Set PDF metadata
    /// </summary>
    Task<byte[]> SetMetadataAsync(byte[] pdfBytes, PdfMetadata metadata, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Validate customization options
    /// </summary>
    Task<ValidationResult> ValidateOptionsAsync(PdfCustomizationOptions options);
}
```

### Step 4: Basic Service Implementation (20 minutes)

#### `Services/PdfCustomizationService.cs`
```csharp
using Microsoft.Extensions.Logging;
using DocToPdf.Customization.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace DocToPdf.Customization.Services;

public class PdfCustomizationService : IPdfCustomizationService
{
    private readonly ILogger<PdfCustomizationService> _logger;

    public PdfCustomizationService(ILogger<PdfCustomizationService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> ApplyCustomizationAsync(byte[] pdfBytes, PdfCustomizationOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Applying PDF customization");
        
        // Validation
        var validation = await ValidateOptionsAsync(options);
        if (!validation.IsValid)
        {
            throw new ArgumentException($"Invalid options: {string.Join(", ", validation.Errors)}");
        }

        var result = pdfBytes;

        try
        {
            // Apply customizations in sequence
            if (options.Metadata != null)
            {
                result = await SetMetadataAsync(result, options.Metadata, cancellationToken);
            }

            if (options.Header != null || options.Footer != null)
            {
                result = await AddHeaderFooterAsync(result, options.Header, options.Footer, cancellationToken);
            }

            if (options.Watermark != null)
            {
                result = await AddWatermarkAsync(result, options.Watermark, cancellationToken);
            }

            _logger.LogInformation("PDF customization completed successfully");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying PDF customization");
            throw;
        }
    }

    public async Task<byte[]> AddWatermarkAsync(byte[] pdfBytes, WatermarkOptions watermark, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding watermark to PDF");
        
        // For now, return original PDF (implement watermark logic using QuestPDF or SkiaSharp)
        // This is a placeholder - real implementation would overlay watermark
        
        await Task.Delay(100, cancellationToken); // Simulate processing
        return pdfBytes;
    }

    public async Task<byte[]> AddHeaderFooterAsync(byte[] pdfBytes, HeaderFooterOptions? header, HeaderFooterOptions? footer, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding header/footer to PDF");
        
        // Placeholder implementation
        await Task.Delay(100, cancellationToken);
        return pdfBytes;
    }

    public async Task<byte[]> ApplyThemeAsync(byte[] pdfBytes, PdfTheme theme, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Applying theme {Theme} to PDF", theme);
        
        // Placeholder implementation
        await Task.Delay(100, cancellationToken);
        return pdfBytes;
    }

    public async Task<byte[]> SetMetadataAsync(byte[] pdfBytes, PdfMetadata metadata, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Setting PDF metadata");
        
        // Placeholder implementation - would use PdfSharp or similar to set metadata
        await Task.Delay(50, cancellationToken);
        return pdfBytes;
    }

    public async Task<ValidationResult> ValidateOptionsAsync(PdfCustomizationOptions options)
    {
        var result = new ValidationResult { IsValid = true };
        
        // Validate watermark
        if (options.Watermark != null)
        {
            if (string.IsNullOrWhiteSpace(options.Watermark.Text) && string.IsNullOrWhiteSpace(options.Watermark.ImagePath))
            {
                result.Errors.Add("Watermark must have either text or image path");
                result.IsValid = false;
            }
            
            if (options.Watermark.Opacity is < 0.1f or > 1.0f)
            {
                result.Errors.Add("Watermark opacity must be between 0.1 and 1.0");
                result.IsValid = false;
            }
        }
        
        // Validate header/footer templates
        if (options.Header != null && string.IsNullOrWhiteSpace(options.Header.Template))
        {
            result.Errors.Add("Header template cannot be empty");
            result.IsValid = false;
        }
        
        if (options.Footer != null && string.IsNullOrWhiteSpace(options.Footer.Template))
        {
            result.Errors.Add("Footer template cannot be empty");
            result.IsValid = false;
        }

        await Task.CompletedTask;
        return result;
    }
}
```

### Step 5: Dependency Injection Extension (5 minutes)

#### `Extensions/ServiceCollectionExtensions.cs`
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DocToPdf.Customization.Models;
using DocToPdf.Customization.Services;

namespace DocToPdf.Customization.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add DocToPdf customization services to the DI container
    /// </summary>
    public static IServiceCollection AddDocToPdfCustomization(
        this IServiceCollection services,
        Action<PdfCustomizationOptions>? configure = null)
    {
        // Register services
        services.TryAddScoped<IPdfCustomizationService, PdfCustomizationService>();
        
        // Configure options if provided
        if (configure != null)
        {
            services.Configure(configure);
        }
        
        return services;
    }
    
    /// <summary>
    /// Add DocToPdf customization services with explicit options
    /// </summary>
    public static IServiceCollection AddDocToPdfCustomization(
        this IServiceCollection services,
        PdfCustomizationOptions options)
    {
        return services.AddDocToPdfCustomization(opt =>
        {
            opt.Watermark = options.Watermark;
            opt.Header = options.Header;
            opt.Footer = options.Footer;
            opt.Theme = options.Theme;
            opt.Metadata = options.Metadata;
            opt.Fonts = options.Fonts;
        });
    }
}
```

### Step 6: Update Project File (2 minutes)

Update `DocToPdf.Customization.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    
    <!-- NuGet Package Info -->
    <PackageId>DocToPdf.Customization</PackageId>
    <Version>1.0.0</Version>
    <Authors>DocToPdf Contributors</Authors>
    <Description>Advanced PDF customization features for DocToPdf - watermarks, themes, headers/footers, and metadata</Description>
    <PackageTags>pdf;customization;watermark;theme;header;footer</PackageTags>
    <PackageProjectUrl>https://github.com/yourusername/DocToPdf</PackageProjectUrl>
    <RepositoryUrl>https://github.com/yourusername/DocToPdf</RepositoryUrl>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    
    <!-- Symbol packages -->
    <IncludeSymbols>true</IncludeSymbols>
    <SymbolPackageFormat>snupkg</SymbolPackageFormat>
    <EmbedUntrackedSources>true</EmbedUntrackedSources>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="DocToPdf" Version="1.1.0" />
    <PackageReference Include="SkiaSharp" Version="3.119.0" />
    <PackageReference Include="System.Drawing.Common" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="8.0.0" />
    <PackageReference Include="System.ComponentModel.Annotations" Version="5.0.0" />
  </ItemGroup>

</Project>
```

### Step 7: Build and Test (5 minutes)

```powershell
# Build the project
dotnet build

# Create NuGet package
dotnet pack --configuration Release --output ../nupkg

# Test in showcase project
cd ../showcase
dotnet add package DocToPdf.Customization --source ../nupkg
```

### Step 8: Update Showcase to Use Customization (10 minutes)

#### Update `showcase/Program.cs`:
```csharp
using DocToPdf.Extensions;
using DocToPdf.Customization.Extensions; // Add this

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DocToPdf services
builder.Services.AddDocToPdf();

// Add DocToPdf Customization services
builder.Services.AddDocToPdfCustomization(); // Add this

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

#### Update `showcase/Controllers/PdfController.cs`:
```csharp
using Microsoft.AspNetCore.Mvc;
using DocToPdf.Services;
using DocToPdf.Customization.Services; // Add this
using DocToPdf.Customization.Models;   // Add this

namespace showcase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IDocumentToPdfService _pdfService;
    private readonly IPdfCustomizationService _customizationService; // Add this

    public PdfController(IDocumentToPdfService pdfService, IPdfCustomizationService customizationService)
    {
        _pdfService = pdfService;
        _customizationService = customizationService; // Add this
    }

    // Existing endpoints...

    [HttpPost("convert-with-watermark")]
    public async Task<IActionResult> ConvertWithWatermark([FromBody] ConvertWithWatermarkRequest request)
    {
        try
        {
            // Convert to PDF first
            var pdfBytes = await _pdfService.ConvertMarkdownToPdfAsync(request.Content);
            
            // Apply watermark
            var watermarkOptions = new WatermarkOptions
            {
                Text = request.WatermarkText ?? "DRAFT",
                Opacity = request.Opacity ?? 0.3f,
                Position = WatermarkPosition.Center,
                Rotation = 45f
            };
            
            var customizedPdf = await _customizationService.AddWatermarkAsync(pdfBytes, watermarkOptions);
            
            return File(customizedPdf, "application/pdf", "document-with-watermark.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}

public class ConvertWithWatermarkRequest
{
    public string Content { get; set; } = string.Empty;
    public string? WatermarkText { get; set; }
    public float? Opacity { get; set; }
}
```

### Step 9: Test the Implementation (5 minutes)

```powershell
# Start the showcase
cd showcase
dotnet run

# Test the new endpoint
# Navigate to https://localhost:7071/swagger
# Try the new /api/pdf/convert-with-watermark endpoint
```

---

## 🎯 Next Steps: Feature 2, 3, 4

After completing Feature 1, repeat similar pattern for:

### Feature 2: DocToPdf.DataReports
**Focus**: Excel/JSON/CSV → PDF conversion  
**Key dependencies**: DocumentFormat.OpenXml, CsvHelper, Scriban  
**Estimated time**: 3-4 hours

### Feature 3: DocToPdf.BatchProcessing  
**Focus**: Parallel processing, progress tracking  
**Key dependencies**: System.Threading.Channels  
**Estimated time**: 2-3 hours

### Feature 4: DocToPdf.Security
**Focus**: Password protection, encryption  
**Key dependencies**: PdfSharp  
**Estimated time**: 3-4 hours

---

## 📦 Final Integration Example

Once all features are complete:

```csharp
// Complete enterprise setup
builder.Services
    .AddDocToPdf()
    .AddDocToPdfCustomization(options =>
    {
        options.Theme = PdfTheme.Corporate;
        options.Watermark = new WatermarkOptions { Text = "CONFIDENTIAL" };
    })
    .AddDocToPdfDataReports()
    .AddDocToPdfBatchProcessing()
    .AddDocToPdfSecurity();

// Usage example
public async Task<byte[]> CreateSecureReport(ReportData data)
{
    // 1. Convert data to PDF
    var pdfBytes = await _dataReportService.ConvertJsonToPdfAsync(data, reportOptions);
    
    // 2. Apply customization (theme, watermark)
    var customizedPdf = await _customizationService.ApplyCustomizationAsync(pdfBytes, customOptions);
    
    // 3. Add security (password, encryption)
    var securePdf = await _securityService.EncryptPdfAsync(customizedPdf, securityOptions);
    
    return securePdf;
}
```

Start met **DocToPdf.Customization** - dit is de meest visuele feature en geeft de beste demo! 🚀
