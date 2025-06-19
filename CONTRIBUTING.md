# Contributing to DocToPdf

We love your input! We want to make contributing to DocToPdf as easy and transparent as possible, whether it's:

- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features
- Becoming a maintainer

## Development Process

We use GitHub to host code, to track issues and feature requests, as well as accept pull requests.

### Pull Requests

Pull requests are the best way to propose changes to the codebase. We actively welcome your pull requests:

1. **Fork** the repo and create your branch from `main`
2. **Add tests** if you've added code that should be tested
3. **Ensure the test suite passes** by running `dotnet test`
4. **Update documentation** if you've changed APIs or added features
5. **Make sure your code follows** the existing style conventions
6. **Issue that pull request**!

### Code Style

- Follow standard C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and single-purpose
- Use `async`/`await` for I/O operations

Example:
```csharp
/// <summary>
/// Converts a Markdown document to HTML with Mermaid diagram support
/// </summary>
/// <param name="markdownContent">The Markdown content to convert</param>
/// <returns>HTML content with rendered Mermaid diagrams</returns>
public static async Task<string> ConvertMarkdownToHtml(string markdownContent)
{
    // Implementation...
}
```

### Commit Messages

- Use the present tense ("Add feature" not "Added feature")
- Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit the first line to 72 characters or less
- Reference issues and pull requests liberally after the first line

Examples:
```
Add support for SVG image formats

- Implement SVG to PNG conversion using ImageSharp
- Update ImageConverter to handle SVG files
- Add tests for SVG conversion functionality

Fixes #123
```

## Bug Reports

We use GitHub issues to track public bugs. Report a bug by [opening a new issue](https://github.com/YOUR_USERNAME/DocToPdf/issues/new?template=bug_report.md).

**Great Bug Reports** tend to have:

- A quick summary and/or background
- Steps to reproduce
  - Be specific!
  - Give sample code if you can
- What you expected would happen
- What actually happens
- Notes (possibly including why you think this might be happening, or stuff you tried that didn't work)

### Bug Report Template

```markdown
## Bug Description
A clear and concise description of what the bug is.

## To Reproduce
Steps to reproduce the behavior:
1. Go to '...'
2. Click on '....'
3. Scroll down to '....'
4. See error

## Expected Behavior
A clear and concise description of what you expected to happen.

## Screenshots/Logs
If applicable, add screenshots or error logs to help explain your problem.

## Environment
- OS: [e.g., Windows 10, macOS 12.0, Ubuntu 20.04]
- .NET Version: [e.g., .NET 8.0]
- DocToPdf Version: [e.g., 1.0.0]

## Additional Context
Add any other context about the problem here.
```

## Feature Requests

We love feature requests! They help us understand what you need and improve the project. Please:

1. **Search existing issues** first to avoid duplicates
2. **Provide context** about your use case
3. **Explain the problem** you're trying to solve
4. **Suggest a solution** if you have ideas

### Feature Request Template

```markdown
## Feature Description
A clear and concise description of the feature you'd like to see.

## Problem/Use Case
Describe the problem this feature would solve or the use case it addresses.

## Proposed Solution
Describe the solution you'd like to see implemented.

## Alternatives Considered
Describe any alternative solutions or features you've considered.

## Additional Context
Add any other context, screenshots, or examples about the feature request.
```

## Development Setup

### Prerequisites

- .NET 8.0 SDK or later
- Git
- Your favorite C# IDE (Visual Studio, VS Code, Rider)

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/DocToPdf.git
   cd DocToPdf
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run tests**
   ```bash
   dotnet test
   ```

5. **Run the application**
   ```bash
   dotnet run -- test.html  # Convert a test file
   ```

### Project Structure

```
DocToPdf/
├── Program.cs              # Entry point and CLI handling
├── DocumentConverter.cs    # Core document conversion logic
├── ImageConverter.cs       # Image processing and optimization
├── MermaidConverter.cs     # Mermaid diagram rendering
├── PdfDocument.cs          # PDF generation with QuestPDF
├── DocToPdf.csproj        # Project configuration
├── .scripts/              # Helper PowerShell scripts
│   ├── test.ps1           # Testing script
│   ├── clean.ps1          # Cleanup script
│   └── run.ps1            # Build and run script
├── input/                 # Sample input files for testing
├── output/                # Generated PDF output
└── docs/                  # Additional documentation
```

### Adding New Features

1. **Document converters**: Add new document type support in `DocumentConverter.cs`
2. **Image formats**: Extend `ImageConverter.cs` for new image formats
3. **PDF styling**: Modify `PdfDocument.cs` for layout changes
4. **CLI options**: Update `Program.cs` for new command-line features

### Testing

We encourage comprehensive testing:

- **Unit tests** for individual converter methods
- **Integration tests** for end-to-end conversion flows
- **Sample files** in the `input/` directory for manual testing

Add test files to appropriate directories and ensure they pass before submitting PR.

### Documentation

- Update `README.md` for user-facing changes
- Update `CHANGELOG.md` following semantic versioning
- Add XML documentation for new public APIs
- Update inline comments for complex logic

## License

By contributing, you agree that your contributions will be licensed under the same MIT License that covers the project. Feel free to contact the maintainers if that's a concern.

## Questions?

Don't hesitate to ask questions! You can:

- [Open an issue](https://github.com/YOUR_USERNAME/DocToPdf/issues/new)
- [Start a discussion](https://github.com/YOUR_USERNAME/DocToPdf/discussions)
- Contact the maintainers directly

## Recognition

Contributors will be recognized in:
- The project README
- Release notes
- Special thanks in documentation

Thank you for contributing to DocToPdf! 🎉
