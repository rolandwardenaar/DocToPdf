using SkiaSharp;
using Svg.Skia;

namespace DocToPdf.Converters;

public static class ImageConverter
{
    /// <summary>
    /// Converteer een afbeelding naar PNG formaat als het niet wordt ondersteund
    /// </summary>
    /// <param name="imagePath">Pad naar de afbeelding</param>
    /// <returns>Byte array van de geconverteerde PNG afbeelding, of null als conversie niet mogelijk is</returns>
    public static byte[]? ConvertToSupportedFormat(string imagePath)
    {
        if (!File.Exists(imagePath))
            return null;

        string extension = Path.GetExtension(imagePath).ToLower();
        
        try
        {
            switch (extension)
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    // Al ondersteund, gewoon teruggeven
                    return File.ReadAllBytes(imagePath);
                
                case ".svg":
                    return ConvertSvgToPng(imagePath);
                
                case ".gif":
                case ".bmp":
                case ".tiff":
                case ".webp":
                    return ConvertImageToPng(imagePath);
                
                default:
                    Console.WriteLine($"Waarschuwing: Onbekend afbeelding formaat {extension}: {imagePath}");
                    return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij conversie van {imagePath}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Converteer SVG naar PNG met SkiaSharp
    /// </summary>
    private static byte[]? ConvertSvgToPng(string svgPath)
    {
        try
        {
            var svg = new SKSvg();
            var picture = svg.Load(svgPath);
            
            if (picture == null)
            {
                Console.WriteLine($"Waarschuwing: Kan SVG bestand niet laden: {svgPath}");
                return null;
            }

            var bounds = picture.CullRect;
            int width = (int)Math.Ceiling(bounds.Width);
            int height = (int)Math.Ceiling(bounds.Height);
            
            // Minimale afmetingen voor leesbaarheid
            if (width < 100) width = 200;
            if (height < 100) height = 150;
            
            // Maximale afmetingen om geheugen te beperken
            if (width > 2000) width = 2000;
            if (height > 2000) height = 2000;

            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            using var canvas = surface.Canvas;
            
            canvas.Clear(SKColors.White); // Witte achtergrond voor betere zichtbaarheid
            
            // Schaal SVG naar gewenste afmetingen
            if (bounds.Width > 0 && bounds.Height > 0)
            {
                float scaleX = width / bounds.Width;
                float scaleY = height / bounds.Height;
                float scale = Math.Min(scaleX, scaleY);
                
                canvas.Scale(scale);
            }
            
            canvas.DrawPicture(picture);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 90);
            
            Console.WriteLine($"SVG geconverteerd naar PNG: {svgPath} ({width}x{height})");
            return data.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij SVG conversie van {svgPath}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Converteer andere afbeelding formaten naar PNG met SkiaSharp
    /// </summary>
    private static byte[]? ConvertImageToPng(string imagePath)
    {
        try
        {
            using var inputStream = File.OpenRead(imagePath);
            using var inputBitmap = SKBitmap.Decode(inputStream);
            
            if (inputBitmap == null)
            {
                Console.WriteLine($"Waarschuwing: Kan afbeelding niet decoderen: {imagePath}");
                return null;
            }

            using var image = SKImage.FromBitmap(inputBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 90);
            
            string extension = Path.GetExtension(imagePath).ToUpper();
            Console.WriteLine($"{extension} geconverteerd naar PNG: {imagePath}");
            return data.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij afbeelding conversie van {imagePath}: {ex.Message}");
            return null;
        }
    }
}
