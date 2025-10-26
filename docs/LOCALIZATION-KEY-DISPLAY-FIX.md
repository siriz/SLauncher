# ?? �ٱ��� UI ���ҽ� Ű ǥ�� ���� �ذ� ���̵�

## ? **���� ��Ȳ:**

UI�� ���ҽ� Ű �̸��� �״�� ǥ�õǴ� ����:
- "AppTitle" (��� "SLauncher - Modern app launcher for Windows")
- "SearchPlaceholder" (��� "Search through everything...")
- "AddFileButton" (��� "Add a file")
- ���...

---

## ?? **���� ����:**

`LocalizationManager`�� ���ҽ� ����(`.resw`)�� ����� ã�� ���ϰų� �ε����� ����

**������ ���ε�:**
1. **���ҽ� ���� ��� ����:** ���� ���ϰ� �ٸ� ��ġ�� ����
2. **���� ���� ����:** ���� �� ���ҽ� ������ ��� ���丮�� ������� ����
3. **���ڵ� ����:** UTF-8 BOM �������� �ѱ�/�Ϻ�� ����
4. **�ʱ�ȭ ���� ����:** LocalizationManager�� �ʹ� �ʰ� �ʱ�ȭ��

---

## ? **����� �ذ�å:**

### **1. ���� ��� �˻� �߰�**

**����:** `SLauncher/Classes/LocalizationManager.cs`

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

**����:** ���� ȯ�濡 ���� �⺻ ���丮�� �ٸ� �� ����

---

### **2. ���� ����� �α�**

**����:** `SLauncher/Classes/LocalizationManager.cs`

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

### **3. ���� ���� �߰�**

**����:** `SLauncher/Classes/LocalizationDiagnostics.cs` (�ű� ����)

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

**���:**
```csharp
// MainWindow.xaml.cs - Container_Loaded()
LocalizationDiagnostics.RunDiagnostics();
```

---

### **4. �ʱ�ȭ ���� ����ȭ**

**����:** `SLauncher/App.xaml.cs`

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

### **5. UI �ʱ�ȭ ����**

**����:** `SLauncher/MainWindow.UI.cs`

```csharp
private void InitializeLocalizedUI()
{
    System.Diagnostics.Debug.WriteLine("[MainWindow.UI] Initializing localized UI...");
    
    // Set with logging
 string appTitle = LocalizationManager.GetString("AppTitle");
  System.Diagnostics.Debug.WriteLine($"[MainWindow.UI] AppTitle: {appTitle}");
    this.Title = appTitle;
    
    // ... �� �׸񸶴� �α�
    
    System.Diagnostics.Debug.WriteLine("[MainWindow.UI] Localized UI initialized successfully");
}
```

---

### **6. ���� ���� ����**

**����:** `SLauncher/SLauncher.csproj`

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

## ?? **����� ��� Ȯ�� ���:**

### **1. Visual Studio���� ����� ����:**

```
1. F5 Ű �Ǵ� "����� ����" Ŭ��
2. ���� ����Ǹ� Visual Studio�� ���ư���
3. �ϴ��� "���(Output)" â Ȯ��
4. ��Ӵٿ�� "�����" ����
```

### **2. ���� ���:**

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
     [AppTitle] = SLauncher - Windows�� ��� �� ��ó
     [SearchPlaceholder] = SLauncher�� ��� �׸� �˻�
     [AddFileButton] = ���� �߰�
   ja-JP: FOUND at D:\...\Strings\ja-JP\Resources.resw
   - Parsing: SUCCESS
  [AppTitle] = SLauncher - Windows�ī���󫢫׫������?
     [SearchPlaceholder] = SLauncher�Ϊ��٪ƪ�?��
     [AddFileButton] = �ի��������ʥ

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

### **3. ������ �ִ� ��� ���:**

```
? **��θ� ã�� �� ����:**
[LocalizationManager] Resource base path: D:\...\Strings
[LocalizationManager] Base path exists: False
[LocalizationManager] ERROR: Resource file not found at D:\...\Strings\en-US\Resources.resw

? **������ ������� ����:**
1. Checking base paths:
 D:\...\bin\x64\Debug\...\Strings: NOT FOUND

? **Ű�� ã�� �� ����:**
3. Current LocalizationManager state:
   [AppTitle] = AppTitle ? (KEY NOT FOUND)
   [SearchPlaceholder] = SearchPlaceholder ? (KEY NOT FOUND)

? **���ڵ� ���� (�ѱ� ����):**
   ko-KR: FOUND
   - Parsing: SUCCESS
     [AppTitle] = SLauncher - Windows?? ??? ?? ???  �� ���� ����
```

---

## ?? **������ �ذ� ���:**

### **���� 1: ���ҽ� ������ ã�� �� ����**

**����:**
```
[LocalizationManager] Base path exists: False
[LocalizationManager] ERROR: Resource file not found
```

**�ذ�:**
1. **�������� ���� ����:**
```powershell
Copy-Item -Path "D:\Works\Playground\C#\SLauncher\SLauncher\Strings" `
          -Destination "D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\" `
       -Recurse -Force
```

2. **Clean & Rebuild:**
```
Visual Studio �� ���� �� �ַ�� ����
Visual Studio �� ���� �� �ַ�� �ٽ� ����
```

3. **���� ��� Ȯ��:**
```
��� â���� "Copied Strings folder to output directory" �޽��� Ȯ��
```

---

### **���� 2: ���ҽ� Ű�� �״�� ǥ�õ�**

**����:**
```
UI�� "AppTitle", "SearchPlaceholder" ���� �״�� ǥ��
```

**�ذ�:**
1. **���� ���� Ȯ��:**
```
����� ��¿��� "KEY NOT FOUND" Ȯ��
```

2. **���ҽ� ���� ���� Ȯ��:**
```powershell
# ���� ���ҽ� Ȯ��
$xml = [xml](Get-Content "Strings\en-US\Resources.resw" -Encoding UTF8)
$xml.root.data | Where-Object { $_.name -eq "AppTitle" }
```

3. **GetString ��ȯ�� Ȯ��:**
```csharp
string value = LocalizationManager.GetString("AppTitle");
System.Diagnostics.Debug.WriteLine($"AppTitle value: {value}");
// "AppTitle"�� ��ȯ�Ǹ� ���ҽ��� ã�� ���� ��
```

---

### **���� 3: �ѱ�/�Ϻ�� ������ ǥ�õ�**

**����:**
```
�ѱ��� ���� ��: "SLauncher - Windows?? ??? ?? ???"
�Ϻ��� ���� ��: "SLauncher - Windows??????????????????????"
```

**�ذ�:**
1. **UTF-8 BOM���� ������:**
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
Visual Studio �� ���� �� �ַ�� �ٽ� ����
```

---

### **���� 4: �ʱ�ȭ ���� ����**

**����:**
```
[MainWindow.UI] AppTitle: AppTitle  �� Ű �״�� ��ȯ
```

**Ȯ��:**
```
����� ��¿��� ���� Ȯ��:
1. [App] Initializing LocalizationManager...
2. [LocalizationManager] Successfully loaded X resources
3. [MainWindow.UI] Initializing localized UI...
```

**����:**
- `App.xaml.cs`���� LocalizationManager�� ���� �ʱ�ȭ
- `MainWindow.xaml.cs`���� Container_Loaded ���� �κп��� InitializeLocalizedUI ȣ��

---

## ?? **���� ���� Ȯ��:**

**�������� ���� ����:**
```
SLauncher\
������ bin\
��   ������ x64\
��       ������ Debug\
��      ������ net8.0-windows10.0.22621.0\
��           ������ win-x64\
��       ������ SLauncher.exe
��           ������ Strings\           �� �� ������ �־�� ��!
��   ������ en-US\
��  ��   ������ Resources.resw
��               ������ ko-KR\
��  ��   ������ Resources.resw
��       ������ ja-JP\
��             ������ Resources.resw
������ Strings\ �� ���� �ҽ�
    ������ en-US\
    ��   ������ Resources.resw
    ������ ko-KR\
    ��   ������ Resources.resw
    ������ ja-JP\
      ������ Resources.resw
```

**Ȯ�� ���:**
```powershell
# ��� ���丮�� Strings ���� Ȯ��
Get-ChildItem "bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\Strings" -Recurse -Filter "*.resw"

# ��� ����:
# en-US\Resources.resw
# ko-KR\Resources.resw
# ja-JP\Resources.resw
```

---

## ? **���� üũ����Ʈ:**

### **���� ��:**
- [ ] "Copied Strings folder to output directory" �޽��� Ȯ��
- [ ] bin\...\win-x64\Strings ���� ���� Ȯ��
- [ ] �� �� Resources.resw ���� ���� Ȯ��

### **���� ��:**
- [ ] "[App] LocalizationManager initialized" �޽��� Ȯ��
- [ ] "[LocalizationManager] Successfully loaded X resources" Ȯ��
- [ ] "[MainWindow.UI] Localized UI initialized successfully" Ȯ��
- [ ] ���� ������� ��� Ű�� ? ǥ��

### **UI Ȯ��:**
- [ ] â ������ "SLauncher - Modern app launcher..." ǥ��
- [ ] �˻�â �÷��̽�Ȧ���� ����� ǥ��
- [ ] ��ư �ؽ�Ʈ�� "Add a file", "Add a folder" ������ ǥ��
- [ ] �� ȭ�� �޽����� ����� ǥ��

### **��� ��ȯ:**
- [ ] ���� �� Language �� �ѱ��� ����
- [ ] ����� �� �ѱ۷� ǥ�õ� (������ ����)
- [ ] ���� �� Language �� ������ ����
- [ ] ����� �� �Ϻ���� ǥ�õ� (������ ����)

---

## ?? **������ ���� ���:**

1. **`SLauncher/Classes/LocalizationManager.cs`**
   - ���� ��� �˻� �߰�
   - ���� ����� �α�
   - ����� UTF-8 ���ڵ�

2. **`SLauncher/Classes/LocalizationDiagnostics.cs`** (�ű�)
   - ���� ���� �߰�

3. **`SLauncher/App.xaml.cs`**
   - �ʱ�ȭ ���� �α� �߰�

4. **`SLauncher/MainWindow.UI.cs`**
   - InitializeLocalizedUI ����
   - ���� �α� �߰�

5. **`SLauncher/MainWindow.xaml.cs`**
   - Container_Loaded�� ���� �߰�

6. **`SLauncher/SLauncher.csproj`**
   - ���� Ÿ������ �ڵ� ���� ����

---

## ?? **���� �� �׽�Ʈ:**

### **1. Clean & Rebuild:**
```
Visual Studio �� ���� �� �ַ�� ����
Visual Studio �� ���� �� �ַ�� �ٽ� ����
```

### **2. ����� ����:**
```
F5 �Ǵ� "����� ����"
```

### **3. ��� â Ȯ��:**
```
Visual Studio �� ���� �� ���
��Ӵٿ� �� "�����" ����
```

### **4. ���� ��� Ȯ��:**
```
��� ��� ���� FOUND ?
��� �׽�Ʈ Ű ?
UI �ʱ�ȭ ���� ?
```

---

## ?? **�߰� ��:**

### **������ ��ӵǸ�:**

1. **���� ���� ����:**
```powershell
$source = "D:\Works\Playground\C#\SLauncher\SLauncher\Strings"
$dest = "D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\Strings"
Copy-Item -Path $source -Destination $dest -Recurse -Force
```

2. **���� ��� ���丮 ����:**
```powershell
Remove-Item "bin\x64\Debug" -Recurse -Force
```

3. **������Ʈ ��ε�:**
```
Visual Studio �� ������Ʈ ��ε� �� ������Ʈ �ٽ� �ε�
```

4. **Visual Studio �����:**
```
��� ���� ���� �� Visual Studio ���� �� �����
```

---

## ? **���� UI�� ���������� �ؽ�Ʈ�� ǥ�õ˴ϴ�!**

**Before:**
- ? UI�� "AppTitle", "SearchPlaceholder" �� Ű �̸��� ǥ�õ�
- ? ���ҽ� ������ ã�� ����
- ? �ѱ�/�Ϻ�� ����

**After:**
- ? UI�� �������� �ؽ�Ʈ ǥ��
- ? ���ҽ� ���� ���� �ε�
- ? ��� �� ����� ǥ�õ�
- ? ���� ������ ���� �ľ� ����

**����� ������� ��� ������ ������ �� �ֽ��ϴ�!** ??
