# 🎯 DocToPdf Showcase - NuGet Package Demo

This ASP.NET Core Web API application demonstrates **production usage** of the DocToPdf NuGet package.

## ✨ Key Features

- **Real NuGet Package Usage**: Uses `<PackageReference Include="DocToPdf" Version="1.1.0" />` 
- **Modern Web UI**: Interactive document conversion interface
- **Live Examples**: Pre-loaded content for testing all conversion types
- **Professional Integration**: Demonstrates proper DI setup and error handling
- **API Documentation**: Swagger UI for exploring the API

## 🚀 Quick Start

1. **Run the showcase**:
   ```bash
   dotnet run
   ```

2. **Open browser**: Navigate to the displayed URL (e.g., `http://localhost:51558`)

3. **Try conversions**: Use the web interface to convert Markdown, HTML, or Text to PDF

4. **Explore API**: Visit `/swagger` for API documentation

## 💡 What This Shows

### NuGet Package Integration
```xml
<!-- This showcase uses the actual NuGet package -->
<PackageReference Include="DocToPdf" Version="1.1.0" />
```

### Dependency Injection Setup
```csharp
// Program.cs - exactly how users would add it
builder.Services.AddDocToPdf();

// Controller - standard DI pattern
public PdfController(IDocumentToPdfService pdfService) => _pdfService = pdfService;
```

### API Usage Examples
```csharp
// Convert markdown to PDF bytes
var pdfBytes = await _pdfService.ConvertMarkdownToPdfAsync(content, title, basePath: null);

// Return as downloadable file
return File(pdfBytes, "application/pdf", "document.pdf");
```

## 🏗️ Architecture

- **Frontend**: Modern HTML/CSS/JS interface with real-time conversion
- **Backend**: ASP.NET Core Web API with DocToPdf service injection
- **Package Source**: Uses local NuGet package via `NuGet.config`

## 🔧 For Your Own Projects

Copy this integration pattern:

1. **Install package**: `dotnet add package DocToPdf`
2. **Add service**: `builder.Services.AddDocToPdf();`
3. **Inject & use**: In your controllers or services

---

**Perfect example of DocToPdf NuGet package usage in production!** 🎉
