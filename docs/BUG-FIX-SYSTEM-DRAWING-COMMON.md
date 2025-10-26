# ?? Critical Bug Fix - System.Drawing.Common Missing

## ? 문제 발생

### 증상:
```
앱을 실행하면 아무 일도 일어나지 않음
프로세스가 즉시 종료됨
에러 코드: 0xc000027b (STATUS_DLL_NOT_FOUND)
```

### 디버그 로그:
```
'[37460] SLauncher.exe' 프로그램이 종료되었습니다(코드: 3221226107 (0xc000027b)).
```

**에러 코드 의미:**
- `0xc000027b` = `STATUS_DLL_NOT_FOUND`
- 필요한 DLL 파일을 찾을 수 없음

---

## ?? 원인 분석

### 문제의 코드 (IconHelpers.cs):
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

**문제:**
- `System.Drawing.Imaging.ImageCodecInfo`를 사용 중
- 이 클래스는 **System.Drawing.Common** 라이브러리에 있음
- WinUI 3 프로젝트에 이 패키지가 **누락**됨

### 왜 빌드는 성공했나?
```
1. IconHelpers는 static 클래스
2. ImageFileExtensions는 static 필드
3. 컴파일 타임에는 타입만 확인
4. 런타임에 static 필드 초기화 시 DLL을 찾지 못해 크래시
```

---

## ? 해결 방법

### 1. System.Drawing.Common 패키지 추가

```bash
cd "D:\Works\Playground\C#\SLauncher\SLauncher"
dotnet add package System.Drawing.Common
```

**결과:**
```
info : 'System.Drawing.Common' 패키지 '9.0.10' 버전에 대한 PackageReference가 추가되었습니다.
```

### 2. SLauncher.csproj 변경사항

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
  <PackageReference Include="System.Drawing.Common" Version="9.0.10" />  ← 추가
  <PackageReference Include="System.Text.Json" Version="9.0.0" />
  <PackageReference Include="WinUIEx" Version="2.5.0" />
</ItemGroup>
```

---

## ?? System.Drawing.Common 사용 위치

### IconHelpers.cs
```csharp
using System.Drawing;
using System.Drawing.Imaging;

public static class IconHelpers
{
    // 1. ImageCodecInfo 사용
    public static List<string> ImageFileExtensions = ImageCodecInfo.GetImageEncoders()
    .Select(c => c.FilenameExtension)
     .SelectMany(e => e.Split(';'))
   .Select(e => e.Replace("*", "").ToLower())
        .ToList();

    // 2. System.Drawing.Bitmap 사용
    private static System.Drawing.Bitmap ResizeJumbo(System.Drawing.Bitmap imgToResize, System.Drawing.Size size)
    {
   System.Drawing.Bitmap b = new System.Drawing.Bitmap(size.Width, size.Height);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)b);
 // ...
    }

    // 3. System.Drawing.Color 사용
    private static bool IsScaledDown(System.Drawing.Bitmap bitmap)
    {
     System.Drawing.Color empty = System.Drawing.Color.FromArgb(0, 0, 0, 0);
// ...
    }

    // 4. System.Drawing.Icon 사용
    private async static Task<BitmapImage> GetPathIconWin32(string path, bool isDirectory)
    {
        // ...
        System.Drawing.Icon ico = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon).Clone();
        System.Drawing.Bitmap bitmapIcon = ico.ToBitmap();
      // ...
    }
}
```

**총 사용 횟수:**
- `System.Drawing.Bitmap`: 여러 곳
- `System.Drawing.Graphics`: 1곳
- `System.Drawing.Icon`: 1곳
- `System.Drawing.Color`: 1곳
- `System.Drawing.Imaging.ImageCodecInfo`: 1곳
- `System.Drawing.Size`: 1곳

---

## ?? System.Drawing.Common에 대한 주의사항

### Microsoft의 공식 입장:

> **System.Drawing.Common은 Windows에서만 지원됩니다**
> - .NET 6부터 Linux/macOS에서는 지원 중단
> - Windows에서는 계속 지원됨
> - 참조: https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only

**SLauncher는 Windows 전용 앱이므로 문제없음 ?**

### 대안 (필요 시):
```
1. SkiaSharp - 크로스 플랫폼 그래픽 라이브러리
2. ImageSharp - 순수 .NET 이미지 처리 라이브러리
3. WinUI 3 네이티브 API - 하지만 기능 제한적
```

**현재는 System.Drawing.Common 유지:**
- Windows 전용 앱
- 안정적이고 검증됨
- Win32 API와 호환 필요

---

## ?? 디버깅 과정

### 1. 증상 확인
```
? 빌드 성공
? 앱 실행 즉시 종료
? 에러 메시지 없음
```

### 2. 디버그 로그 확인
```powershell
# Visual Studio Output Window
Debug -> Windows -> Output
```

**발견된 에러:**
```
'SLauncher.exe' 프로그램이 종료되었습니다(코드: 3221226107 (0xc000027b)).
```

### 3. 에러 코드 분석
```
0xc000027b = STATUS_DLL_NOT_FOUND
→ 필요한 DLL이 없음
```

### 4. DLL 종속성 확인
```powershell
# Dependencies.exe 또는 dumpbin 사용
dumpbin /dependents SLauncher.exe
```

### 5. 코드 검토
```csharp
// IconHelpers.cs 첫 줄부터 크래시 발생 가능성
public static List<string> ImageFileExtensions = ImageCodecInfo.GetImageEncoders()
// ↑ static 필드 초기화 = 앱 시작 시 즉시 실행
```

### 6. 패키지 확인
```bash
dotnet list package
# System.Drawing.Common 없음 발견!
```

### 7. 패키지 추가 및 해결
```bash
dotnet add package System.Drawing.Common
# ? 문제 해결!
```

---

## ?? 재발 방지

### 1. 프로젝트 템플릿 업데이트
```xml
<!-- SLauncher.csproj 필수 패키지 -->
<ItemGroup>
  <!-- WinUI 3 Core -->
  <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
  <PackageReference Include="WinUIEx" Version="2.5.0" />
  
  <!-- UI Components -->
  <PackageReference Include="CommunityToolkit.WinUI.Controls.SettingsControls" Version="8.1.240916" />
  <PackageReference Include="CommunityToolkit.WinUI.Extensions" Version="8.1.240916" />
  
  <!-- System Libraries -->
  <PackageReference Include="System.Drawing.Common" Version="9.0.10" />  ← 필수!
  <PackageReference Include="System.Text.Json" Version="9.0.0" />
  
  <!-- Build Tools -->
  <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.26100.1742" />
</ItemGroup>
```

### 2. 빌드 스크립트 업데이트
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

### 3. CI/CD 파이프라인
```yaml
# .github/workflows/build.yml
- name: Verify Dependencies
  run: |
    dotnet list package | grep "System.Drawing.Common" || dotnet add package System.Drawing.Common
```

---

## ?? 유사한 문제 진단 방법

### 앱이 시작 안 될 때 체크리스트:

#### 1. **빌드 에러 확인**
```
? 빌드 성공했나?
? 경고는 없나?
```

#### 2. **디버그 로그 확인**
```
Visual Studio:
- Debug → Windows → Output
- Show output from: Debug
```

#### 3. **Event Viewer 확인**
```
Windows + X → Event Viewer
→ Windows Logs → Application
→ 최근 에러 확인
```

#### 4. **Dependencies 확인**
```powershell
# Dependencies.exe 다운로드
# https://github.com/lucasg/Dependencies
Dependencies.exe SLauncher.exe
```

#### 5. **Missing DLLs 확인**
```
- STATUS_DLL_NOT_FOUND (0xc000027b)
- ENTRYPOINT_NOT_FOUND (0xc0000139)
- DLL_INIT_FAILED (0xc0000142)
```

#### 6. **패키지 복원**
```bash
dotnet restore
dotnet clean
dotnet build
```

#### 7. **런타임 확인**
```bash
# .NET Runtime 설치 확인
dotnet --list-runtimes

# 필요한 런타임:
- Microsoft.NETCore.App 8.0.x
- Microsoft.WindowsDesktop.App 8.0.x
```

---

## ? 해결 확인

### Before:
```
? 앱 실행 → 즉시 종료
? 에러 코드: 0xc000027b
? System.Drawing.Common 누락
```

### After:
```
? System.Drawing.Common 9.0.10 설치
? 빌드 성공
? 앱 정상 실행
? 아이콘 로딩 정상 작동
```

---

## ?? 학습 포인트

### 1. **Static 필드 초기화 주의**
```csharp
// 위험: 앱 시작 시 즉시 실행
public static List<string> ImageFileExtensions = ImageCodecInfo.GetImageEncoders()
           .Select(c => c.FilenameExtension)
    .ToList();

// 안전: 필요할 때만 실행
public static List<string> ImageFileExtensions => 
    ImageCodecInfo.GetImageEncoders()
        .Select(c => c.FilenameExtension)
  .ToList();

// 더 안전: Lazy<T> 사용
private static Lazy<List<string>> _imageFileExtensions = 
    new Lazy<List<string>>(() => 
        ImageCodecInfo.GetImageEncoders()
            .Select(c => c.FilenameExtension)
          .ToList());

public static List<string> ImageFileExtensions => _imageFileExtensions.Value;
```

### 2. **빌드 성공 ≠ 실행 성공**
```
컴파일 타임: 타입만 확인
런타임: 실제 DLL 필요
```

### 3. **에러 코드 의미 이해**
```
0xc000027b = DLL을 찾을 수 없음
0xc0000139 = DLL은 있지만 함수를 찾을 수 없음
0xc0000142 = DLL 초기화 실패
```

---

## ?? 향후 권장사항

### 1. **Dependency Checker 추가**
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
        // System.Drawing.Common 체크
        var test = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
    }
    catch (Exception ex)
    {
  MessageBox.Show($"Missing dependency: {ex.Message}");
        Environment.Exit(1);
    }
}
```

### 2. **Error Handling 개선**
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

### 3. **문서화**
```markdown
# SLauncher Dependencies

## Required NuGet Packages:
- Microsoft.WindowsAppSDK (1.6.x)
- System.Drawing.Common (9.0.x) ← 필수!
- WinUIEx (2.5.x)

## Required Runtimes:
- .NET 8.0 Runtime
- Windows Desktop Runtime
```

---

## ?? 결론

**문제:** System.Drawing.Common 패키지 누락으로 앱이 시작 즉시 크래시

**해결:** `dotnet add package System.Drawing.Common` 실행

**결과:** 앱 정상 작동 ?

**교훈:** 
- Static 필드 초기화는 신중하게
- 빌드 성공 ≠ 실행 성공
- 디버그 로그는 항상 확인
- 의존성 관리 중요

**이제 SLauncher가 정상적으로 실행됩니다!** ??
