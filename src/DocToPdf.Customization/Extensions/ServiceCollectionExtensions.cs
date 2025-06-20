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
