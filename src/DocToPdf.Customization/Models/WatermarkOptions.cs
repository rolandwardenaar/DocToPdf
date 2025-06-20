using System.ComponentModel.DataAnnotations;

namespace DocToPdf.Customization.Models;

public class WatermarkOptions
{
    [Required]
    public string Text { get; set; } = string.Empty;
    
    public string? ImagePath { get; set; }
    
    [Range(0.1, 1.0)]
    public float Opacity { get; set; } = 0.3f;
    
    public WatermarkPosition Position { get; set; } = WatermarkPosition.Center;
    
    [Range(-180, 180)]
    public float Rotation { get; set; } = 45f;
    
    public string FontName { get; set; } = "Arial";
    public float FontSize { get; set; } = 48f;
    public string Color { get; set; } = "#CCCCCC";
}

public enum WatermarkPosition
{
    TopLeft,
    TopCenter,
    TopRight,
    MiddleLeft,
    Center,
    MiddleRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}
