using Mammoth;
using Markdig;

namespace DocToPdf.Core.Converters;

public static class DocumentConverter
{
    /// <summary>
    /// Converteer Markdown naar HTML met Mermaid ondersteuning
    /// </summary>
    /// <param name="markdown">De Markdown inhoud</param>
    /// <returns>HTML inhoud</returns>
    public static async Task<string> ConvertMarkdownToHtml(string markdown)
    {
        // Eerst Mermaid code blocks verwerken
        var processedMarkdown = await MermaidConverter.ProcessMermaidCodeBlocks(markdown);
        
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return Markdown.ToHtml(processedMarkdown, pipeline);
    }

    /// <summary>
    /// Converteer DOCX naar HTML met afbeelding extractie
    /// </summary>
    /// <param name="filePath">Pad naar het DOCX bestand</param>
    /// <returns>Tuple van HTML inhoud en afbeelding dictionary</returns>
    public static (string Html, Dictionary<string, byte[]> Images) ConvertDocxToHtml(string filePath)
    {
        var images = new Dictionary<string, byte[]>();
        var converter = new Mammoth.DocumentConverter()
            .ImageConverter(img =>
            {
                string imageId = Guid.NewGuid().ToString();
                using var stream = img.GetStream();
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                images[imageId] = memoryStream.ToArray();
                return new Dictionary<string, string> { { "src", imageId } };
            });

        var result = converter.ConvertToHtml(filePath);
        return (result.Value, images);
    }
}
