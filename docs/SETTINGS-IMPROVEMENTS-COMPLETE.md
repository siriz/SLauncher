# ? Settings ���� �Ϸ�!

## ?? ������ ���

### 1. **Always On Top ���� ����**
- Settings â�� ���� â�� Always On Top�� �������� ���� �ذ�
- Settings â�� �׻� ���� â ���� ǥ�õ�

### 2. **�۷ι� ��Ű ����� ����**
- Settings���� �۷ι� ��Ű ���� ����
- Ctrl+Space, Alt+Tab, Ctrl+Alt+F1 �� �پ��� ���� ����
- ����� ���� ��� ���� (���� ���� ��)

---

## ?? ������ ����

### **1. UIFunctionsClass.cs**

#### CreateModalWindow ����:

```csharp
public static void CreateModalWindow(Window modalWindow, Window parentWindow)
{
    // Disable parent window
    Shell32.EnableWindow(WinRT.Interop.WindowNative.GetWindowHandle(parentWindow), false);

    // ? Set modal window to always on top
    if (modalWindow is WinUIEx.WindowEx modalWindowEx)
 {
   modalWindowEx.IsAlwaysOnTop = true;
    }
    
 // Enable parent window when closed
    modalWindow.Closed += (s, e) => 
   Shell32.EnableWindow(WinRT.Interop.WindowNative.GetWindowHandle(parentWindow), true);
    
    modalWindow.Activate();
}
```

**ȿ��:**
- Settings â�� �׻� ���� â ���� ǥ��
- Always On Top ������ ������� Settings ���� ����

---

### **2. SettingsWindow.xaml**

#### �۷ι� ��Ű ���� UI �߰�:

```xml
<wct:SettingsCard
    Margin="0,5,0,0"
    Description="Press the hotkey anywhere to show/hide SLauncher. Click to change."
    Header="Global Hotkey">
    <wct:SettingsCard.HeaderIcon>
        <FontIcon Glyph="&#xE765;" />
    </wct:SettingsCard.HeaderIcon>
    <Button
     x:Name="HotkeyButton"
        Content="Ctrl + Space"
 Width="150"
     Click="HotkeyButton_Click"
 ToolTipService.ToolTip="Click to change hotkey" />
</wct:SettingsCard>
```

---

### **3. SettingsWindow.xaml.cs**

#### �߰��� �޼���:

**UpdateHotkeyButtonText()**
```csharp
private void UpdateHotkeyButtonText()
{
  HotkeyButton.Content = UserSettingsClass.GlobalHotkey;
}
```

**HotkeyButton_Click()**
```csharp
private async void HotkeyButton_Click(object sender, RoutedEventArgs e)
{
    // Show dialog to change hotkey
    ContentDialog hotkeyDialog = new ContentDialog
    {
        Title = "Change Global Hotkey",
      Content = CreateHotkeyDialogContent(),
        PrimaryButtonText = "Save",
        CloseButtonText = "Cancel"
};

    var result = await hotkeyDialog.ShowAsync();
    
    if (result == ContentDialogResult.Primary)
 {
      // Save new hotkey
        string newHotkey = $"{modifier}+{key}";
   UserSettingsClass.GlobalHotkey = newHotkey;
        UserSettingsClass.WriteSettingsFile();
        UpdateHotkeyButtonText();
    
        // Show restart notice
        await ShowRestartNotice();
 }
}
```

**CreateHotkeyDialogContent()**
```csharp
private StackPanel CreateHotkeyDialogContent()
{
    var panel = new StackPanel { Spacing = 10 };
    
    // Modifier ComboBox
    var modifierCombo = new ComboBox();
    modifierCombo.Items.Add("Ctrl");
    modifierCombo.Items.Add("Alt");
    modifierCombo.Items.Add("Shift");
    modifierCombo.Items.Add("Ctrl+Shift");
    modifierCombo.Items.Add("Ctrl+Alt");
    
    // Key ComboBox
    var keyCombo = new ComboBox();
    keyCombo.Items.Add("Space");
    keyCombo.Items.Add("Tab");
    keyCombo.Items.Add("Enter");
    keyCombo.Items.Add("Esc");
    keyCombo.Items.Add("F1" - "F4");
    
    panel.Children.Add(modifierCombo);
    panel.Children.Add(keyCombo);
    
    return panel;
}
```

---

### **4. GlobalHotkeyManager.cs**

#### ���� ��Ű ��� �߰�:

**RegisterHotkey() �޼���:**

```csharp
public bool RegisterHotkey(string hotkeyString, Action onHotkeyPressed)
{
    // Parse hotkey string (e.g., "Ctrl+Space", "Ctrl+Alt+F1")
    uint modifiers = MOD_NOREPEAT;
    uint vk = 0;
    
    string[] parts = hotkeyString.Split('+');
    
    // Parse modifiers
    for (int i = 0; i < parts.Length - 1; i++)
    {
        string part = parts[i].Trim();
        if (part == "Ctrl") modifiers |= MOD_CONTROL;
        else if (part == "Alt") modifiers |= MOD_ALT;
        else if (part == "Shift") modifiers |= MOD_SHIFT;
        else if (part == "Win") modifiers |= MOD_WIN;
    }
    
    // Get virtual key code
    string key = parts[parts.Length - 1].Trim();
  vk = GetVirtualKeyCode(key);
    
    // Register hotkey
    return RegisterHotKey(_windowHandle, HOTKEY_ID, modifiers, vk);
}
```

**GetVirtualKeyCode() �޼���:**

```csharp
private uint GetVirtualKeyCode(string key)
{
    switch (key.ToLower())
    {
        case "space": return 0x20;
    case "tab": return 0x09;
        case "enter": return 0x0D;
        case "esc": return 0x1B;
        case "f1": return 0x70;
    case "f2": return 0x71;
 case "f3": return 0x72;
     case "f4": return 0x73;
     // ...
        default: return 0;
    }
}
```

---

### **5. MainWindow.xaml.cs**

#### InitializeGlobalHotkey ����:

```csharp
private void InitializeGlobalHotkey()
{
    try
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        hotkeyManager = new GlobalHotkeyManager(hwnd);

        // ? Use hotkey from settings instead of hardcoded
bool success = hotkeyManager.RegisterHotkey(
            UserSettingsClass.GlobalHotkey, 
     () => {
        this.DispatcherQueue.TryEnqueue(() => {
       ToggleWindowVisibility();
       });
   });

      if (!success)
        {
  Debug.WriteLine($"Failed to register {UserSettingsClass.GlobalHotkey}");
}
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Error initializing global hotkey: {ex}");
    }
}
```

---

## ?? ��� ���

### **1. Always On Top ���� �ذ�**

**Before (����):**
```
1. Always On Top Ȱ��ȭ
2. Settings ��ư Ŭ��
3. Settings â�� ���� â �ڷ� ������ ?
4. Settings ���� �Ұ���
```

**After (�ذ�):**
```
1. Always On Top Ȱ��ȭ
2. Settings ��ư Ŭ��
3. Settings â�� ���� â ���� ǥ�� ?
4. ���������� ���� ���� ����
```

---

### **2. �۷ι� ��Ű ����**

**�ܰ�:**
```
1. Settings ��ư Ŭ��
2. "Global Hotkey" ���� ã��
3. ���� ��Ű ��ư Ŭ�� (��: "Ctrl + Space")
4. Modifier ���� (Ctrl, Alt, Shift, Ctrl+Shift, Ctrl+Alt)
5. Key ���� (Space, Tab, Enter, Esc, F1-F4)
6. "Save" Ŭ��
7. ����� �ȳ� Ȯ��
8. SLauncher �����
9. �� ��Ű�� �۵� ?
```

---

## ?? �����Ǵ� ��Ű ����

### **Modifiers:**
- `Ctrl`
- `Alt`
- `Shift`
- `Ctrl+Shift`
- `Ctrl+Alt`

### **Keys:**
- `Space`
- `Tab`
- `Enter`
- `Esc`
- `F1`, `F2`, `F3`, `F4`

### **���� ����:**
```
Ctrl+Space      �� �⺻��
Alt+Space     �� PowerToys Run ��Ÿ��
Ctrl+Shift+Space
Ctrl+Alt+F1
Alt+Tab         �� (����: Windows �⺻ ����Ű�� �浹 ����)
```

---

## ?? ���ǻ���

### **1. ��Ű �浹**

**�ٸ� �۰� �浹 ��:**
```
Error: Failed to register Alt+Space hotkey
```

**�ذ�:**
- �ٸ� �� ����
- �ٸ� ��Ű ���� ����
- Windows �⺻ ����Ű�� ���ϱ�

### **2. Windows ���� ����Ű**

���ؾ� �� ����:
```
? Win+D      (����ȭ�� ǥ��)
? Win+E   (���� Ž����)
? Alt+Tab    (â ��ȯ)
? Alt+F4      (â �ݱ�)
? Ctrl+Alt+Del  (���� ȭ��)
```

### **3. ����� �ʿ�**

```
��Ű ���� �� �ݵ�� SLauncher�� ������ؾ� �մϴ�!
```

**����:**
- GlobalHotkeyManager�� �� ���� �� �� ���� ���
- ��Ÿ�ӿ� ��Ű �����Ϸ��� UnregisterHotKey �� RegisterHotKey �ʿ�
- ����� ��������� ������ ó��

---

## ?? UI ��ũ����

### **Settings â:**

```
������������������������������������������������������������������������������
��  SLauncher Settings [X] ��
������������������������������������������������������������������������������
��         ��
��  Settings        ��
��  ��������������������������������������������������������������  ��
��      ��
��  ?? Change header text              ��
��     [My files, folders, websites]   ��
��           ��
��  ??? Enable fullscreen               ��
��     [Yes/No]          ��
��             ��
��  ?? Grid alignment      ��
��     [Left ��]             ��
��        ��
��  ?? Start with Windows         ��
��     [Yes/No]         ��
��          ��
��  ?? Always on top          ��
��     [Yes/No]  ��
�� ��
��  ?? Global Hotkey       ��
��     [Ctrl + Space]  �� Ŭ�� ����     ��
��    ��
������������������������������������������������������������������������������
�� [Close]     ��
������������������������������������������������������������������������������
```

### **��Ű ���� ���̾�α�:**

```
������������������������������������������������������������������������������
��  Change Global Hotkey          ��
������������������������������������������������������������������������������
��      ��
��  Modifier Key:          ��
��  ����������������������������������������������            ��
��  �� Ctrl              �� ��         ��
��  ����������������������������������������������            ��
��  - Ctrl     ��
��  - Alt          ��
��  - Shift        ��
��  - Ctrl+Shift              ��
��  - Ctrl+Alt             ��
��       ��
��  Key:           ��
��  ����������������������������������������������         ��
��  �� Space   �� ��            ��
��  ����������������������������������������������            ��
��  - Space              ��
��  - Tab     ��
��  - Enter  ��
��  - Esc         ��
��  - F1, F2, F3, F4      ��
��               ��
������������������������������������������������������������������������������
��       [Cancel]  [Save]       ��
������������������������������������������������������������������������������
```

### **����� �ȳ�:**

```
������������������������������������������������������������������������������
��  Restart Required  ��
������������������������������������������������������������������������������
��  ��
��  Please restart SLauncher for the   ��
��  hotkey change to take effect.      ��
��    ��
������������������������������������������������������������������������������
��              [OK]     ��
������������������������������������������������������������������������������
```

---

## ?? ���� ���� ����

### **1. �ǽð� ��Ű ����**

```csharp
public void ChangeHotkey(string newHotkey)
{
    // Unregister old hotkey
    UnregisterHotKey(_windowHandle, HOTKEY_ID);
    
    // Register new hotkey
    RegisterHotkey(newHotkey, _onHotkeyPressed);
}
```

**����:**
- ����� ���ʿ�
- ��� ����

### **2. ��Ű �浹 ����**

```csharp
if (!success)
{
    ContentDialog conflictDialog = new ContentDialog
    {
        Title = "Hotkey Conflict",
        Content = $"{newHotkey} is already in use by another application. Choose a different hotkey.",
        CloseButtonText = "OK"
    };
    await conflictDialog.ShowAsync();
}
```

### **3. ��Ű �׽�Ʈ ��ư**

```xml
<Button Content="Test Hotkey" 
        Click="TestHotkeyButton_Click"/>
```

**���:**
- ��Ű ��� �� �׽�Ʈ
- �浹 ���� Ȯ��
- ���� ���� �̸�����

### **4. �� ���� Ű ����**

```csharp
case "a": case "b": case "c": // ... case "z":
    return (uint)(key.ToUpper()[0]);  // A-Z
case "0": case "1": // ... case "9":
    return (uint)(key[0]);     // 0-9
case "f5": case "f6": // ... case "f12":
    return 0x74 + uint.Parse(key.Substring(1)) - 5;  // F5-F12
```

### **5. ������ ����**

```xml
<MenuFlyout>
    <MenuFlyoutItem Text="Ctrl+Space (Default)" />
    <MenuFlyoutItem Text="Alt+Space (PowerToys)" />
    <MenuFlyoutItem Text="Ctrl+Shift+Space" />
    <MenuFlyoutSeparator />
<MenuFlyoutItem Text="Custom..." />
</MenuFlyout>
```

---

## ?? �׽�Ʈ �ó�����

### **Test 1: Always On Top ����**
```
1. Always On Top Ȱ��ȭ
2. Settings ����
3. Settings â�� ���� ǥ�õ� ?
4. ���� ���� ���� ?
```

### **Test 2: ��Ű ����**
```
1. Settings �� Global Hotkey Ŭ��
2. Alt + Space ����
3. Save Ŭ��
4. ����� �ȳ� Ȯ�� ?
5. SLauncher �����
6. Alt+Space�� ��� ?
```

### **Test 3: ��Ű �浹**
```
1. �ٸ� �ۿ��� Alt+Space ��� ��
2. Settings���� Alt+Space ����
3. SLauncher �����
4. Debug â�� ���� �޽��� ?
5. ��Ű �۵� �� �� (����� ����)
```

### **Test 4: ���� Settings â**
```
1. Settings â ����
2. �ٽ� Settings ��ư Ŭ��
3. �� ��° Settings â�� ù ��° ���� ǥ�� ?
```

---

## ?? �ڵ� ���� ���

### **����� ����:**

1. ? `UIFunctionsClass.cs`
   - CreateModalWindow�� IsAlwaysOnTop �߰�

2. ? `SettingsWindow.xaml`
   - �۷ι� ��Ű SettingsCard �߰�

3. ? `SettingsWindow.xaml.cs`
   - HotkeyButton_Click �߰�
   - CreateHotkeyDialogContent �߰�
   - UpdateHotkeyButtonText �߰�

4. ? `GlobalHotkeyManager.cs`
   - RegisterHotkey �޼��� �߰�
   - GetVirtualKeyCode �޼��� �߰�

5. ? `MainWindow.xaml.cs`
   - InitializeGlobalHotkey ���� (���� ��Ű ���)

---

## ? ���� �Ϸ�!

### **�ذ�� ����:**

1. ? **Always On Top ����**
   - Settings â�� �׻� ���� â ���� ǥ��
 - IsAlwaysOnTop = true�� ���� ����

2. ? **�۷ι� ��Ű ����� ����**
   - Settings���� ��Ű ���� ����
   - Modifier + Key ���� ����
   - ����� �� ����

### **�߰��� ���:**

1. ? ��Ű ���� ���̾�α�
2. ? ���� ��Ű ���
3. ? ��Ű ���ڿ� �Ľ�
4. ? ����� �ȳ�

---

## ?? ���� �� �׽�Ʈ

### **����:**
```
Visual Studio �� Rebuild Solution �� F5
```

### **�׽�Ʈ:**
```
1. Always On Top Ȱ��ȭ
2. Settings ���� �� ���� ǥ�� Ȯ��
3. Global Hotkey ����
4. ����� �� �� ��Ű�� �׽�Ʈ
```

---

## ?? �Ϸ�!

**Settings ������ ���������� �Ϸ�Ǿ����ϴ�!**

**�ذ�:**
- ? Always On Top ���� �� Settings â ���� ����
- ? �۷ι� ��Ű�� Settings���� ���� ����
- ? Ctrl+Space, Alt+Space �� �پ��� ���� ����

**���� ���ϰ� Settings�� ����ϰ� ���ϴ� ��Ű�� ������ �� �ֽ��ϴ�!** ?
