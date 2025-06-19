# Clean script voor DocToPdf project
# Ruimt build artifacts op

Write-Host "Cleaning DocToPdf project..." -ForegroundColor Yellow

# Clean dotnet project
dotnet clean

# Verwijder bin en obj directories
if (Test-Path "bin") {
    Remove-Item "bin" -Recurse -Force
    Write-Host "Removed bin directory" -ForegroundColor Green
}

if (Test-Path "obj") {
    Remove-Item "obj" -Recurse -Force  
    Write-Host "Removed obj directory" -ForegroundColor Green
}

# Clean output directory (behoud de directory zelf)
if (Test-Path "output") {
    Get-ChildItem "output" | Remove-Item -Recurse -Force
    Write-Host "Cleaned output directory" -ForegroundColor Green
}

Write-Host "Clean completed!" -ForegroundColor Green
