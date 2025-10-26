# ?? Screenshot Guide for SLauncher

This guide explains how to add screenshots to the SLauncher README.

## ?? Screenshot Location

All screenshots should be placed in:
```
.github/images/
```

## ?? Required Screenshots

### 1. Main Window (`main-window.png`)
**What to capture:**
- Full main window with several apps/shortcuts displayed
- Show at least 2-3 tabs
- Include some color-coded tabs
- Make sure icons are visible and clear

**Recommended size:** 1200x800px

**How to capture:**
1. Open SLauncher
2. Add some sample apps/shortcuts
3. Press `Windows + Shift + S` to use Snipping Tool
4. Capture the entire window
5. Save as `main-window.png` in `.github/images/`

---

### 2. Tab Management (`tab-management.png`)
**What to capture:**
- Focus on the tab bar with multiple tabs
- Show tabs with different colors
- Right-click menu on a tab (showing Rename, Change Color, Delete options)

**Recommended size:** 800x400px

**How to capture:**
1. Create 3-4 tabs with different colors
2. Right-click on a tab to show context menu
3. Capture the tab area
4. Save as `tab-management.png`

---

### 3. Multi-Language Support (`multi-language.png`)
**What to capture:**
- Settings window showing language dropdown
- Or a before/after comparison showing different languages
- Include the language selector expanded

**Recommended size:** 1000x600px

**How to capture:**
Option A (Settings):
1. Open Settings (?? button)
2. Click on Language dropdown to show options
3. Capture the window
4. Save as `multi-language.png`

Option B (Comparison):
1. Take 2-3 screenshots in different languages
2. Use an image editor to combine them side-by-side
3. Save as `multi-language.png`

---

### 4. Settings Window (`settings.png`)
**What to capture:**
- Full settings window
- Show all available options
- Make sure language selector is visible

**Recommended size:** 800x600px

**How to capture:**
1. Click Settings button (??)
2. Capture the entire settings window
3. Save as `settings.png`

---

## ?? Screenshot Guidelines

### Image Quality
- **Format:** PNG (preferred) or JPG
- **Resolution:** At least 1080p for main screenshots
- **File size:** Keep under 500KB per image (use compression if needed)

### Composition
- **Clean background:** Use a neutral Windows desktop background
- **Sample content:** Add realistic but clean sample apps/shortcuts
- **No personal info:** Avoid showing personal file paths or data
- **Good lighting:** Use a light theme for better visibility

### Consistency
- Use the same theme (light/dark) for all screenshots
- Keep window sizes consistent
- Use similar sample content across screenshots

---

## ?? Image Optimization

Before adding to Git, optimize images:

### Using Online Tools
- [TinyPNG](https://tinypng.com/) - PNG compression
- [Squoosh](https://squoosh.app/) - Advanced image optimization

### Using Command Line
```powershell
# Install ImageMagick (if not installed)
# Then resize and optimize:
magick convert input.png -resize 1200x800 -quality 85 output.png
```

---

## ?? Updating README

After adding screenshots, the README will automatically display them:

```markdown
### Main Window
![Main Window](.github/images/main-window.png)
*Organize your apps and shortcuts with a clean, modern interface*
```

---

## ? Checklist

Before committing screenshots:

- [ ] All 4 required screenshots added
- [ ] Images are properly named
- [ ] Images are optimized (< 500KB each)
- [ ] No personal information visible
- [ ] Images are clear and high quality
- [ ] Consistent theme across all screenshots
- [ ] README displays images correctly

---

## ?? Adding Screenshots to Git

```bash
# Add screenshots
git add .github/images/*.png

# Add updated README
git add README.md

# Commit
git commit -m "docs: Add screenshots to README"

# Push
git push origin master
```

---

## ?? Example File Structure

```
.github/
戌式式 images/
    戍式式 main-window.png  (Main interface)
    戍式式 tab-management.png     (Tab features)
    戍式式 multi-language.png     (Language switching)
    戌式式 settings.png           (Settings window)
```

---

## ?? Tips

1. **Take screenshots in English** for the main images (most universal)
2. **Use the multi-language screenshot** to showcase Korean and Japanese support
3. **Show real features** - users should see what they'll actually get
4. **Keep it simple** - don't overcrowd with too many items
5. **Update periodically** - refresh screenshots when UI changes significantly

---

## ?? Alternative Hosting

If GitHub file size becomes an issue, consider:
- [Imgur](https://imgur.com/) - Free image hosting
- [GitHub Releases](../../releases) - Attach to release notes
- [GitHub Wiki](../../wiki) - Separate wiki with images

Then link in README:
```markdown
![Main Window](https://i.imgur.com/YOUR_IMAGE_ID.png)
```

---

**Ready to add your screenshots? Follow the steps above and make SLauncher look amazing on GitHub!** ???
