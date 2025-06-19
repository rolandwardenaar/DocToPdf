# 🔧 Technical Implementation Specifications

## 📋 Implementation Instructions voor Copilot

Deze specificaties bieden **exacte instructies** voor het implementeren van elke feature package. Volg deze stap-voor-stap voor consistente, professionele implementatie.

---

## 🎨 Feature 1: DocToPdf.Customization - Technical Spec

### 🏗️ Project Setup Commands

```powershell
# Create project structure
New-Item -ItemType Directory -Path "DocToPdf.Customization"
Set-Location "DocToPdf.Customization"

# Create project file
dotnet new classlib -f net8.0
dotnet add package DocToPdf --version 1.1.0
dotnet add package SkiaSharp --version 3.119.0
dotnet add package System.Drawing.Common --version 8.0.0
```

### 📁 Required File Structure
```
DocToPdf.Customization/
├── Models/
│   ├── PdfCustomizationOptions.cs
│   ├── WatermarkOptions.cs
│   ├── HeaderFooterOptions.cs
│   ├── PdfTheme.cs
│   └── PdfMetadata.cs
├── Services/
│   ├── IPdfCustomizationService.cs
│   └── PdfCustomizationService.cs
├── Extensions/
│   └── ServiceCollectionExtensions.cs
├── Themes/
│   ├── IThemeProvider.cs
│   ├── DefaultThemeProvider.cs
│   └── ThemeDefinitions.cs
└── Utils/
    ├── FontManager.cs
    └── WatermarkRenderer.cs
```

### 🔧 Core Implementation Files

#### 1. Models/PdfCustomizationOptions.cs
```csharp
using System.ComponentModel.DataAnnotations;

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

public enum PdfTheme
{
    Default,
    Corporate,
    Modern,
    Minimal,
    Academic,
    Creative
}

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

public class FontOptions
{
    public string DefaultFont { get; set; } = "Arial";
    public Dictionary<string, string> FontMappings { get; set; } = new();
    public List<string> EmbeddedFonts { get; set; } = new();
}
```

#### 2. Services/IPdfCustomizationService.cs
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

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}
```

#### 3. Implementation Strategy voor PdfCustomizationService.cs
```csharp
using Microsoft.Extensions.Logging;
using DocToPdf.Customization.Models;
using DocToPdf.Customization.Utils;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DocToPdf.Customization.Services;

public class PdfCustomizationService : IPdfCustomizationService
{
    private readonly ILogger<PdfCustomizationService> _logger;
    private readonly IThemeProvider _themeProvider;
    private readonly FontManager _fontManager;
    private readonly WatermarkRenderer _watermarkRenderer;

    public PdfCustomizationService(
        ILogger<PdfCustomizationService> logger,
        IThemeProvider themeProvider,
        FontManager fontManager,
        WatermarkRenderer watermarkRenderer)
    {
        _logger = logger;
        _themeProvider = themeProvider;
        _fontManager = fontManager;
        _watermarkRenderer = watermarkRenderer;
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
            // Apply in sequence: Theme -> Metadata -> Header/Footer -> Watermark
            if (options.Theme != PdfTheme.Default)
            {
                result = await ApplyThemeAsync(result, options.Theme, cancellationToken);
            }

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

    // Implement other methods using QuestPDF, SkiaSharp, etc.
}
```

### 📋 Package Configuration (DocToPdf.Customization.csproj)
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
    <PackageIcon>icon.png</PackageIcon>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    
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

  <ItemGroup>
    <None Include="README.md" Pack="true" PackagePath="\" />
    <None Include="icon.png" Pack="true" PackagePath="\" />
  </ItemGroup>

</Project>
```

---

## 📊 Feature 2: DocToPdf.DataReports - Technical Spec

### 🏗️ Project Setup Commands

```powershell
# Create project
New-Item -ItemType Directory -Path "DocToPdf.DataReports"
Set-Location "DocToPdf.DataReports"
dotnet new classlib -f net8.0

# Add dependencies
dotnet add package DocToPdf --version 1.1.0
dotnet add package DocumentFormat.OpenXml --version 3.3.0
dotnet add package CsvHelper --version 33.0.1
dotnet add package Scriban --version 5.10.0
dotnet add package ScottPlot --version 5.0.51
```

### 📁 Required File Structure
```
DocToPdf.DataReports/
├── Models/
│   ├── DataReportOptions.cs
│   ├── ExcelConversionOptions.cs
│   ├── JsonReportOptions.cs
│   └── ChartDefinition.cs
├── Services/
│   ├── IDataReportService.cs
│   └── DataReportService.cs
├── Converters/
│   ├── ExcelConverter.cs
│   ├── JsonConverter.cs
│   └── CsvConverter.cs
├── Templates/
│   ├── IReportTemplateEngine.cs
│   ├── ScribanTemplateEngine.cs
│   └── DefaultTemplates/
├── Charts/
│   ├── IChartGenerator.cs
│   └── ScottPlotChartGenerator.cs
└── Extensions/
    └── ServiceCollectionExtensions.cs
```

### 🔧 Core Models Implementation

#### Models/ExcelConversionOptions.cs
```csharp
namespace DocToPdf.DataReports.Models;

public class ExcelConversionOptions : DataReportOptions
{
    public string? WorksheetName { get; set; }
    public string? CellRange { get; set; } // "A1:G10"
    public bool IncludeFormulas { get; set; } = false;
    public bool IncludeCharts { get; set; } = true;
    public bool IncludeImages { get; set; } = true;
    public ExcelTableStyle TableStyle { get; set; } = ExcelTableStyle.Modern;
    public bool AutoFitColumns { get; set; } = true;
    public Dictionary<string, string>? ColumnMappings { get; set; }
}

public enum ExcelTableStyle
{
    Simple,
    Modern,
    Corporate,
    Minimal
}

public class DataReportOptions
{
    public string? TemplatePath { get; set; }
    public ReportStyle Style { get; set; } = ReportStyle.Default;
    public TableOptions? TableOptions { get; set; }
    public List<ChartDefinition>? Charts { get; set; }
    public PdfPageSettings? PageSettings { get; set; }
}

public class TableOptions
{
    public bool IncludeHeaders { get; set; } = true;
    public bool AlternateRowColors { get; set; } = true;
    public string HeaderColor { get; set; } = "#4472C4";
    public string AlternateRowColor { get; set; } = "#F2F2F2";
    public int FontSize { get; set; } = 10;
    public string FontFamily { get; set; } = "Arial";
}
```

#### Services/IDataReportService.cs  
```csharp
namespace DocToPdf.DataReports.Services;

public interface IDataReportService
{
    // Excel Conversion
    Task<byte[]> ConvertExcelToPdfAsync(string excelPath, ExcelConversionOptions? options = null, CancellationToken cancellationToken = default);
    Task<byte[]> ConvertExcelToPdfAsync(Stream excelStream, ExcelConversionOptions? options = null, CancellationToken cancellationToken = default);
    
    // JSON Conversion
    Task<byte[]> ConvertJsonToPdfAsync(string jsonData, JsonReportOptions options, CancellationToken cancellationToken = default);
    Task<byte[]> ConvertJsonToPdfAsync<T>(T data, JsonReportOptions options, CancellationToken cancellationToken = default) where T : class;
    
    // CSV Conversion
    Task<byte[]> ConvertCsvToPdfAsync(string csvPath, DataReportOptions? options = null, CancellationToken cancellationToken = default);
    Task<byte[]> ConvertCsvToPdfAsync(Stream csvStream, DataReportOptions? options = null, CancellationToken cancellationToken = default);
    
    // Template-based Reports
    Task<byte[]> GenerateReportFromTemplateAsync(string templatePath, object data, CancellationToken cancellationToken = default);
    
    // Validation
    Task<ValidationResult> ValidateExcelFileAsync(string path);
    Task<ValidationResult> ValidateTemplateAsync(string templatePath);
}
```

---

## ⚡ Feature 3: DocToPdf.BatchProcessing - Technical Spec

### 🏗️ Project Setup Commands

```powershell
New-Item -ItemType Directory -Path "DocToPdf.BatchProcessing"
Set-Location "DocToPdf.BatchProcessing"
dotnet new classlib -f net8.0
dotnet add package DocToPdf --version 1.1.0
dotnet add package System.Threading.Channels --version 8.0.0
```

### 📁 Required File Structure
```
DocToPdf.BatchProcessing/
├── Models/
│   ├── BatchConversionRequest.cs
│   ├── BatchConversionResult.cs
│   ├── BatchProgress.cs
│   └── BatchProcessingOptions.cs
├── Services/
│   ├── IBatchProcessingService.cs
│   └── BatchProcessingService.cs
├── Processing/
│   ├── ParallelProcessor.cs
│   ├── StreamProcessor.cs
│   └── ProgressReporter.cs
└── Extensions/
    └── ServiceCollectionExtensions.cs
```

### 🔧 Core Models Implementation

#### Models/BatchConversionRequest.cs
```csharp
namespace DocToPdf.BatchProcessing.Models;

public class BatchConversionRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SourcePath { get; set; } = string.Empty;
    public string? OutputPath { get; set; }
    public ConversionType Type { get; set; }
    public Dictionary<string, object>? Options { get; set; }
    public int Priority { get; set; } = 0; // Higher = more priority
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum ConversionType
{
    Markdown,
    Html,
    Text,
    Docx,
    Excel,
    Json,
    Csv
}

public class BatchProcessingOptions
{
    public int MaxConcurrency { get; set; } = Environment.ProcessorCount;
    public bool ContinueOnError { get; set; } = true;
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
    public bool SaveToFiles { get; set; } = false;
    public string? OutputDirectory { get; set; }
    public long MaxMemoryUsageMB { get; set; } = 1024; // 1GB default
    public bool EnableProgressReporting { get; set; } = true;
}

public class BatchProgress
{
    public string BatchId { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public int FailedItems { get; set; }
    public int SkippedItems { get; set; }
    public double Percentage => TotalItems > 0 ? (double)CompletedItems / TotalItems * 100 : 0;
    public string? CurrentItem { get; set; }
    public TimeSpan ElapsedTime { get; set; }
    public TimeSpan EstimatedTimeRemaining { get; set; }
    public long MemoryUsageMB { get; set; }
    public Dictionary<string, object> CustomData { get; set; } = new();
}
```

#### Services/IBatchProcessingService.cs
```csharp
namespace DocToPdf.BatchProcessing.Services;

public interface IBatchProcessingService
{
    /// <summary>
    /// Process batch sequentially
    /// </summary>
    Task<List<BatchConversionResult>> ConvertBatchAsync(
        IEnumerable<BatchConversionRequest> requests,
        BatchProcessingOptions? options = null,
        IProgress<BatchProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Process batch in parallel
    /// </summary>
    Task<List<BatchConversionResult>> ConvertBatchParallelAsync(
        IEnumerable<BatchConversionRequest> requests,
        int maxConcurrency = 4,
        IProgress<BatchProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Stream processing results as they complete
    /// </summary>
    IAsyncEnumerable<BatchConversionResult> ConvertBatchStreamAsync(
        IEnumerable<BatchConversionRequest> requests,
        BatchProcessingOptions? options = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Priority-based batch processing
    /// </summary>
    Task<List<BatchConversionResult>> ConvertBatchWithPriorityAsync(
        IEnumerable<BatchConversionRequest> requests,
        BatchProcessingOptions? options = null,
        IProgress<BatchProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
```

---

## 🔒 Feature 4: DocToPdf.Security - Technical Spec

### 🏗️ Project Setup Commands

```powershell
New-Item -ItemType Directory -Path "DocToPdf.Security"
Set-Location "DocToPdf.Security"
dotnet new classlib -f net8.0
dotnet add package DocToPdf --version 1.1.0
dotnet add package PdfSharp --version 6.1.1
dotnet add package System.Security.Cryptography.X509Certificates --version 4.3.2
```

### 📁 Required File Structure
```
DocToPdf.Security/
├── Models/
│   ├── PdfSecurityOptions.cs
│   ├── DigitalSignatureOptions.cs
│   ├── PdfPermissions.cs
│   └── SecurityValidationResult.cs
├── Services/
│   ├── IPdfSecurityService.cs
│   └── PdfSecurityService.cs
├── Cryptography/
│   ├── PdfEncryption.cs
│   ├── DigitalSignatureManager.cs
│   └── CertificateManager.cs
└── Extensions/
    └── ServiceCollectionExtensions.cs
```

### 🔧 Core Models Implementation

#### Models/PdfSecurityOptions.cs
```csharp
using System.Security.Cryptography.X509Certificates;

namespace DocToPdf.Security.Models;

public class PdfSecurityOptions
{
    public string? UserPassword { get; set; }
    public string? OwnerPassword { get; set; }
    public EncryptionLevel Encryption { get; set; } = EncryptionLevel.AES256;
    public PdfPermissions Permissions { get; set; } = PdfPermissions.All;
    public DigitalSignatureOptions? DigitalSignature { get; set; }
    public bool RemoveMetadata { get; set; } = false;
    public bool PreventPrintScreen { get; set; } = false;
}

[Flags]
public enum PdfPermissions
{
    None = 0,
    Print = 1,
    ModifyContents = 2,
    CopyContents = 4,
    ModifyAnnotations = 8,
    FillForms = 16,
    ExtractContents = 32,
    AssembleDocument = 64,
    PrintHighQuality = 128,
    All = Print | ModifyContents | CopyContents | ModifyAnnotations | 
          FillForms | ExtractContents | AssembleDocument | PrintHighQuality
}

public enum EncryptionLevel
{
    None,
    Standard40Bit,
    Standard128Bit,
    AES128,
    AES256
}

public class DigitalSignatureOptions
{
    public X509Certificate2 Certificate { get; set; } = null!;
    public string? Reason { get; set; }
    public string? Location { get; set; }
    public string? ContactInfo { get; set; }
    public bool VisibleSignature { get; set; } = false;
    public SignaturePosition? Position { get; set; }
    public DateTime? SigningTime { get; set; }
}

public class SignaturePosition
{
    public int Page { get; set; } = 1;
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; } = 200;
    public float Height { get; set; } = 50;
}

public class PdfSecurityInfo
{
    public bool IsEncrypted { get; set; }
    public bool HasUserPassword { get; set; }
    public bool HasOwnerPassword { get; set; }
    public EncryptionLevel EncryptionLevel { get; set; }
    public PdfPermissions Permissions { get; set; }
    public bool IsSigned { get; set; }
    public List<SignatureInfo> Signatures { get; set; } = new();
}

public class SignatureInfo
{
    public string SignerName { get; set; } = string.Empty;
    public DateTime SigningTime { get; set; }
    public bool IsValid { get; set; }
    public string? Reason { get; set; }
    public string? Location { get; set; }
}
```

### 🛠️ Implementation Commands for Each Package

#### Commands voor DocToPdf.Customization
```powershell
# 1. Create complete project structure
mkdir DocToPdf.Customization\Models, DocToPdf.Customization\Services, DocToPdf.Customization\Extensions, DocToPdf.Customization\Themes, DocToPdf.Customization\Utils

# 2. Create all model files
# (Create each .cs file with the specifications above)

# 3. Implement services
# (Implement IPdfCustomizationService and PdfCustomizationService)

# 4. Add DI extensions
# (Implement ServiceCollectionExtensions.AddDocToPdfCustomization())

# 5. Build and test
dotnet build
dotnet pack --configuration Release

# 6. Local testing
dotnet add ..\TestProject\TestProject.csproj package DocToPdf.Customization --source .\bin\Release\
```

#### Commands voor alle andere packages
Herhaal hetzelfde patroon voor:
- DocToPdf.DataReports  
- DocToPdf.BatchProcessing
- DocToPdf.Security

### 🧪 Testing Strategy per Package

#### Unit Tests Template
```csharp
// Tests/PdfCustomizationServiceTests.cs
[TestClass]
public class PdfCustomizationServiceTests
{
    private readonly ITestOutputHelper _output;
    private readonly PdfCustomizationService _service;

    public PdfCustomizationServiceTests(ITestOutputHelper output)
    {
        _output = output;
        _service = CreateService();
    }

    [Test]
    public async Task ApplyWatermark_ShouldAddWatermarkToPdf()
    {
        // Arrange
        var pdfBytes = CreateTestPdf();
        var watermark = new WatermarkOptions 
        { 
            Text = "TEST WATERMARK",
            Opacity = 0.5f 
        };

        // Act
        var result = await _service.AddWatermarkAsync(pdfBytes, watermark);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > pdfBytes.Length);
        // Verify watermark exists in PDF
    }
}
```

Dit geeft een complete, implementeerbare specificatie voor alle 4 features! 🚀
