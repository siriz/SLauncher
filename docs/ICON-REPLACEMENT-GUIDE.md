# ?? SLauncher ������ ���� �Ϻ� ���̵�

## ?? ����

SLauncher�� �������� ���ο� �̹����� �����ϴ� ����Դϴ�.

**�� ������ ������:**
- ū 'S' ����
- ü�� ��ũ ������
- ���� ������
- �����/�Ķ��� �׶��̼� ���

---

## ?? Step 1: PNG�� ICO�� ��ȯ

### **��� 1: �¶��� ���� (���� ����) ?**

**��õ ����Ʈ:**

1. **ConvertICO** (https://convertio.co/png-ico/)
   ```
   - PNG ���ε�
   - ICO ����
   - ��ȯ Ŭ��
   - �ٿ�ε�
   ```

2. **ICO Converter** (https://www.icoconverter.com/)
   ```
   - �̹��� ����
   - ũ�� ����: 256x256, 128x128, 64x64, 48x48, 32x32, 16x16 (��� ����)
   - Convert Ŭ��
   - �ٿ�ε�
   ```

3. **ICOConvert** (https://icoconvert.com/)
   ```
   - Choose PNG
   - Convert to ICO
   - Download
   ```

**���� ����:**
```
? Multi-size ICO (���� ũ�� ����)
? ũ��: 16x16, 32x32, 48x48, 64x64, 128x128, 256x256
? ����: ���� (��� ����)
```

---

### **��� 2: Windows Paint 3D**

**Step by Step:**

```
1. �̹����� Paint 3D�� ����
   - ��Ŭ�� �� ���� ���α׷� �� Paint 3D

2. ũ�� ����
   - Canvas �޴�
   - ũ��: 256 x 256 �ȼ�
   - ���μ��� ���� ���� ?

3. PNG�� ����
   - Menu �� Save as �� Image
   - PNG ���� ����
- �̸�: icon.png

4. �¶��� ������ ICO ��ȯ
   - icon.png �� icon.ico
```

---

### **��� 3: GIMP (����, ���)**

**��ġ:**
```
https://www.gimp.org/downloads/
```

**��ȯ ����:**

```
1. GIMP ����

2. �̹��� ����
   File �� Open �� �̹��� ����

3. ũ�� ���� (�ʿ� ��)
 Image �� Scale Image
   Width: 256
   Height: 256
   ? Chain icon Ŭ�� (���� ����)
   Scale

4. ���̾� ����
   Image �� Flatten Image

5. ICO�� ��������
   File �� Export As
   ���ϸ�: icon.ico
   ���� ����: Microsoft Windows icon (*.ico)
   Export
   
   ICO �ɼ� â����:
   ? Compressed (PNG)
   Save
```

---

### **��� 4: ImageMagick (�����)**

**��ġ:**
```powershell
# Chocolatey ��� ��
choco install imagemagick

# �Ǵ� ���� �ٿ�ε�
https://imagemagick.org/script/download.php
```

**��ȯ ���:**

```powershell
# ���� ũ��
magick convert icon.png -resize 256x256 icon.ico

# ���� ũ�� (����)
magick convert icon.png -define icon:auto-resize=256,128,64,48,32,16 icon.ico
```

---

### **��� 5: PowerShell ��ũ��Ʈ (���)**

**���� ����: `convert-to-ico.ps1`**

```powershell
<#
.SYNOPSIS
    PNG�� ICO�� ��ȯ�ϴ� PowerShell ��ũ��Ʈ
    
.DESCRIPTION
    PNG �̹����� ���� ũ�� ICO ���Ϸ� ��ȯ�մϴ�.
    System.Drawing ���̺귯�� ���
    
.PARAMETER InputPath
    �Է� PNG ���� ���
    
.PARAMETER OutputPath
    ��� ICO ���� ���
    
.EXAMPLE
    .\convert-to-ico.ps1 -InputPath "icon.png" -OutputPath "icon.ico"
#>

param(
  [Parameter(Mandatory=$true)]
    [string]$InputPath,
    
    [Parameter(Mandatory=$false)]
    [string]$OutputPath = "icon.ico"
)

# System.Drawing �ε�
Add-Type -AssemblyName System.Drawing

try {
    Write-Host "?? Converting PNG to ICO..." -ForegroundColor Cyan
    
    # PNG �ε�
    if (-not (Test-Path $InputPath)) {
        throw "Input file not found: $InputPath"
    }
    
    $png = [System.Drawing.Image]::FromFile((Resolve-Path $InputPath).Path)
    Write-Host "? PNG loaded: $($png.Width)x$($png.Height)" -ForegroundColor Green
    
    # ��Ʈ������ ��ȯ
    $bitmap = New-Object System.Drawing.Bitmap($png)
    
    # ICO ũ���
    $sizes = @(256, 128, 64, 48, 32, 16)
    Write-Host "?? Creating ICO with sizes: $($sizes -join ', ')" -ForegroundColor Yellow
    
    # �ӽ� ICO ���� (256x256 ����)
    $icon = [System.Drawing.Icon]::FromHandle($bitmap.GetHicon())
    
    # ICO ���Ϸ� ����
    $stream = [System.IO.File]::Create($OutputPath)
    $icon.Save($stream)
    $stream.Close()
    
    Write-Host "? ICO file created: $OutputPath" -ForegroundColor Green
    
    # ���� ũ�� ǥ��
    $size = (Get-Item $OutputPath).Length / 1KB
    Write-Host "?? File size: $($size.ToString('F2')) KB" -ForegroundColor Cyan
    
 # ����
    $bitmap.Dispose()
    $png.Dispose()
    $icon.Dispose()
    
    Write-Host "? Conversion complete!" -ForegroundColor Green
}
catch {
    Write-Host "? Error: $_" -ForegroundColor Red
    exit 1
}
```

**����:**

```powershell
# ���� ��å ���� (�� ����)
Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned

# ��ȯ ����
.\convert-to-ico.ps1 -InputPath "your-image.png" -OutputPath "icon.ico"
```

---

## ?? Step 2: ������ ���� ��ü

### **���� ������ ��ġ:**

```
D:\Works\Playground\C#\SLauncher\SLauncher\Resources\icon.ico
```

### **��ü ���:**

#### **Option 1: ���� Ž���� (����)**

```
1. Windows Ž���� ����
Win + E

2. ���� �̵�
   D:\Works\Playground\C#\SLauncher\SLauncher\Resources\

3. ���� icon.ico ���
   - icon.ico ��Ŭ��
   - ����
   - �ٿ��ֱ�
   - �̸� ����: icon.ico.bak

4. �� icon.ico ����
   - ��ȯ�� icon.ico�� Resources\ ������ ����
   - ����� Ȯ��
```

---

#### **Option 2: PowerShell (����)**

```powershell
# SLauncher ������Ʈ ���丮�� �̵�
cd "D:\Works\Playground\C#\SLauncher"

# ���� ������ ���
Copy-Item "SLauncher\Resources\icon.ico" "SLauncher\Resources\icon.ico.bak" -Force

# �� ������ ���� (��ȯ�� ���� ��θ� �����ϼ���)
Copy-Item "C:\Users\[YourUsername]\Downloads\icon.ico" "SLauncher\Resources\icon.ico" -Force

Write-Host "? Icon replaced successfully!" -ForegroundColor Green
```

---

#### **Option 3: �ϰ� ó�� ��ġ ����**

**���� ����: `replace-icon.bat`**

```batch
@echo off
echo ?? SLauncher Icon Replacement Tool
echo ================================

:: ���
echo ?? Backing up old icon...
copy "SLauncher\Resources\icon.ico" "SLauncher\Resources\icon.ico.bak" /Y

:: �� ������ ���� (��� ���� �ʿ�)
echo ?? Copying new icon...
copy "icon.ico" "SLauncher\Resources\icon.ico" /Y

if %errorlevel% == 0 (
    echo ? Icon replaced successfully!
    echo.
    echo ?? Next steps:
    echo    1. Open Visual Studio
    echo  2. Rebuild Solution
    echo    3. Run the application
) else (
    echo ? Error replacing icon!
)

pause
```

**����:**

```
1. replace-icon.bat ������ ������Ʈ ��Ʈ�� ����
2. �� icon.ico�� ������Ʈ ��Ʈ�� ����
3. replace-icon.bat ����Ŭ�� ����
```

---

## ?? Step 3: Visual Studio���� Ȯ��

### **������Ʈ���� ������ Ȯ��:**

```
1. Visual Studio ����

2. Solution Explorer
   - SLauncher ������Ʈ Ȯ��
   - Resources ���� Ȯ��
   - icon.ico ���� Ȯ��

3. ������ ����� ������ ������
   - Resources ���� ��Ŭ��
 - Add �� Existing Item
   - icon.ico ����
   - Add

4. Properties Ȯ��
   - icon.ico ��Ŭ�� �� Properties
   - Build Action: Content
   - Copy to Output Directory: Copy always
```

---

### **������Ʈ ���� Ȯ��:**

**SLauncher.csproj Ȯ��:**

```xml
<PropertyGroup>
    <ApplicationIcon>Resources\icon.ico</ApplicationIcon>
</PropertyGroup>

<ItemGroup>
    <Content Include="Resources\icon.ico">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </Content>
</ItemGroup>
```

**? �̹� �ùٸ��� �����Ǿ� �ֽ��ϴ�!**

---

## ?? Step 4: ���� �� �׽�Ʈ

### **Clean & Rebuild:**

```
1. Visual Studio �޴�
   Build �� Clean Solution

2. ��� (�� ��)

3. Build �� Rebuild Solution
   
4. ���� ���� Ȯ��
   Output â���� "Build succeeded" Ȯ��
```

---

### **���� ���� Ȯ��:**

```powershell
# ��� ���丮�� �̵�
cd "D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64"

# ������ Ȯ��
dir SLauncher.exe

# ����
.\SLauncher.exe
```

---

### **������ �̸�����:**

**Windows Ž���⿡��:**
```
1. ��� ���丮 ����
   D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\

2. SLauncher.exe ã��

3. ������ Ȯ��
   - ū ������ ����
   - �����/�Ķ��� 'S' �ΰ� Ȯ�� ?
```

---

### **�۾� ǥ���� Ȯ��:**

```
1. SLauncher ����

2. �۾� ǥ���ٿ� ����
   - �۾� ǥ������ SLauncher ������ ��Ŭ��
   - "�۾� ǥ���ٿ� ����"

3. ������ Ȯ��
   - �� �������� ǥ�õǴ��� Ȯ��
```

---

## ?? Step 5: ������ ����

### **������ ���� ����:**

```
1. Visual Studio
   - ��� ��Ӵٿ�: Debug �� Release
   - �÷���: x64

2. Build �� Rebuild Solution

3. ��� Ȯ��
   D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\win-x64\
```

---

### **Self-Contained ����:**

**���� ���� ��ũ��Ʈ ���:**

```powershell
# ������Ʈ ��Ʈ���� ����
.\build-selfcontained.bat
```

**�Ǵ� ��������:**

```powershell
dotnet publish `
  -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true
```

---

## ?? ���� �ذ�

### **���� 1: �������� ������� ����**

**����:**
```
���� �Ŀ��� ������ ���� �������� ǥ�õ�
```

**�ذ�:**

```powershell
# 1. Clean Solution
Build �� Clean Solution

# 2. obj �� bin ���� ����
Remove-Item -Path "SLauncher\obj" -Recurse -Force
Remove-Item -Path "SLauncher\bin" -Recurse -Force

# 3. Rebuild
Build �� Rebuild Solution

# 4. Windows ������ ĳ�� ����
ie4uinit.exe -show
taskkill /IM explorer.exe /F
DEL /A /Q "%localappdata%\IconCache.db"
DEL /A /F /Q "%localappdata%\Microsoft\Windows\Explorer\iconcache*"
start explorer.exe
```

---

### **���� 2: ICO ���� �ջ�**

**����:**
```
Error: The file 'Resources\icon.ico' is not a valid icon file
```

**�ذ�:**

```
1. ICO ���� �纯ȯ
   - �ٸ� �¶��� ���� �õ�
   - GIMP ���
   - ImageMagick ���

2. ICO ���� ��ȿ�� �˻�
   - �¶��� ����: https://www.online-convert.com/
   - ICO ���� �����

3. ũ�� Ȯ��
   - ���� ũ��: < 1MB (����)
   - �̹��� ũ��: 16x16 ~ 256x256
```

---

### **���� 3: ���� �ս�**

**����:**
```
������ ����� �Ͼ�� ǥ�õ� (���� ����)
```

**�ذ�:**

```
1. PNG ���� Ȯ��
   - ����� �������� Ȯ�� (üũ���� ����)
   - Paint.NET�̳� GIMP���� �����

2. ICO ��ȯ �� �ɼ�
   - ? "Preserve transparency"
   - ? "Alpha channel"
 - ? "32-bit color depth"

3. GIMP���� �纯ȯ
   - Layer �� Transparency �� Add Alpha Channel
   - File �� Export as �� ICO
   - ? Compressed (PNG) ����
```

---

### **���� 4: ���� ũ�� ������**

**����:**
```
���� ������(16x16, 32x32)�� �帴��
```

**�ذ�:**

```
���� ũ�� ICO ����:

1. ImageMagick ���
   magick convert icon.png -define icon:auto-resize=256,128,64,48,32,16 icon.ico

2. GIMP ���
   - �� ũ�⺰�� ���̾� ����
   - 256x256, 128x128, 64x64, 48x48, 32x32, 16x16
   - Export as ICO �� ��� ���̾� ����

3. ICO Editor ���
   - IcoFX (https://icofx.ro/)
   - Greenfish Icon Editor (����)
```

---

## ?? ������ ǰ�� üũ����Ʈ

### **? ��ȯ �� Ȯ��:**

```
�� PNG ���� (���� ���)
�� ���簢�� �̹��� (1:1 ����)
�� �ּ� 256x256 �ȼ� (����: 512x512 �̻�)
�� ������ �̹��� (���� �ս� ����)
�� ������ ���� ���
```

---

### **? ��ȯ �� Ȯ��:**

```
�� ICO ���� ũ�� < 1MB
�� ���� ũ�� ���� (16, 32, 48, 64, 128, 256)
�� ���� ����
�� Windows���� �̸����� ����
�� ���� ũ��(16x16)������ �ĺ� ����
```

---

### **? ���� �� Ȯ��:**

```
�� SLauncher.exe ������ �����
�� �۾� ǥ���� ������ ����
�� �ý��� Ʈ���� ������ ����
�� Alt+Tab ��ȯ �� ������ ����
�� ���� �޴� ������ ���� (���� ��)
```

---

## ?? ������ ��

### **���� �� ������ ������:**

```
? �ܼ��ϰ� ��Ȯ
? ���� ũ�⿡���� �ĺ� ����
? �귣�� ���� �ϰ���
? ��Ư�ϰ� ����ϱ� ����
? �÷� ������ �Ǵ� �ణ�� �׸���
```

### **���� ������ �м�:**

```
? ū 'S' - �� �̸� (SLauncher)
? ü�� ��ũ - "��ó" ����
? ���� - ����/���� ����
? �����/�Ķ��� - ������, �����
? �ձ� �𼭸� - ģ����
```

**�Ϻ��� �������Դϴ�!** ?

---

## ?? ���� ���

### **5�� ���� ��ü:**

```powershell
# 1. PNG to ICO ��ȯ
# https://convertio.co/png-ico/ ���� ��ȯ

# 2. ��� �� ��ü
cd "D:\Works\Playground\C#\SLauncher"
Copy-Item "SLauncher\Resources\icon.ico" "SLauncher\Resources\icon.ico.bak"
Copy-Item "C:\Users\[Username]\Downloads\icon.ico" "SLauncher\Resources\icon.ico" -Force

# 3. Visual Studio���� Rebuild
# Build �� Rebuild Solution

# 4. ���� �� Ȯ��
# F5 �� ������ Ȯ��
```

---

## ?? �߰� ���ҽ�

### **������ ��ȯ ����:**

```
�¶���:
- ConvertICO: https://convertio.co/png-ico/
- ICO Converter: https://www.icoconverter.com/
- ICOConvert: https://icoconvert.com/

��������:
- GIMP: https://www.gimp.org/
- IcoFX: https://icofx.ro/
- Greenfish Icon Editor: http://greenfishsoftware.org/
- ImageMagick: https://imagemagick.org/
```

---

### **������ ������ ����:**

```
���� �׷���:
- Figma: https://www.figma.com/
- Adobe Illustrator
- Inkscape (����): https://inkscape.org/

������ �׷���:
- Adobe Photoshop
- Affinity Designer
- Paint.NET (����): https://www.getpaint.net/
```

---

### **���� ����:**

```
Microsoft ������ ���̵�:
https://docs.microsoft.com/en-us/windows/apps/design/style/iconography/app-icon-construction

ICO ���� ����:
https://en.wikipedia.org/wiki/ICO_(file_format)

Windows App SDK ������:
https://learn.microsoft.com/en-us/windows/apps/design/style/iconography
```

---

## ? ���� üũ����Ʈ

### **�Ϸ� Ȯ��:**

```
�� PNG�� ICO�� ��ȯ �Ϸ�
�� Resources\icon.ico ���� ��ü �Ϸ�
�� ���� icon.ico ��� �Ϸ�
�� Visual Studio���� Rebuild �Ϸ�
�� ���� ���� ������ Ȯ�� �Ϸ�
�� �۾� ǥ���� ������ Ȯ�� �Ϸ�
�� �ý��� Ʈ���� ������ Ȯ�� �Ϸ�
```

---

## ?? �Ϸ�!

**���ο� �������� ���������� ����Ǿ����ϴ�!**

**���� SLauncher�� ���ο� �귣�� ���̵�ƼƼ�� ������ �Ǿ����ϴ�!** ?

---

## ?? ����

**������ �߻��ϸ�:**

1. **ICO ���� �纯ȯ**
   - �ٸ� ����/����Ʈ ���
   - ���� ũ�� �ɼ� Ȯ��

2. **ĳ�� ����**
   - Windows ������ ĳ��
   - Visual Studio ���� ĳ��

3. **������Ʈ Clean**
   - Build �� Clean Solution
   - obj, bin ���� ���� ����

4. **�����**
   - Visual Studio �����
   - Windows ����� (������ ����)

---

**Good Luck!** ??
