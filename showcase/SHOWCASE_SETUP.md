# 🚀 DocToPdf Showcase Application

## ⚠️ Setup Instructions

Due to the dual nature of the DocToPdf project (both library and console app), de showcase heeft wat handmatige setup nodig.

### 🛠️ Quick Setup

1. **Create een nieuw ASP.NET Core Web API project**:
   ```bash
   dotnet new webapi -n DocToPdfShowcase
   cd DocToPdfShowcase
   ```

2. **Add de DocToPdf reference**:
   ```xml
   <!-- Voor lokale development -->
   <ProjectReference Include="..\DocToPdf.csproj" />
   
   <!-- OF voor NuGet package gebruik -->
   <PackageReference Include="DocToPdf" Version="1.1.0" />
   ```

3. **Update Program.cs**:
   ```csharp
   using DocToPdf.Extensions;

   var builder = WebApplication.CreateBuilder(args);

   builder.Services.AddControllers();
   builder.Services.AddEndpointsApiExplorer();
   builder.Services.AddSwaggerGen();

   // Add DocToPdf services
   builder.Services.AddDocToPdf();

   builder.Services.AddCors(options =>
   {
       options.AddDefaultPolicy(builder =>
       {
           builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
       });
   });

   var app = builder.Build();

   if (app.Environment.IsDevelopment())
   {
       app.UseSwagger();
       app.UseSwaggerUI();
   }

   app.UseHttpsRedirection();
   app.UseCors();
   app.UseStaticFiles();
   app.UseAuthorization();
   app.MapControllers();

   // Serve demo HTML at root
   app.MapFallbackToFile("demo.html");

   app.Run();
   ```

4. **Kopieer de bestanden**:
   - `Controllers/PdfController.cs` - API endpoints
   - `wwwroot/demo.html` - Frontend interface

5. **Run de showcase**:
   ```bash
   dotnet run
   ```

### 📱 Wat de Showcase Demonstreert

- ✅ **Dependency Injection**: `builder.Services.AddDocToPdf()`
- ✅ **ASP.NET Core Integration**: Controller met `IDocumentToPdfService`
- ✅ **API Endpoints**: 
  - `POST /api/pdf/convert/markdown`
  - `POST /api/pdf/convert/html`
  - `POST /api/pdf/convert/text`
  - `GET /api/pdf/examples`
- ✅ **Modern UI**: Responsive web interface
- ✅ **Real-time Conversion**: Direct PDF downloads
- ✅ **Error Handling**: Comprehensive error responses

### 🎯 Key Features Demonstrated

1. **Service Injection**:
   ```csharp
   public PdfController(IDocumentToPdfService pdfService)
   {
       _pdfService = pdfService;
   }
   ```

2. **Async Conversion**:
   ```csharp
   var pdfBytes = await _pdfService.ConvertMarkdownToPdfAsync(
       request.MarkdownContent, 
       request.Title ?? "Generated PDF"
   );
   ```

3. **File Response**:
   ```csharp
   return File(pdfBytes, "application/pdf", fileName);
   ```

### 📦 Production Usage

Voor productie gebruik, vervang de project reference door:

```xml
<PackageReference Include="DocToPdf" Version="1.1.0" />
```

En installeer via:
```bash
dotnet add package DocToPdf
```

### 🌟 Showcase Highlights

- **Modern UI**: Gradient design, responsive layout
- **Live Examples**: Pre-loaded content voor testing
- **Multiple Formats**: Markdown, HTML, en Text conversie
- **Mermaid Support**: Automatische diagram conversie
- **Error Handling**: User-friendly error messages
- **Download Management**: Automatic PDF downloads
- **API Documentation**: Swagger integration

### 🔧 Troubleshooting

Als je build errors krijgt:
1. Zorg ervoor dat je `Microsoft.NET.Sdk.Web` SDK gebruikt
2. Check dat alle ASP.NET Core packages correct geïnstalleerd zijn
3. Gebruik .NET 8.0 target framework
4. Verwijder oude `bin/` en `obj/` directories

### 📊 Example API Calls

```bash
# Convert Markdown
curl -X POST "https://localhost:7000/api/pdf/convert/markdown" \
     -H "Content-Type: application/json" \
     -d '{"markdownContent": "# Hello World", "title": "Test"}'

# Convert HTML  
curl -X POST "https://localhost:7000/api/pdf/convert/html" \
     -H "Content-Type: application/json" \
     -d '{"htmlContent": "<h1>Hello</h1>", "title": "Test"}'
```

---

**Deze showcase demonstreert de volledige kracht van DocToPdf als professional .NET library!** 🎉
