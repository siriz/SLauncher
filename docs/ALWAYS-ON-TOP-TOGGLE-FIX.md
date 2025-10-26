# ?? Always On Top ��� ���� �ذ�

## ? ����

**����:**
```
1. Settings â ����
2. "Always on Top" ��� ON
3. ���� â�� Always on Top�� ��
4. Settings â�� ���� â �ڷ� ������ ?
5. Settings â ���� �Ұ���
```

**����:**
- Settings â�� �� ���� `IsAlwaysOnTop = true` ����
- ��� ���� �� ���� â�� `IsAlwaysOnTop`�� `true`�� �Ǹ�
- Z-order�� ����Ǹ鼭 Settings â�� �ڷ� �и�

---

## ? �ذ� ���

### **Settings â�� �׻� �ֻ����� ����**

**AlwaysOnTopToggleSwitch_Toggled ����:**

```csharp
private void AlwaysOnTopToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
    UserSettingsClass.AlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
    UserSettingsClass.WriteSettingsFile();

    // Update main window's always on top state
    App.MainWindow.IsAlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
    
    // ? Keep settings window on top of main window
    this.IsAlwaysOnTop = true;
    
    // ? Bring settings window to front
  UIFunctionsClass.BringWindowToFront(this);
}
```

---

## ?? �۵� ����

### Before (����):
```
1. Settings â ���� (IsAlwaysOnTop = true)
2. "Always on Top" ��� ON
3. ���� â IsAlwaysOnTop = true
4. Z-order ����
5. Settings â�� ���� â �ڷ� ?
```

### After (�ذ�):
```
1. Settings â ���� (IsAlwaysOnTop = true)
2. "Always on Top" ��� ON
3. ���� â IsAlwaysOnTop = true
4. Settings â IsAlwaysOnTop = true (�缳��) ?
5. Settings â�� BringToFront ?
6. Settings â�� �ֻ��� ���� ?
```

---

## ?? �ڵ� ����

### **1. this.IsAlwaysOnTop = true**

```csharp
// Settings â�� ��������� Always on Top ����
this.IsAlwaysOnTop = true;
```

**����:**
- ���� â�� Always on Top ���°� ����� ��
- Settings â�� Always on Top �缳��
- Z-order���� �켱���� ����

### **2. BringWindowToFront**

```csharp
// Settings â�� �� ������ ��������
UIFunctionsClass.BringWindowToFront(this);
```

**UIFunctionsClass.cs:**
```csharp
public static void BringWindowToFront(Window window)
{
    IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
    ShowWindow(hWnd, SW_RESTORE);  // ����
    SetForegroundWindow(hWnd);     // ���׶����
}
```

---

## ?? �׽�Ʈ �ó�����

### **Test 1: �⺻ ���**
```
1. Settings â ����
2. "Always on Top" ��� OFF �� ON
3. Settings â�� ������ ���� ���� ?
4. ���� â�� Always on Top ?
```

### **Test 2: ���� �� ���**
```
1. Settings â ����
2. "Always on Top" OFF �� ON
3. Settings â ���� ���� ?
4. "Always on Top" ON �� OFF
5. Settings â ������ ���� ���� ?
6. "Always on Top" OFF �� ON (�ٽ�)
7. Settings â ������ ���� ���� ?
```

### **Test 3: �ٸ� â���� ��ȣ�ۿ�**
```
1. Settings â ���� (Always on Top OFF)
2. Chrome â ����
3. "Always on Top" ��� ON
4. Settings â�� Chrome ���� ���� ?
5. ���� â�� Chrome ���� ���� ?
```

### **Test 4: Settings �ݱ� �� �翭��**
```
1. Settings â ����
2. "Always on Top" ON
3. Settings â �ݱ�
4. Settings â �ٽ� ����
5. Settings â�� ���� ���� ?
```

---

## ?? �߰� ���� (���û���)

### **1. �ٸ� ��� â�� ó��**

��� ��� â�� Always on Top ���� �� �ֻ��� ����:

```csharp
// MainWindow.xaml.cs
private List<Window> openModalWindows = new List<Window>();

private void OpenModalWindow(Window modalWindow)
{
    UIFunctionsClass.CreateModalWindow(modalWindow, this);
    openModalWindows.Add(modalWindow);
    
modalWindow.Closed += (s, e) => {
      openModalWindows.Remove(modalWindow);
    };
}

// Always on Top ���� �� ��� ��� â ������Ʈ
private void UpdateModalWindowsAlwaysOnTop()
{
    foreach (var modal in openModalWindows)
    {
        if (modal is WinUIEx.WindowEx modalEx)
  {
            modalEx.IsAlwaysOnTop = true;
     UIFunctionsClass.BringWindowToFront(modal);
        }
    }
}
```

### **2. �ڵ� ��Ŀ�� ����**

Settings â�� �������� �� �ڵ����� ����:

```csharp
// SettingsWindow.xaml.cs
private DispatcherTimer focusTimer;

private void Container_Loaded(object sender, RoutedEventArgs e)
{
    // ... ���� �ڵ� ...
    
    // ��Ŀ�� üũ Ÿ�̸�
    focusTimer = new DispatcherTimer();
  focusTimer.Interval = TimeSpan.FromMilliseconds(500);
    focusTimer.Tick += FocusTimer_Tick;
    focusTimer.Start();
}

private void FocusTimer_Tick(object sender, object e)
{
    // Always on Top�� Ȱ��ȭ�Ǿ� �ְ�
    // ���� â�� Settings â�� ������ ������
    if (UserSettingsClass.AlwaysOnTop && !this.AppWindow.IsVisible)
    {
        this.IsAlwaysOnTop = true;
        UIFunctionsClass.BringWindowToFront(this);
    }
}
```

### **3. Z-Order ����͸�**

â ������ ����Ǹ� �ڵ����� Settings â �ֻ��� ����:

```csharp
// Win32 API
[DllImport("user32.dll")]
private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, 
    int X, int Y, int cx, int cy, uint uFlags);

private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
private const uint SWP_NOMOVE = 0x0002;
private const uint SWP_NOSIZE = 0x0001;

private void EnsureTopmost()
{
    IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
    SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
}
```

---

## ?? Z-Order ����

### **Windows Z-Order ����:**

```
����������������������������������������������������������������������
��  HWND_TOPMOST (Always on Top)   ��  �� �ֻ���
����������������������������������������������������������������������
��  Settings â (IsAlwaysOnTop=true)��
��  ���� â (IsAlwaysOnTop=true)    ��
����������������������������������������������������������������������
��  �Ϲ� â��      ��
��Chrome, Explorer, etc.     ��
����������������������������������������������������������������������
��  HWND_BOTTOM         ��  �� ������
����������������������������������������������������������������������
```

### **��� �� ��ȭ:**

**Before (����):**
```
[Always on Top OFF]
Settings â (IsAlwaysOnTop=true) �� �ֻ���
���� â (IsAlwaysOnTop=false)
Chrome

[Always on Top ON ���]
���� â (IsAlwaysOnTop=true) �� Z-order �������� �ֻ�����
Settings â (IsAlwaysOnTop=true) �� �ڷ� �и� ?
Chrome
```

**After (�ذ�):**
```
[Always on Top OFF]
Settings â (IsAlwaysOnTop=true) �� �ֻ���
���� â (IsAlwaysOnTop=false)
Chrome

[Always on Top ON ���]
Settings â (IsAlwaysOnTop=true) �� BringToFront�� �ֻ��� ���� ?
���� â (IsAlwaysOnTop=true)
Chrome
```

---

## ?? ����� ����

### **Before (����):**
```
?? �����: "Always on Top�� �Ѹ� Settings â�� �������!"
?? �����: "Settings�� �ٽ� ����� �ϳ���?"
?? �����: "�����ؿ�..."
```

### **After (�ذ�):**
```
?? �����: "Always on Top�� �ѵ� Settings â�� �״�� �ֳ׿�!"
?? �����: "���ؿ�!"
? �����: "�Ϻ��ؿ�!"
```

---

## ?? ���� ������ ����

### **���� 1: �ٸ� ��� â**

**����:**
- EditItemWindow, AddWebsiteDialog � ������ �� ����

**�ذ�:**
- CreateModalWindow���� �̹� `IsAlwaysOnTop = true` ������
- �ʿ�� �� ��� â���� ������ ���� ����

### **���� 2: ��Ŀ�� �ս�**

**����:**
- BringToFront �� Settings â�� ��Ŀ���� ���� �� ����

**�ذ�:**
```csharp
UIFunctionsClass.BringWindowToFront(this);
this.Activate();  // ��Ŀ�� ���� ����
```

### **���� 3: ������**

**����:**
- â�� �ڷ� ���ٰ� �ٽ� ������ ���鼭 ������ �� ����

**�ذ�:**
- `SuspendLayout` / `ResumeLayout` ��� (WinForms)
- WinUI 3������ �Ϲ������� ���� ����

---

## ? ���� �Ϸ�!

### **����� ����:**
- ? `SettingsWindow.xaml.cs`
  - `AlwaysOnTopToggleSwitch_Toggled` ����
  - Settings â Always on Top �缳��
  - BringWindowToFront ȣ��

### **����:**
- ? Always on Top ��� �� Settings â �ֻ��� ����
- ? ���� â�� �������� ����
- ? ���� �� ����ص� ���� �۵�

---

## ?? �׽�Ʈ ���

### **����:**
```
Visual Studio �� Rebuild Solution �� F5
```

### **�׽�Ʈ:**
```
1. Settings â ����
2. "Always on Top" ��� OFF
3. "Always on Top" ��� ON
4. Settings â�� ������ ���� ?
5. ���� ���� ���� ?
6. ���� �� ��� �׽�Ʈ ?
```

---

## ?? �Ϸ�!

**Always on Top ��� ������ �ذ�Ǿ����ϴ�!**

**����:**
- ? Settings â���� Always on Top�� �ѵ� �������� ����
- ? �׻� �ֻ������� ���� ���� ����
- ? ���� ����� ����

**�׽�Ʈ�غ�����!** ?
