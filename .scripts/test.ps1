# Test script voor DocToPdf
# Bouwt het project en test het met een eenvoudige HTML file

Write-Host "Building DocToPdf project..." -ForegroundColor Green
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful!" -ForegroundColor Green
    
    # Maak een test HTML file
    $testHtml = @"
<!DOCTYPE html>
<html>
<head>
    <title>Test Document</title>
</head>
<body>
    <h1>Test Document</h1>
    <p>Dit is een test document om DocToPdf te testen.</p>
    <h2>Features</h2>
    <ul>
        <li>HTML naar PDF conversie</li>
        <li>DOCX naar PDF conversie</li>
        <li>Mermaid diagram ondersteuning</li>
        <li>Afbeelding verwerking</li>
    </ul>
</body>
</html>
"@

    $testHtml | Out-File -FilePath "input\test.html" -Encoding UTF8
    
    Write-Host "Created test HTML file in input directory" -ForegroundColor Yellow
    Write-Host "You can now run: dotnet run input\test.html output\test.pdf" -ForegroundColor Cyan
} else {
    Write-Host "Build failed!" -ForegroundColor Red
}
