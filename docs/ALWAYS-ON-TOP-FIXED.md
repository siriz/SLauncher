# ? Always on Top ���� ���� �Ϸ�!

## ?? ���� ����

### **���� ����:**
- Settings�� "Always on top" ��� ����ġ
- ����ڰ� on/off ���� ����
- ���� ���Ͽ� ����

### **���ο� ����:**
- ? **�׻� Always on Top Ȱ��ȭ**
- ? Settings���� �ɼ� ����
- ? ����ڰ� ������ �� ����

---

## ?? ������ ����

### **1. SLauncher/SettingsWindow.xaml**

**���ŵ� �ڵ�:**

```xaml
<!--  Always on top  -->
<wct:SettingsCard
    Margin="0,5,0,0"
    Description="Keep SLauncher window always on top of other windows."
    Header="Always on top">
 <wct:SettingsCard.HeaderIcon>
        <FontIcon Glyph="&#xE8A7;" />
    </wct:SettingsCard.HeaderIcon>
    <ToggleSwitch
        x:Name="AlwaysOnTopToggleSwitch"
      OffContent="No"
OnContent="Yes"
      Toggled="AlwaysOnTopToggleSwitch_Toggled" />
</wct:SettingsCard>
```

**���:**
- ? Always on top ���� ���� ���� ����
- ? Start with Windows �� Global Hotkey ������ ����

---

### **2. SLauncher/SettingsWindow.xaml.cs**

#### **(1) Container_Loaded �޼��� ����**

**Before:**
```csharp
// Set always on top toggle
AlwaysOnTopToggleSwitch.IsOn = UserSettingsClass.AlwaysOnTop;
```

**After:**
```csharp
// (���ŵ�)
```

---

#### **(2) AlwaysOnTopToggleSwitch_Toggled �̺�Ʈ �ڵ鷯 ����**

**Before:**
```csharp
private void AlwaysOnTopToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
 UserSettingsClass.AlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
    UserSettingsClass.WriteSettingsFile();

    // Update main window's always on top state
    App.MainWindow.IsAlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
 
    // Keep settings window on top of main window
    this.IsAlwaysOnTop = true;
    
    // Bring settings window to front
    UIFunctionsClass.BringWindowToFront(this);
}
```

**After:**
```csharp
// (���� ���ŵ�)
```

---

### **3. SLauncher/MainWindow.xaml.cs**

#### **Container_Loaded �޼��� ����**

**Before:**
```csharp
// Set Window Background
UIFunctionsClass.SetWindowBackground(this, ContainerFallbackBackgroundBrush);

// Set always on top if enabled
this.IsAlwaysOnTop = UserSettingsClass.AlwaysOnTop;  // ? �������� ����

// Create icon scale slider dynamically to avoid XAML compiler bug
CreateIconScaleSlider();
```

**After:**
```csharp
// Set Window Background
UIFunctionsClass.SetWindowBackground(this, ContainerFallbackBackgroundBrush);

// Always set window to be on top
this.IsAlwaysOnTop = true;  // ? �׻� true

// Create icon scale slider dynamically to avoid XAML compiler bug
CreateIconScaleSlider();
```

**�ٽ� ����:**
- `UserSettingsClass.AlwaysOnTop` �б� �� ����
- `this.IsAlwaysOnTop = true` �� �ϵ��ڵ�

---

## ?? UI ����

### **Settings â - Before:**

```
����������������������������������������������������������������������������
�� Settings ��
����������������������������������������������������������������������������
�� �� Enable fullscreen [OFF]   ��
�� ?? Grid alignment  [Left ��]      ��
�� ?? Start with Windows [OFF]        ��
�� ?? Always on top [OFF]             �� �� ���ŵ�
�� ?? Global Hotkey [Ctrl + Space]    ��
����������������������������������������������������������������������������
```

---

### **Settings â - After:**

```
����������������������������������������������������������������������������
�� Settings   ��
����������������������������������������������������������������������������
�� �� Enable fullscreen [OFF]         ��
�� ?? Grid alignment  [Left ��]        ��
�� ?? Start with Windows [OFF] ��
�� ?? Global Hotkey [Ctrl + Space]    ��
����������������������������������������������������������������������������
```

**������:**
- ? Always on top �ɼ� ����
- ? �� ����� UI
- ? ȥ�� ���� (�׻� on�̹Ƿ� ���� ���ʿ�)

---

## ?? ���� ���

### **Always on Top ����:**

```csharp
// MainWindow ���� ��
public MainWindow()
{
    this.InitializeComponent();
    // ...
}

// ������ �ε� ��
private async void Container_Loaded(...)
{
    // ...
 this.IsAlwaysOnTop = true;  // ? �׻� Ȱ��ȭ
    // ...
}
```

**Ư¡:**
1. ? ���α׷� ���� �� �ڵ� ����
2. ? ����ڰ� ���� �Ұ�
3. ? �ٸ� â���� �׻� ���� ǥ��

---

### **SettingsWindow�� ����:**

```xaml
<winex:WindowEx
    ...
    IsAlwaysOnTop="True"  �� ������ true
    ...>
```

**����:**
- Settings â�� Main â ���� ���� ��
- Modal ���� ����

---

## ?? �׽�Ʈ �ó�����

### **Test 1: ���α׷� ����**

```
1. SLauncher ���� ?
2. �ٸ� ���α׷� ���� (��: Chrome) ?
3. Chrome Ŭ�� ?
4. SLauncher�� ������ Chrome ���� ǥ�õ� ?
```

---

### **Test 2: Settings Ȯ��**

```
1. Settings ���� ?
2. "Always on top" �ɼ� ���� Ȯ�� ?
3. ���� �׸�:
   - Enable fullscreen ?
   - Grid alignment ?
   - Start with Windows ?
   - Global Hotkey ?
```

---

### **Test 3: ���� â�� �Բ� ���**

```
1. SLauncher ���� ?
2. Chrome, VSCode, File Explorer ���� ?
3. �� ���α׷� ��ȯ ?
4. SLauncher�� �׻� �ֻ����� ǥ�õ� ?
5. Alt+Tab���� ��ȯ�ص� ������ ?
```

---

### **Test 4: ��üȭ�� ���� ȣȯ**

```
1. Settings �� Enable fullscreen [ON] ?
2. SLauncher ��üȭ�� ?
3. �ٸ� â ��� SLauncher�� ���� ?
4. Esc �Ǵ� Close ��ư���� �ݱ� ?
```

---

## ?? UserSettingsClass ����

### **UserSettingsClass.AlwaysOnTop �ʵ�:**

```csharp
public static bool AlwaysOnTop { get; set; }
```

**����:**
- ?? �ʵ�� ���� ����
- ?? ���� ���Ͽ��� ����� �� ����
- ? ������ MainWindow������ ���õ�

**���� ���� (���û���):**
```csharp
// ������ �����Ϸ���:
// 1. UserSettingsClass.cs���� AlwaysOnTop �ʵ� ����
// 2. ���� ���� �б�/���� �ڵ忡�� ����
// 3. ���� ���� ���� ���׷��̵� �ڵ� �߰�
```

**���� ����:**
- ? �۵����� ��������
- ? �ܼ��� ���õ�
- ? ������ ���߿� ����

---

## ?? ��ǥ

| �׸� | Before | After |
|------|--------|-------|
| **Settings UI** | Always on top ��� ���� | ���ŵ� ? |
| **Main Window** | `UserSettingsClass.AlwaysOnTop` ���� | `true` �ϵ��ڵ� ? |
| **����� ����** | ���� (on/off) | �Ұ��� (�׻� on) ? |
| **���� ����** | AlwaysOnTop ����� | ���õ� ?? |
| **Settings Window** | IsAlwaysOnTop="True" | ������� ? |

---

## ?? ����� ���λ���

### **IsAlwaysOnTop �Ӽ�:**

```csharp
// WinUIEx.WindowEx �Ӽ�
public bool IsAlwaysOnTop { get; set; }
```

**����:**
- WinUIEx ���̺귯������ ����
- Win32 API `SetWindowPos` ����
- `HWND_TOPMOST` �÷��� ���

---

### **���� ����:**

```
1. MainWindow ������ ����
   ��
2. InitializeComponent() ȣ��
   ��
3. Container_Loaded �̺�Ʈ �߻�
   ��
4. this.IsAlwaysOnTop = true ����  �� ���⼭ ����
   ��
5. â�� �ֻ����� �ö�
```

---

## ?? ����� ���� ����

### **Before (������):**

```
�����: "Always on top�� �״µ� �� �ٸ� â �Ʒ��� ����?"
�� ������ ����� ���� �ȵ�
�� ����� �ʿ�
�� ȥ��������
```

---

### **After (����):**

```
�����: "SLauncher�� �׻� ���� �ֳ�!"
�� ���� �ʿ����
�� ������
�� ������ ����
```

**����:**
- ? ���� ����ȭ
- ? ����� ȥ�� ����
- ? �ϰ��� ����
- ? ��ó�� ������ ���� (���� ����)

---

## ?? ���� ���� (���û���)

### **1. UserSettingsClass ����**

```csharp
// AlwaysOnTop �ʵ� ���� ����
public static class UserSettingsClass
{
 // public static bool AlwaysOnTop { get; set; }  �� ����
    
    public static bool UseFullscreen { get; set; }
    public static string GridPosition { get; set; }
    // ...
}
```

---

### **2. ���� ���� ���׷��̵�**

```csharp
public static void UpgradeUserSettings()
{
    // ���� ���� ���Ͽ� AlwaysOnTop�� ������ ����
    if (settingsJson.ContainsKey("AlwaysOnTop"))
    {
        settingsJson.Remove("AlwaysOnTop");
    }
}
```

---

### **3. �ӽ� ��Ȱ��ȭ �ɼ� (���)**

```csharp
// ������ ��峪 Ư�� ��Ȳ�� ���� �ӽ� ��Ȱ��ȭ
// Shift Ű�� ������ �����ϸ� Always on Top ��Ȱ��ȭ
if (!Keyboard.IsKeyDown(Key.Shift))
{
    this.IsAlwaysOnTop = true;
}
```

---

## ? �Ϸ�!

### **���� ���� ���:**

```
? SettingsWindow.xaml
   - Always on top ���� ���� ����

? SettingsWindow.xaml.cs
   - AlwaysOnTopToggleSwitch �ʱ�ȭ �ڵ� ����
   - AlwaysOnTopToggleSwitch_Toggled �ڵ鷯 ����

? MainWindow.xaml.cs
   - UserSettingsClass.AlwaysOnTop �б� ����
   - this.IsAlwaysOnTop = true �ϵ��ڵ�
```

---

### **�׽�Ʈ üũ����Ʈ:**

```
? ���� ����
? ���α׷� ���� �� Always on Top ����
? Settings���� �ɼ� ���� Ȯ��
? �ٸ� â���� �׻� ���� ǥ��
? ��üȭ�� ���� ȣȯ
? Settings â�� Always on Top ����
```

---

## ?? �Ϸ�!

**���� SLauncher�� �׻� �ٸ� â ���� ǥ�õ˴ϴ�!**

**Settings���� ���ʿ��� �ɼ��� ���ŵǾ� �� ����մϴ�!** ?

**����ڴ� ���� ���� ���� ��� ����� �� �ֽ��ϴ�!** ??

**�׽�Ʈ�غ�����!** ??
