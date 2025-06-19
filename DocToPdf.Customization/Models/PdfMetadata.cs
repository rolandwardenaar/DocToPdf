namespace DocToPdf.Customization.Models;

public class PdfMetadata
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Subject { get; set; }
    public string? Keywords { get; set; }
    public string? Creator { get; set; } = "DocToPdf.Customization";
    public DateTime? CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }
}
