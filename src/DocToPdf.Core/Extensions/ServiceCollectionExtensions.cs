using DocToPdf.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DocToPdf.Core.Extensions;

/// <summary>
/// Extension methods for configuring DocToPdf services in dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DocToPdf services to the specified IServiceCollection
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to</param>
    /// <returns>The IServiceCollection so that additional calls can be chained</returns>
    public static IServiceCollection AddDocToPdf(this IServiceCollection services)
    {
        services.TryAddScoped<IDocumentToPdfService, DocumentToPdfService>();
        return services;
    }

    /// <summary>
    /// Adds DocToPdf services to the specified IServiceCollection with custom configuration
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to</param>
    /// <param name="configure">An action to configure the DocToPdf options</param>
    /// <returns>The IServiceCollection so that additional calls can be chained</returns>
    public static IServiceCollection AddDocToPdf(this IServiceCollection services, Action<DocToPdfOptions> configure)
    {
        services.Configure(configure);
        services.TryAddScoped<IDocumentToPdfService, DocumentToPdfService>();
        return services;
    }
}

/// <summary>
/// Options for configuring DocToPdf services
/// </summary>
public class DocToPdfOptions
{
    /// <summary>
    /// Default output directory for PDF files
    /// </summary>
    public string DefaultOutputDirectory { get; set; } = "output";

    /// <summary>
    /// Default input directory for source files
    /// </summary>
    public string DefaultInputDirectory { get; set; } = "input";

    /// <summary>
    /// Enable Mermaid diagram processing
    /// </summary>
    public bool EnableMermaidDiagrams { get; set; } = true;

    /// <summary>
    /// Maximum image size in bytes (default: 10MB)
    /// </summary>
    public long MaxImageSizeBytes { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    /// Default PDF page size
    /// </summary>
    public string DefaultPageSize { get; set; } = "A4";

    /// <summary>
    /// Default PDF margins in points
    /// </summary>
    public float DefaultMargin { get; set; } = 50;
}
