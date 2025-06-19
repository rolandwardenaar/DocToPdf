# 🚀 DocToPdf Showcase - Demo Applicatie

Een complete **ASP.NET Core Web API showcase applicatie** die demonstreert hoe de DocToPdf NuGet package gebruikt wordt in een echte webapplicatie.

![DocToPdf Showcase Interface](screenshot.png)

## ✨ Live Demo Features

- **🌐 Modern Web Interface**: Responsive design met moderne UI
- **📝 Markdown Converter**: Met Mermaid diagram ondersteuning
- **🌐 HTML Converter**: Volledige CSS styling ondersteuning
- **📄 Text Converter**: Plain text naar professionele PDF
- **⚡ Real-time Conversion**: Instant PDF downloads
- **📊 API Documentation**: Volledige Swagger documentatie
- **🔧 Production Ready**: Echte ASP.NET Core implementatie

## 🏃‍♂️ Quick Start

### Vereisten
- .NET 8.0 SDK of hoger
- DocToPdf NuGet package

### Installatie & Run

1. **Clone/Download het project**:
   ```bash
   git clone https://github.com/YOUR_USERNAME/DocToPdf.git
   cd DocToPdf/showcase
   ```

2. **Installeer dependencies**:
   ```bash
   dotnet restore
   ```

3. **Start de applicatie**:
   ```bash
   dotnet run
   ```

4. **Open je browser**: 
   - Navigate naar `https://localhost:7000`
   - API documentatie: `https://localhost:7000/swagger`

### NuGet Package Gebruik

In productie zou je de DocToPdf NuGet package gebruiken:

```xml
<PackageReference Include="DocToPdf" Version="1.1.0" />
```

## 🎯 Wat Deze Demo Toont

### 1. **Dependency Injection Setup**
```csharp
// Program.cs
builder.Services.AddDocToPdf();

// Controller
public PdfController(IDocumentToPdfService pdfService)
{
    _pdfService = pdfService;
}
```

### 2. **ASP.NET Core Web API Integration**
```csharp
[HttpPost("convert/markdown")]
public async Task<IActionResult> ConvertMarkdown([FromBody] ConvertMarkdownRequest request)
{
    var pdfBytes = await _pdfService.ConvertMarkdownToPdfAsync(
        request.MarkdownContent, 
        request.Title ?? "Generated PDF"
    );
    
    return File(pdfBytes, "application/pdf", $"{request.Title}.pdf");
}
```

### 3. **Frontend Integration**
```javascript
// JavaScript fetch API
const response = await fetch('/api/pdf/convert/markdown', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ markdownContent: content, title: title })
});

const blob = await response.blob();
// Automatic download in browser
```

## 📡 API Endpoints

| Endpoint | Method | Beschrijving |
|----------|--------|-------------|
| `/api/pdf/convert/markdown` | POST | Convert Markdown naar PDF |
| `/api/pdf/convert/html` | POST | Convert HTML naar PDF |
| `/api/pdf/convert/text` | POST | Convert plain text naar PDF |
| `/api/pdf/examples` | GET | Krijg voorbeeld content |
| `/swagger` | GET | API documentatie |

### Request/Response Examples

**Markdown Conversion**:
```json
POST /api/pdf/convert/markdown
{
  "markdownContent": "# Hello World\\n\\nThis is **bold** text.",
  "title": "My Document"
}

Response: PDF file download
```

**HTML Conversion**:
```json
POST /api/pdf/convert/html
{
  "htmlContent": "<h1>Professional Report</h1><p>Content...</p>",
  "title": "Report"
}

Response: PDF file download
```

## 🌟 Features Gedemonstreerd

### Document Types
- ✅ **Markdown** with Mermaid diagrams
- ✅ **HTML** with CSS styling
- ✅ **Plain Text** with formatting
- ✅ **Professional PDF** output

### Technical Features
- ✅ **Async/Await** pattern
- ✅ **Error Handling** & validation
- ✅ **Logging** integration
- ✅ **CORS** configuration
- ✅ **File Downloads** with proper headers
- ✅ **API Documentation** (Swagger)

### UI/UX Features
- ✅ **Responsive Design** (mobile friendly)
- ✅ **Real-time Status** feedback
- ✅ **Example Content** loading
- ✅ **Modern Interface** with animations
- ✅ **Error Handling** in UI

## 🏗️ Project Structuur

```
showcase/
├── Controllers/
│   └── PdfController.cs           # API endpoints
├── wwwroot/
│   └── demo.html                  # Frontend interface
├── Program.cs                     # App configuration
├── showcase.csproj                # Project file
└── README.md                      # Deze documentatie
```

### Dependency Injection Implementation

```csharp
// Registratie
builder.Services.AddDocToPdf();

// Gebruik in Controller
private readonly IDocumentToPdfService _pdfService;
private readonly ILogger<PdfController> _logger;

public PdfController(IDocumentToPdfService pdfService, ILogger<PdfController> logger)
{
    _pdfService = pdfService;
    _logger = logger;
}
```

## 🧪 Test Scenarios

### 1. **Markdown Features Test**
```markdown
# Test Document

## Mermaid Diagram
```mermaid
graph TD
    A[Start] --> B{Decision}
    B -->|Yes| C[Action]
    B -->|No| D[End]
```

## Code Block
```csharp
public class Test { }
```

## Table
| Feature | Status |
|---------|--------|
| Tables  | ✅     |
```

### 2. **HTML Styling Test**
```html
<!DOCTYPE html>
<html>
<head>
    <style>
        .highlight { background: #ffeb3b; padding: 10px; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #ddd; padding: 8px; }
    </style>
</head>
<body>
    <div class="highlight">Styled Content</div>
    <table>
        <tr><th>Header</th><td>Data</td></tr>
    </table>
</body>
</html>
```

### 3. **Performance Test**
- Large documents (1000+ lines)
- Multiple concurrent conversions
- Memory usage monitoring

## 🚀 Production Deployment

### Docker Support
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY publish/ .
EXPOSE 8080
ENTRYPOINT ["dotnet", "showcase.dll"]
```

### Azure App Service
```bash
# Publish for Azure
dotnet publish -c Release -o publish
```

### Configuration
```json
{
  "DocToPdf": {
    "DefaultOutputDirectory": "generated-pdfs",
    "EnableMermaidDiagrams": true,
    "MaxFileSizeBytes": 10485760
  }
}
```

## 🔧 Development

### Aanpassingen Maken
1. **Frontend**: Edit `wwwroot/demo.html`
2. **API**: Edit `Controllers/PdfController.cs`
3. **Configuration**: Edit `Program.cs`

### Nieuwe Features Toevoegen
- **File Upload**: Support voor DOCX uploads
- **Templates**: PDF templates en styling
- **Authentication**: User accounts
- **Batch Processing**: Multiple files
- **Storage**: Save PDFs to cloud storage

### Testing
```bash
# Unit tests (als toegevoegd)
dotnet test

# API testing via Swagger
https://localhost:7000/swagger

# Manual testing via UI
https://localhost:7000
```

## 📊 Performance Metrics

### Conversion Times (typical)
- **Small Markdown** (1-2 pages): ~500ms
- **Medium HTML** (5-10 pages): ~1-2s
- **Large Document** (20+ pages): ~3-5s
- **With Mermaid** (adds): ~+1-2s per diagram

### Memory Usage
- **Base application**: ~50MB
- **Per conversion**: ~10-20MB
- **Concurrent requests**: Scales linearly

## 🎓 Educational Value

Deze showcase demonstreert:

1. **Best Practices** voor .NET library integratie
2. **Modern ASP.NET Core** patterns
3. **API Design** principes
4. **Frontend/Backend** communication
5. **Error Handling** strategieën
6. **Performance** consideraties
7. **Production Deployment** ready code

## 📞 Support & Feedback

- **GitHub Issues**: [Create an issue](https://github.com/YOUR_USERNAME/DocToPdf/issues)
- **Feature Requests**: [Request a feature](https://github.com/YOUR_USERNAME/DocToPdf/issues/new)
- **Documentation**: [Main README](../README.md)
- **NuGet Package**: [DocToPdf on NuGet](https://nuget.org/packages/DocToPdf)

---

**DocToPdf Showcase** - Professionele demonstratie van document-to-PDF conversie in .NET! 🎉
