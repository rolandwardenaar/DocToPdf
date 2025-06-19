# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial project setup with professional structure
- Document conversion support for Markdown (.md), HTML (.html), and Word (.docx) files
- PDF generation using QuestPDF library
- Mermaid diagram rendering with PuppeteerSharp
- Image processing and optimization for multiple formats (JPG, PNG, BMP, GIF, TIFF, WebP)
- Automatic input/output directory management
- Command-line interface for single file and bulk conversion
- Professional PDF layout with headers, footers, and page numbering
- Helper PowerShell scripts for testing, cleaning, and running
- Comprehensive error handling and logging
- GitHub Actions workflows for CI/CD
- Complete documentation (README, CONTRIBUTING, CHANGELOG)
- MIT license

### Technical Details
- Target framework: .NET 8.0
- Dependencies: QuestPDF, HtmlAgilityPack, DocumentFormat.OpenXml, PuppeteerSharp, SixLabors.ImageSharp, Markdig
- Project structure optimized for maintainability and extensibility
- Modular design with separate converter classes

## [1.0.0] - 2024-12-19

### Added
- Initial release of DocToPdf
- Core functionality for document to PDF conversion
- Support for Markdown, HTML, and DOCX input formats
- Mermaid diagram integration
- Professional PDF output with QuestPDF
- Command-line interface
- Comprehensive documentation

### Technical Implementation
- Clean architecture with separation of concerns
- Extensible converter pattern for different document types
- Robust error handling throughout the conversion pipeline
- Efficient image processing and optimization
- Professional PDF styling and layout

### Infrastructure
- GitHub repository setup
- MIT license
- Comprehensive README with usage examples
- Contributing guidelines
- Automated testing setup (foundation)
- CI/CD pipeline with GitHub Actions

---

## Development Notes

### Version History
- **v1.0.0**: Initial stable release with core functionality
- **Future versions**: Will follow semantic versioning based on feature additions, improvements, and breaking changes

### Release Process
1. Update CHANGELOG.md with new version details
2. Update version in DocToPdf.csproj
3. Create git tag with version number
4. Push to GitHub and create release
5. GitHub Actions will automatically build and publish

### Breaking Changes Policy
- Major version (X.0.0): Breaking changes that require code modifications
- Minor version (X.Y.0): New features, backward compatible
- Patch version (X.Y.Z): Bug fixes, backward compatible
