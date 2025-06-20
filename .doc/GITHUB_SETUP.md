# GitHub Setup Instructies voor DocToPdf

## 🚀 Stap-voor-stap GitHub Repository Setup

### 1. Maak een nieuwe GitHub repository aan

1. Ga naar [GitHub.com](https://github.com) en log in
2. Klik op de **"+"** knop rechts boven → **"New repository"**
3. Vul de repository details in:
   - **Repository name**: `DocToPdf`
   - **Description**: `Professional .NET console application for converting documents (Markdown, HTML, DOCX) to PDF with Mermaid diagram support`
   - **Visibility**: ✅ Public (voor open source)
   - **❌ NIET** "Add a README file" aanvinken (we hebben al een README)
   - **❌ NIET** ".gitignore" aanvinken (we hebben al een .gitignore)
   - **❌ NIET** "Choose a license" aanvinken (we hebben al MIT license)

### 2. Verbind je lokale repository met GitHub

Kopieer de commands die GitHub toont na het aanmaken van de repository. Het ziet er ongeveer zo uit:

```bash
git remote add origin https://github.com/YOUR_USERNAME/DocToPdf.git
git branch -M main
git push -u origin main
```

**OF** gebruik deze commands (vervang YOUR_USERNAME):

```bash
# Voeg GitHub remote toe
git remote add origin https://github.com/YOUR_USERNAME/DocToPdf.git

# Hernoem branch naar main (GitHub standaard)
git branch -M main

# Push alle commits en tags naar GitHub
git push -u origin main
git push origin v1.0.0
```

### 3. Repository Configuratie

Na het pushen, configureer je repository:

#### **Topics/Tags toevoegen:**
1. Ga naar je repository op GitHub
2. Klik op het ⚙️ tandwiel icoon naast "About"
3. Voeg deze topics toe:
   - `pdf-converter`
   - `document-conversion`
   - `markdown-to-pdf`
   - `docx-to-pdf`
   - `mermaid-diagrams`
   - `questpdf`
   - `dotnet`
   - `csharp`
   - `cli-tool`

#### **Repository Description:**
```
Professional .NET console application for converting documents (Markdown, HTML, DOCX) to PDF with Mermaid diagram support
```

#### **Website (optioneel):**
Link naar documentatie of demo site

### 4. GitHub Actions activeren

1. Ga naar **Actions** tab in je repository
2. GitHub zal automatisch de workflows detecteren die we hebben toegevoegd
3. Klik **"I understand my workflows, go ahead and enable them"**

### 5. Release maken (optioneel)

1. Ga naar **Releases** in je repository
2. Klik **"Create a new release"**
3. Kies tag **v1.0.0**
4. Release title: **"DocToPdf v1.0.0 - Initial Release"**
5. Beschrijving:
```markdown
## 🎉 First Stable Release

DocToPdf v1.0.0 brings professional document to PDF conversion to the .NET ecosystem!

### ✨ Features
- **Document Conversion**: Markdown (.md), HTML (.html), and DOCX (.docx) to PDF
- **Mermaid Diagrams**: Automatic diagram rendering with PuppeteerSharp  
- **Professional PDF**: High-quality output with QuestPDF
- **Image Support**: Multi-format image processing and optimization
- **CLI Interface**: Easy command-line usage for single files or bulk conversion

### 🛠️ Technical Highlights
- Built on .NET 8.0
- Clean, modular architecture
- Comprehensive error handling
- Extensible design for future formats

### 📦 Installation
```bash
git clone https://github.com/YOUR_USERNAME/DocToPdf.git
cd DocToPdf
dotnet restore
dotnet build
```

### 🏃‍♂️ Quick Start
```bash
# Convert a single file
dotnet run -- document.md

# Convert all files in input/ directory
dotnet run
```

See the [README.md](README.md) for complete documentation.
```

### 6. Repository Settings (optioneel maar aanbevolen)

#### **Security:**
- Ga naar **Settings** → **Security**
- Enable **Dependency graph**
- Enable **Dependabot alerts**
- Enable **Dependabot security updates**

#### **Collaborations:**
- Ga naar **Settings** → **Manage access**
- Configureer wie kan bijdragen aan het project

#### **Branch Protection (voor teams):**
- Ga naar **Settings** → **Branches**
- Add rule voor `main` branch
- Require pull request reviews
- Require status checks (GitHub Actions)

## 🎯 Resultaat

Na deze stappen heb je:

✅ **Professional GitHub Repository**
✅ **Automated CI/CD** met GitHub Actions  
✅ **Complete Documentation**
✅ **Tagged Release** (v1.0.0)
✅ **Open Source** (MIT License)
✅ **Discoverable** via topics en beschrijving

## 📋 Commands Summary

```bash
# 1. Setup remote
git remote add origin https://github.com/YOUR_USERNAME/DocToPdf.git

# 2. Push to GitHub  
git branch -M main
git push -u origin main
git push origin v1.0.0

# 3. Verify everything works
git status
git log --oneline
```

## 🔗 Nuttige Links

- [GitHub CLI](https://cli.github.com/) - Voor command-line GitHub management
- [GitHub Desktop](https://desktop.github.com/) - Voor GUI-based Git management  
- [Shields.io](https://shields.io/) - Voor badges in je README
- [GitHub Pages](https://pages.github.com/) - Voor project websites

---

**🎉 Gefeliciteerd!** Je DocToPdf project is nu professioneel gepubliceerd op GitHub!
