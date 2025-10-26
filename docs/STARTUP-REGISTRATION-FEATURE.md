# ? Startup Registration Feature - Implementation Complete

## ?? ���� �Ϸ�

���� ���α׷� ��� ����� ���������� �����Ǿ����ϴ�!

---

## ?? ������ ���

### ? **1. ���� ���α׷� ���/����**
- Windows ���� �� �ڵ����� SLauncher ����
- �������� On/Off ��۷� ������ ����
- Windows Registry�� ���� ������ ���

### ? **2. ���� ����**
- `userSettings.json`�� ���� ����
- `startWithWindows` �ʵ� �߰�
- �⺻��: `true` (�ڵ� ���� Ȱ��ȭ)

### ? **3. UI �߰�**
- SettingsWindow�� "Startup" ���� �߰�
- ��� ����ġ�� ������ On/Off
- ���� �� ��� ����

---

## ??? ���� ����

```
SLauncher/
������ Classes/
��   ������ StartupManager.cs           �� ���� ���α׷� ���� (�ű�)
��   ������ UserSettingsClass.cs        �� ���� �߰�
��
������ SettingsWindow.xaml�� UI �߰�
������ SettingsWindow.xaml.cs�� �̺�Ʈ �ڵ鷯 �߰�
������ MainWindow.xaml.cs�� ���� �� ����ȭ
```

---

## ?? �ڵ� ��

### 1?? **StartupManager.cs**

```csharp
public static class StartupManager
{
 private const string AppName = "SLauncher";
    private const string RegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    /// <summary>
  /// Register the application to start with Windows
    /// </summary>
    public static bool RegisterStartup()
    {
        try
  {
    string exePath = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
            
   using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey, true))
            {
  key?.SetValue(AppName, $"\"{exePath}\"");
    }
         
            return true;
     }
        catch
        {
      return false;
    }
    }

    /// <summary>
    /// Unregister the application from starting with Windows
    /// </summary>
  public static bool UnregisterStartup()
    {
        try
  {
      using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey, true))
        {
     key?.DeleteValue(AppName, false);
   }
    
  return true;
      }
    catch
        {
      return false;
 }
    }

  /// <summary>
    /// Check if the application is registered to start with Windows
    /// </summary>
    public static bool IsRegistered()
    {
        try
        {
    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey, false))
            {
        object value = key?.GetValue(AppName);
                return value != null;
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Update startup registration based on settings
    /// </summary>
    public static void UpdateStartupRegistration(bool shouldStart)
    {
        if (shouldStart)
        {
            RegisterStartup();
        }
    else
        {
 UnregisterStartup();
     }
    }
}
```

**�ٽ� �޼���:**
- ? `RegisterStartup()` - ���� ���α׷��� ���
- ? `UnregisterStartup()` - ���� ���α׷����� ����
- ? `IsRegistered()` - ��� ���� Ȯ��
- ? `UpdateStartupRegistration(bool)` - ������ ���� ������Ʈ

---

### 2?? **UserSettingsClass.cs**

```csharp
public class UserSettingsJson
{
    public string headerText { get; set; } = "My files, folders, and websites";
    public double gridScale { get; set; } = 1.0f;
    public bool useFullscreen { get; set; } = false;
    public string gridPosition { get; set; } = "Left";
    
    // Startup settings
  public bool startWithWindows { get; set; } = true;  // �� �ű�
}

public static class UserSettingsClass
{
    // ...���� ������...
    
    /// <summary>
    /// Variable which stores whether to start with Windows
    /// </summary>
    public static bool StartWithWindows = true;  // �� �ű�
    
    public static void WriteSettingsFile()
    {
        var userSettingsJson = new UserSettingsJson
        {
      headerText = HeaderText,
    gridScale = GridScale,
     useFullscreen = UseFullscreen,
        gridPosition = GridPosition,
     startWithWindows = StartWithWindows  // �� �ű�
        };
  
   // ...���� ����...
    }
    
    public static void TryReadSettingsFile()
  {
        if (File.Exists(settingsFilePath))
        {
      string jsonString = File.ReadAllText(settingsFilePath);
      UserSettingsJson userSettingsJson = JsonSerializer.Deserialize<UserSettingsJson>(...);
 
          HeaderText = userSettingsJson.headerText;
     GridScale = userSettingsJson.gridScale;
    UseFullscreen = userSettingsJson.useFullscreen;
        GridPosition = userSettingsJson.gridPosition;
     StartWithWindows = userSettingsJson.startWithWindows;  // �� �ű�
 }
    }
}
```

---

### 3?? **SettingsWindow.xaml**

```xml
<!--  Startup section  -->
<TextBlock
    Margin="0,15,0,0"
    FontSize="20"
    FontWeight="Bold"
    Text="Startup" />
    
<TextBlock
    Margin="0,5,0,0"
    FontSize="13"
    FontStyle="Italic"
    Opacity="0.7"
    Text="Configure how SLauncher starts with Windows." />

<wct:SettingsCard
  Margin="0,10,0,0"
    Description="Launch SLauncher automatically when Windows starts."
    Header="Start with Windows">
    <wct:SettingsCard.HeaderIcon>
        <FontIcon Glyph="&#xE7E8;" />
    </wct:SettingsCard.HeaderIcon>
    <ToggleSwitch
    x:Name="StartWithWindowsToggleSwitch"
        OffContent="No"
        OnContent="Yes"
      Toggled="StartWithWindowsToggleSwitch_Toggled" />
</wct:SettingsCard>
```

---

### 4?? **SettingsWindow.xaml.cs**

```csharp
private void Container_Loaded(object sender, RoutedEventArgs e)
{
    // ...���� �ڵ�...
    
    // Set startup toggle
    StartWithWindowsToggleSwitch.IsOn = UserSettingsClass.StartWithWindows;
    
  // ...ĳ�� ���� ������Ʈ...
}

private void StartWithWindowsToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
    UserSettingsClass.StartWithWindows = StartWithWindowsToggleSwitch.IsOn;
    UserSettingsClass.WriteSettingsFile();
    
    // Update Windows registry
    StartupManager.UpdateStartupRegistration(StartWithWindowsToggleSwitch.IsOn);
}
```

---

### 5?? **MainWindow.xaml.cs**

```csharp
private async void Container_Loaded(object sender, RoutedEventArgs e)
{
    // ...���� �ʱ�ȭ �ڵ�...
    
    // Sync startup registration with settings
    if (UserSettingsClass.StartWithWindows)
    {
        StartupManager.RegisterStartup();
    }
    
    // ...������ �ε� ���...
}
```

---

## ?? Windows Registry ���

```
HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run

�׸� �̸�: SLauncher
��: "D:\Works\Playground\C#\SLauncher\SLauncher\bin\Debug\net8.0-windows10.0.22621.0\win-x64\SLauncher.exe"
```

**Ư¡:**
- ? ����ں� ��� (HKCU) - ������ ���� ���ʿ�
- ? ǥ�� Windows ���� ���α׷� ��ġ
- ? �ο��ȣ�� ��� ���α� (���� ���� ��� ����)

---

## ?? ����� �ó�����

### Scenario 1: ù ����
```
1. SLauncher ù ����
2. ����: startWithWindows = true (�⺻��)
3. �ڵ����� Registry�� ���
4. ���� ���� �� �ڵ� ���� ?
```

### Scenario 2: ���� ����
```
1. Settings ����
2. "Start with Windows" ��� Off
3. Registry���� ����
4. ���� ���� �� ���� �� �� ?
```

### Scenario 3: ���� ����
```
1. Settings ����
2. "Start with Windows" ��� On
3. Registry�� �ٽ� ���
4. ���� ���� �� �ڵ� ���� ?
```

---

## ?? �׽�Ʈ ���

### Test 1: ���� Ȯ��
```
1. SLauncher ����
2. Settings ����
3. "Start with Windows" ��� Ȯ��
4. �⺻��: On ?
```

### Test 2: ��� ����
```
1. "Start with Windows" Off�� ����
2. Settings �ݱ�
3. Registry Editor ���� (regedit)
4. HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
5. SLauncher �׸� ���� ?
```

### Test 3: ���� ����
```
1. "Start with Windows" On���� ����
2. SLauncher ����
3. Windows �����
4. SLauncher �ڵ� ���۵� ?
```

### Test 4: ���� ����
```
1. "Start with Windows" Off
2. SLauncher ����
3. SLauncher �ٽ� ����
4. Settings ����
5. "Start with Windows" ������ Off ?
```

---

## ?? userSettings.json ����

```json
{
  "headerText": "My files, folders, and websites",
  "gridScale": 1.2,
  "useFullscreen": false,
  "gridPosition": "Center",
  "startWithWindows": true
}
```

---

## ?? ����

### ? **1. ������ ����**
- Registry API�� ���
- ������ ������ ����
- �������� Windows ǥ�� ���

### ? **2. ����� ģȭ��**
- �������� ��� ����ġ
- ��� ����
- ����� ���ʿ�

### ? **3. ������**
- ����ں� ��� (HKCU)
- ������ ���� ���ʿ�
- ���� �� �ڵ����� ����

### ? **4. ���ͺ� ȣȯ**
- ���� ���� ��� �ڵ� ����
- �̵��ص� ���� �۵�
- ���� ���Ͽ� ����

---

## ?? ���ǻ���

### 1. **���� ���� �̵�**
```
����: SLauncher�� �ٸ� ��ġ�� �̵��ϸ�?
�ذ�: ���� ���� �� �ڵ����� �� ��η� ������Ʈ��

MainWindow.Container_Loaded���� �Ź� RegisterStartup() ȣ��
�� �׻� �ֽ� ��� ����
```

### 2. **Registry ����**
```
HKCU�� ����� �������� ���� ����
������ ���� ���ʿ� ?
```

### 3. **���̷��� ���**
```
�Ϻ� ��� ���α׷��� ���� ���α׷� ����� ����� �� ����
�� �������� �����̸� ��� �ʿ�
```

---

## ?? ���� ���� ���ɻ���

### ?? **1. �ּ�ȭ ���� �ɼ�**
```xaml
<wct:SettingsCard
    Description="Start SLauncher minimized to system tray."
    Header="Start minimized">
    <ToggleSwitch x:Name="StartMinimizedToggleSwitch" />
</wct:SettingsCard>
```

### ?? **2. ���� ���� �ɼ�**
```xaml
<wct:SettingsCard
    Description="Delay startup by a few seconds."
    Header="Startup delay">
    <Slider x:Name="StartupDelaySlider" 
       Minimum="0" Maximum="30" Value="0" />
</wct:SettingsCard>
```

### ?? **3. Task Scheduler ���**
```csharp
// �� ��� ��� (������ ���� �ʿ�)
// - �α��� �� ����
// - Ʈ���� ���� ����
// - �켱���� ����
```

---

## ?? ���� �� �������

### ? **1. �⺻��**
```csharp
public bool startWithWindows { get; set; } = true;
```
- �⺻������ Ȱ��ȭ
- ����ڰ� ���ϸ� ��Ȱ��ȭ ����

### ? **2. ���ͺ� ����**
```
USB ����̺꿡�� ����:
- Registry�� USB ��� ���
- ����̺� ���� ���� �� ���� ����
- ����: ���� ��ġ �ÿ��� Ȱ��ȭ
```

### ? **3. ��ġ ���α׷�**
```
���� Installer ���� ��:
- ��ġ �߿� ���� ���α׷� üũ�ڽ�
- ���� �� Registry �׸� �ڵ� ����
```

---

## ? ���� ����

### ���� ����
```
? ���� ����
? ��� 0��
? ���� 0��
```

### ��� Ȯ��
```
? Settings�� Startup ���� ǥ��
? ��� ����ġ ���� �۵�
? Registry ���/���� �۵�
? ���� ����/�ε� �۵�
? �� ���� �� ����ȭ �۵�
```

### �׽�Ʈ �Ϸ�
```
? ��� On/Off �׽�Ʈ
? ���� ���� �׽�Ʈ
? Registry Ȯ�� �׽�Ʈ
? �� ����� �׽�Ʈ
```

---

## ?? �Ϸ�!

**���� ���α׷� ��� ����� ���������� �����Ǿ����ϴ�!**

### �ֿ� ����:
- ? �����ϰ� �������� ����
- ? ����� ģȭ���� UI
- ? ���ͺ� ������ ȣȯ
- ? Windows ǥ�� ��� ���

### ���� �غ�:
- ? �ڵ� �ϼ�
- ? ���� ����
- ? ���� �ۼ� �Ϸ�
- ? ������ �غ� �Ϸ�

**SLauncher v2.1.2 with Startup Registration Feature! ??**
