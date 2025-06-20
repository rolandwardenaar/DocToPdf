using DocToPdf.Core.Converters;
using DocToPdf.Core.Models;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DocToPdf.Core.Services;

/// <summary>
/// Service implementation for converting documents to PDF
/// </summary>
public class DocumentToPdfService : IDocumentToPdfService
{
    private readonly ILogger<DocumentToPdfService> _logger;

    static DocumentToPdfService()
    {
        // Set QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public DocumentToPdfService(ILogger<DocumentToPdfService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertHtmlToPdfAsync(string htmlContent, string title = "Generated PDF", string? basePath = null)
    {
        _logger.LogInformation("Converting HTML content to PDF with title: {Title}", title);

        try
        {
            var document = new PdfDocument(htmlContent, new Dictionary<string, byte[]>(), basePath ?? Environment.CurrentDirectory, title);
            var pdfBytes = document.GeneratePdf();
            
            _logger.LogInformation("Successfully converted HTML to PDF. Size: {Size} bytes", pdfBytes.Length);
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert HTML to PDF");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ConvertHtmlToPdfAsync(string htmlContent, string outputPath, string title = "Generated PDF", string? basePath = null)
    {
        _logger.LogInformation("Converting HTML content to PDF file: {OutputPath}", outputPath);

        try
        {
            var document = new PdfDocument(htmlContent, new Dictionary<string, byte[]>(), basePath ?? Environment.CurrentDirectory, title);
            document.GeneratePdf(outputPath);
            
            _logger.LogInformation("Successfully saved PDF to: {OutputPath}", outputPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert HTML to PDF file: {OutputPath}", outputPath);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertMarkdownToPdfAsync(string markdownContent, string title = "Generated PDF", string? basePath = null)
    {
        _logger.LogInformation("Converting Markdown content to PDF with title: {Title}", title);

        try
        {
            var htmlContent = await DocumentConverter.ConvertMarkdownToHtml(markdownContent);
            return await ConvertHtmlToPdfAsync(htmlContent, title, basePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert Markdown to PDF");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ConvertMarkdownToPdfAsync(string markdownContent, string outputPath, string title = "Generated PDF", string? basePath = null)
    {
        _logger.LogInformation("Converting Markdown content to PDF file: {OutputPath}", outputPath);

        try
        {
            var htmlContent = await DocumentConverter.ConvertMarkdownToHtml(markdownContent);
            await ConvertHtmlToPdfAsync(htmlContent, outputPath, title, basePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert Markdown to PDF file: {OutputPath}", outputPath);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertTextToPdfAsync(string textContent, string title = "Generated PDF")
    {
        _logger.LogInformation("Converting plain text content to PDF with title: {Title}", title);

        try
        {
            // Convert plain text to simple HTML
            var htmlContent = $@"<!DOCTYPE html>
<html>
<head>
    <title>{title}</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; margin: 40px; }}
        pre {{ white-space: pre-wrap; word-wrap: break-word; }}
    </style>
</head>
<body>
    <pre>{System.Net.WebUtility.HtmlEncode(textContent)}</pre>
</body>
</html>";

            return await ConvertHtmlToPdfAsync(htmlContent, title, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert text to PDF");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ConvertTextToPdfAsync(string textContent, string outputPath, string title = "Generated PDF")
    {
        _logger.LogInformation("Converting plain text content to PDF file: {OutputPath}", outputPath);

        try
        {
            var pdfBytes = await ConvertTextToPdfAsync(textContent, title);
            await File.WriteAllBytesAsync(outputPath, pdfBytes);
            
            _logger.LogInformation("Successfully saved text PDF to: {OutputPath}", outputPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert text to PDF file: {OutputPath}", outputPath);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<byte[]> ConvertDocxToPdfAsync(string docxFilePath, string? title = null)
    {
        _logger.LogInformation("Converting DOCX file to PDF: {FilePath}", docxFilePath);

        try
        {
            var fileName = Path.GetFileNameWithoutExtension(docxFilePath);
            var documentTitle = title ?? fileName;
            var basePath = Path.GetDirectoryName(docxFilePath) ?? Environment.CurrentDirectory;

            var (htmlContent, images) = DocumentConverter.ConvertDocxToHtml(docxFilePath);
            var document = new PdfDocument(htmlContent, images, basePath, documentTitle);
            var pdfBytes = document.GeneratePdf();
            
            _logger.LogInformation("Successfully converted DOCX to PDF. Size: {Size} bytes", pdfBytes.Length);
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert DOCX to PDF: {FilePath}", docxFilePath);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ConvertDocxToPdfAsync(string docxFilePath, string outputPath, string? title = null)
    {
        _logger.LogInformation("Converting DOCX file to PDF file: {FilePath} -> {OutputPath}", docxFilePath, outputPath);

        try
        {
            var pdfBytes = await ConvertDocxToPdfAsync(docxFilePath, title);
            await File.WriteAllBytesAsync(outputPath, pdfBytes);
            
            _logger.LogInformation("Successfully saved DOCX PDF to: {OutputPath}", outputPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert DOCX to PDF file: {FilePath} -> {OutputPath}", docxFilePath, outputPath);
            throw;
        }
    }
}
