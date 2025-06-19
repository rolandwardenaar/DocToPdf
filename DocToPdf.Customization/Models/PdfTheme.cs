namespace DocToPdf.Customization.Models;

public enum PdfTheme
{
    Default,
    Corporate,
    Modern,
    Minimal,
    Academic,
    Creative
}

public class ThemeDefinition
{
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#000000";
    public string SecondaryColor { get; set; } = "#666666";
    public string AccentColor { get; set; } = "#0066CC";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string FontFamily { get; set; } = "Arial";
    public float DefaultFontSize { get; set; } = 12f;
    public float HeaderFontSize { get; set; } = 16f;
    public float LineSpacing { get; set; } = 1.2f;
    public float Margins { get; set; } = 25f;
}
