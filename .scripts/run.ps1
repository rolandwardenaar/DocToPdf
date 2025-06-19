# Run script voor DocToPdf project
# Bouwt en voert het project uit met parameters

param(
    [Parameter(Mandatory=$true)]
    [string]$InputFile,
    
    [Parameter(Mandatory=$true)]
    [string]$OutputFile
)

Write-Host "Building and running DocToPdf..." -ForegroundColor Green

# Build first
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful! Running conversion..." -ForegroundColor Green
    
    # Check if input file exists
    if (!(Test-Path $InputFile)) {
        Write-Host "Error: Input file '$InputFile' not found!" -ForegroundColor Red
        exit 1
    }
    
    # Create output directory if it doesn't exist
    $outputDir = Split-Path $OutputFile -Parent
    if ($outputDir -and !(Test-Path $outputDir)) {
        New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
    }
    
    # Run the application
    dotnet run -- $InputFile $OutputFile
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Conversion completed successfully!" -ForegroundColor Green
        if (Test-Path $OutputFile) {
            Write-Host "Output file created: $OutputFile" -ForegroundColor Cyan
        }
    } else {
        Write-Host "Conversion failed!" -ForegroundColor Red
    }
} else {
    Write-Host "Build failed!" -ForegroundColor Red
}
