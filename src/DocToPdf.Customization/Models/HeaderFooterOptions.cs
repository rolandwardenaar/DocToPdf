using System.ComponentModel.DataAnnotations;

namespace DocToPdf.Customization.Models;

public class HeaderFooterOptions
{
    [Required]
    public string Template { get; set; } = string.Empty; // "Page {page} of {totalPages}"
    
    public string? FontName { get; set; }
    public float FontSize { get; set; } = 10f;
    public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;
    public string Color { get; set; } = "#000000";
    public float MarginTop { get; set; } = 20f;
    public float MarginBottom { get; set; } = 20f;
}

public enum HorizontalAlignment
{
    Left,
    Center,
    Right
}
