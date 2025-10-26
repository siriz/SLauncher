# ?? SLauncher v1.0.0 - First Release

## ?? Release Information

**Version:** 1.0.0  
**Release Date:** 2025-01-26  
**Build Type:** Self-Contained Portable  
**Based on:** LauncherX v2.1.2  

---

## ?? What is SLauncher?

SLauncher is a **fork of LauncherX** with significant enhancements focused on **multi-language support** and **improved user experience**.

### Original Project
- **Original Author:** ClickPhase
- **Original Project:** LauncherX
- **License:** MIT

---

## ? Major Features (New in Fork)

### ?? **Multi-Language Support**
- ? **3 Languages:** English, Korean («—±πæÓ), Japanese (ÏÌ‹‚Âﬁ)
- ? **Real-time Switching:** Change language without restarting
- ? **Complete Localization:** All UI elements translated
- ? **Automatic Detection:** System language auto-detection

### ?? **Tab Management System**
- ? **Multiple Tabs:** Organize apps into categories
- ? **Color Coding:** 10 preset colors for easy identification
- ? **Rename Tabs:** Custom tab names
- ? **Context Menu:** Right-click for quick actions
- ? **Persistent State:** Tabs saved automatically

### ?? **System Tray Integration**
- ? **Minimize to Tray:** Keep running in background
- ? **Quick Access:** Right-click tray menu
- ? **Exit Options:** Close or minimize

### ?? **Global Hotkey**
- ? **Ctrl+Space:** Quick show/hide from anywhere
- ? **Customizable:** (Planned for v1.1.0)

### ??? **Enhanced UI**
- ? **Icon Scaling:** Ctrl + Mouse Wheel
- ? **Modern Design:** WinUI 3 based
- ? **Responsive Layout:** Adapts to window size
- ? **Smooth Animations:** Polished experience

---

## ?? Inherited Features (from LauncherX)

### Core Functionality
- ? File and folder shortcuts
- ? Website bookmarks with favicons
- ? Drag and drop support
- ? Search functionality
- ? Linked folders (auto-sync)
- ? Portable (all data in UserCache)

---

## ?? System Requirements

### Minimum
- **OS:** Windows 10 version 1809 (build 17763) or later
- **Architecture:** x64
- **Disk Space:** ~300 MB (self-contained)

### Recommended
- **OS:** Windows 11
- **.NET:** Included (self-contained build)
- **RAM:** 4 GB or more

---

## ?? What's New in v1.0.0

### ? New Features
1. **Multi-Language System**
   - Localization framework implemented
   - 3 languages fully supported
   - Real-time language switching

2. **Tab Management**
   - Create unlimited tabs
   - Color-code for organization
   - Persistent tab state

3. **System Tray**
   - Minimize to system tray
   - Quick access menu
   - Background operation

4. **Global Hotkey**
   - Ctrl+Space to toggle
   - Works system-wide

### ?? Improvements
- Refactored MainWindow into partial classes for better maintainability
- Enhanced error handling with detailed logging
- Improved settings persistence
- Better icon extraction for shortcuts
- Optimized resource loading

### ?? Bug Fixes
- Fixed tab initialization issues
- Resolved language key display problems
- Fixed settings button double-click
- Corrected favicon caching
- Fixed drag-drop event registration

---

## ?? Installation

### Option 1: Portable (Recommended)
1. Download `SLauncher-v1.0.0-Portable.zip`
2. Extract to any folder
3. Run `SLauncher.exe`
4. No installation or admin rights required

### Option 2: Build from Source
```bash
git clone https://github.com/siriz/SLauncher.git
cd SLauncher
dotnet build -c Release
```

---

## ?? Language Support

| Language | Code | Coverage |
|----------|------|----------|
| English | en-US | 100% ? |
| «—±πæÓ | ko-KR | 100% ? |
| ÏÌ‹‚Âﬁ | ja-JP | 100% ? |

**Switch languages in:** Settings °Ê Language dropdown

---

## ?? Documentation

- [Main README](../README.md) - Project overview
- [Localization Guide](LOCALIZATION-README.md) - Multi-language details
- [VS Build Guide](VS-BUILD-GUIDE.md) - Building from source
- [Deployment Guide](DEPLOYMENT-GUIDE.md) - Distribution options

---

## ?? Roadmap (v1.x)

### v1.1.0 (Planned)
- [ ] Customizable hotkeys
- [ ] More color presets
- [ ] Export/Import settings
- [ ] Search improvements

### v1.2.0 (Planned)
- [ ] Plugin system
- [ ] Themes support
- [ ] Cloud sync (optional)

---

## ?? Credits

### Original Project
**LauncherX by ClickPhase**
- Original concept and implementation
- Core functionality and design
- MIT License

### This Fork (SLauncher)
**SIRIZ**
- Multi-language support
- Tab management system
- Enhanced UI/UX
- System tray integration

---

## ?? License

MIT License - See [LICENSE.txt](../LICENSE.txt)

**Original work Copyright ? 2020-present ClickPhase**  
**Modified work Copyright ? 2025 SIRIZ**

---

## ?? Known Issues

1. **First Launch:** May take 10-20 seconds (one-time initialization)
2. **Favicon Download:** Requires internet for whitelisted sites
3. **Icon Extraction:** Some shortcuts may show default icon

---

## ?? Feedback

Found a bug or have a suggestion?
- Open an issue on [GitHub](https://github.com/siriz/SLauncher/issues)
- Check [Documentation](../docs/) for common questions

---

## ?? File Size

| Build Type | Compressed | Extracted |
|------------|------------|-----------|
| Self-Contained | ~100 MB | ~300 MB |
| Framework-Dependent | ~5 MB | ~15 MB |

*Self-contained includes all runtimes (.NET 8 + Windows App SDK)*

---

**Thank you for using SLauncher!** ??

**SLauncher v1.0.0** - Modern app launcher with multi-language support  
Based on LauncherX v2.1.2 by ClickPhase
