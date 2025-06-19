using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using HtmlAgilityPack;
using DocToPdf.Converters;

namespace DocToPdf.Models;

public class PdfDocument : IDocument
{
    private readonly string _htmlContent;
    private readonly Dictionary<string, byte[]> _images;
    private readonly string _basePath;
    private readonly string _title;

    public PdfDocument(string htmlContent, Dictionary<string, byte[]> images, string basePath, string title = "Generated PDF")
    {
        _htmlContent = htmlContent;
        _images = images;
        _basePath = basePath;
        _title = title;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily(Fonts.TimesNewRoman));

                page.Header()
                    .Text(_title)
                    .FontSize(16)
                    .Bold()
                    .AlignCenter();

                page.Content()
                    .PaddingVertical(20)
                    .Column(column =>
                    {
                        column.Spacing(10);
                        var doc = new HtmlDocument();
                        doc.LoadHtml(_htmlContent);
                        var bodyNodes = doc.DocumentNode.SelectNodes("//body")?.FirstOrDefault()?.ChildNodes
                                        ?? doc.DocumentNode.ChildNodes;
                        
                        foreach (var node in bodyNodes)
                        {
                            AddHtmlNodeToColumn(node, column);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
    }

    private void AddHtmlNodeToColumn(HtmlNode node, ColumnDescriptor column)
    {
        if (node.NodeType == HtmlNodeType.Text)
        {
            var text = node.InnerText.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                column.Item().Text(text);
            }
        }
        else if (node.NodeType == HtmlNodeType.Element)
        {
            switch (node.Name.ToLower())
            {
                case "h1":
                    column.Item().Text(node.InnerText).FontSize(20).Bold();
                    break;
                case "h2":
                    column.Item().Text(node.InnerText).FontSize(16).Bold();
                    break;
                case "h3":
                    column.Item().Text(node.InnerText).FontSize(14).Bold();
                    break;
                case "p":
                    column.Item().Text(node.InnerText).FontSize(12);
                    break;
                case "ul":
                case "ol":
                    foreach (var li in node.SelectNodes("li") ?? Enumerable.Empty<HtmlNode>())
                    {
                        column.Item().Element(item => item.PaddingLeft(20).Text($"• {li.InnerText}").FontSize(12));
                    }
                    break;
                case "table":
                    ProcessTable(node, column);
                    break;
                case "img":
                    ProcessImage(node, column);
                    break;
                case "div":
                case "span":
                case "body":
                    // Process children for container elements
                    foreach (var child in node.ChildNodes)
                    {
                        AddHtmlNodeToColumn(child, column);
                    }
                    break;
                default:
                    // For unknown elements, try to process children
                    if (node.HasChildNodes)
                    {
                        foreach (var child in node.ChildNodes)
                        {
                            AddHtmlNodeToColumn(child, column);
                        }
                    }
                    break;
            }
        }
    }

    private void ProcessTable(HtmlNode node, ColumnDescriptor column)
    {
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                var firstRow = node.SelectNodes(".//tr[1]/td | .//tr[1]/th");
                if (firstRow != null)
                {
                    foreach (var _ in firstRow)
                    {
                        columns.RelativeColumn();
                    }
                }
            });

            foreach (var row in node.SelectNodes(".//tr") ?? Enumerable.Empty<HtmlNode>())
            {
                foreach (var cell in row.SelectNodes("td | th") ?? Enumerable.Empty<HtmlNode>())
                {
                    var cellElement = table.Cell().Padding(5);
                    if (cell.Name.ToLower() == "th")
                    {
                        cellElement.Text(cell.InnerText).Bold();
                    }
                    else
                    {
                        cellElement.Text(cell.InnerText);
                    }
                }
            }
        });
    }

    private void ProcessImage(HtmlNode node, ColumnDescriptor column)
    {
        var src = node.GetAttributeValue("src", "");
        byte[]? imageData = null;

        if (_images.ContainsKey(src))
        {
            // Afbeelding uit DOCX
            imageData = _images[src];
        }
        else if (!string.IsNullOrEmpty(src))
        {
            // Lokale afbeelding uit HTML
            string imagePath = Path.Combine(_basePath, src);
            if (File.Exists(imagePath))
            {
                // Gebruik ImageConverter voor alle formaten
                imageData = ImageConverter.ConvertToSupportedFormat(imagePath);
            }
        }        if (imageData != null)
        {
            try
            {
                column.Item().Image(imageData).FitWidth();
            }            catch (Exception ex)            {
                Console.WriteLine($"Waarschuwing: Kan afbeelding niet toevoegen aan PDF - {ex.Message}");
            }
        }
    }    /// <summary>
    /// Genereer PDF bestand naar het opgegeven pad
    /// </summary>
    /// <param name="outputPath">Pad waar het PDF bestand opgeslagen moet worden</param>
    public void GeneratePdf(string outputPath)
    {
        Document.Create(container => this.Compose(container)).GeneratePdf(outputPath);
    }
}
