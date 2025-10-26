# ?? Air-Gapped Environment Deployment Guide

## ?? Your Environment Constraints

- ? **No Internet Access** - External connections blocked
- ? **No Admin Rights** - Cannot install runtimes
- ? **Windows 11 x64** - Supported

---

## ? Solution: Self-Contained Build

### What is Self-Contained?

**Includes everything needed:**
- .NET 8 Runtime ?
- Windows App SDK ?
- All dependencies ?

**No installation required:**
- Extract ZIP ⊥ Run EXE ?

**Trade-off:**
- File size: **180-250MB** (vs 10-30MB Framework-Dependent)

---

## ?? Build Instructions

### Step 1: Fix Google API (Already Done ?)

**Changed:**
```csharp
// BEFORE (requires internet):
Uri iconUri = new Uri("https://www.google.com/s2/favicons?sz=128&domain_url=" + websiteUrl);

// AFTER (offline):
Uri defaultImageUri = new Uri(Path.GetFullPath(@"Resources\websitePlaceholder.png"));
```

**Result:** Website icons will use placeholder image

---

### Step 2: Build Self-Contained Version

```cmd
build-selfcontained.bat
```

**What it does:**
1. Finds Visual Studio MSBuild
2. Builds with `/p:SelfContained=true`
3. Includes all runtimes
4. Output: ~180-250MB

**Build time:** 2-3 minutes (slower than normal build)

---

### Step 3: Create Deployment Package

**Manual compression (PowerShell may fail):**

1. **Navigate to build folder:**
   ```
   SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\win-x64\
   ```

2. **Select all files** (Ctrl+A)

3. **Right-click ⊥ Send to ⊥ Compressed (zipped) folder**

4. **Rename:** `SLauncher-v2.1.2-SelfContained.zip`

---

## ?? Deployment Package

### Final Package Contents:

```
SLauncher-v2.1.2-SelfContained.zip (180-250MB)
戍式式 SLauncher.exe
戍式式 SLauncher.dll
戍式式 Resources/
弛   戍式式 icon.ico
弛   戍式式 icon.png
弛   戍式式 LinkedFolderIcon.png
弛   戌式式 websitePlaceholder.png
戍式式 Microsoft.*.dll (Runtime files)
戍式式 Windows*.dll (App SDK files)
戌式式 (100+ dependency DLLs)
```

---

## ?? User Installation

### For End Users (Employees):

**Super simple:**
```
1. Extract ZIP file
2. Run SLauncher.exe
3. Done!
```

**No steps required:**
- ? No runtime installation
- ? No admin rights
- ? No internet connection
- ? Just extract and run!

---

## ?? Deployment Checklist

### Before Deployment:

- [ ] Run `build-selfcontained.bat`
- [ ] Verify build success
- [ ] Check file size (180-250MB)
- [ ] Test on clean PC (no runtimes)
- [ ] Create ZIP file
- [ ] Test ZIP extraction
- [ ] Verify SLauncher.exe runs
- [ ] Test file/folder operations
- [ ] Create README.txt
- [ ] Copy to shared folder

---

## ?? README.txt Template

```txt
========================================
SLauncher v2.1.2 - Self-Contained
========================================

?? QUICK START:
1. Extract ALL files from ZIP
2. Run SLauncher.exe
3. Start adding files/folders!

? NO INSTALLATION REQUIRED
- No admin rights needed
- No internet needed
- No runtime installation

?? FEATURES:
- Add files, folders, websites
- Drag & drop support
- Search functionality
- Customizable settings

?? IMPORTANT:
- Extract ALL files (don't run from ZIP)
- Keep all DLL files together
- Don't delete any files

?? TIPS:
- Drag files into window to add
- Use search box to find items
- Right-click items for options

SUPPORT: [Your IT contact]
```

---

## ?? Comparison

| Feature | Framework-Dependent | Self-Contained |
|---------|-------------------|----------------|
| **File Size** | 10-30MB | 180-250MB |
| **Internet** | Required (IT install) | Not required |
| **Admin Rights** | Required (IT install) | Not required |
| **Runtime Install** | Yes | No |
| **User Experience** | Complex | Simple |
| **Air-Gapped** | ? NO | ? YES |
| **Your Case** | ? Won't work | ? Perfect! |

---

## ?? Known Limitations

### Website Icons:
- **Issue:** Cannot fetch from internet
- **Solution:** Uses placeholder icon
- **Impact:** All websites show same icon

### Future Updates:
- **Impact:** Full ZIP (180-250MB) every time
- **Solution:** Delta updates (advanced)

---

## ?? Troubleshooting

### "File not found" error
**Cause:** Partial extraction
**Solution:** Extract ALL files from ZIP

### "Access denied" error
**Cause:** Extracted to protected location
**Solution:** Extract to Documents or Desktop

### Program doesn't start
**Cause:** Antivirus blocking
**Solution:** Add to antivirus exceptions

### Slow startup (first run)
**Cause:** .NET JIT compilation
**Solution:** Normal, will be faster next time

---

## ? Success Criteria

Your deployment is ready when:

- ? Build completes successfully
- ? File size is 180-250MB
- ? No internet required for operation
- ? No admin rights required
- ? Works on clean PC (no runtimes)
- ? All features work (files, folders)
- ? Website feature shows placeholder icons

---

## ?? Next Steps

1. **Build:** Run `build-selfcontained.bat`
2. **Test:** Extract and test on another PC
3. **Package:** Create ZIP file
4. **Deploy:** Copy to shared folder
5. **Support:** Provide README.txt

---

## ?? Pro Tips

### Faster Distribution:
- Use company file server
- Create desktop shortcut
- Pin to taskbar

### User Training:
- Demo during team meeting
- Create quick video
- Share keyboard shortcuts

### Maintenance:
- Version numbering in ZIP filename
- Keep old versions for rollback
- Document changes in release notes

---

## ?? Support

**For build issues:**
- Check Visual Studio installation
- Verify .NET 8 SDK installed
- Review build output logs

**For deployment issues:**
- Test ZIP extraction
- Check file permissions
- Verify antivirus settings

**For user issues:**
- Check README.txt first
- Verify all files extracted
- Contact IT support

---

## ? Summary

**Your situation:**
- ? No internet
- ? No admin rights

**Solution:**
- ? Self-Contained build
- ? 180-250MB file size
- ? Extract and run

**Result:**
- ? Works perfectly in air-gapped environment!
