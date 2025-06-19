using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Infrastructure;
using DocToPdf.Converters;
using DocToPdf.Models;

namespace DocToPdf
{
    public class Program
    {
        static Program()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static async Task Main(string[] args)
        {
            try
            {
                // Zorg ervoor dat input en output directories bestaan
                EnsureDirectories();
                
                if (args.Length == 0)
                {
                    // Geen parameters: converteer alle ondersteunde bestanden in de input directory
                    await ConvertAllSupportedFilesInDirectory();
                }                else if (args.Length == 1)
                {
                    // Enkele parameter: converteer het specifieke bestand uit input directory
                    var inputDir = Path.Combine(Directory.GetCurrentDirectory(), "input");
                    var fullPath = Path.Combine(inputDir, args[0]);
                    await ConvertSingleFile(fullPath);
                }
                else
                {
                    Console.WriteLine("Gebruik: DocToPdf [<bestandsnaam>]");
                    Console.WriteLine("  <bestandsnaam> - Specifiek bestand uit input/ directory");
                    Console.WriteLine("  (geen parameters) - Converteer alle bestanden uit input/ directory");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het genereren van PDF(s): {ex.Message}");
            }
            finally
            {
                // Cleanup Mermaid resources
                await MermaidConverter.DisposeAsync();
            }
        }

        private static void EnsureDirectories()
        {
            var inputDir = Path.Combine(Directory.GetCurrentDirectory(), "input");
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            
            if (!Directory.Exists(inputDir))
            {
                Directory.CreateDirectory(inputDir);
                Console.WriteLine("Input directory aangemaakt: input/");
            }
            
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
                Console.WriteLine("Output directory aangemaakt: output/");
            }
        }

        private static async Task ConvertAllSupportedFilesInDirectory()
        {
            string[] supportedExtensions = { ".md", ".html", ".docx" };
            var inputDir = Path.Combine(Directory.GetCurrentDirectory(), "input");
            
            var files = Directory
                .GetFiles(inputDir)
                .Where(file => supportedExtensions.Contains(Path.GetExtension(file).ToLower()))
                .ToList();

            if (!files.Any())
            {
                Console.WriteLine("Geen ondersteunde bestanden (.md, .html, .docx) gevonden in de input/ directory.");
                Console.WriteLine("Plaats bestanden in de input/ directory om ze te converteren.");
                return;
            }

            Console.WriteLine($"Gevonden {files.Count} bestand(en) in input/ directory:");
            foreach (var file in files)
            {
                Console.WriteLine($"  - {Path.GetFileName(file)}");
            }
            Console.WriteLine();

            foreach (var file in files)
            {
                await ConvertSingleFile(file);
            }
        }

        private static async Task ConvertSingleFile(string filePath)
        {
            // Controleer of het bestand in de input directory staat
            var inputDir = Path.Combine(Directory.GetCurrentDirectory(), "input");
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Bestand {filePath} bestaat niet.");
                return;
            }

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string outputPath = Path.Combine(outputDir, $"{fileName}.pdf");
            string extension = Path.GetExtension(filePath).ToLower();
            string basePath = Path.GetDirectoryName(filePath) ?? inputDir;

            string content;
            Dictionary<string, byte[]> images = new();

            switch (extension)
            {
                case ".md":
                    content = await DocumentConverter.ConvertMarkdownToHtml(File.ReadAllText(filePath));
                    break;
                case ".html":
                    content = File.ReadAllText(filePath);
                    break;
                case ".docx":
                    (content, images) = DocumentConverter.ConvertDocxToHtml(filePath);
                    break;
                default:
                    Console.WriteLine($"Bestand {filePath}: Ondersteunde formaten zijn .md, .html, .docx.");
                    return;
            }

            // Genereer PDF
            var document = new PdfDocument(content, images, basePath, fileName);
            document.GeneratePdf(outputPath);

            Console.WriteLine($"✓ PDF succesvol opgeslagen als: output/{fileName}.pdf");
        }
    }
}