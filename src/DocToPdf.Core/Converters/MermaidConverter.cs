using PuppeteerSharp;
using System.Text.RegularExpressions;

namespace DocToPdf.Core.Converters;

public static class MermaidConverter
{
    private static IBrowser? _browser;
    private static bool _browserInitialized = false;

    /// <summary>
    /// Converteer Mermaid diagram code naar PNG afbeelding
    /// </summary>
    /// <param name="mermaidCode">De Mermaid diagram code</param>
    /// <returns>Byte array van de PNG afbeelding, of null als conversie niet mogelijk is</returns>
    public static async Task<byte[]?> ConvertMermaidToPng(string mermaidCode)
    {
        try
        {
            await EnsureBrowserInitialized();
            
            if (_browser == null)
            {
                Console.WriteLine("Waarschuwing: Browser niet beschikbaar voor Mermaid conversie");
                return null;
            }

            using var page = await _browser.NewPageAsync();
            
            // HTML template met Mermaid
            string htmlContent = GenerateMermaidHtml(mermaidCode);
            
            await page.SetContentAsync(htmlContent);
            
            // Wacht tot Mermaid diagram is gerenderd
            await page.WaitForSelectorAsync("#mermaid-diagram", new WaitForSelectorOptions { Timeout = 10000 });
            
            // Krijg de afmetingen van het diagram
            var diagramElement = await page.QuerySelectorAsync("#mermaid-diagram");
            if (diagramElement == null)
            {
                Console.WriteLine("Waarschuwing: Mermaid diagram niet gevonden");
                return null;
            }

            // Screenshot maken van het diagram element
            var screenshot = await diagramElement.ScreenshotDataAsync(new ElementScreenshotOptions
            {
                Type = ScreenshotType.Png,
                OmitBackground = false
            });

            Console.WriteLine($"Mermaid diagram geconverteerd naar PNG ({screenshot.Length} bytes)");
            return screenshot;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij Mermaid conversie: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Verwerk Mermaid code blocks in Markdown tekst
    /// </summary>
    /// <param name="markdown">De originele Markdown tekst</param>
    /// <returns>Verwerkte Markdown met Mermaid diagrammen vervangen door afbeeldingen</returns>
    public static async Task<string> ProcessMermaidCodeBlocks(string markdown)
    {
        try
        {
            // Regex patroon voor Mermaid code blocks
            var mermaidPattern = @"```mermaid\s*\n(.*?)\n```";
            var regex = new Regex(mermaidPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            
            var matches = regex.Matches(markdown);
            if (matches.Count == 0)
            {
                return markdown; // Geen Mermaid diagrammen gevonden
            }

            Console.WriteLine($"Mermaid: {matches.Count} diagram(men) gevonden");
            string result = markdown;
            
            for (int i = matches.Count - 1; i >= 0; i--) // Achterwaarts om posities niet te verstoren
            {
                var match = matches[i];
                var mermaidCode = match.Groups[1].Value.Trim();
                
                // Converteer Mermaid naar PNG
                var imageData = await ConvertMermaidToPng(mermaidCode);
                
                if (imageData != null)
                {
                    // Sla de afbeelding op als tijdelijk bestand
                    var tempImagePath = Path.Combine(Path.GetTempPath(), $"mermaid_{Guid.NewGuid():N}.png");
                    await File.WriteAllBytesAsync(tempImagePath, imageData);
                    
                    // Vervang de Mermaid code block met een afbeelding reference
                    var imageMarkdown = $"![Mermaid Diagram]({tempImagePath})";
                    result = result.Remove(match.Index, match.Length).Insert(match.Index, imageMarkdown);
                }
                else
                {
                    // Als conversie faalt, vervang met een foutmelding
                    var errorMarkdown = $"*[Mermaid diagram kon niet worden gerenderd]*";
                    result = result.Remove(match.Index, match.Length).Insert(match.Index, errorMarkdown);
                }
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij verwerken van Mermaid diagrammen: {ex.Message}");
            return markdown; // Return originele markdown bij fout
        }
    }

    /// <summary>
    /// Genereer HTML template voor Mermaid rendering
    /// </summary>
    private static string GenerateMermaidHtml(string mermaidCode)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <script src=""https://cdn.jsdelivr.net/npm/mermaid@10.6.1/dist/mermaid.min.js""></script>
    <style>
        body {{ 
            margin: 0; 
            padding: 20px; 
            background: white; 
            font-family: Arial, sans-serif; 
        }}
        #mermaid-diagram {{ 
            background: white; 
            display: inline-block;
            border: 1px solid #e0e0e0;
            border-radius: 4px;
            padding: 10px;
        }}
    </style>
</head>
<body>
    <div id=""mermaid-diagram"">
        <pre class=""mermaid"">{mermaidCode}</pre>
    </div>
    <script>
        mermaid.initialize({{ 
            startOnLoad: true,
            theme: 'default',
            flowchart: {{ 
                htmlLabels: true,
                useMaxWidth: true 
            }},
            securityLevel: 'loose'
        }});
    </script>
</body>
</html>";
    }

    /// <summary>
    /// Initialiseer de browser voor Mermaid rendering
    /// </summary>
    private static async Task EnsureBrowserInitialized()
    {
        if (_browserInitialized) return;

        try
        {
            Console.WriteLine("Mermaid: Browser initialiseren...");
            
            await new BrowserFetcher().DownloadAsync();
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
            });
            
            _browserInitialized = true;
            Console.WriteLine("Mermaid: Browser geïnitialiseerd");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Waarschuwing: Kan browser niet initialiseren voor Mermaid: {ex.Message}");
            _browserInitialized = true; // Voorkom herhaalde pogingen
        }
    }

    /// <summary>
    /// Cleanup resources
    /// </summary>
    public static async Task DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
            _browser.Dispose();
            _browser = null;
        }
    }
}
