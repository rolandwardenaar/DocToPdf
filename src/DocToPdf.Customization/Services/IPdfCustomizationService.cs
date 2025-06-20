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
