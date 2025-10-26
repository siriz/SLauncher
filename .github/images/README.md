# Images for README

## Required Images

Please add the following screenshot images to this directory:

### 1. MainWindow.png
- Screenshot of the main application window
- Recommended size: 1600x1000 pixels (or similar 16:10 aspect ratio)
- Should show:
  - Search bar at the top
  - Multiple tabs (Apps, AI Study, PRU, etc.)
  - App icons in grid layout
  - Add buttons (file, folder, website)
  - Icon size slider at bottom

### 2. SettingsWindow.png
- Screenshot of the settings window
- Recommended size: 1200x900 pixels (or similar 4:3 aspect ratio)
- Should show:
  - General settings (Fullscreen, Grid alignment, Start with Windows)
  - Global Hotkey configuration
  - Language selection dropdown
  - Cache management options

## Current Images

- ? **icon.png** - App logo (already added)
- ? **MainWindow.png** - Main window screenshot (pending)
- ? **SettingsWindow.png** - Settings window screenshot (pending)

## How to Add Screenshots

1. Take screenshots of the application
2. Save them as `MainWindow.png` and `SettingsWindow.png`
3. Copy them to this directory (`.github/images/`)
4. Commit and push the changes

```bash
# Copy screenshots to images directory
cp /path/to/MainWindow.png .github/images/
cp /path/to/SettingsWindow.png .github/images/

# Commit the images
git add .github/images/*.png
git commit -m "docs: Add application screenshots"
git push
```

The README files are already configured to display these images once they are added!
