# ?? Critical Bug Fix - System.Drawing.Common Missing

## ? ���� �߻�

### ����:
```
���� �����ϸ� �ƹ� �ϵ� �Ͼ�� ����
���μ����� ��� �����
���� �ڵ�: 0xc000027b (STATUS_DLL_NOT_FOUND)
```

### ����� �α�:
```
'[37460] SLauncher.exe' ���α׷��� ����Ǿ����ϴ�(�ڵ�: 3221226107 (0xc000027b)).
```

**���� �ڵ� �ǹ�:**
- `0xc000027b` = `STATUS_DLL_NOT_FOUND`
- �ʿ��� DLL ������ ã�� �� ����

---

## ?? ���� �м�

### ������ �ڵ� (IconHelpers.cs):
```csharp
public static class IconHelpers
{
    // Get a list of all image file extensions
    public static List<string> ImageFileExtensions = ImageCodecInfo.GetImageEncoders()
     .Select(c => c.FilenameExtension)
          .SelectMany(e => e.Split(';'))
   .Select(e => e.Replace("*", "").ToLower())
    .ToList();
    // ...
}
```

**����:**
- `System.Drawing.Imaging.ImageCodecInfo`�� ��� ��
- �� Ŭ������ **System.Drawing.Common** ���̺귯���� ����
- WinUI 3 ������Ʈ�� �� ��Ű���� **����**��

### �� ����� �����߳�?
```
1. IconHelpers�� static Ŭ����
2. ImageFileExtensions�� static �ʵ�
3. ������ Ÿ�ӿ��� Ÿ�Ը� Ȯ��
4. ��Ÿ�ӿ� static �ʵ� �ʱ�ȭ �� DLL�� ã�� ���� ũ����
```

---

## ? �ذ� ���

### 1. System.Drawing.Common ��Ű�� �߰�

```bash
cd "D:\Works\Playground\C#\SLauncher\SLauncher"
dotnet add package System.Drawing.Common
```

**���:**
```
info : 'System.Drawing.Common' ��Ű�� '9.0.10' ������ ���� PackageReference�� �߰��Ǿ����ϴ�.
```

### 2. SLauncher.csproj �������

**Before:**
```xml
<ItemGroup>
  <PackageReference Include="CommunityToolkit.WinUI.Controls.SettingsControls" Version="8.1.240916" />
  <PackageReference Include="CommunityToolkit.WinUI.Extensions" Version="8.1.240916" />
  <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.26100.1742" />
  <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
  <PackageReference Include="System.Text.Json" Version="9.0.0" />
  <PackageReference Include="WinUIEx" Version="2.5.0" />
</ItemGroup>
```

**After:**
```xml
<ItemGroup>
  <PackageReference Include="CommunityToolkit.WinUI.Controls.SettingsControls" Version="8.1.240916" />
  <PackageReference Include="CommunityToolkit.WinUI.Extensions" Version="8.1.240916" />
  <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.26100.1742" />
  <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
  <PackageReference Include="System.Drawing.Common" Version="9.0.10" />  �� �߰�
  <PackageReference Include="System.Text.Json" Version="9.0.0" />
  <PackageReference Include="WinUIEx" Version="2.5.0" />
</ItemGroup>
```

---

## ?? System.Drawing.Common ��� ��ġ

### IconHelpers.cs
```csharp
using System.Drawing;
using System.Drawing.Imaging;

public static class IconHelpers
{
    // 1. ImageCodecInfo ���
    public static List<string> ImageFileExtensions = ImageCodecInfo.GetImageEncoders()
    .Select(c => c.FilenameExtension)
     .SelectMany(e => e.Split(';'))
   .Select(e => e.Replace("*", "").ToLower())
        .ToList();

    // 2. System.Drawing.Bitmap ���
    private static System.Drawing.Bitmap ResizeJumbo(System.Drawing.Bitmap imgToResize, System.Drawing.Size size)
    {
   System.Drawing.Bitmap b = new System.Drawing.Bitmap(size.Width, size.Height);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)b);
 // ...
    }

    // 3. System.Drawing.Color ���
    private static bool IsScaledDown(System.Drawing.Bitmap bitmap)
    {
     System.Drawing.Color empty = System.Drawing.Color.FromArgb(0, 0, 0, 0);
// ...
    }

    // 4. System.Drawing.Icon ���
    private async static Task<BitmapImage> GetPathIconWin32(string path, bool isDirectory)
    {
        // ...
        System.Drawing.Icon ico = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon).Clone();
        System.Drawing.Bitmap bitmapIcon = ico.ToBitmap();
      // ...
    }
}
```

**�� ��� Ƚ��:**
- `System.Drawing.Bitmap`: ���� ��
- `System.Drawing.Graphics`: 1��
- `System.Drawing.Icon`: 1��
- `System.Drawing.Color`: 1��
- `System.Drawing.Imaging.ImageCodecInfo`: 1��
- `System.Drawing.Size`: 1��

---

## ?? System.Drawing.Common�� ���� ���ǻ���

### Microsoft�� ���� ����:

> **System.Drawing.Common�� Windows������ �����˴ϴ�**
> - .NET 6���� Linux/macOS������ ���� �ߴ�
> - Windows������ ��� ������
> - ����: https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only

**SLauncher�� Windows ���� ���̹Ƿ� �������� ?**

### ��� (�ʿ� ��):
```
1. SkiaSharp - ũ�ν� �÷��� �׷��� ���̺귯��
2. ImageSharp - ���� .NET �̹��� ó�� ���̺귯��
3. WinUI 3 ����Ƽ�� API - ������ ��� ������
```

**����� System.Drawing.Common ����:**
- Windows ���� ��
- �������̰� ������
- Win32 API�� ȣȯ �ʿ�

---

## ?? ����� ����

### 1. ���� Ȯ��
```
? ���� ����
? �� ���� ��� ����
? ���� �޽��� ����
```

### 2. ����� �α� Ȯ��
```powershell
# Visual Studio Output Window
Debug -> Windows -> Output
```

**�߰ߵ� ����:**
```
'SLauncher.exe' ���α׷��� ����Ǿ����ϴ�(�ڵ�: 3221226107 (0xc000027b)).
```

### 3. ���� �ڵ� �м�
```
0xc000027b = STATUS_DLL_NOT_FOUND
�� �ʿ��� DLL�� ����
```

### 4. DLL ���Ӽ� Ȯ��
```powershell
# Dependencies.exe �Ǵ� dumpbin ���
dumpbin /dependents SLauncher.exe
```

### 5. �ڵ� ����
```csharp
// IconHelpers.cs ù �ٺ��� ũ���� �߻� ���ɼ�
public static List<string> ImageFileExtensions = ImageCodecInfo.GetImageEncoders()
// �� static �ʵ� �ʱ�ȭ = �� ���� �� ��� ����
```

### 6. ��Ű�� Ȯ��
```bash
dotnet list package
# System.Drawing.Common ���� �߰�!
```

### 7. ��Ű�� �߰� �� �ذ�
```bash
dotnet add package System.Drawing.Common
# ? ���� �ذ�!
```

---

## ?? ��� ����

### 1. ������Ʈ ���ø� ������Ʈ
```xml
<!-- SLauncher.csproj �ʼ� ��Ű�� -->
<ItemGroup>
  <!-- WinUI 3 Core -->
  <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
  <PackageReference Include="WinUIEx" Version="2.5.0" />
  
  <!-- UI Components -->
  <PackageReference Include="CommunityToolkit.WinUI.Controls.SettingsControls" Version="8.1.240916" />
  <PackageReference Include="CommunityToolkit.WinUI.Extensions" Version="8.1.240916" />
  
  <!-- System Libraries -->
  <PackageReference Include="System.Drawing.Common" Version="9.0.10" />  �� �ʼ�!
  <PackageReference Include="System.Text.Json" Version="9.0.0" />
  
  <!-- Build Tools -->
  <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.26100.1742" />
</ItemGroup>
```

### 2. ���� ��ũ��Ʈ ������Ʈ
```batch
@echo off
echo Checking dependencies...
dotnet list package | findstr "System.Drawing.Common"
if errorlevel 1 (
    echo ERROR: System.Drawing.Common is missing!
    echo Adding package...
    dotnet add package System.Drawing.Common
)
echo Building...
dotnet build
```

### 3. CI/CD ����������
```yaml
# .github/workflows/build.yml
- name: Verify Dependencies
  run: |
    dotnet list package | grep "System.Drawing.Common" || dotnet add package System.Drawing.Common
```

---

## ?? ������ ���� ���� ���

### ���� ���� �� �� �� üũ����Ʈ:

#### 1. **���� ���� Ȯ��**
```
? ���� �����߳�?
? ���� ����?
```

#### 2. **����� �α� Ȯ��**
```
Visual Studio:
- Debug �� Windows �� Output
- Show output from: Debug
```

#### 3. **Event Viewer Ȯ��**
```
Windows + X �� Event Viewer
�� Windows Logs �� Application
�� �ֱ� ���� Ȯ��
```

#### 4. **Dependencies Ȯ��**
```powershell
# Dependencies.exe �ٿ�ε�
# https://github.com/lucasg/Dependencies
Dependencies.exe SLauncher.exe
```

#### 5. **Missing DLLs Ȯ��**
```
- STATUS_DLL_NOT_FOUND (0xc000027b)
- ENTRYPOINT_NOT_FOUND (0xc0000139)
- DLL_INIT_FAILED (0xc0000142)
```

#### 6. **��Ű�� ����**
```bash
dotnet restore
dotnet clean
dotnet build
```

#### 7. **��Ÿ�� Ȯ��**
```bash
# .NET Runtime ��ġ Ȯ��
dotnet --list-runtimes

# �ʿ��� ��Ÿ��:
- Microsoft.NETCore.App 8.0.x
- Microsoft.WindowsDesktop.App 8.0.x
```

---

## ? �ذ� Ȯ��

### Before:
```
? �� ���� �� ��� ����
? ���� �ڵ�: 0xc000027b
? System.Drawing.Common ����
```

### After:
```
? System.Drawing.Common 9.0.10 ��ġ
? ���� ����
? �� ���� ����
? ������ �ε� ���� �۵�
```

---

## ?? �н� ����Ʈ

### 1. **Static �ʵ� �ʱ�ȭ ����**
```csharp
// ����: �� ���� �� ��� ����
public static List<string> ImageFileExtensions = ImageCodecInfo.GetImageEncoders()
           .Select(c => c.FilenameExtension)
    .ToList();

// ����: �ʿ��� ���� ����
public static List<string> ImageFileExtensions => 
    ImageCodecInfo.GetImageEncoders()
        .Select(c => c.FilenameExtension)
  .ToList();

// �� ����: Lazy<T> ���
private static Lazy<List<string>> _imageFileExtensions = 
    new Lazy<List<string>>(() => 
        ImageCodecInfo.GetImageEncoders()
            .Select(c => c.FilenameExtension)
          .ToList());

public static List<string> ImageFileExtensions => _imageFileExtensions.Value;
```

### 2. **���� ���� �� ���� ����**
```
������ Ÿ��: Ÿ�Ը� Ȯ��
��Ÿ��: ���� DLL �ʿ�
```

### 3. **���� �ڵ� �ǹ� ����**
```
0xc000027b = DLL�� ã�� �� ����
0xc0000139 = DLL�� ������ �Լ��� ã�� �� ����
0xc0000142 = DLL �ʱ�ȭ ����
```

---

## ?? ���� �������

### 1. **Dependency Checker �߰�**
```csharp
// App.xaml.cs
public App()
{
    CheckDependencies();
    this.InitializeComponent();
}

private void CheckDependencies()
{
    try
    {
        // System.Drawing.Common üũ
        var test = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
    }
    catch (Exception ex)
    {
  MessageBox.Show($"Missing dependency: {ex.Message}");
        Environment.Exit(1);
    }
}
```

### 2. **Error Handling ����**
```csharp
public static class IconHelpers
{
    private static List<string> _imageFileExtensions;
    
    public static List<string> ImageFileExtensions
    {
        get
        {
            if (_imageFileExtensions == null)
   {
     try
   {
           _imageFileExtensions = ImageCodecInfo.GetImageEncoders()
       .Select(c => c.FilenameExtension)
             .SelectMany(e => e.Split(';'))
        .Select(e => e.Replace("*", "").ToLower())
            .ToList();
   }
                catch (Exception ex)
         {
    // Log error and use fallback
         _imageFileExtensions = new List<string> 
  { 
     ".jpg", ".jpeg", ".png", ".gif", ".bmp" 
         };
  }
            }
    return _imageFileExtensions;
        }
    }
}
```

### 3. **����ȭ**
```markdown
# SLauncher Dependencies

## Required NuGet Packages:
- Microsoft.WindowsAppSDK (1.6.x)
- System.Drawing.Common (9.0.x) �� �ʼ�!
- WinUIEx (2.5.x)

## Required Runtimes:
- .NET 8.0 Runtime
- Windows Desktop Runtime
```

---

## ?? ���

**����:** System.Drawing.Common ��Ű�� �������� ���� ���� ��� ũ����

**�ذ�:** `dotnet add package System.Drawing.Common` ����

**���:** �� ���� �۵� ?

**����:** 
- Static �ʵ� �ʱ�ȭ�� �����ϰ�
- ���� ���� �� ���� ����
- ����� �α״� �׻� Ȯ��
- ������ ���� �߿�

**���� SLauncher�� ���������� ����˴ϴ�!** ??
