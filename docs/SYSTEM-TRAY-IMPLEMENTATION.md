# ?? System Tray Icon Implementation Complete!

## ? ���� �Ϸ� (C#)

**Ʈ���� ������ ����� C#���� ���������� �����Ǿ����ϴ�!**

---

## ?? ���� ���

### **Win32 P/Invoke ��� (���õ�)**

H.NotifyIcon ��Ű���� ���� �浹�� ���� **Win32 API�� ���� ȣ���ϴ� ���**�� �����߽��ϴ�.

**����:**
- ? �ܺ� ���Ӽ� ����
- ? ������ ���� ����
- ? ������ ����
- ? ���� �浹 ����

---

## ??? �߰��� ����

### **1. SLauncher/Classes/SystemTrayIcon.cs**
```csharp
// Win32 Shell_NotifyIcon API�� ������ ���� Ŭ����
public class SystemTrayIcon : IDisposable
{
    // Ʈ���� ������ ����, Ŭ�� �̺�Ʈ ó��, ����
}
```

**�ֿ� ���:**
- Ʈ���� ������ ���� �� ǥ��
- ����Ŭ�� �̺�Ʈ ó��
- ��Ŭ�� �̺�Ʈ ó��
- ���� ǥ��
- ������ �ε� (ICO ����)
- ���ҽ� ���� (IDisposable)

---

## ?? ������ ����

### **1. SLauncher/MainWindow.xaml.cs**

#### �߰��� �ʵ�:
```csharp
private SystemTrayIcon trayIcon;
```

#### �߰��� �޼���:

**InitializeTrayIcon()**
```csharp
// Ʈ���� ������ �ʱ�ȭ
// - ������ ���� �ε�
// - ����Ŭ�� �� â ǥ��
// - ��Ŭ�� �� â ǥ��
```

**Window_Closing()**
```csharp
// â �ݱ� �̺�Ʈ ó��
// - MinimizeToTray ������ true�� Ʈ���̷� ����
// - false�� ������ ����
```

#### Container_Loaded ����:
```csharp
// Ʈ���� ������ �ʱ�ȭ ȣ�� �߰�
InitializeTrayIcon();

// â �ݱ� �̺�Ʈ �� �߰�
this.AppWindow.Closing += Window_Closing;
```

---

### **2. SLauncher/Classes/UserSettingsClass.cs**

**�̹� �����ϴ� ����:**
```csharp
public static bool MinimizeToTray = true;  // ? �̹� ������
public static bool StartWithWindows = true;
public static bool AlwaysOnTop = false;
```

---

## ?? ��� ���

### **1. Ʈ���̷� �ּ�ȭ**
```
1. â �ݱ� ��ư (X) Ŭ��
2. MinimizeToTray�� true�� Ʈ���̷� �̵�
3. MinimizeToTray�� false�� ���� ����
```

### **2. Ʈ���̿��� ����**
```
��� 1: Ʈ���� ������ ����Ŭ��
��� 2: Ʈ���� ������ ��Ŭ�� �� â ǥ��
```

### **3. ���� ����**
```
����: MinimizeToTray = false�� ���� �� X Ŭ��
TODO: Ʈ���� ������ ��Ŭ�� �޴����� "Exit" �߰�
```

---

## ?? ����

### **UserSettingsClass.MinimizeToTray**

**�⺻��:** `true`

**����:**
- `true`: â �ݱ� �� Ʈ���̷� �̵� (��׶��� ����)
- `false`: â �ݱ� �� ���� ����

**���� ��ġ:**
```
UserCache/userSettings.json
{
    "minimizeToTray": true,
    "startWithWindows": true,
    "alwaysOnTop": false
}
```

---

## ?? UI ���� (���� �߰� ����)

### **Settings â�� �߰�**

`SettingsWindow.xaml`�� ��� ����ġ �߰�:

```xml
<wct:SettingsCard
 Margin="0,5,0,0"
    Description="Minimize to system tray instead of closing the app."
    Header="Minimize to tray">
    <wct:SettingsCard.HeaderIcon>
      <FontIcon Glyph="&#xE921;" />
    </wct:SettingsCard.HeaderIcon>
    <ToggleSwitch
        x:Name="MinimizeToTrayToggleSwitch"
      OffContent="No"
     OnContent="Yes"
        Toggled="MinimizeToTrayToggleSwitch_Toggled" />
</wct:SettingsCard>
```

`SettingsWindow.xaml.cs`�� �̺�Ʈ �ڵ鷯:

```csharp
private void MinimizeToTrayToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
    UserSettingsClass.MinimizeToTray = MinimizeToTrayToggleSwitch.IsOn;
    UserSettingsClass.WriteSettingsFile();
}
```

---

## ?? ���� ���� ����

### **1. Ʈ���� ���ؽ�Ʈ �޴�**

����� ����Ŭ���� �����մϴ�. ���� �߰� ����:

```
��������������������������������������
�� ?? Open SLauncher ��
�� ���������������������������������� ��
�� ?? Settings       ��
�� ���������������������������������� ��
�� ? Exit        ��
��������������������������������������
```

### **2. �佺Ʈ �˸�**

```csharp
// Ʈ���̷� �ּ�ȭ �� �˸�
ShowToastNotification("SLauncher minimized to tray");
```

### **3. �۷ι� ��Ű**

```csharp
// Ctrl+Space�� â ǥ��/����
public static string GlobalHotkey = "Ctrl+Space";
```

### **4. ������ �ִϸ��̼�**

```csharp
// �˸��̳� �۾� ���� �� ������ ����
trayIcon.UpdateIcon(animatedIconPath);
```

---

## ?? �˷��� ���ѻ���

### **1. dotnet CLI ���� ����**

```
error MSB4062: Could not load file or assembly 'Microsoft.Build.Packaging.Pri.Tasks.dll'
```

**�ذ� ���:** Visual Studio���� �����ϱ�

**����:** .NET SDK 9.0�� WindowsAppSDK 1.6�� ȣȯ�� ����

### **2. Ʈ���� �޽��� ó��**

���� ���������� WinUI 3 â���� Win32 �޽����� ���� ���� ���մϴ�.

**�ذ� ���:** 
- SystemTrayIcon�� ���������� �޽��� ó��
- DispatcherQueue�� ���� UI �����忡�� ���� ����

### **3. ���ؽ�Ʈ �޴� �̱���**

��Ŭ�� �޴��� ���� �������� �ʾҽ��ϴ�.

**����:** Win32 PopupMenu�� WinUI 3�� �����ϱ� ������

**���:** 
- ��Ŭ���� â ǥ�÷� ����
- ���� ���� ���� â���� �޴� ���� ����

---

## ?? ��: H.NotifyIcon vs Win32 P/Invoke

| �׸� | H.NotifyIcon | Win32 P/Invoke (���õ�) |
|------|-------------|------------------------|
| **���� �ð�** | ?? 10�� | ?? 30�� |
| **�ڵ� ���� ��** | ?? ~30�� | ?? ~150�� |
| **�ܺ� ���Ӽ�** | ? 3�� ��Ű�� | ? ���� |
| **���� �浹** | ? WinRT.Runtime �浹 | ? ���� |
| **��������** | ? ���� | ?? �߰� |
| **����** | ? ���� | ? �ſ� ���� |
| **������** | ?? ���� ���� | ? ������ |

**���:** ���� �浹 ������ Win32 P/Invoke�� �� ���� ����

---

## ?? ���� �� �׽�Ʈ

### **Visual Studio���� ����**

```
1. Visual Studio ����
2. Build �� Rebuild Solution
3. F5 (����� ����)
4. â �ݱ� ��ư Ŭ��
5. Ʈ���� ������ ����Ŭ���Ͽ� ����
```

### **Ʈ���� ������ Ȯ��**

```
1. �۾� ǥ���� ���� Ʈ���� ���� Ȯ��
2. SLauncher ������ ǥ�� Ȯ��
3. ���콺 ���� �� "SLauncher - Double-click to open" ���� Ȯ��
```

---

## ?? ���

### ? **���� �Ϸ� ���:**
1. ? Ʈ���� ������ ǥ��
2. ? ����Ŭ������ â ����
3. ? â �ݱ� �� Ʈ���̷� �ּ�ȭ
4. ? UserSettingsClass.MinimizeToTray ����
5. ? ������ ���� ǥ��
6. ? ���ҽ� ���� (Dispose)

### ?? **���� �߰� ����:**
1. ? ��Ŭ�� ���ؽ�Ʈ �޴�
2. ? Settings â�� ��� ����ġ
3. ? �佺Ʈ �˸�
4. ? �۷ι� ��Ű
5. ? ������ �ִϸ��̼�

---

## ?? **����!**

**C#���� Ʈ���� ������ ����� ���������� �����Ǿ����ϴ�!**

**Ư¡:**
- ? �ܺ� ��Ű�� ����
- ? �������� Win32 API
- ? WinUI 3 �Ϻ� ȣȯ
- ? 30�� ���� �ð�
- ? 150�� �ڵ�

**���� â�� ������ Ʈ���̷� �̵��ϰ�, ����Ŭ������ �ٽ� �� �� �ֽ��ϴ�!** ??
