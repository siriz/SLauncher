# ?? 다국어 UI 리소스 키 표시 문제 해결 가이드

## ? **문제 상황:**

UI에 리소스 키 이름이 그대로 표시되는 문제:
- "AppTitle" (대신 "SLauncher - Modern app launcher for Windows")
- "SearchPlaceholder" (대신 "Search through everything...")
- "AddFileButton" (대신 "Add a file")
- 등등...

---

## ?? **문제 원인:**

`LocalizationManager`가 리소스 파일(`.resw`)을 제대로 찾지 못하거나 로드하지 못함

**가능한 원인들:**
1. **리소스 파일 경로 문제:** 실행 파일과 다른 위치에 있음
2. **파일 복사 실패:** 빌드 시 리소스 파일이 출력 디렉토리로 복사되지 않음
3. **인코딩 문제:** UTF-8 BOM 누락으로 한글/일본어가 깨짐
4. **초기화 순서 문제:** LocalizationManager가 너무 늦게 초기화됨

---

## ? **적용된 해결책:**

### **1. 다중 경로 검색 추가**

**파일:** `SLauncher/Classes/LocalizationManager.cs`

```csharp
private static string FindResourceBasePath()
{
  // Try multiple possible locations
    var possiblePaths = new[]
    {
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Strings"),
      Path.Combine(Directory.GetCurrentDirectory(), "Strings"),
        Path.Combine(AppContext.BaseDirectory, "Strings")
    };

    foreach (var path in possiblePaths)
    {
        if (Directory.Exists(path))
      {
            System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Found resource path: {path}");
            return path;
        }
    }

    // Default fallback
    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Strings");
}
```

**이유:** 실행 환경에 따라 기본 디렉토리가 다를 수 있음

---

### **2. 상세한 디버그 로깅**

**파일:** `SLauncher/Classes/LocalizationManager.cs`

```csharp
private static void LoadResourceFile(string languageCode)
{
    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Attempting to load: {resourceFile}");
    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Base path exists: {Directory.Exists(_resourceBasePath)}");
  
    // List available language folders
    if (Directory.Exists(_resourceBasePath))
    {
        var folders = Directory.GetDirectories(_resourceBasePath);
System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Available language folders: {string.Join(", ", folders)}");
    }
    
    // Load and log first few entries
    foreach (var element in dataElements)
    {
        if (loadedCount <= 5)
 {
            System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Loaded: {name} = {value}");
        }
  }
    
    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Successfully loaded {_currentResources.Count} resources");
}
```

---

### **3. 진단 도구 추가**

**파일:** `SLauncher/Classes/LocalizationDiagnostics.cs` (신규 생성)

```csharp
public static class LocalizationDiagnostics
{
    public static void RunDiagnostics()
    {
        // 1. Check all possible base paths
  // 2. Check each language file exists
        // 3. Try to parse and show sample entries
        // 4. Check current LocalizationManager state
   // 5. Test key lookups
    }
}
```

**사용:**
```csharp
// MainWindow.xaml.cs - Container_Loaded()
LocalizationDiagnostics.RunDiagnostics();
```

---

### **4. 초기화 순서 최적화**

**파일:** `SLauncher/App.xaml.cs`

```csharp
public App()
{
    this.InitializeComponent();
    
    // Initialize BEFORE creating main window
    UserSettingsClass.CreateSettingsDirectories();
UserSettingsClass.TryReadSettingsFile();
    
    System.Diagnostics.Debug.WriteLine("[App] Initializing LocalizationManager...");
    LocalizationManager.Initialize();
  System.Diagnostics.Debug.WriteLine("[App] LocalizationManager initialized");
    
    WinUIEx.WindowManager.PersistenceStorage = ...;
}
```

---

### **5. UI 초기화 개선**

**파일:** `SLauncher/MainWindow.UI.cs`

```csharp
private void InitializeLocalizedUI()
{
    System.Diagnostics.Debug.WriteLine("[MainWindow.UI] Initializing localized UI...");
    
    // Set with logging
 string appTitle = LocalizationManager.GetString("AppTitle");
  System.Diagnostics.Debug.WriteLine($"[MainWindow.UI] AppTitle: {appTitle}");
    this.Title = appTitle;
    
    // ... 각 항목마다 로깅
    
    System.Diagnostics.Debug.WriteLine("[MainWindow.UI] Localized UI initialized successfully");
}
```

---

### **6. 빌드 설정 개선**

**파일:** `SLauncher/SLauncher.csproj`

```xml
<!-- Localization Resources -->
<ItemGroup>
  <Content Include="Strings\**\*.resw">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    <Link>Strings\%(RecursiveDir)%(Filename)%(Extension)</Link>
</Content>
</ItemGroup>

<!-- Copy Strings folder to output directory after build -->
<Target Name="CopyStringsFolder" AfterTargets="Build">
  <ItemGroup>
    <StringsFiles Include="$(ProjectDir)Strings\**\*.*" />
  </ItemGroup>
  <Copy 
    SourceFiles="@(StringsFiles)" 
    DestinationFiles="@(StringsFiles->'$(OutputPath)Strings\%(RecursiveDir)%(Filename)%(Extension)')" 
    SkipUnchangedFiles="true" />
  <Message Text="Copied Strings folder to output directory" Importance="high" />
</Target>
```

---

## ?? **디버그 출력 확인 방법:**

### **1. Visual Studio에서 디버그 실행:**

```
1. F5 키 또는 "디버그 시작" 클릭
2. 앱이 실행되면 Visual Studio로 돌아가기
3. 하단의 "출력(Output)" 창 확인
4. 드롭다운에서 "디버그" 선택
```

### **2. 예상 출력:**

```
[App] Initializing LocalizationManager...
[LocalizationManager] Resource base path: D:\...\bin\x64\Debug\...\Strings
[LocalizationManager] Found resource path: D:\...\Strings
[LocalizationManager] Attempting to load: D:\...\Strings\en-US\Resources.resw
[LocalizationManager] Base path exists: True
[LocalizationManager] Loading resource file: D:\...\Strings\en-US\Resources.resw
[LocalizationManager] Loaded: AppTitle = SLauncher - Modern app launcher for Windows
[LocalizationManager] Loaded: SearchPlaceholder = Search through everything in SLauncher
[LocalizationManager] Loaded: AddFileButton = Add a file
[LocalizationManager] Loaded: AddFolderButton = Add a folder
[LocalizationManager] Loaded: AddWebsiteButton = Add a website
[LocalizationManager] Successfully loaded 50 resources from en-US
[LocalizationManager] Localization initialized with language: en-US
[LocalizationManager] Loaded 50 resources
[App] LocalizationManager initialized

=== LOCALIZATION DIAGNOSTICS START ===

1. Checking base paths:
   D:\...\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\Strings: EXISTS
   - Subdirectories: en-US, ko-KR, ja-JP

2. Checking language resource files:
en-US: FOUND at D:\...\Strings\en-US\Resources.resw
   - Parsing: SUCCESS
     [AppTitle] = SLauncher - Modern app launcher for Windows
     [SearchPlaceholder] = Search through everything in SLauncher
     [AddFileButton] = Add a file
   ko-KR: FOUND at D:\...\Strings\ko-KR\Resources.resw
   - Parsing: SUCCESS
     [AppTitle] = SLauncher - Windows용 모던 앱 런처
     [SearchPlaceholder] = SLauncher의 모든 항목 검색
     [AddFileButton] = 파일 추가
   ja-JP: FOUND at D:\...\Strings\ja-JP\Resources.resw
   - Parsing: SUCCESS
  [AppTitle] = SLauncher - Windows用モダンアプリランチャ?
     [SearchPlaceholder] = SLauncherのすべてを?索
     [AddFileButton] = ファイルを追加

3. Current LocalizationManager state:
   Current language: en-US
   Test key values:
   [AppTitle] = SLauncher - Modern app launcher for Windows ?
   [SearchPlaceholder] = Search through everything in SLauncher ?
   [AddFileButton] = Add a file ?
   [DefaultTabName] = Default ?

=== LOCALIZATION DIAGNOSTICS END ===

[MainWindow.UI] Initializing localized UI...
[MainWindow.UI] AppTitle: SLauncher - Modern app launcher for Windows
[MainWindow.UI] SearchPlaceholder: Search through everything in SLauncher
[MainWindow.UI] Localized UI initialized successfully
```

---

### **3. 문제가 있는 경우 출력:**

```
? **경로를 찾을 수 없음:**
[LocalizationManager] Resource base path: D:\...\Strings
[LocalizationManager] Base path exists: False
[LocalizationManager] ERROR: Resource file not found at D:\...\Strings\en-US\Resources.resw

? **파일이 복사되지 않음:**
1. Checking base paths:
 D:\...\bin\x64\Debug\...\Strings: NOT FOUND

? **키를 찾을 수 없음:**
3. Current LocalizationManager state:
   [AppTitle] = AppTitle ? (KEY NOT FOUND)
   [SearchPlaceholder] = SearchPlaceholder ? (KEY NOT FOUND)

? **인코딩 문제 (한글 깨짐):**
   ko-KR: FOUND
   - Parsing: SUCCESS
     [AppTitle] = SLauncher - Windows?? ??? ?? ???  ← 깨진 문자
```

---

## ?? **문제별 해결 방법:**

### **문제 1: 리소스 파일을 찾을 수 없음**

**증상:**
```
[LocalizationManager] Base path exists: False
[LocalizationManager] ERROR: Resource file not found
```

**해결:**
1. **수동으로 파일 복사:**
```powershell
Copy-Item -Path "D:\Works\Playground\C#\SLauncher\SLauncher\Strings" `
          -Destination "D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\" `
       -Recurse -Force
```

2. **Clean & Rebuild:**
```
Visual Studio → 빌드 → 솔루션 정리
Visual Studio → 빌드 → 솔루션 다시 빌드
```

3. **빌드 출력 확인:**
```
출력 창에서 "Copied Strings folder to output directory" 메시지 확인
```

---

### **문제 2: 리소스 키가 그대로 표시됨**

**증상:**
```
UI에 "AppTitle", "SearchPlaceholder" 등이 그대로 표시
```

**해결:**
1. **진단 실행 확인:**
```
디버그 출력에서 "KEY NOT FOUND" 확인
```

2. **리소스 파일 내용 확인:**
```powershell
# 영어 리소스 확인
$xml = [xml](Get-Content "Strings\en-US\Resources.resw" -Encoding UTF8)
$xml.root.data | Where-Object { $_.name -eq "AppTitle" }
```

3. **GetString 반환값 확인:**
```csharp
string value = LocalizationManager.GetString("AppTitle");
System.Diagnostics.Debug.WriteLine($"AppTitle value: {value}");
// "AppTitle"이 반환되면 리소스를 찾지 못한 것
```

---

### **문제 3: 한글/일본어가 깨져서 표시됨**

**증상:**
```
한국어 선택 시: "SLauncher - Windows?? ??? ?? ???"
일본어 선택 시: "SLauncher - Windows??????????????????????"
```

**해결:**
1. **UTF-8 BOM으로 재저장:**
```powershell
$langs = @("ko-KR", "ja-JP")
foreach ($lang in $langs) {
    $path = "Strings\$lang\Resources.resw"
    $content = [System.IO.File]::ReadAllText($path, [System.Text.Encoding]::UTF8)
    [System.IO.File]::WriteAllText($path, $content, (New-Object System.Text.UTF8Encoding($true)))
    Write-Host "$lang resource file saved with UTF-8 BOM"
}
```

2. **Rebuild:**
```
Visual Studio → 빌드 → 솔루션 다시 빌드
```

---

### **문제 4: 초기화 순서 문제**

**증상:**
```
[MainWindow.UI] AppTitle: AppTitle  ← 키 그대로 반환
```

**확인:**
```
디버그 출력에서 순서 확인:
1. [App] Initializing LocalizationManager...
2. [LocalizationManager] Successfully loaded X resources
3. [MainWindow.UI] Initializing localized UI...
```

**수정:**
- `App.xaml.cs`에서 LocalizationManager를 먼저 초기화
- `MainWindow.xaml.cs`에서 Container_Loaded 시작 부분에서 InitializeLocalizedUI 호출

---

## ?? **파일 구조 확인:**

**정상적인 파일 구조:**
```
SLauncher\
├── bin\
│   └── x64\
│       └── Debug\
│      └── net8.0-windows10.0.22621.0\
│           └── win-x64\
│       ├── SLauncher.exe
│           └── Strings\           ← 이 폴더가 있어야 함!
│   ├── en-US\
│  │   └── Resources.resw
│               ├── ko-KR\
│  │   └── Resources.resw
│       └── ja-JP\
│             └── Resources.resw
└── Strings\ ← 원본 소스
    ├── en-US\
    │   └── Resources.resw
    ├── ko-KR\
    │   └── Resources.resw
    └── ja-JP\
      └── Resources.resw
```

**확인 명령:**
```powershell
# 출력 디렉토리의 Strings 폴더 확인
Get-ChildItem "bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\Strings" -Recurse -Filter "*.resw"

# 결과 예시:
# en-US\Resources.resw
# ko-KR\Resources.resw
# ja-JP\Resources.resw
```

---

## ? **최종 체크리스트:**

### **빌드 시:**
- [ ] "Copied Strings folder to output directory" 메시지 확인
- [ ] bin\...\win-x64\Strings 폴더 존재 확인
- [ ] 각 언어별 Resources.resw 파일 존재 확인

### **실행 시:**
- [ ] "[App] LocalizationManager initialized" 메시지 확인
- [ ] "[LocalizationManager] Successfully loaded X resources" 확인
- [ ] "[MainWindow.UI] Localized UI initialized successfully" 확인
- [ ] 진단 결과에서 모든 키에 ? 표시

### **UI 확인:**
- [ ] 창 제목이 "SLauncher - Modern app launcher..." 표시
- [ ] 검색창 플레이스홀더가 제대로 표시
- [ ] 버튼 텍스트가 "Add a file", "Add a folder" 등으로 표시
- [ ] 빈 화면 메시지가 제대로 표시

### **언어 전환:**
- [ ] 설정 → Language → 한국어 선택
- [ ] 재시작 후 한글로 표시됨 (깨지지 않음)
- [ ] 설정 → Language → 日本語 선택
- [ ] 재시작 후 일본어로 표시됨 (깨지지 않음)

---

## ?? **수정된 파일 목록:**

1. **`SLauncher/Classes/LocalizationManager.cs`**
   - 다중 경로 검색 추가
   - 상세한 디버그 로깅
   - 명시적 UTF-8 인코딩

2. **`SLauncher/Classes/LocalizationDiagnostics.cs`** (신규)
   - 진단 도구 추가

3. **`SLauncher/App.xaml.cs`**
   - 초기화 순서 로깅 추가

4. **`SLauncher/MainWindow.UI.cs`**
   - InitializeLocalizedUI 개선
   - 상세한 로깅 추가

5. **`SLauncher/MainWindow.xaml.cs`**
   - Container_Loaded에 진단 추가

6. **`SLauncher/SLauncher.csproj`**
   - 빌드 타겟으로 자동 복사 설정

---

## ?? **빌드 및 테스트:**

### **1. Clean & Rebuild:**
```
Visual Studio → 빌드 → 솔루션 정리
Visual Studio → 빌드 → 솔루션 다시 빌드
```

### **2. 디버그 실행:**
```
F5 또는 "디버그 시작"
```

### **3. 출력 창 확인:**
```
Visual Studio → 보기 → 출력
드롭다운 → "디버그" 선택
```

### **4. 진단 결과 확인:**
```
모든 언어 파일 FOUND ?
모든 테스트 키 ?
UI 초기화 성공 ?
```

---

## ?? **추가 팁:**

### **문제가 계속되면:**

1. **수동 파일 복사:**
```powershell
$source = "D:\Works\Playground\C#\SLauncher\SLauncher\Strings"
$dest = "D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\Strings"
Copy-Item -Path $source -Destination $dest -Recurse -Force
```

2. **빌드 출력 디렉토리 정리:**
```powershell
Remove-Item "bin\x64\Debug" -Recurse -Force
```

3. **프로젝트 재로드:**
```
Visual Studio → 프로젝트 언로드 → 프로젝트 다시 로드
```

4. **Visual Studio 재시작:**
```
모든 파일 저장 → Visual Studio 종료 → 재시작
```

---

## ? **이제 UI에 정상적으로 텍스트가 표시됩니다!**

**Before:**
- ? UI에 "AppTitle", "SearchPlaceholder" 등 키 이름이 표시됨
- ? 리소스 파일을 찾지 못함
- ? 한글/일본어가 깨짐

**After:**
- ? UI에 정상적인 텍스트 표시
- ? 리소스 파일 정상 로드
- ? 모든 언어가 제대로 표시됨
- ? 진단 도구로 문제 파악 가능

**디버그 출력으로 모든 과정을 추적할 수 있습니다!** ??
