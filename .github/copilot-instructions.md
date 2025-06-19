# GitHub Copilot Instructions for DocToPdf Project

## 📋 General Development Guidelines

### API Documentation Usage
**CRITICAL**: Always read and use the official API documentation for the exact versions of libraries used in this project before suggesting code or implementations.

**Required Actions**:
1. **Check project files** (`.csproj`, `packages.config`, `package.json`) to identify exact library versions
2. **Use official documentation** for the specific version found in the project
3. **Verify API compatibility** before suggesting method calls, properties, or configurations
4. **Reference version-specific examples** from official docs when available

### Library-Specific Instructions

#### QuestPDF
- **Always check**: Current project uses QuestPDF version specified in `DocToPdf.csproj`
- **Documentation source**: Official QuestPDF documentation for the exact version
- **Key areas**: Document structure, styling, layouts, components, PDF generation
- **Breaking changes**: Be aware that QuestPDF API may change between versions

#### DocumentFormat.OpenXml
- **Always check**: Version used in project dependencies
- **Documentation source**: Microsoft Learn documentation for Office Open XML
- **Key areas**: Excel/Word document reading, cell/paragraph parsing, formatting extraction
- **Compatibility**: Ensure suggested code works with the specific version in use

#### Markdig
- **Always check**: Version specified in project
- **Documentation source**: GitHub repository and API reference for exact version
- **Key areas**: Markdown parsing, extensions, HTML rendering, custom renderers

#### SkiaSharp (for DocToPdf.Customization)
- **Always check**: Version compatibility with .NET version used
- **Documentation source**: Official SkiaSharp documentation
- **Key areas**: Image processing, graphics rendering, font handling

#### System.Text.Json
- **Always check**: .NET version determines System.Text.Json capabilities
- **Documentation source**: Microsoft Learn for specific .NET version
- **Key areas**: JSON serialization/deserialization, custom converters

## 🏗️ Project-Specific Instructions

### DocToPdf Core Library
- **Target Framework**: .NET 8.0
- **Primary PDF Library**: QuestPDF (check exact version in csproj)
- **Service Pattern**: Use dependency injection with `IServiceCollection` extensions
- **Async Pattern**: All public APIs should be async with `CancellationToken` support
- **Logging**: Use `ILogger<T>` for all services
- **Error Handling**: Wrap operations in try-catch with meaningful exceptions

### Extension Packages Development
When developing DocToPdf extension packages:

#### DocToPdf.Customization
- **Dependencies**: SkiaSharp, System.Drawing.Common (check versions)
- **API Pattern**: Follow existing `IPdfCustomizationService` interface
- **Features**: Watermarks, headers/footers, themes, metadata
- **PDF Manipulation**: Use QuestPDF for document modification

#### DocToPdf.DataReports
- **Dependencies**: DocumentFormat.OpenXml, CsvHelper, Scriban, ScottPlot
- **Excel Processing**: Use OpenXml SDK (check version for API changes)
- **Template Engine**: Scriban for dynamic content generation
- **Charts**: ScottPlot for data visualization

#### DocToPdf.BatchProcessing
- **Dependencies**: System.Threading.Channels
- **Patterns**: Use `Parallel.ForEachAsync`, `IAsyncEnumerable<T>`, `IProgress<T>`
- **Memory Management**: Implement streaming for large batches
- **Cancellation**: Support `CancellationToken` throughout

#### DocToPdf.Security
- **Dependencies**: PdfSharp or alternative (check project choice)
- **Security Features**: Password protection, encryption, digital signatures
- **Certificates**: Use `System.Security.Cryptography.X509Certificates`

## 🔧 Code Standards

### API Design
- **Async Methods**: Always end with `Async` and return `Task<T>`
- **Cancellation**: Include `CancellationToken cancellationToken = default` parameter
- **Validation**: Validate inputs and throw meaningful exceptions
- **Documentation**: XML comments for all public APIs
- **Consistency**: Follow established patterns across all packages

### Dependency Injection
- **Registration**: Use extension methods like `AddDocToPdf[Feature]()`
- **Scoping**: Use appropriate service lifetime (Scoped for services, Singleton for configurations)
- **Options Pattern**: Use `IOptions<T>` for configuration
- **Interface Segregation**: Create focused interfaces for each feature

### Error Handling
```csharp
// Example pattern for service methods
public async Task<byte[]> ProcessAsync(byte[] input, CancellationToken cancellationToken = default)
{
    _logger.LogInformation("Starting process operation");
    
    try
    {
        // Validate input
        if (input == null || input.Length == 0)
            throw new ArgumentException("Input cannot be null or empty", nameof(input));
        
        // Process with cancellation support
        var result = await ProcessInternalAsync(input, cancellationToken);
        
        _logger.LogInformation("Process operation completed successfully");
        return result;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error during process operation");
        throw;
    }
}
```

## 📚 Documentation Requirements

### API Documentation
- **XML Comments**: All public classes, methods, properties
- **Examples**: Include usage examples in `<example>` tags
- **Exceptions**: Document all possible exceptions in `<exception>` tags
- **Remarks**: Add implementation notes in `<remarks>` tags

### README Files
- **Installation**: NuGet package installation instructions
- **Quick Start**: Basic usage examples
- **Configuration**: DI setup and options configuration
- **Advanced Usage**: Complex scenarios and customizations

## 🧪 Testing Guidelines

### Unit Tests
- **Coverage**: Aim for >80% code coverage
- **Naming**: `MethodName_Scenario_ExpectedResult` pattern
- **Async Testing**: Use proper async test patterns
- **Mocking**: Mock external dependencies, test actual logic

### Integration Tests
- **Real Dependencies**: Test with actual PDF generation
- **File I/O**: Test file reading/writing scenarios
- **Performance**: Include performance benchmarks for batch operations

## 🚀 Version Management

### Package Versioning
- **Semantic Versioning**: Follow SemVer (Major.Minor.Patch)
- **Breaking Changes**: Increment major version
- **New Features**: Increment minor version
- **Bug Fixes**: Increment patch version

### Dependency Updates
- **Check Compatibility**: Always verify API compatibility when updating
- **Test Thoroughly**: Run full test suite after dependency updates
- **Document Changes**: Note any API changes in release notes

## 💡 Best Practices

### Performance
- **Async/Await**: Use ConfigureAwait(false) in library code
- **Memory Management**: Dispose resources properly
- **Streaming**: Use streaming for large files
- **Caching**: Implement caching where appropriate

### Security
- **Input Validation**: Sanitize all user inputs
- **Exception Handling**: Don't leak sensitive information in exceptions
- **Dependencies**: Keep dependencies updated for security patches

### Maintainability
- **SOLID Principles**: Follow SOLID design principles
- **Clean Code**: Write self-documenting code
- **Separation of Concerns**: Keep features modular and independent
- **Consistent Patterns**: Use established patterns across the codebase

---

**Remember**: Always consult the official documentation for the exact versions used in this project before implementing any features or suggesting code changes. API compatibility is crucial for a stable, professional library.
