namespace DocToPdf.Customization.Models;

public class PdfCustomizationOptions
{
    public WatermarkOptions? Watermark { get; set; }
    public HeaderFooterOptions? Header { get; set; }
    public HeaderFooterOptions? Footer { get; set; }
    public PdfTheme Theme { get; set; } = PdfTheme.Default;
    public PdfMetadata? Metadata { get; set; }
    public FontOptions? Fonts { get; set; }
}

public class FontOptions
{
    public string DefaultFont { get; set; } = "Arial";
    public Dictionary<string, string> FontMappings { get; set; } = new();
    public List<string> EmbeddedFonts { get; set; } = new();
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}
