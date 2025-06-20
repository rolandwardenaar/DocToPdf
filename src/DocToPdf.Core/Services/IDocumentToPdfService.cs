using DocToPdf.Core.Models;

namespace DocToPdf.Core.Services;

/// <summary>
/// Service interface for converting documents to PDF
/// </summary>
public interface IDocumentToPdfService
{
    /// <summary>
    /// Convert HTML content to PDF
    /// </summary>
    /// <param name="htmlContent">HTML content to convert</param>
    /// <param name="title">PDF document title</param>
    /// <param name="basePath">Base path for resolving relative image paths</param>
    /// <returns>PDF content as byte array</returns>
    Task<byte[]> ConvertHtmlToPdfAsync(string htmlContent, string title = "Generated PDF", string? basePath = null);

    /// <summary>
    /// Convert HTML content to PDF and save to file
    /// </summary>
    /// <param name="htmlContent">HTML content to convert</param>
    /// <param name="outputPath">Output file path</param>
    /// <param name="title">PDF document title</param>
    /// <param name="basePath">Base path for resolving relative image paths</param>
    Task ConvertHtmlToPdfAsync(string htmlContent, string outputPath, string title = "Generated PDF", string? basePath = null);

    /// <summary>
    /// Convert Markdown content to PDF
    /// </summary>
    /// <param name="markdownContent">Markdown content to convert</param>
    /// <param name="title">PDF document title</param>
    /// <param name="basePath">Base path for resolving relative image paths</param>
    /// <returns>PDF content as byte array</returns>
    Task<byte[]> ConvertMarkdownToPdfAsync(string markdownContent, string title = "Generated PDF", string? basePath = null);

    /// <summary>
    /// Convert Markdown content to PDF and save to file
    /// </summary>
    /// <param name="markdownContent">Markdown content to convert</param>
    /// <param name="outputPath">Output file path</param>
    /// <param name="title">PDF document title</param>
    /// <param name="basePath">Base path for resolving relative image paths</param>
    Task ConvertMarkdownToPdfAsync(string markdownContent, string outputPath, string title = "Generated PDF", string? basePath = null);

    /// <summary>
    /// Convert plain text to PDF
    /// </summary>
    /// <param name="textContent">Plain text content to convert</param>
    /// <param name="title">PDF document title</param>
    /// <returns>PDF content as byte array</returns>
    Task<byte[]> ConvertTextToPdfAsync(string textContent, string title = "Generated PDF");

    /// <summary>
    /// Convert plain text to PDF and save to file
    /// </summary>
    /// <param name="textContent">Plain text content to convert</param>
    /// <param name="outputPath">Output file path</param>
    /// <param name="title">PDF document title</param>
    Task ConvertTextToPdfAsync(string textContent, string outputPath, string title = "Generated PDF");

    /// <summary>
    /// Convert DOCX file to PDF
    /// </summary>
    /// <param name="docxFilePath">Path to DOCX file</param>
    /// <param name="title">PDF document title</param>
    /// <returns>PDF content as byte array</returns>
    Task<byte[]> ConvertDocxToPdfAsync(string docxFilePath, string? title = null);

    /// <summary>
    /// Convert DOCX file to PDF and save to file
    /// </summary>
    /// <param name="docxFilePath">Path to DOCX file</param>
    /// <param name="outputPath">Output file path</param>
    /// <param name="title">PDF document title</param>
    Task ConvertDocxToPdfAsync(string docxFilePath, string outputPath, string? title = null);
}
