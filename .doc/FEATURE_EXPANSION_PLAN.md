# 🏗️ DocToPdf Feature Expansion - Master Implementation Plan

## 🎯 Overview

Dit plan breidt DocToPdf uit met 4 major features als **modulaire NuGet packages**, elk onafhankelijk installeerbaar en configureerbaar.

## 📦 Package Architecture

```
DocToPdf (Core)                     // Bestaande library
├── DocToPdf.Customization         // PDF styling, watermarks, themes
├── DocToPdf.DataReports           // Excel/JSON naar PDF reports  
├── DocToPdf.BatchProcessing       // Bulk operations, progress tracking
└── DocToPdf.Security              // Password protection, encryption
```

## 🚀 Feature 1: DocToPdf.Customization

### 📋 Scope
- Watermarks (text/image)
- Custom headers/footers met templates
- PDF themes en styling
- Custom fonts support
- PDF metadata management

### 🔧 Implementation Plan

#### Core Models
```csharp
// Models/PdfCustomization.cs
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
    public string Text { get; set; }
    public string? ImagePath { get; set; }
    public float Opacity { get; set; } = 0.3f;
    public WatermarkPosition Position { get; set; } = WatermarkPosition.Center;
    public float Rotation { get; set; } = 45f;
}

public class HeaderFooterOptions
{
    public string Template { get; set; } // "Page {page} of {totalPages}"
    public string? FontName { get; set; }
    public float FontSize { get; set; } = 10f;
    public HorizontalAlignment Alignment { get; set; }
}

public enum PdfTheme
{
    Default,
    Corporate,
    Modern,
    Minimal,
    Academic
}
```

#### Service Interface
```csharp
// Services/IPdfCustomizationService.cs
public interface IPdfCustomizationService
{
    Task<byte[]> ApplyCustomizationAsync(byte[] pdfBytes, PdfCustomizationOptions options);
    Task<PdfTheme> CreateCustomThemeAsync(ThemeDefinition definition);
    Task AddWatermarkAsync(byte[] pdfBytes, WatermarkOptions watermark);
    Task SetMetadataAsync(byte[] pdfBytes, PdfMetadata metadata);
}
```

#### Implementation Strategy
```csharp
// Services/PdfCustomizationService.cs
public class PdfCustomizationService : IPdfCustomizationService
{
    // Use QuestPDF's theming capabilities
    // Implement watermark overlay
    // Template engine for headers/footers
    // Font management via SkiaSharp
}
```

#### DI Extension
```csharp
// Extensions/ServiceCollectionExtensions.cs
public static IServiceCollection AddDocToPdfCustomization(
    this IServiceCollection services,
    Action<PdfCustomizationOptions>? configure = null)
{
    services.Configure<PdfCustomizationOptions>(configure ?? (_ => { }));
    services.AddScoped<IPdfCustomizationService, PdfCustomizationService>();
    return services;
}
```

#### Usage Example
```csharp
// In Program.cs
builder.Services.AddDocToPdfCustomization(options =>
{
    options.Watermark = new WatermarkOptions 
    { 
        Text = "CONFIDENTIAL", 
        Opacity = 0.3f 
    };
    options.Header = new HeaderFooterOptions 
    { 
        Template = "{title} - {date}" 
    };
    options.Theme = PdfTheme.Corporate;
});

// In Controller
var customizedPdf = await _customizationService.ApplyCustomizationAsync(
    pdfBytes, 
    new PdfCustomizationOptions { ... }
);
```

---

## 🚀 Feature 2: DocToPdf.DataReports

### 📋 Scope
- Excel (XLSX) naar PDF reports
- JSON data naar formatted PDF
- CSV naar PDF tabellen
- Data-driven template engine
- Chart generation integration

### 🔧 Implementation Plan

#### Core Models
```csharp
// Models/DataReportOptions.cs
public class DataReportOptions
{
    public string? TemplatePath { get; set; }
    public ReportStyle Style { get; set; } = ReportStyle.Default;
    public bool IncludeCharts { get; set; } = true;
    public TableOptions? TableOptions { get; set; }
    public List<ChartDefinition>? Charts { get; set; }
}

public class ExcelConversionOptions : DataReportOptions
{
    public string? WorksheetName { get; set; }
    public string? CellRange { get; set; }
    public bool IncludeFormulas { get; set; } = false;
}

public class JsonReportOptions : DataReportOptions
{
    public string? JsonSchema { get; set; }
    public Dictionary<string, object>? Variables { get; set; }
}
```

#### Service Interface
```csharp
// Services/IDataReportService.cs
public interface IDataReportService
{
    Task<byte[]> ConvertExcelToPdfAsync(string excelPath, ExcelConversionOptions? options = null);
    Task<byte[]> ConvertExcelToPdfAsync(Stream excelStream, ExcelConversionOptions? options = null);
    
    Task<byte[]> ConvertJsonToPdfAsync(string jsonData, JsonReportOptions options);
    Task<byte[]> ConvertJsonToPdfAsync<T>(T data, JsonReportOptions options) where T : class;
    
    Task<byte[]> ConvertCsvToPdfAsync(string csvPath, DataReportOptions? options = null);
    Task<byte[]> ConvertCsvToPdfAsync(Stream csvStream, DataReportOptions? options = null);
    
    Task<byte[]> GenerateReportFromTemplateAsync(string templatePath, object data);
}
```

#### Implementation Strategy
```csharp
// Services/DataReportService.cs
public class DataReportService : IDataReportService
{
    // Excel: DocumentFormat.OpenXml (free, Microsoft)
    // JSON: System.Text.Json (built-in)
    // CSV: CsvHelper (free, open source)
    // Templates: Scriban (free template engine)
    // Charts: ScottPlot (free charting, MIT license)
}
```

#### Template Engine
```csharp
// Templates/ReportTemplateEngine.cs
public class ReportTemplateEngine
{
    // Use Scriban for templating
    // Support for loops, conditions, formatting
    // Custom template functions for charts, tables
}
```

#### Usage Example
```csharp
// Excel to PDF
var excelPdf = await _dataReportService.ConvertExcelToPdfAsync(
    "sales-data.xlsx", 
    new ExcelConversionOptions 
    { 
        WorksheetName = "Q1 Sales",
        IncludeCharts = true 
    }
);

// JSON to PDF with template
var jsonPdf = await _dataReportService.ConvertJsonToPdfAsync(
    salesData,
    new JsonReportOptions 
    { 
        TemplatePath = "templates/sales-report.template" 
    }
);
```

---

## 🚀 Feature 3: DocToPdf.BatchProcessing

### 📋 Scope
- Bulk document conversions
- Progress reporting en cancellation
- Memory-efficient streaming
- Parallel processing
- Result aggregation

### 🔧 Implementation Plan

#### Core Models
```csharp
// Models/BatchProcessing.cs
public class BatchConversionRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SourcePath { get; set; }
    public string? OutputPath { get; set; }
    public ConversionType Type { get; set; }
    public Dictionary<string, object>? Options { get; set; }
}

public class BatchConversionResult
{
    public string RequestId { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public byte[]? PdfBytes { get; set; }
    public string? OutputPath { get; set; }
    public TimeSpan ProcessingTime { get; set; }
}

public class BatchProgress
{
    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public int FailedItems { get; set; }
    public double Percentage => TotalItems > 0 ? (double)CompletedItems / TotalItems * 100 : 0;
    public string? CurrentItem { get; set; }
}
```

#### Service Interface
```csharp
// Services/IBatchProcessingService.cs
public interface IBatchProcessingService
{
    Task<List<BatchConversionResult>> ConvertBatchAsync(
        IEnumerable<BatchConversionRequest> requests,
        BatchProcessingOptions? options = null,
        IProgress<BatchProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    Task<List<BatchConversionResult>> ConvertBatchParallelAsync(
        IEnumerable<BatchConversionRequest> requests,
        int maxConcurrency = 4,
        IProgress<BatchProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    IAsyncEnumerable<BatchConversionResult> ConvertBatchStreamAsync(
        IEnumerable<BatchConversionRequest> requests,
        CancellationToken cancellationToken = default);
}
```

#### Implementation Strategy
```csharp
// Services/BatchProcessingService.cs
public class BatchProcessingService : IBatchProcessingService
{
    // Use Parallel.ForEachAsync for parallel processing
    // Channel<T> for streaming results
    // Memory management voor grote batches
    // Progress reporting via IProgress<T>
    // Cancellation token support
}
```

#### Usage Example
```csharp
// Batch processing met progress
var requests = new[]
{
    new BatchConversionRequest { SourcePath = "doc1.md", Type = ConversionType.Markdown },
    new BatchConversionRequest { SourcePath = "doc2.html", Type = ConversionType.Html }
};

var progress = new Progress<BatchProgress>(p => 
    Console.WriteLine($"Progress: {p.Percentage:F1}% ({p.CompletedItems}/{p.TotalItems})"));

var results = await _batchService.ConvertBatchAsync(requests, progress: progress);

// Streaming batch processing
await foreach (var result in _batchService.ConvertBatchStreamAsync(requests))
{
    if (result.Success)
        await File.WriteAllBytesAsync(result.OutputPath, result.PdfBytes);
}
```

---

## 🚀 Feature 4: DocToPdf.Security

### 📋 Scope
- Password protection (user/owner passwords)
- PDF encryption (40-bit, 128-bit, 256-bit AES)
- Permission management
- Digital signatures
- Certificate-based security

### 🔧 Implementation Plan

#### Core Models
```csharp
// Models/PdfSecurity.cs
public class PdfSecurityOptions
{
    public string? UserPassword { get; set; }
    public string? OwnerPassword { get; set; }
    public EncryptionLevel Encryption { get; set; } = EncryptionLevel.AES256;
    public PdfPermissions Permissions { get; set; } = PdfPermissions.All;
    public DigitalSignatureOptions? DigitalSignature { get; set; }
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
    public X509Certificate2 Certificate { get; set; }
    public string? Reason { get; set; }
    public string? Location { get; set; }
    public string? ContactInfo { get; set; }
}
```

#### Service Interface
```csharp
// Services/IPdfSecurityService.cs
public interface IPdfSecurityService
{
    Task<byte[]> EncryptPdfAsync(byte[] pdfBytes, PdfSecurityOptions options);
    Task<byte[]> SetPasswordAsync(byte[] pdfBytes, string userPassword, string? ownerPassword = null);
    Task<byte[]> SetPermissionsAsync(byte[] pdfBytes, PdfPermissions permissions);
    Task<byte[]> SignPdfAsync(byte[] pdfBytes, DigitalSignatureOptions signatureOptions);
    
    Task<bool> VerifyPasswordAsync(byte[] pdfBytes, string password);
    Task<PdfSecurityInfo> GetSecurityInfoAsync(byte[] pdfBytes);
    Task<bool> VerifySignatureAsync(byte[] pdfBytes);
}
```

#### Implementation Strategy
```csharp
// Services/PdfSecurityService.cs
public class PdfSecurityService : IPdfSecurityService
{
    // Use iText7 Community (AGPL) or PdfSharp (MIT) for encryption
    // System.Security.Cryptography for certificate handling
    // Custom encryption implementation if needed for commercial use
    
    // Alternative: Use QuestPDF + custom encryption layer
    // Implement PDF encryption according to PDF specification
}
```

#### Usage Example
```csharp
// Password protection
var securedPdf = await _securityService.EncryptPdfAsync(pdfBytes, new PdfSecurityOptions
{
    UserPassword = "user123",
    OwnerPassword = "owner456", 
    Permissions = PdfPermissions.Print | PdfPermissions.CopyContents,
    Encryption = EncryptionLevel.AES256
});

// Digital signature
var signedPdf = await _securityService.SignPdfAsync(pdfBytes, new DigitalSignatureOptions
{
    Certificate = myCertificate,
    Reason = "Document approval",
    Location = "Amsterdam, NL"
});
```

---

## 📦 Package Structure & Dependencies

### Project Structure
```
DocToPdf.Extensions/
├── DocToPdf.Customization/
│   ├── Models/
│   ├── Services/
│   ├── Extensions/
│   ├── Themes/
│   └── DocToPdf.Customization.csproj
├── DocToPdf.DataReports/
│   ├── Models/
│   ├── Services/
│   ├── Templates/
│   ├── Converters/
│   └── DocToPdf.DataReports.csproj
├── DocToPdf.BatchProcessing/
│   ├── Models/
│   ├── Services/
│   ├── Extensions/
│   └── DocToPdf.BatchProcessing.csproj
└── DocToPdf.Security/
    ├── Models/
    ├── Services/
    ├── Cryptography/
    └── DocToPdf.Security.csproj
```

### Dependencies Per Package

#### DocToPdf.Customization
```xml
<PackageReference Include="DocToPdf" Version="1.1.0" />
<PackageReference Include="SkiaSharp" Version="3.119.0" />
<PackageReference Include="System.Drawing.Common" Version="8.0.0" />
```

#### DocToPdf.DataReports  
```xml
<PackageReference Include="DocToPdf" Version="1.1.0" />
<PackageReference Include="DocumentFormat.OpenXml" Version="3.3.0" />
<PackageReference Include="CsvHelper" Version="33.0.1" />
<PackageReference Include="Scriban" Version="5.10.0" />
<PackageReference Include="ScottPlot" Version="5.0.51" />
```

#### DocToPdf.BatchProcessing
```xml
<PackageReference Include="DocToPdf" Version="1.1.0" />
<PackageReference Include="System.Threading.Channels" Version="8.0.0" />
```

#### DocToPdf.Security
```xml
<PackageReference Include="DocToPdf" Version="1.1.0" />
<PackageReference Include="PdfSharp" Version="6.1.1" />
<!-- OR alternative implementation -->
<PackageReference Include="System.Security.Cryptography.X509Certificates" Version="4.3.2" />
```

## 🔧 Integration Example

```csharp
// Complete setup with all extensions
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

// Usage in controller
public class AdvancedPdfController : ControllerBase
{
    private readonly IDocumentToPdfService _pdfService;
    private readonly IPdfCustomizationService _customization;
    private readonly IDataReportService _dataReports;
    private readonly IBatchProcessingService _batchProcessing;
    private readonly IPdfSecurityService _security;

    public async Task<IActionResult> CreateSecureCustomReport()
    {
        // 1. Generate PDF from data
        var pdfBytes = await _dataReports.ConvertJsonToPdfAsync(reportData, options);
        
        // 2. Apply customization
        var customizedPdf = await _customization.ApplyCustomizationAsync(pdfBytes, customOptions);
        
        // 3. Add security
        var securePdf = await _security.EncryptPdfAsync(customizedPdf, securityOptions);
        
        return File(securePdf, "application/pdf", "secure-report.pdf");
    }
}
```

## 🚧 Implementation Timeline

### Phase 1 (Week 1-2): Foundation
- Project structure setup
- Core models en interfaces
- Base implementations

### Phase 2 (Week 3-4): DocToPdf.Customization
- Watermark implementation
- Header/footer templates  
- Theme system
- Testing & documentation

### Phase 3 (Week 5-6): DocToPdf.DataReports
- Excel converter
- JSON template engine
- CSV processor
- Chart integration

### Phase 4 (Week 7-8): DocToPdf.BatchProcessing  
- Parallel processing
- Progress reporting
- Memory optimization
- Performance testing

### Phase 5 (Week 9-10): DocToPdf.Security
- Encryption implementation
- Password protection
- Digital signatures
- Security testing

### Phase 6 (Week 11-12): Integration & Polish
- Cross-package integration
- Comprehensive testing
- Documentation
- NuGet packaging

## 📋 Development Guidelines

### Code Standards
- Async/await throughout
- Comprehensive logging
- Input validation
- Error handling
- Unit test coverage >80%
- XML documentation
- Cancellation token support

### Testing Strategy
- Unit tests voor elke service
- Integration tests voor cross-package features
- Performance benchmarks
- Security testing voor encryption features
- Memory leak testing voor batch processing

### Documentation Requirements
- README per package
- API documentation
- Usage examples
- Migration guides
- Performance guidelines

Dit plan biedt een complete roadmap voor het uitbreiden van DocToPdf met enterprise-grade features als modulaire NuGet packages! 🚀
