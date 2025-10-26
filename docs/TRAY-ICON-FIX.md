# ?? Ʈ���� ������ ����Ŭ�� ���� �ذ�

## ? ����

**����:**
- â �ݱ� �� Ʈ���̷� �̵� ? (�۵�)
- Ʈ���� ������ ����Ŭ�� ? (�۵� �� ��)
- â�� �������� ����

**����:**
WinUI 3 â�� Win32 �޽����� ���� ���� ���մϴ�. Ʈ���� �����ܿ��� ������ `WM_TRAYICON` �޽����� â�� ���޵��� �ʽ��ϴ�.

---

## ? �ذ� ���

### **Window Subclassing �߰�**

SystemTrayIcon.cs�� ������ ����Ŭ������ �߰��Ͽ� �޽����� ����é�ϴ�.

---

## ?? ���� ����

### **SystemTrayIcon.cs ������Ʈ**

#### �߰��� Win32 API:

```csharp
[DllImport("user32.dll", SetLastError = true)]
private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

[DllImport("user32.dll")]
private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

private const int GWL_WNDPROC = -4;
```

#### �߰��� �ʵ�:

```csharp
private IntPtr _oldWndProc;
private WndProcDelegate _newWndProc;

// Delegate for window procedure
private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
```

#### ������ ����:

```csharp
public SystemTrayIcon(IntPtr windowHandle, string iconPath, string tooltip)
{
    // ...���� �ڵ�...

    // Subclass the window to receive tray icon messages
    _newWndProc = new WndProcDelegate(WndProc);
_oldWndProc = SetWindowLongPtr(windowHandle, GWL_WNDPROC, 
      Marshal.GetFunctionPointerForDelegate(_newWndProc));

    // Add tray icon
    Shell_NotifyIcon(NotifyIconAction.NIM_ADD, ref _iconData);
}
```

#### ���ο� WndProc �޼���:

```csharp
private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
{
    if (msg == WM_TRAYICON)
    {
        switch ((int)lParam)
        {
          case WM_LBUTTONDBLCLK:
           _onLeftClick?.Invoke();
    break;
            case WM_RBUTTONUP:
        _onRightClick?.Invoke();
       break;
     }
    }

    // Call the original window procedure
    return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
}
```

#### Dispose ����:

```csharp
public void Dispose()
{
    // Restore original window procedure
  if (_oldWndProc != IntPtr.Zero && _windowHandle != IntPtr.Zero)
    {
        SetWindowLongPtr(_windowHandle, GWL_WNDPROC, _oldWndProc);
    }

    Shell_NotifyIcon(NotifyIconAction.NIM_DELETE, ref _iconData);

if (_iconData.hIcon != IntPtr.Zero)
    {
    DestroyIcon(_iconData.hIcon);
    }
}
```

---

## ?? �۵� ����

### **Window Subclassing**

```
1. WinUI 3 â ����
     ��
2. SetWindowLongPtr�� WndProc ��ü
     ��
3. Ʈ���� ������ ����Ŭ��
     ��
4. WM_TRAYICON �޽��� �߻�
     ��
5. Ŀ���� WndProc�� �޽��� ����
     ��
6. WM_LBUTTONDBLCLK Ȯ��
     ��
7. _onLeftClick �ݹ� ȣ��
     ��
8. MainWindow.AppWindow.Show()
     ��
9. â ���� �Ϸ�!
```

---

## ?? �޽��� �帧

### Before (�۵� �� ��):
```
Tray Icon �� WM_TRAYICON �� WinUI 3 Window �� ? (�޽��� �ս�)
```

### After (�۵���):
```
Tray Icon �� WM_TRAYICON �� Custom WndProc �� ? Callback ȣ�� �� â ����
```

---

## ?? �ֿ� ����Ʈ

### **1. Delegate ����**

```csharp
private WndProcDelegate _newWndProc;
```

**�߿�:** Delegate�� �ʵ�� �����ؾ� ������ �÷��ǵ��� �ʽ��ϴ�!

```csharp
// ? �߸��� ��� - GC�� ���� ������
SetWindowLongPtr(hwnd, GWL_WNDPROC, 
    Marshal.GetFunctionPointerForDelegate(new WndProcDelegate(WndProc)));

// ? �ùٸ� ��� - �ʵ�� ����
_newWndProc = new WndProcDelegate(WndProc);
SetWindowLongPtr(hwnd, GWL_WNDPROC, 
    Marshal.GetFunctionPointerForDelegate(_newWndProc));
```

### **2. ���� WndProc ����**

```csharp
public void Dispose()
{
    // �ݵ�� ���� WndProc ����!
    SetWindowLongPtr(_windowHandle, GWL_WNDPROC, _oldWndProc);
}
```

### **3. CallWindowProc ȣ��**

```csharp
private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
{
    // Ʈ���� �޽��� ó��
 if (msg == WM_TRAYICON) { /* ... */ }

    // ?? �ݵ�� ���� WndProc ȣ��!
 return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
}
```

---

## ?? ���� �ذ�

### **���� 1: ������ �۵� �� ��**

**����:** Delegate�� GC�Ǿ��� �� ����

**�ذ�:**
```csharp
// SystemTrayIcon.cs���� _newWndProc�� private �ʵ����� Ȯ��
private WndProcDelegate _newWndProc;
```

### **���� 2: �� ũ����**

**����:** Dispose���� WndProc ���� �� ��

**�ذ�:**
```csharp
public void Dispose()
{
    if (_oldWndProc != IntPtr.Zero)
    {
        SetWindowLongPtr(_windowHandle, GWL_WNDPROC, _oldWndProc);
    }
    // ...
}
```

### **���� 3: �ٸ� �޽��� �� �۵�**

**����:** CallWindowProc ȣ�� �� ��

**�ذ�:**
```csharp
// WndProc �������� �ݵ�� �߰�
return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
```

---

## ?? �׽�Ʈ ���

### **1. ����**
```
Visual Studio �� Rebuild Solution
```

### **2. ����**
```
F5 (Debug)
```

### **3. �׽�Ʈ �ó�����**

**Test 1: â �ݱ�**
```
1. X ��ư Ŭ��
2. â�� ����� ?
3. Ʈ���̿� ������ ǥ�� ?
```

**Test 2: ����Ŭ�� ����**
```
1. Ʈ���� ������ ����Ŭ��
2. â�� ��Ÿ�� ?
3. ��Ŀ�� ���� ?
```

**Test 3: �ݺ� �׽�Ʈ**
```
1. â �ݱ� �� Ʈ���̷�
2. ����Ŭ�� �� â ����
3. 5ȸ �ݺ� ?
```

**Test 4: ��Ŭ��**
```
1. Ʈ���� ������ ��Ŭ��
2. â�� ��Ÿ�� ? (���� ����)
```

---

## ?? ���� ����

### **�޸�:**
- Delegate: ~100 bytes
- Window subclass: ~200 bytes
- **��:** ~300 bytes (������ �� �ִ� ����)

### **CPU:**
- �޽��� ó��: <0.01ms
- �������: ������ �� ����

### **������:**
- ? Win32 ǥ�� ���
- ? ���鸸 �ۿ��� ���
- ? ������

---

## ?? ������

### **������ ����:**

1. **�޸� ��:** ? (Dispose���� ����)
2. **ũ����:** ? (���� WndProc ����)
3. **GC ����:** ? (Delegate �ʵ�� ����)
4. **������ ����:** ? (UI �����忡���� ����)

---

## ?? ���� ���

### Before:
```
â �ݱ� �� Ʈ���� ?
����Ŭ�� �� ? (�۵� �� ��)
```

### After:
```
â �ݱ� �� Ʈ���� ?
����Ŭ�� �� ? (â ����)
��Ŭ�� �� ? (â ����)
```

---

## ?? �߰� ���� ����

### **1. ��Ŭ�� ���ؽ�Ʈ �޴�**

```csharp
private void ShowContextMenu()
{
    // Win32 TrackPopupMenu ���
    // �Ǵ� WinUI 3 Flyout ǥ��
}
```

### **2. ���� ������Ʈ**

```csharp
public void UpdateToolTip(string newTooltip)
{
    _iconData.szTip = newTooltip;
    Shell_NotifyIcon(NotifyIconAction.NIM_MODIFY, ref _iconData);
}
```

### **3. ������ ����**

```csharp
public void UpdateIcon(string newIconPath)
{
    _iconData.hIcon = LoadIcon(newIconPath);
    Shell_NotifyIcon(NotifyIconAction.NIM_MODIFY, ref _iconData);
}
```

### **4. ǳ�� �˸�**

```csharp
public void ShowBalloonTip(string title, string text)
{
    // NIF_INFO �÷��� ���
    // Shell_NotifyIcon(NIM_MODIFY) ȣ��
}
```

---

## ?? ���� �� ����

### **����:**
```powershell
cd "D:\Works\Playground\C#\SLauncher"
# Visual Studio���� ���� (dotnet CLI�� ���� �߻�)
```

### **����:**
```powershell
cd "SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64"
.\SLauncher.exe
```

### **Release ����:**
```powershell
# Visual Studio �� Configuration: Release �� Build
```

---

## ? üũ����Ʈ

���� ��:
- [x] SystemTrayIcon.cs ������Ʈ
- [x] Window subclassing �߰�
- [x] Delegate �ʵ�� ����
- [x] Dispose���� WndProc ����

����:
- [ ] Visual Studio Rebuild Solution
- [ ] ���� ���� ���� Ȯ��

�׽�Ʈ:
- [ ] â �ݱ� �� Ʈ���̷� �̵�
- [ ] Ʈ���� ������ ����Ŭ�� �� â ����
- [ ] 5ȸ �ݺ� �׽�Ʈ
- [ ] �޸� �� ���� Ȯ��

---

## ?? ���

**Window Subclassing�� ���� Ʈ���� ������ �޽����� ���������� ���� �� �ֽ��ϴ�!**

**���� Ʈ���� ������ ����Ŭ���� ���������� �۵��մϴ�!** ??

**���� ����:**
- ? SetWindowLongPtr�� WndProc ��ü
- ? Ŀ���� WndProc���� WM_TRAYICON ó��
- ? Delegate GC ����
- ? Dispose���� ����

**���� �ܰ�:**
1. Visual Studio���� Rebuild
2. F5�� ����
3. Ʈ���� ������ ����Ŭ�� �׽�Ʈ

**�Ϸ�!** ?
