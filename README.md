# SLauncher

<div align="center">

**A modern, multi-language app launcher for Windows**

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)
[![WinUI 3](https://img.shields.io/badge/WinUI-3-0078D4)](https://microsoft.github.io/microsoft-ui-xaml/)

**[English](README.md)** | **[�ѱ���](README.ko-KR.md)** | **[������](README.ja-JP.md)**

[Features](#-features) ? [Installation](#-installation) ? [Usage](#-usage) ? [Building](#-building-from-source) ? [Contributing](#-contributing)

</div>

---

## ?? Features

### Core Functionality
- ? **Quick Launch** - Access your favorite apps, folders, and websites instantly
- ?? **Global Hotkey** - Show/hide launcher from anywhere (default: `Ctrl + Space`)
- ?? **Tab Organization** - Organize items into multiple tabs with custom names and colors
- ??? **Drag & Drop** - Easy item management with intuitive drag-and-drop
- ?? **Smart Search** - Find items or directly open files/folders/URLs from search
- ?? **Icon Scaling** - Adjust icon size with slider or `Ctrl + Mouse Wheel` (0.25x - 6.00x)

### Multi-Language Support
- ???? **English**
- ???? **�ѱ���** (Korean)
- ???? **������** (Japanese)
- ? Real-time language switching - No restart required!

### Modern UI
- ?? **Windows 11 Design** - Native WinUI 3 with Mica/Acrylic effects
- ?? **Theme Support** - Automatically follows system dark/light theme
- ?? **Fullscreen Mode** - Immersive launcher experience
- ?? **Grid Alignment** - Choose between Left or Center alignment
- ?? **Tab Colors** - 8 preset colors for tab customization

### Performance & Portability
- ?? **Portable** - All data stored in executable folder (`UserCache/`)
- ?? **Fast Startup** - Cached data for instant loading
- ?? **Favicon Cache** - Website icons cached locally
- ?? **Lightweight** - Minimal resource usage

---

## ?? Installation

### Requirements
- **Windows 10** version 1809 (Build 17763) or later
- **Windows 11** (recommended for best experience)
- **.NET 8.0 Runtime** (included in self-contained builds)

### Quick Install
1. Download the latest release from [Releases](https://github.com/yourusername/SLauncher/releases)
2. Extract the ZIP file to any folder
3. Run `SLauncher.exe`
4. (Optional) Enable "Start with Windows" in Settings

### Portable Mode
All settings and data are stored in the `UserCache` folder next to the executable:
```
SLauncher/
������ SLauncher.exe
������ UserCache/
    ������ Settings/   # User preferences
    ������ Files/        # Item data
  ������ FaviconCache/    # Website icons
```

---

## ?? Usage

### Adding Items

#### Method 1: Buttons
- **Add File**: Click button and select `.exe`, `.lnk`, or any file
- **Add Folder**: Click button and select any folder
- **Add Website**: Click button and enter URL (e.g., `https://github.com`)

#### Method 2: Drag & Drop
- Drag files, folders, or shortcuts directly into the window
- Drag between tabs to move items
- Drag one item onto another to create a group

### Managing Items

| Action | Method |
|--------|--------|
| **Edit** | Right-click item �� Edit |
| **Delete** | Right-click item �� Delete or press `Delete` key |
| **Create Group** | Drag one item onto another |
| **Reorder** | Drag items to new positions |

### Tab Management

#### Creating Tabs
- Click **+** button next to tabs
- Each tab can have different items and settings

#### Tab Options (Right-click tab)
- **Rename** - Give tab a custom name
- **Change Color** - Choose from 8 preset colors
- **Delete** - Remove tab (requires confirmation if has items)

### Search

The search box supports multiple input types:

| Input Type | Example | Result |
|------------|---------|--------|
| **Item Name** | `notepad` | Search through all items |
| **File Path** | `C:\Windows\notepad.exe` | Open file directly |
| **Folder Path** | `C:\Users\Documents` | Open folder in Explorer |
| **Website URL** | `https://google.com` | Open in default browser |
| **Search Query** | `search:keyword` | Search with default browser |

### Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl + Space` | Show/hide launcher (customizable in Settings) |
| `Ctrl + Mouse Wheel` | Adjust icon size (works anywhere in window) |
| `Delete` | Delete selected item |
| `Esc` | Close launcher (in fullscreen mode) |
| `Enter` | Open first search result |

### Icon Scaling

Adjust icon size using:
- **Slider** in bottom-right corner
- **Ctrl + Mouse Wheel** anywhere in the window
- Range: 0.25x to 6.00x

---

## ?? Settings

### General Settings
- **Enable Fullscreen** - Use launcher in fullscreen mode
- **Grid Alignment** - Left (fills width) or Center (fixed width)
- **Start with Windows** - Launch automatically at startup
- **Global Hotkey** - Customize show/hide hotkey
  - Modifiers: Ctrl, Alt, Shift, Ctrl+Shift, Ctrl+Alt
  - Keys: Space, Tab, Enter, Esc, F1-F4
- **Language** - Select preferred language (English, Korean, Japanese)

### Cache Management
- **View Cache Size** - Monitor favicon cache usage
- **Clear Cache** - Remove all cached website icons
- **Open Cache Folder** - Access cache directory directly
- **Cache Location** - Portable: `UserCache\FaviconCache\`

---

## ??? Building from Source

### Prerequisites
- **Visual Studio 2022** (17.8 or later)
  - Workload: ".NET Desktop Development"
  - Component: "Windows App SDK C# Templates"
- **Windows App SDK 1.5** or later
- **.NET 8.0 SDK**

### Clone and Build

```bash
# Clone repository
git clone https://github.com/yourusername/SLauncher.git
cd SLauncher

# Restore NuGet packages
dotnet restore

# Build solution
dotnet build -c Release

# Or open in Visual Studio
start SLauncher.sln
```

### Project Structure

```
SLauncher/
������ SLauncher/              # Main WinUI 3 project
��   ������ Classes/       # Core classes
��   ��   ������ LocalizationManager.cs   # Multi-language support
��   ��   ������ UserSettingsClass.cs     # Settings management
��   ��   ������ GlobalHotkeyManager.cs   # Hotkey registration
��   ��   ������ IconHelpers.cs       # Icon extraction & cache
��   ������ Controls/           # Custom controls
��   ��   ������ GridViewTile.xaml    # App tile control
��   ��   ������ GridViewTileGroup.xaml   # Group control
��   ��   ������ AboutSectionControl.xaml # About page
��   ������ Strings/  # Localization resources
��   �� ������ en-US/Resources.resw     # English
��   ��   ������ ko-KR/Resources.resw     # Korean
��   ��   ������ ja-JP/Resources.resw# Japanese
��   ������ MainWindow*.cs  # Main window (partial classes)
��   ��   ������ MainWindow.xaml.cs       # Main logic
��   ��   ������ MainWindow.UI.cs   # UI management
��   ��   ������ MainWindow.Tabs.cs       # Tab management
��   �������� MainWindow.Items.cs      # Item management
��   ��   ������ MainWindow.DragDrop.cs# Drag & drop
��   ��   ������ MainWindow.Search.cs     # Search logic
��   ��   ������ MainWindow.Hotkeys.cs    # Hotkey & tray
��   ������ SettingsWindow*.cs  # Settings window (partial classes)
��       ������ SettingsWindow.xaml.cs      # Main logic
��       ������ SettingsWindow.Localization.cs # Language UI
��       ������ SettingsWindow.Cache.cs     # Cache management
��       ������ SettingsWindow.Hotkey.cs    # Hotkey config
��       ������ SettingsWindow.Settings.cs  # Settings toggles
������ WinFormsClassLibrary/   # Helper library (file dialogs)
```

### Partial Classes Pattern

Both `MainWindow` and `SettingsWindow` use partial classes for better code organization:
- Each partial class file handles a specific feature area
- Makes code easier to navigate and maintain
- Follows the same pattern as `MainWindow` for consistency

---

## ?? Localization

### Supported Languages

| Language | Code | Status | Resources |
|----------|------|--------|-----------|
| ???? English | en-US | ? Complete | 90 strings |
| ???? �ѱ��� | ko-KR | ? Complete | 91 strings |
| ???? ������ | ja-JP | ? Complete | 91 strings |

### Adding New Language

1. **Create Resource File**
   ```
   SLauncher/Strings/{language-code}/Resources.resw
   ```

2. **Copy Template**
   ```bash
   cp SLauncher/Strings/en-US/Resources.resw SLauncher/Strings/{language-code}/
   ```

3. **Translate**
   - Open `Resources.resw` in Visual Studio
   - Translate `<value>` content (keep `<data name>` unchanged)
   - Test UI layout (some languages are longer)

4. **Add to Settings**
   ```xaml
   <!-- SettingsWindow.xaml -->
   <ComboBox x:Name="LanguageComboBox">
    <ComboBoxItem Content="Your Language Name" Tag="{language-code}" />
   </ComboBox>
   ```

5. **Test**
   - Build and run
   - Select new language in Settings �� Language
   - Verify all UI elements display correctly

### Translation Guidelines
- Keep placeholders: `{0}`, `{1}` (used for string formatting)
- Maintain line breaks and formatting
- Use native conventions (punctuation, quotes)
- Test with long translations (may affect layout)
- Keep technical terms consistent (e.g., "cache", "hotkey")

---

## ?? Contributing

Contributions are welcome! Here's how you can help:

### Ways to Contribute
- ?? **Translations** - Add new languages or improve existing ones
- ?? **Bug Reports** - Report issues on [GitHub Issues](https://github.com/yourusername/SLauncher/issues)
- ? **Feature Requests** - Suggest new features
- ?? **Code** - Submit pull requests
- ?? **Documentation** - Improve README or code comments

### Development Workflow
1. **Fork** the repository
2. **Clone** your fork
   ```bash
   git clone https://github.com/your-username/SLauncher.git
   ```
3. **Create** feature branch
   ```bash
   git checkout -b feature/AmazingFeature
   ```
4. **Make** changes and test thoroughly
5. **Commit** with clear messages
 ```bash
   git commit -m "feat: Add amazing feature"
   ```
6. **Push** to your fork
   ```bash
   git push origin feature/AmazingFeature
   ```
7. **Open** Pull Request

### Code Style Guidelines
- Follow existing code patterns
- Use meaningful variable/method names
- Add XML comments for public methods
- Keep methods focused (single responsibility)
- Use partial classes for large files
- Add localization for all UI text

---

## ?? License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### Third-Party Libraries
- **WinUI 3** - MIT License
- **CommunityToolkit.WinUI** - MIT License
- **WinUIEx** - MIT License
- **System.Drawing.Common** - MIT License

---

## ?? Acknowledgments

- **Based on**: [LauncherX](https://github.com/Lolle2000la/LauncherX) by Lolle2000la
- **UI Framework**: [WinUI 3](https://microsoft.github.io/microsoft-ui-xaml/)
- **Community Toolkit**: [Windows Community Toolkit](https://github.com/CommunityToolkit/Windows)
- **Window Management**: [WinUIEx](https://github.com/dotMorten/WinUIEx)
- **Icons**: [Segoe Fluent Icons](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)

---

## ?? Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/SLauncher/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/SLauncher/discussions)
- **Email**: your.email@example.com

---

## ?? Changelog

### v1.0.0 (Latest)
- ? Multi-language support (English, Korean, Japanese)
- ? Real-time language switching
- ? Partial class refactoring for better code organization
- ? Window-wide Ctrl+MouseWheel icon scaling
- ?? Fixed Grid Alignment localization
- ?? Fixed Cache Management subtitle localization
- ?? Comprehensive localization (90+ strings per language)

### Previous Versions
- Tab management with colors
- Global hotkey support
- System tray integration
- Favicon caching

See full changelog in [CHANGELOG.md](CHANGELOG.md)

---

<div align="center">

**Made with ?? for Windows power users**

[? Back to Top](#slauncher)

</div>
