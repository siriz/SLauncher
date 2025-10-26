# ?? Portable Self-Contained Deployment Guide

## ?? Final Configuration

### ? Confirmed Settings:

1. **Deployment Type:** Self-Contained (180-250MB)
2. **Cache Location:** `ExecutableFolder\UserCache\`
3. **Admin Rights:** Not required
4. **Internet:** Only for whitelisted sites (first download)
5. **Portability:** 100% - All data in executable folder

---

## ?? Folder Structure

### After Deployment:

```
SLauncher/
戍式式 SLauncher.exe   ∠ Main executable
戍式式 SLauncher.dll
戍式式 Resources/
弛   戍式式 icon.ico
弛   戍式式 icon.png
弛   戍式式 LinkedFolderIcon.png
弛   戌式式 websitePlaceholder.png
戍式式 UserCache/   ∠ All user data here! ?
弛   戍式式 userSettings.json            ∠ User settings
弛   戍式式 Files/   ∠ Saved items
弛   弛   戍式式 0.json
弛   弛   戍式式 1.json
弛   弛   戌式式 ...
弛   戌式式 FaviconCache/    ∠ Website icons
弛       戍式式 google.com.png
弛       戍式式 github.com.png
弛       戌式式 ...
戍式式 Microsoft.*.dll (100+ runtime DLLs)
戌式式 Windows*.dll (App SDK DLLs)
```

---

## ? Key Features

### 1. Fully Portable ?
```
- Copy folder ⊥ Works immediately
- No installation
- No registry entries
- No AppData usage
```

### 2. Self-Contained ?
```
- .NET 8 Runtime included
- Windows App SDK included
- All dependencies included
- No external requirements
```

### 3. User Data in Executable Folder ?
```
- Settings: UserCache\userSettings.json
- Items: UserCache\Files\
- Icons: UserCache\FaviconCache\
- Easy backup: Just copy UserCache folder
```

### 4. Whitelist-Friendly ?
```
- First download: Requires network
- Cached: Works offline
- Fallback: Uses placeholder
```

---

## ?? Build Instructions

### Single Command:

```cmd
build-release.bat
```

**What it does:**
1. Finds Visual Studio MSBuild
2. Builds Self-Contained (all runtimes)
3. Cleans unnecessary files (.pdb, .xml)
4. **Output:** ~180-250MB

**Build time:** 2-3 minutes

---

## ?? Deployment Steps

### 1. Build

```cmd
cd D:\Works\Playground\C#\SLauncher
build-release.bat
```

### 2. Create ZIP

**Manual compression:**
```
1. Go to: SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\win-x64\
2. Select ALL files (Ctrl+A)
3. Right-click ⊥ Send to ⊥ Compressed (zipped) folder
4. Rename: SLauncher-v2.1.2-Portable-SelfContained.zip
```

**Expected size:** 
- Before compression: 180-250MB
- After compression: ~50-80MB (ZIP compression)

### 3. Deploy

```
1. Copy ZIP to company shared folder
2. Create README.txt
3. Notify users
```

---

## ?? User Instructions

### For End Users:

```
?? SLauncher Installation Guide

1. Extract ZIP file to desired location
   Example: C:\Tools\SLauncher\

2. Run SLauncher.exe
   - No installation required
   - No admin rights required

3. Start using!
   - Add files, folders, websites
   - All data saved in UserCache folder

4. Backup (optional)
   - Copy entire SLauncher folder
   - Or just UserCache folder for settings/data
```

---

## ?? Data Management

### User Data Location:

**Everything in one place:**
```
[SLauncher Installation Folder]\UserCache\
```

### Backup:

**Option A: Full Backup**
```
Copy entire SLauncher folder ⊥ Done!
```

**Option B: Data Only**
```
Copy UserCache folder ⊥ Restore to new installation
```

### Migration:

**Move to another PC:**
```
1. Copy SLauncher folder
2. Paste on new PC
3. Run SLauncher.exe
4. All settings/data preserved!
```

### Multiple Installations:

**Possible scenarios:**
```
Desktop: C:\Tools\SLauncher\
USB Drive: E:\Portable\SLauncher\
Network: \\Server\Apps\SLauncher\

Each has independent UserCache!
```

---

## ?? Settings - Cache Management

### UI Features:

1. **View Cache Size**
```
Settings ⊥ Cache Management
Displays: "Cache size: 2.5 MB"
Location: UserCache\FaviconCache\
```

2. **Clear Cache**
```
Settings ⊥ Clear Cache button
Confirmation dialog
All favicons deleted
Re-downloaded on next use
```

3. **Open Cache Folder**
```
Settings ⊥ Open Folder button
Windows Explorer opens
Shows: ExecutableFolder\UserCache\FaviconCache\
```

---

## ?? Size Breakdown

### Deployment Package:

| Component | Size |
|-----------|------|
| SLauncher.exe + DLLs | ~5 MB |
| .NET 8 Runtime | ~80-100 MB |
| Windows App SDK | ~70-80 MB |
| Dependencies | ~20-50 MB |
| **Total (Uncompressed)** | **180-250 MB** |
| **Total (ZIP)** | **50-80 MB** |

### User Data (UserCache):

| Component | Typical Size |
|-----------|--------------|
| userSettings.json | ~1 KB |
| Files/ (100 items) | ~20-50 KB |
| FaviconCache/ (50 sites) | ~1-2 MB |
| **Total UserCache** | **1-3 MB** |

---

## ?? Important Notes

### Antivirus:

**May flag as suspicious:**
- Many DLL files
- Self-extracting behavior
- Network connections (favicon download)

**Solution:**
```
Add to antivirus exceptions:
- SLauncher.exe
- Or entire SLauncher folder
```

### File Permissions:

**Installation location:**
```
? Recommended:
  - C:\Tools\SLauncher\
  - C:\Users\[Username]\Applications\SLauncher\
  - D:\Programs\SLauncher\

? Avoid:
  - C:\Program Files\SLauncher\  (requires admin)
  - C:\Windows\SLauncher\  (blocked)
```

### Network Access:

**First use of website feature:**
```
- Requires whitelisted internet
- Downloads favicon once
- Saves to UserCache\FaviconCache\
- Subsequent loads: Offline
```

---

## ?? Comparison: Before vs After

### Before (Original Plan):

| Feature | Value |
|---------|-------|
| Cache Location | %AppData%\SLauncher\ |
| Deployment | Framework-Dependent (10-30MB) |
| Admin Rights | Required (runtime install) |
| Portability | ? Tied to user profile |

### After (Current Plan): ?

| Feature | Value |
|---------|-------|
| Cache Location | ExecutableFolder\UserCache\ |
| Deployment | Self-Contained (180-250MB) |
| Admin Rights | Not required |
| Portability | ? 100% Portable |

---

## ? Advantages

### For IT Department:

```
? Single ZIP deployment
? No Group Policy needed
? No runtime deployment
? Easy rollback (just replace folder)
? Version coexistence (multiple folders)
```

### For End Users:

```
? Extract and run
? No installation
? No admin rights
? Easy backup (copy folder)
? Portable (USB, network, etc.)
```

### For You (Developer):

```
? Single build command
? Self-contained testing
? No deployment complexity
? Easy debugging (all files in one place)
```

---

## ?? Maintenance

### Updates:

```
1. Build new version
2. Create new ZIP
3. Users extract to new folder
4. Optional: Copy UserCache from old version
```

### Troubleshooting:

**"Settings lost after update"**
```
Solution: Copy UserCache folder from old version
```

**"Icons gone"**
```
Solution: Copy UserCache\FaviconCache\ from old version
```

**"Items missing"**
```
Solution: Copy UserCache\Files\ from old version
```

---

## ?? Deployment Checklist

Before releasing:

- [ ] Run `build-release.bat`
- [ ] Verify build success
- [ ] Check file size (~180-250MB)
- [ ] Test on clean PC
- [ ] Verify UserCache folder creation
- [ ] Test favicon caching
- [ ] Test Settings ⊥ Cache Management
- [ ] Create ZIP file
- [ ] Test ZIP extraction
- [ ] Test on USB drive (portability)
- [ ] Create README.txt
- [ ] Document known issues
- [ ] Prepare user training materials

---

## ?? README.txt Template

```txt
========================================
SLauncher v2.1.2
Portable Self-Contained Edition
========================================

?? INSTALLATION:
1. Extract ALL files to desired location
2. Run SLauncher.exe
3. Done! No installation needed.

? FEATURES:
- 100% Portable
- No admin rights required
- No internet required (after first use)
- All data in UserCache folder

?? FOLDER STRUCTURE:
SLauncher\
戍式式 SLauncher.exe (Run this!)
戍式式 UserCache\ (Your data)
弛   戍式式 Settings
弛   戍式式 Items
弛   戌式式 Website icons
戌式式 Runtime files (Don't delete!)

?? BACKUP:
- Copy entire SLauncher folder
- Or just UserCache for settings/data

?? UPDATE:
- Extract new version to new folder
- Copy UserCache from old version
- Done!

?? REQUIREMENTS:
- Windows 11 64-bit
- No other requirements!

? FIRST RUN:
- May take 10-20 seconds (one-time)
- Creates UserCache folder automatically

?? WEBSITE ICONS:
- First use: Requires whitelisted internet
- Downloads and caches locally
- Offline after first download

?? SUPPORT:
[Your IT Contact]
```

---

## ?? Testing Guide

### Before Deployment:

1. **Clean PC Test**
```
- No .NET Runtime
- No Windows App SDK
- Extract ZIP
- Run SLauncher.exe
- Should work immediately ?
```

2. **Portability Test**
```
- Extract to USB drive
- Run from USB
- Verify UserCache on USB ?
```

3. **Network Test**
```
- Add whitelisted website
- Verify favicon download
- Disconnect network
- Restart SLauncher
- Verify cached icon loads ?
```

4. **Settings Test**
```
- Open Settings
- Cache Management section exists ?
- Cache size displays ?
- Clear Cache works ?
- Open Folder works ?
```

5. **Backup/Restore Test**
```
- Add items
- Copy UserCache
- Delete SLauncher folder
- Extract fresh copy
- Paste UserCache
- Run SLauncher.exe
- Items restored ?
```

---

## ?? Summary

### Perfect for Your Environment:

? **Whitelisted Internet:** Favicon caching works
? **No Admin Rights:** Self-contained deployment
? **Portable:** UserCache in executable folder
? **Air-gapped Friendly:** Works offline after first use
? **Easy Maintenance:** Single folder deployment

### Final Package:

```
File: SLauncher-v2.1.2-Portable-SelfContained.zip
Size: ~50-80MB (compressed)
Extracted: ~180-250MB
Installation: Extract and run
Requirements: None!
```

---

## ?? Ready to Deploy!

**Next steps:**

1. Run `build-release.bat`
2. Create ZIP
3. Test on another PC
4. Deploy to users

**You're all set!** ??
