# SLauncher

**Modern app launcher for Windows** - Built with WinUI 3

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=.net)](https://dotnet.microsoft.com/)
[![WinUI 3](https://img.shields.io/badge/WinUI-3-0078D4?logo=windows)](https://microsoft.github.io/microsoft-ui-xaml/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

---

## ? Features

- ?? **Tab Support** - Organize your apps and shortcuts into multiple tabs
- ?? **Multi-Language** - English, Korean (и措橫), Japanese (擨塓槧)
- ?? **Real-time Language Switching** - No restart required
- ?? **Global Hotkey** - Quick access with Ctrl+Space
- ?? **System Tray** - Minimize to tray and quick actions
- ?? **Customizable Tabs** - Rename and color-code your tabs
- ??? **Icon Scaling** - Adjust icon sizes with Ctrl+Mouse Wheel
- ?? **Linked Folders** - Auto-sync folder contents
- ?? **Website Shortcuts** - Add websites with favicons
- ?? **Portable** - All settings in AppData folder

---

## ?? Quick Start

### Requirements
- Windows 10 version 1809 (build 17763) or later
- Windows 11 recommended
- .NET 8 Runtime (included in self-contained builds)

### Installation

1. **Download** the latest release from [Releases](../../releases)
2. **Extract** the ZIP file
3. **Run** `SLauncher.exe`

---

## ?? Documentation

Comprehensive documentation is available in the [`docs`](docs/) folder:

### Getting Started
- [README](docs/README.md) - Original README with detailed information
- [VS Build Guide](docs/VS-BUILD-GUIDE.md) - Building from source

### Features
- [Localization](docs/LOCALIZATION-README.md) - Multi-language support details
- [Tab Feature](docs/TAB-FEATURE-COMPLETE.md) - Tab management guide
- [Global Hotkey](docs/GLOBAL-HOTKEY-COMPLETE.md) - Hotkey configuration
- [System Tray](docs/SYSTEM-TRAY-IMPLEMENTATION.md) - Tray icon functionality

### Deployment
- [Deployment Guide](docs/DEPLOYMENT-GUIDE.md) - Distribution options
- [Portable Build](docs/PORTABLE-SELFCONTAINED-GUIDE.md) - Self-contained builds
- [Air-Gapped Deployment](docs/AIR-GAPPED-DEPLOYMENT.md) - Offline installation

### Development
- [MainWindow Refactoring](docs/MAINWINDOW-REFACTORING-COMPLETE.md) - Code structure
- [Bug Fixes](docs/) - Various bug fix documentation

---

## ?? Supported Languages

| Language | Code | Status |
|----------|------|--------|
| ???? English | en-US | ? Complete |
| ???? и措橫 | ko-KR | ? Complete |
| ???? 擨塓槧 | ja-JP | ? Complete |

Change language in **Settings** ⊥ **Language** (no restart required!)

---

## ?? Usage

### Adding Items

1. **Files**: Click "Add File" button or drag & drop
2. **Folders**: Click "Add Folder" button
3. **Websites**: Click "Add Website" button and enter URL

### Managing Tabs

- **New Tab**: Click the `+` button
- **Rename Tab**: Right-click tab ⊥ Rename
- **Change Color**: Right-click tab ⊥ Change Color
- **Delete Tab**: Right-click tab ⊥ Delete

### Keyboard Shortcuts

- `Ctrl+Space` - Show/Hide SLauncher (global hotkey)
- `Ctrl+Mouse Wheel` - Adjust icon size
- `Enter` in search - Launch selected item

---

## ??? Building from Source

### Prerequisites
- Visual Studio 2022 (17.8 or later)
- .NET 8 SDK
- Windows App SDK 1.6

### Build Steps

```powershell
# Clone repository
git clone https://github.com/yourusername/SLauncher.git
cd SLauncher

# Restore packages
dotnet restore

# Build
dotnet build -c Release
```

See [VS Build Guide](docs/VS-BUILD-GUIDE.md) for detailed instructions.

---

## ?? Project Structure

```
SLauncher/
戍式式 SLauncher/         # Main WinUI 3 application
弛 戍式式 Classes/         # Core logic and helpers
弛   戍式式 Controls/  # Custom UI controls
弛   戍式式 Strings/        # Localization resources
弛   戌式式 Resources/       # Images and icons
戍式式 WinFormsClassLibrary/      # WinForms interop (for file picker)
戍式式 Setup/        # Installer scripts
戌式式 docs/ # Documentation
```

---

## ?? Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Areas for Contribution
- ?? Translations (more languages)
- ?? Bug fixes
- ? New features
- ?? Documentation improvements

---

## ?? License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## ?? Acknowledgments

- **WinUI 3** - Modern Windows UI framework
- **CommunityToolkit** - WinUI extensions
- **WinUIEx** - Window management utilities
- **System.Drawing.Common** - Icon extraction

---

## ?? Contact

- **Issues**: [GitHub Issues](../../issues)
- **Discussions**: [GitHub Discussions](../../discussions)

---

**Made with ?? for Windows power users**

---

## ?? Version

**Current Version**: 2.1.2

See [RELEASE-NOTES-v2.1.2.md](docs/RELEASE-NOTES-v2.1.2.md) for changelog.
