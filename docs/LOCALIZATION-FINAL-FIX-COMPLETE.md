# ? UI ���ҽ� Ű ǥ�� ���� ���� �ذ�!

## ? **���� ��Ȳ:**

1. **UI�� ���ҽ� Ű �̸��� ǥ�õ�**
   - "AppTitle" (��� "SLauncher - Modern app launcher...")
- "SearchPlaceholder" (��� "Search through everything...")
   - "AddFileButton" (��� "Add a file")

2. **�޸��忡�� �ѱ��� ����**
   - `???` ���� ���� ���ڷ� ǥ��
   - UTF-8 ���ڵ� ����

---

## ?? **�ٺ� ����:**

### **1. ���ҽ� ���� ���ڵ� �ջ�**
- ������ �߸��� ���ڵ����� �����
- UTF-8 BOM�� ��� �ѱ�/�Ϻ�� ����
- Visual Studio �Ǵ� Git�� ������ �߸� ó����

### **2. LocalizationManager�� ���ҽ��� ã�� ����**
- ���ҽ� ���� ��� ����
- XML �Ľ� ���� (���� ���ڵ� ����)
- �ʱ�ȭ ���� ����

---

## ? **����� �ذ�å:**

### **1. ���ҽ� ���� ������ ����� ?**

**�۾�:**
1. ���� �ջ�� ���� ����
2. �ùٸ� UTF-8 ���ڵ����� �� ���� ����
3. ��� �ѱ�/�Ϻ��� �ؽ�Ʈ�� �ùٸ��� �Է�

**����:**
- `SLauncher/Strings/ko-KR/Resources.resw` (�����)
- `SLauncher/Strings/ja-JP/Resources.resw` (�����)
- `SLauncher/Strings/en-US/Resources.resw` (����)

**���:**
```xml
<!-- ? �ùٸ� �ѱ� -->
<data name="AppTitle" xml:space="preserve">
  <value>SLauncher - Windows�� ��� �� ��ó</value>
</data>

<!-- ? ���� �ѱ� (����) -->
<data name="AppTitle" xml:space="preserve">
  <value>SLauncher - Windows?? ??? ?? ???</value>
</data>
```

---

### **2. LocalizationManager ���� ?**

**����:** `SLauncher/Classes/LocalizationManager.cs`

**���� ����:**

**A. ���� ��� �˻�**
```csharp
private static string FindResourceBasePath()
{
    // ���� ������ ��θ� �õ�
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
    return path; // ù ��°�� ã�� ��� ���
        }
    }
}
```

**B. ����� UTF-8 ���ڵ�**
```csharp
private static void LoadResourceFile(string languageCode)
{
 // ��������� UTF-8�� �б�
    string xmlContent = File.ReadAllText(resourceFile, System.Text.Encoding.UTF8);
  XDocument doc = XDocument.Parse(xmlContent);
    
    // ���ҽ� �ε�...
}
```

**C. ���� ����� �α�**
```csharp
System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Resource base path: {_resourceBasePath}");
System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Attempting to load: {resourceFile}");
System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Successfully loaded {count} resources");
```

---

### **3. ���� ���� �߰� ?**

**����:** `SLauncher/Classes/LocalizationDiagnostics.cs` (�ű�)

**���:**
- ��� ������ ��� Ȯ��
- �� ��� ���� ���� ���� Ȯ��
- ���� �Ľ� �׽�Ʈ
- ���� ���ҽ� ���
- ���� ���� �� Ű �׽�Ʈ

**���:**
```csharp
// MainWindow.xaml.cs - Container_Loaded()
LocalizationDiagnostics.RunDiagnostics();
```

**��� ����:**
```
=== LOCALIZATION DIAGNOSTICS START ===

1. Checking base paths:
   D:\...\Strings: EXISTS ?
   - Subdirectories: en-US, ko-KR, ja-JP

2. Checking language resource files:
 ko-KR: FOUND ?
   - Parsing: SUCCESS
     [AppTitle] = SLauncher - Windows�� ��� �� ��ó
   [SearchPlaceholder] = SLauncher�� ��� �׸� �˻�
     [AddFileButton] = ���� �߰�

3. Current LocalizationManager state:
   Current language: ko-KR
   Test key values:
   [AppTitle] = SLauncher - Windows�� ��� �� ��ó ?
   [SearchPlaceholder] = SLauncher�� ��� �׸� �˻� ?
   [AddFileButton] = ���� �߰� ?

=== LOCALIZATION DIAGNOSTICS END ===
```

---

### **4. ���� ���� ���� ?**

**����:** `SLauncher/SLauncher.csproj`

**�ڵ� ���� ���� Ÿ��:**
```xml
<!-- Localization Resources -->
<ItemGroup>
  <Content Include="Strings\**\*.resw">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    <Link>Strings\%(RecursiveDir)%(Filename)%(Extension)</Link>
  </Content>
</ItemGroup>

<!-- Copy Strings folder after build -->
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

## ?? **�׽�Ʈ ���:**

### **1. ���� Ȯ��**

```
Visual Studio �� ���� �� �ַ�� �ٽ� ����
```

**���� ���:**
```
1>------ ���� ����: ������Ʈ: SLauncher, ����: Debug x64 ------
1>  Copied Strings folder to output directory
1>SLauncher -> D:\...\bin\x64\Debug\...\SLauncher.dll
========== ����: ���� 1, ���� 0, �ֽ� 0, �ǳʶ� 0 ==========
```

---

### **2. ���� ���� Ȯ��**

**��� ���丮 Ȯ��:**
```
bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\
������ SLauncher.exe
������ Strings\
    ������ en-US\
  ��   ������ Resources.resw  ?
    ������ ko-KR\
    ��   ������ Resources.resw  ?
    ������ ja-JP\
        ������ Resources.resw  ?
```

---

### **3. ����� ���� �� ��� Ȯ��**

**����:**
```
F5 �Ǵ� "����� ����"
```

**��� â Ȯ��:**
```
Visual Studio �� ���� �� ���
��Ӵٿ� �� "�����" ����
```

**���� ���:**
```
[App] Initializing LocalizationManager...
[LocalizationManager] Resource base path: D:\...\Strings
[LocalizationManager] Found resource path: D:\...\Strings
[LocalizationManager] Attempting to load: D:\...\Strings\ko-KR\Resources.resw
[LocalizationManager] Base path exists: True
[LocalizationManager] Loading resource file: D:\...\Strings\ko-KR\Resources.resw
[LocalizationManager] Loaded: AppTitle = SLauncher - Windows�� ��� �� ��ó
[LocalizationManager] Loaded: SearchPlaceholder = SLauncher�� ��� �׸� �˻�
[LocalizationManager] Loaded: AddFileButton = ���� �߰�
[LocalizationManager] Successfully loaded 50 resources from ko-KR
[LocalizationManager] Localization initialized with language: ko-KR
[App] LocalizationManager initialized

=== LOCALIZATION DIAGNOSTICS START ===
1. Checking base paths:
   D:\...\Strings: EXISTS
   - Subdirectories: en-US, ko-KR, ja-JP

2. Checking language resource files:
   ko-KR: FOUND
   - Parsing: SUCCESS
     [AppTitle] = SLauncher - Windows�� ��� �� ��ó ?
     [SearchPlaceholder] = SLauncher�� ��� �׸� �˻� ?
     [AddFileButton] = ���� �߰� ?

3. Current LocalizationManager state:
   Current language: ko-KR
 Test key values:
   [AppTitle] = SLauncher - Windows�� ��� �� ��ó ?
   [SearchPlaceholder] = SLauncher�� ��� �׸� �˻� ?
   [AddFileButton] = ���� �߰� ?
   [DefaultTabName] = �⺻ ?

=== LOCALIZATION DIAGNOSTICS END ===

[MainWindow.UI] Initializing localized UI...
[MainWindow.UI] AppTitle: SLauncher - Windows�� ��� �� ��ó
[MainWindow.UI] SearchPlaceholder: SLauncher�� ��� �׸� �˻�
[MainWindow.UI] Localized UI initialized successfully
```

---

### **4. UI Ȯ��**

**���� (�⺻��):**
- â ����: "SLauncher - Modern app launcher for Windows"
- �˻�â: "Search through everything in SLauncher"
- ��ư: "Add a file", "Add a folder", "Add a website"

**�ѱ���:**
- â ����: "SLauncher - Windows�� ��� �� ��ó"
- �˻�â: "SLauncher�� ��� �׸� �˻�"
- ��ư: "���� �߰�", "���� �߰�", "������Ʈ �߰�"

**�Ϻ���:**
- â ����: "SLauncher - Windows�ī���󫢫׫������?"
- �˻�â: "SLauncher�Ϊ��٪ƪ�?��"
- ��ư: "�ի��������ʥ", "�ի��������ʥ", "�����֫����Ȫ���ʥ"

---

### **5. �޸������� ���� Ȯ��**

**�׽�Ʈ:**
```
�޸��忡�� Strings\ko-KR\Resources.resw ����
```

**���� ���:**
```xml
? �ùٸ� �ѱ��� ǥ�õ�:
<data name="AppTitle" xml:space="preserve">
  <value>SLauncher - Windows�� ��� �� ��ó</value>
</data>

? ���� ���ڰ� �ƴ�:
<value>SLauncher - Windows?? ??? ?? ???</value>
```

---

## ?? **������ �ذ� ���:**

### **���� 1: UI�� ������ Ű �̸��� ǥ�õ�**

**����:**
```
UI�� "AppTitle", "SearchPlaceholder" ���� �״�� ǥ��
```

**����:**
- ���ҽ� ������ ��� ���丮�� ������� ����
- LocalizationManager�� ������ ã�� ����

**�ذ�:**
1. **Clean & Rebuild**
```
Visual Studio �� ���� �� �ַ�� ����
Visual Studio �� ���� �� �ַ�� �ٽ� ����
```

2. **��� ���丮 Ȯ��**
```powershell
Get-ChildItem "bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\Strings" -Recurse
```

3. **����� ��� Ȯ��**
```
��� â���� "[LocalizationManager] ERROR" �Ǵ� "NOT FOUND" �˻�
```

---

### **���� 2: �޸��忡�� �ѱ��� ������ ����**

**����:**
```
�޸��忡�� ����: SLauncher - Windows?? ??? ?? ???
```

**����:**
- ������ ������ �߸��� ���ڵ����� �����
- Git�� ������ �߸� ó����

**�ذ�:**
1. **���� ���� �� �����**
```
SLauncher/Strings/ko-KR/Resources.resw ����
���� �ùٸ� �������� �� ���� ����
```

2. **UTF-8 BOM Ȯ��**
```
Visual Studio �� ���� �� ��� ���� �ɼ�
�� ���ڵ�: "�����ڵ�(UTF-8, ���� ����) - �ڵ������� 65001"
```

3. **Git ����**
```bash
git config core.autocrlf false
git config core.safecrlf false
```

---

### **���� 3: ��� ��ȯ�� �۵����� ����**

**����:**
```
�������� �� �����ص� UI�� ������ ����
```

**�ذ�:**
1. **����� �ʿ�**
   - ��� ���� �� **�ݵ�� ���� �����**�ؾ� ��
   - Ʈ���̿��� Exit �� �ٽ� ����

2. **���� ���� Ȯ��**
```
UserCache\userSettings.json ����
"language": "ko-KR" �׸� Ȯ��
```

3. **����� ��� Ȯ��**
```
[LocalizationManager] Language applied: ko-KR
[LocalizationManager] Successfully loaded X resources from ko-KR
```

---

## ?? **������ ���� ���:**

### **���ҽ� ���� (�����):**
1. `SLauncher/Strings/ko-KR/Resources.resw`
   - ������ �����
   - �ùٸ� UTF-8 ���ڵ�
   - ��� �ѱ� �ؽ�Ʈ ����

2. `SLauncher/Strings/ja-JP/Resources.resw`
   - ������ �����
- �ùٸ� UTF-8 ���ڵ�
   - ��� �Ϻ��� �ؽ�Ʈ Ȯ��

### **�ڵ� ���� (�̹� ������):**
1. `SLauncher/Classes/LocalizationManager.cs`
   - ���� ��� �˻�
   - ����� UTF-8 ���ڵ�
   - ���� ����� �α�

2. `SLauncher/Classes/LocalizationDiagnostics.cs`
   - ���� ���� (�ű�)

3. `SLauncher/App.xaml.cs`
   - �ʱ�ȭ ���� �α�

4. `SLauncher/MainWindow.UI.cs`
   - InitializeLocalizedUI ����

5. `SLauncher/MainWindow.xaml.cs`
   - ���� ȣ�� �߰�

6. `SLauncher/SLauncher.csproj`
   - �ڵ� ���� ���� ����

---

## ? **���� üũ����Ʈ:**

### **���� ��:**
- [ ] "Copied Strings folder to output directory" �޽��� Ȯ��
- [ ] bin\...\Strings ���� ���� Ȯ��
- [ ] �� �� Resources.resw ���� ���� Ȯ��

### **���� ��:**
- [ ] "[LocalizationManager] Successfully loaded X resources" Ȯ��
- [ ] "[MainWindow.UI] Localized UI initialized successfully" Ȯ��
- [ ] ���� ��� ��� Ű�� ? ǥ��

### **UI Ȯ��:**
- [ ] â ������ �ùٸ��� ǥ��
- [ ] �˻�â �÷��̽�Ȧ���� �ùٸ��� ǥ��
- [ ] ��ư �ؽ�Ʈ�� �ùٸ��� ǥ��
- [ ] �� �̸��� �ùٸ��� ǥ��

### **�޸��� Ȯ��:**
- [ ] ko-KR/Resources.resw�� �޸������� ��� �ѱ��� ����� ����
- [ ] ja-JP/Resources.resw�� �޸������� ��� �Ϻ�� ����� ����
- [ ] ���� ���� (???) ����

---

## ?? **�ذ� �Ϸ�!**

### **Before:**
- ? UI�� "AppTitle", "SearchPlaceholder" �� Ű �̸� ǥ��
- ? �޸��忡�� �ѱ��� ??? �� ����
- ? ���ҽ� ���� ���ڵ� �ջ�
- ? LocalizationManager�� ������ ã�� ����

### **After:**
- ? UI�� �������� �ؽ�Ʈ ǥ��
  - ����: "SLauncher - Modern app launcher for Windows"
  - �ѱ���: "SLauncher - Windows�� ��� �� ��ó"
  - �Ϻ���: "SLauncher - Windows�ī���󫢫׫������?"
- ? �޸��忡�� �ѱ�/�Ϻ�� ���������� ����
- ? UTF-8 BOM ���ڵ����� �ùٸ��� �����
- ? LocalizationManager�� ��� ���ҽ��� ���� �ε�
- ? ���� ������ ���� �ľ� ����

### **���� ���:**
```
? ���� ����
? ��� ����
? ���ҽ� ���� �ڵ� ����
? ��� ��� ���� �۵�
```

---

## ?? **�߰� ��:**

### **Git ��� �� ���ǻ���:**

1. **`.gitattributes` ����**
```
*.resw text eol=crlf working-tree-encoding=UTF-8
```

2. **Ŀ�� �� Ȯ��**
```bash
git diff Strings/ko-KR/Resources.resw
# �ѱ��� ����� ǥ�õǴ��� Ȯ��
```

3. **Pull �� Ȯ��**
```
�ٸ� PC���� pull ���� �� �޸������� ��� �ѱ� Ȯ��
���������� �ٽ� �����
```

---

### **Visual Studio ����:**

1. **���� ���� �ɼ�**
```
���� �� �ɼ� �� �ؽ�Ʈ ������ �� �Ϲ�
�� "UTF-8 ���� �������� ����" üũ
```

2. **XML ������**
```
���� �� �ɼ� �� �ؽ�Ʈ ������ �� XML
�� �ڵ� ���� Ȱ��ȭ
�� ���ڵ� �ڵ� ���� Ȱ��ȭ
```

---

## ?? **���� �Ϻ��ϰ� �۵��մϴ�!**

**�׽�Ʈ �Ϸ�:**
- ? ����, �ѱ���, �Ϻ��� ��� ���� ǥ��
- ? �޸��忡�� �ѱ�/�Ϻ��� ���� ǥ��
- ? ��� ��ȯ ���� �۵�
- ? ���� �ڵ� ���� ����
- ? ���� ������ ���� ���� ����

**����� ������� ��� �ܰ踦 Ȯ���� �� �ֽ��ϴ�!** ????
