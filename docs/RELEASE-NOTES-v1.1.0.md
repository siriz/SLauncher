# 🚀 SLauncher v1.1.0

## 📋 Release Information

**Version:** 1.1.0
**Previous version:** [1.0.0](RELEASE-NOTES-v1.0.0.md)

---

## ⚠️ Important change: settings storage location

Settings, saved items and tabs used to be stored in a `UserCache` folder next
to `SLauncher.exe`. As of this release, they're stored per-user in
`%LOCALAPPDATA%\SLauncher\UserCache` instead.

**Why:** the old location lived inside the build/publish output folder, so it
was wiped by rebuilds, `Clean`, and reinstalls - causing settings, items and
tabs to appear to reset. The new location survives all of that.

**What this means for you:**
- Existing data is **migrated automatically** the first time you run this
  version - nothing to do.
- The executable is no longer fully "portable" (copying the folder to a USB
  drive / another PC no longer brings your data with it). Use the new
  **Backup & Restore** feature below instead.

---

## ✨ New Features

### 💼 Backup & Restore
- **Export Settings** - save your settings, saved items and tabs to a single
  `.slauncherbackup` file (Settings → Backup & Restore)
- **Import Settings** - restore from an exported file, on the same PC or a
  different one (restart required afterwards)

---

## 🐛 Bug Fixes

- **Settings/items/tabs no longer reset** on rebuild, reinstall, or when
  switching between Debug/Release builds (see storage location change above)
- **Fullscreen mode's close button** now saves items/tabs before exiting,
  matching the system tray's Exit behavior (previously any unsaved changes
  were lost)
- **Drag-and-drop group creation** no longer breaks after switching tabs a
  few times - event handlers were being re-subscribed on every tab switch,
  causing duplicated/corrupted groups or crashes
- **Memory leak fixed** in the group item dialog, which was leaking a
  `MainWindow.SizeChanged` subscription (and the group itself) every time it
  was opened
- **Korean/Japanese language names and real-time UI re-localization** were
  broken due to two source files being saved with the wrong text encoding -
  fixed
- **Build output size reduced** - removed WindowsAppSDK's per-culture `.mui`
  folders for languages SLauncher doesn't support

---

## 🧹 Internal Cleanup

- Renamed internal LauncherX-era method/variable names and comments to
  SLauncher (license, csproj metadata and the About screen's attribution to
  the original LauncherX project are unchanged)

---

## 📦 Upgrading

Just replace your existing installation with this version - your settings,
items and tabs will be picked up automatically from wherever they currently
are on first launch.
