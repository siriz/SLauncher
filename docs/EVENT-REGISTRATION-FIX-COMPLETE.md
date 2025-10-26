# ?? �� ���� ���� �ذ� �Ϸ�!

## ? **������:**

�� ������ ������� �ʰ� `tabs.json` ���ϵ� �������� �ʴ� ����!

### **�ٺ� ����:**

**`WindowEx_Closed` �̺�Ʈ�� ��ϵ��� �ʾҽ��ϴ�!**

```csharp
// ? MainWindow.xaml.cs - Container_Loaded()
// Hook up window closing event
this.AppWindow.Closing += Window_Closing;  // ? ��ϵ�

// ? Closed �̺�Ʈ�� ������� ����!
// this.Closed += WindowEx_Closed;  // �� ���� ������!

// Initialize global hotkey (Ctrl+Space)
InitializeGlobalHotkey();
```

**���:**
- `WindowEx_Closed()` �޼���� ������ **ȣ����� ����**
- `SaveAllTabs()`�� **������� ����**
- `tabs.json` ������ **�������� ����**
- �� ������ **������� ����**

---

## ? **�ذ� ���:**

### **1. `WindowEx_Closed` �̺�Ʈ ���**

**��ġ:** `SLauncher/MainWindow.xaml.cs`

**���� ��:**
```csharp
// Hook up window closing event
this.AppWindow.Closing += Window_Closing;

// Initialize global hotkey (Ctrl+Space)
InitializeGlobalHotkey();
```

**���� ��:**
```csharp
// Hook up window closing event
this.AppWindow.Closing += Window_Closing;

// Hook up window closed event (for saving)
this.Closed += WindowEx_Closed;  // ? �߰�!

// Initialize global hotkey (Ctrl+Space)
InitializeGlobalHotkey();
```

---

### **2. `Window_Closing` ����**

**��ġ:** `SLauncher/MainWindow.Hotkeys.cs`

**����:** `Window_Closing`���� ���ҽ��� dispose�ϸ� `WindowEx_Closed`�� ȣ����� ���� �� �ֽ��ϴ�.

**���� ��:**
```csharp
private void Window_Closing(object sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
{
    if (UserSettingsClass.MinimizeToTray)
    {
 args.Cancel = true;
        this.AppWindow.Hide();
    }
    else
    {
        trayIcon?.Dispose();  // ? ���⼭ dispose�ϸ� �ȵ�
        hotkeyManager?.Dispose();
    }
}
```

**���� ��:**
```csharp
private void Window_Closing(object sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
{
    if (UserSettingsClass.MinimizeToTray)
    {
        // Just minimize to tray, don't save yet (will save on actual close)
        args.Cancel = true;
        this.AppWindow.Hide();
    }
    // else: allow window to close, WindowEx_Closed will handle saving
}
```

**����:**
- `Window_Closing`�� **â�� �ݱ� ��**�� ȣ���
- `WindowEx_Closed`�� **â�� ���� ��**�� ȣ���
- `Window_Closing`���� ���ҽ��� dispose�ϸ� `WindowEx_Closed`�� ����� ������� ���� �� ����

---

### **3. Ʈ���� ������ Exit �޴� ����**

**��ġ:** `SLauncher/MainWindow.Hotkeys.cs`

**���� ��:**
```csharp
trayIcon.SetOnExitMenu(() =>
{
    this.DispatcherQueue.TryEnqueue(() =>
    {
   // Save items before exit
        UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);  // ? ���� �������� ����
        
   // Dispose tray icon
        trayIcon?.Dispose();
   
        // Exit application
     Microsoft.UI.Xaml.Application.Current.Exit();
    });
});
```

**���� ��:**
```csharp
trayIcon.SetOnExitMenu(() =>
{
    this.DispatcherQueue.TryEnqueue(() =>
    {
  // Save all tabs before exit
        SaveAllTabs();  // ? �� ����!
    
        // Dispose tray icon
        trayIcon?.Dispose();
        hotkeyManager?.Dispose();
        
        // Exit application
        Microsoft.UI.Xaml.Application.Current.Exit();
    });
});
```

---

## ?? **�̺�Ʈ �帧**

### **���� ���� (X ��ư Ŭ��):**

```
����ڰ� X ��ư Ŭ��
    ��
    ���� Window_Closing ȣ��
    ��   ��
    ��   ���� MinimizeToTray == true?
    ��   ��   ���� Yes �� args.Cancel = true, Hide()
    ��   ��        ���� â�� ����, �� ��� ����
    ��   ��
 ��   ���� MinimizeToTray == false?
    ��       ���� Yes �� args.Cancel = false (�⺻��)
    ������ â �ݱ� ��� ����
    ��
    ���� WindowEx_Closed ȣ�� ?
��
        ���� SaveAllTabs()
        ��   ���� SaveCurrentTabItems()
      ��   ���� Collect all items from all tabs
        ��   ���� SaveLauncherXItems(allUniqueItems)
        ��   ���� SaveTabsWithItemList(...)
      ��   ���� tabs.json ����! ?
        ��
        ���� trayIcon.Dispose()
        ���� hotkeyManager.Dispose()
  ���� �� ����
```

---

### **Ʈ���� �޴����� Exit ����:**

```
����ڰ� Ʈ���� ������ �� Exit Ŭ��
    ��
    ���� SetOnExitMenu �ݹ� ����
     ��
        ���� SaveAllTabs() ?
   ��   ���� tabs.json ����! ?
        ��
        ���� trayIcon.Dispose()
        ���� hotkeyManager.Dispose()
  ���� Application.Current.Exit()
```

---

## ?? **���� ���� ��**

### **? Before:**

```
�� ����:
    ��
    ���� Window_Closing
    ��   ���� MinimizeToTray == false
    ��       ���� trayIcon.Dispose()
    ��       ���� hotkeyManager.Dispose()
    ��
    ���� WindowEx_Closed
        ���� ? �̺�Ʈ�� ��ϵ��� �ʾ� ȣ����� ����!
            ���� SaveAllTabs() ���� �ȵ�
            ���� tabs.json ���� �ȵ� ?

���:
- tabs.json ���� ����
- �� ���� ���� �ȵ�
- �� ����� �� �� ���� �ȵ�
```

---

### **? After:**

```
�� ����:
    ��
    ���� Window_Closing
    ��   ���� MinimizeToTray == false
    ��       ���� (�ƹ��͵� ����, WindowEx_Closed���� ó��)
    ��
    ���� WindowEx_Closed ?
        ���� �̺�Ʈ ��ϵ�!
            ���� SaveAllTabs() ����
���� SaveCurrentTabItems()
            ���� Collect all tabs
           ���� SaveLauncherXItems()
     ���� SaveTabsWithItemList()
            ���� tabs.json ����! ?

���:
? tabs.json ���� ����
? �� ���� �����
? �� ����� �� �� ������
```

---

## ?? **�ٽ� ����Ʈ**

### **1. �̺�Ʈ ����� �ʼ�:**

```csharp
// ? �̺�Ʈ �ڵ鷯�� �־ ������� ������ ȣ����� ����
private void WindowEx_Closed(object sender, WindowEventArgs args)
{
    SaveAllTabs();  // �� �ڵ�� ������� ����!
}

// ? �ݵ�� �̺�Ʈ�� ����ؾ� ��
this.Closed += WindowEx_Closed;  // ���� �����!
```

### **2. Window_Closing vs WindowEx_Closed:**

| �̺�Ʈ | ȣ�� ���� | �뵵 |
|--------|----------|------|
| `Window_Closing` | **â�� �ݱ� ��** | �ݱ� ��� (`args.Cancel = true`) |
| `WindowEx_Closed` | **â�� ���� ��** | ���� �۾� (����, dispose) |

### **3. ���� ��ġ:**

```csharp
// ? Window_Closing���� �����ϸ� �ȵ� (��ҵ� �� ����)
private void Window_Closing(...)
{
    SaveAllTabs();  // â�� ������ ��, ��ҵ� �� ����
}

// ? WindowEx_Closed���� ���� (Ȯ���� ���� ��)
private void WindowEx_Closed(...)
{
    SaveAllTabs();  // â�� Ȯ���� ���� ��
}
```

### **4. Ʈ���� ���� ���� ó��:**

```csharp
// Ʈ���̿��� Exit �ÿ��� WindowEx_Closed�� ȣ����� ���� �� ����
// ���� Ʈ���� �޴����� ���� SaveAllTabs() ȣ��
trayIcon.SetOnExitMenu(() =>
{
  SaveAllTabs();  // ��������� ����
    Application.Current.Exit();
});
```

---

## ?? **�׽�Ʈ ���**

### **Test 1: ���� ����**
```
1. �� 3�� ����, �� �ǿ� ������ �߰�
2. X ��ư���� �� ����

Debug Output:
DEBUG: WindowEx_Closed - Starting save process
=== SaveAllTabs START ===
Tab 0 ('�⺻'): 3 items
Tab 1 ('�� 2'): 5 items
Tab 2 ('�� 3'): 2 items
Total unique items across all tabs: 10
Saved all items to Files directory
Successfully wrote tabs.json to D:\...\UserCache\tabs.json
=== SaveAllTabs COMPLETE: Saved 3 tabs ===
DEBUG: WindowEx_Closed - Save complete

Result:
? tabs.json ���� ������!
? ���� ����:
{
  "tabs": [
    {
"id": "tab-0",
      "name": "�⺻",
"icon": "Home",
      "color": "#00000000",
      "itemIndices": ["0", "1", "2"],
      "isSelected": false
    },
    ...
  ],
  "selectedTabIndex": 0
}
```

### **Test 2: Ʈ���̿��� Exit**
```
1. Minimize to Tray Ȱ��ȭ
2. X ��ư Ŭ�� �� Ʈ���̷� �ּ�ȭ
3. Ʈ���� ������ ��Ŭ�� �� Exit

Debug Output:
=== SaveAllTabs START ===
Tab 0 ('�⺻'): 3 items
Successfully wrote tabs.json
=== SaveAllTabs COMPLETE: Saved 1 tabs ===

Result:
? tabs.json ���� ������!
```

### **Test 3: �� �����**
```
1. �� ���� (tabs.json ������)
2. �� �����

Debug Output:
DEBUG LoadTabs: Read tabs.json:
{
  "tabs": [...],
  "selectedTabIndex": 0
}
DEBUG LoadTabs: Loaded 3 tabs
Distributed 3 items to tab '�⺻'
Distributed 5 items to tab '�� 2'
Distributed 2 items to tab '�� 3'
Loaded 3 tabs

Result:
? �� 3�� ������!
? �� ���� ������ ������!
? �� ���� ������!
```

---

## ?? **���� ���� ���**

### **1. MainWindow.xaml.cs**
```diff
  // Hook up window closing event
  this.AppWindow.Closing += Window_Closing;

+ // Hook up window closed event (for saving)
+ this.Closed += WindowEx_Closed;
  
  // Initialize global hotkey (Ctrl+Space)
  InitializeGlobalHotkey();
```

**���� ����:** `WindowEx_Closed` �̺�Ʈ ��� �߰�

---

### **2. MainWindow.Hotkeys.cs (Window_Closing)**
```diff
  private void Window_Closing(object sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
  {
      if (UserSettingsClass.MinimizeToTray)
      {
+         // Just minimize to tray, don't save yet (will save on actual close)
          args.Cancel = true;
     this.AppWindow.Hide();
      }
-     else
-     {
-         trayIcon?.Dispose();
-    hotkeyManager?.Dispose();
-     }
+// else: allow window to close, WindowEx_Closed will handle saving
  }
```

**���� ����:** ���ҽ� dispose ���� (WindowEx_Closed���� ó��)

---

### **3. MainWindow.Hotkeys.cs (Exit Menu)**
```diff
  trayIcon.SetOnExitMenu(() =>
  {
      this.DispatcherQueue.TryEnqueue(() =>
      {
-    // Save items before exit
-         UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);
+         // Save all tabs before exit
+         SaveAllTabs();
          
   // Dispose tray icon
          trayIcon?.Dispose();
+   hotkeyManager?.Dispose();
          
          // Exit application
      Microsoft.UI.Xaml.Application.Current.Exit();
      });
  });
```

**���� ����:** �� ���� �߰�

---

## ? **�ذ� �Ϸ�!**

### **����:**
- ? `WindowEx_Closed` �̺�Ʈ �̵��
- ? `SaveAllTabs()` �̽���
- ? `tabs.json` ���� �̻���
- ? �� ���� ���� �ȵ�

### **�ذ�:**
- ? `WindowEx_Closed` �̺�Ʈ ���
- ? `SaveAllTabs()` ���� ����
- ? `tabs.json` ���� ����
- ? �� ���� �Ϻ��ϰ� ����
- ? �� ����� �� �� ����

### **���� ���:**
```
? ���� ����!
? ��� ����!
```

---

## ?? **���� �� ������ �Ϻ��ϰ� ����ǰ� �����˴ϴ�!**

**Before:**
- tabs.json ���� ���� ?
- �̺�Ʈ �̵�� ?
- ���� �ȵ� ?

**After:**
- tabs.json ���� ���� ?
- �̺�Ʈ ��ϵ� ?
- �Ϻ��� ����/���� ?
