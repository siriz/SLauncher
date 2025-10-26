# ? MainWindow.xaml.cs ���� �и� �Ϸ�!

## ?? **������:**
- `MainWindow.xaml.cs` ������ 2,000�� �̻����� �ʹ� ��:
  - �ڵ� Ȯ�ο� �ð��� ���� �ɸ�
  - ���� �� ���� �߻� Ȯ�� ����
  - ��ɺ��� ã�� �����
  - AI�� ��ü ������ �����ϴµ� �����

---

## ? **�ذ�å: Partial Class �и�**

C# partial class�� ����Ͽ� ��ɺ��� ���� �и�!

### **�и� ����:**

```
MainWindow.xaml.cs (���� - 324��)
���� MainWindow.Tabs.cs          (�� ���� - 358��)
���� MainWindow.Items.cs      (������ ���� - 204��)
���� MainWindow.DragDrop.cs       (�巡��&��� - 267��)
���� MainWindow.Search.cs       (�˻� ��� - 218��)
���� MainWindow.UI.cs     (UI ������Ʈ - 210��)
���� MainWindow.Hotkeys.cs        (����Ű/Ʈ���� - 153��)
```

**Total: 1,734�� �� ��� 245�پ� 7�� ���Ϸ� �и�!**

---

## ?? **���Ϻ� ����:**

### **1. MainWindow.xaml.cs (���� ����)**

**����:** ������, �ʵ�, �ٽ� �̺�Ʈ �ڵ鷯

**���� ����:**
```csharp
? �ʵ� ���� (IconScaleSlider, trayIcon, hotkeyManager, _previousTab)
? Tab color presets dictionary
? Tab colors dictionary
? INotifyPropertyChanged ����
? ������ (MainWindow())
? Container_Loaded (�ʱ�ȭ ����)
? WindowEx_Closed (���� �� ����)
```

**�ڵ� ����:**
```csharp
public sealed partial class MainWindow : WinUIEx.WindowEx, INotifyPropertyChanged
{
    // Fields
    private Slider IconScaleSlider;
    private TextBlock ScaleValueText;
    private SystemTrayIcon trayIcon;
    private GlobalHotkeyManager hotkeyManager;
    private Microsoft.UI.Xaml.Controls.TabViewItem _previousTab;
    
    // Tab color presets
    private readonly Dictionary<string, Color> _tabColorPresets = ...;
    private readonly Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color> _tabColors = ...;
    
    public MainWindow()
    {
        this.InitializeComponent();
  InitializeTabs();
        // ...
    }
    
    private async void Container_Loaded(object sender, RoutedEventArgs e)
    {
     // Initialization logic
    }
    
    private void WindowEx_Closed(object sender, WindowEventArgs args)
    {
     // Cleanup
    }
}
```

---

### **2. MainWindow.Tabs.cs (�� ����)**

**����:** �� ����, ����, ����, ���� ����

**���� �޼���:**
```csharp
? InitializeTabs()
? MainTabView_AddTabButtonClick()
? AttachTabContextMenu()
? ChangeTabColor_Click()
? UpdateTabColorSeparator()
? RenameTab_Click()
? DeleteTab_Click()
? MainTabView_SelectionChanged()
? MainTabView_TabCloseRequested()
? SaveCurrentTabItems()
? LoadCurrentTabItems()
```

**�ٽ� �ڵ�:**
```csharp
/// <summary>
/// Tab management partial class for MainWindow
/// </summary>
public sealed partial class MainWindow
{
    private void InitializeTabs()
    {
        var defaultTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
        defaultTab.Header = "�⺻";
        // ...
    }
    
    private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
    {
        _tabColors[tab] = color;
  UpdateTabColorSeparator(tab, color);
}

    // ... ��Ÿ �� ���� �޼����
}
```

---

### **3. MainWindow.Items.cs (������ ����)**

**����:** ������ �߰�, ����, ����ȭ

**���� �޼���:**
```csharp
? AddGridViewTile()
? SerialiseGridViewItemsToList()
? DeserialiseListToGridViewItems()
? ItemsGridViewItems_VectorChanged()
? AddFileBtn_Click()
? AddFolderBtn_Click()
? AddWebsiteBtn_Click()
```

**�ٽ� �ڵ�:**
```csharp
/// <summary>
/// Item management partial class for MainWindow
/// </summary>
public sealed partial class MainWindow
{
    private GridViewTile AddGridViewTile(string executingPath, 
     string executingArguments, string displayText, BitmapImage imageSource)
    {
   GridViewTile gridViewTile = new GridViewTile();
      gridViewTile.ExecutingPath = executingPath;
        // ...
   return gridViewTile;
    }
    
    private async void AddFileBtn_Click(object sender, RoutedEventArgs e)
    {
        AddFileDialog addFileDialog = new AddFileDialog()
        {
    XamlRoot = Container.XamlRoot
   };
        // ...
    }
}
```

---

### **4. MainWindow.DragDrop.cs (�巡��&���)**

**����:** �巡�� �� ��� ��� (�׷� ����, �ܺ� ���� �߰�)

**���� �޼���:**
```csharp
? TryShowDragDropInterface()
? ItemsGridView_DragItemsStarting()
? GridViewTile_DragEnter()
? GridViewTile_DragLeave()
? GridViewTile_Drop()
? GridViewTileGroup_DragEnter()
? GridViewTileGroup_DragLeave()
? GridViewTileGroup_Drop()
? ItemsGridView_DragItemsCompleted()
? DragDropParent_DragEnter/Over/Leave()
? DragDropInterface_DragEnter/Over/Drop()
```

**�ٽ� �ڵ�:**
```csharp
/// <summary>
/// Drag and drop functionality partial class for MainWindow
/// </summary>
public sealed partial class MainWindow
{
    List<UserControl> GridViewItemsToRemove = new List<UserControl>();
    
    private void GridViewTile_Drop(object sender, DragEventArgs e)
    {
  // Create a new group when a GridViewTile is dropped over a GridViewTile
        GridViewTileGroup newGridViewTileGroup = new GridViewTileGroup();
        // ...
    }
    
    private async void DragDropInterface_Drop(object sender, DragEventArgs e)
    {
        // Handle external files/folders/websites being dropped
     // ...
    }
}
```

---

### **5. MainWindow.Search.cs (�˻� ���)**

**����:** �˻��ڽ� ��� (�ڵ��ϼ�, ��� ����)

**���� �޼���:**
```csharp
? SearchBox_GotFocus()
? SearchBox_TextChanged()
? SearchBox_SuggestionChosen()
? SearchBox_QuerySubmitted()
```

**�ٽ� �ڵ�:**
```csharp
/// <summary>
/// Search functionality partial class for MainWindow
/// </summary>
public sealed partial class MainWindow
{
    List<GridViewTile> AllLauncherXItems = new List<GridViewTile>();
    List<string> SearchBoxDropdownItems = new List<string>();
    
    private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, 
        AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        string query = sender.Text?.Trim();
        
    // Check if the query is a file or folder path
   // Or search through items
        // ...
    }
}
```

---

### **6. MainWindow.UI.cs (UI ����)**

**����:** UI ������Ʈ, ����, �����ϸ�

**���� �޼���:**
```csharp
? UpdateUIFromSettings()
? AlignGridViewLeft()
? AlignGridViewCenter()
? CreateIconScaleSlider()
? IconScaleSlider_ValueChanged()
? ItemsGridView_PointerWheelChanged()
? SettingsButton_Click()
? CloseButton_Click()
? WindowEx_SizeChanged()
```

**�ٽ� �ڵ�:**
```csharp
/// <summary>
/// UI management partial class for MainWindow
/// </summary>
public sealed partial class MainWindow
{
    private bool isSettingsWindowOpen = false;
    
    private void UpdateUIFromSettings()
    {
        // Adjust the size of items in ItemsGridView
        // Set windowing mode
// Align the GridView
        // ...
    }
    
    private void IconScaleSlider_ValueChanged(object sender, 
 Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        double scale = Math.Round(IconScaleSlider.Value, 2);
        UserSettingsClass.GridScale = scale;
        // ...
}
}
```

---

### **7. MainWindow.Hotkeys.cs (����Ű/Ʈ����)**

**����:** ���� ����Ű, �ý��� Ʈ���� ������

**���� �޼���:**
```csharp
? InitializeTrayIcon()
? Window_Closing()
? InitializeGlobalHotkey()
? ToggleWindowVisibility()
```

**�ٽ� �ڵ�:**
```csharp
/// <summary>
/// Hotkeys and system tray partial class for MainWindow
/// </summary>
public sealed partial class MainWindow
{
    private void InitializeTrayIcon()
    {
        var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
        "Resources", "icon.ico");
        trayIcon = new SystemTrayIcon(hwnd, iconPath, "SLauncher - Double-click to open");
        // ...
    }
    
    private void ToggleWindowVisibility()
    {
        bool isVisible = this.AppWindow.IsVisible;
        
        if (isVisible)
        {
      this.AppWindow.Hide();
  }
        else
        {
          this.AppWindow.Show();
            this.Activate();
SearchBox.Focus(FocusState.Programmatic);
  }
    }
}
```

---

## ?? **Partial Class�� ���� ����**

### **C# Partial Class��?**

```csharp
// File 1: Person.cs
public partial class Person
{
    public string FirstName { get; set; }
    
    public void SayHello()
    {
        Console.WriteLine($"Hello, {FirstName}!");
    }
}

// File 2: Person.Work.cs
public partial class Person
{
    public string JobTitle { get; set; }
    
    public void DoWork()
    {
        Console.WriteLine($"{FirstName} is working as {JobTitle}");
    }
}

// ������ �� �ϳ��� Ŭ������ ������:
public class Person
{
    public string FirstName { get; set; }
    public string JobTitle { get; set; }
  
    public void SayHello() { ... }
    public void DoWork() { ... }
}
```

**Ư¡:**
```
? ���� ���Ͽ� ���� Ŭ���� �и� ����
? ������ �� �ϳ��� ������
? ��� ������ ���� namespace�� �־�� ��
? ��� ���Ͽ� 'partial' Ű���� �ʿ�
? ���� ������ (public, private) ����
```

---

## ?? **Before vs After ��**

### **Before (1�� ����):**

```
MainWindow.xaml.cs (2,034��)
��
���� Fields & Properties (70��)
���� Constructor (30��)
���� Helper Methods (150��)
���� Tab Management (500��)
���� Item Management (300��)
���� Drag & Drop (400��)
���� Search (250��)
���� UI Updates (200��)
���� Hotkeys & Tray (134��)

? ������:
- ��ũ�� �ð� ��
- �ڵ� ã�� �����
- ���� �� ���� Ȯ�� ��
- AI context �ʰ�
```

---

### **After (7�� ����):**

```
?? SLauncher/
���� MainWindow.xaml.cs  (324��) ? ����
���� MainWindow.Tabs.cs    (358��) ?? ��
���� MainWindow.Items.cs       (204��) ?? ������
���� MainWindow.DragDrop.cs    (267��) ?? �巡��
���� MainWindow.Search.cs      (218��) ?? �˻�
���� MainWindow.UI.cs        (210��) ??? UI
���� MainWindow.Hotkeys.cs     (153��) ?? ����Ű

? ����:
- ��ɺ��� ������
- ���� �ڵ� ã��
- ���� �� ���� ����
- AI�� �����ϱ� ����
```

---

## ?? **���Ϻ� å�� (Separation of Concerns)**

| ���� | å�� | ������ |
|------|------|--------|
| **MainWindow.xaml.cs** | �ʱ�ȭ, �ʵ� ���� | ��� partial class ��� |
| **MainWindow.Tabs.cs** | �� ����/����/���� | `ItemsGridView`, `_tabColors` |
| **MainWindow.Items.cs** | ������ �߰�/���� | `UserSettingsClass` |
| **MainWindow.DragDrop.cs** | �巡��&��� ó�� | `GridViewTile`, `GridViewTileGroup` |
| **MainWindow.Search.cs** | �˻� �� ��� ���� | `AllLauncherXItems` |
| **MainWindow.UI.cs** | UI ������Ʈ, �����ϸ� | `UserSettingsClass`, `ItemsGridView` |
| **MainWindow.Hotkeys.cs** | ����Ű, Ʈ���� | `SystemTrayIcon`, `GlobalHotkeyManager` |

---

## ?? **��� ����**

### **���� 1: �� ���� ����**

**Before:**
```
1. MainWindow.xaml.cs ����
2. 2,000�� �߿��� �� ���� �ڵ� ã�� (Ctrl+F)
3. ���� �޼��尡 ���� �־� ã�� �����
```

**After:**
```
1. MainWindow.Tabs.cs ����
2. 358�ٸ� Ȯ���ϸ� ��!
3. �� ���� ��� �ڵ尡 �� ����!
```

---

### **���� 2: �˻� ��� ����**

**Before:**
```
1. MainWindow.xaml.cs ����
2. SearchBox ���� �޼��� 4�� ã��
3. �巡��&���, UI �ڵ�� ���� ����
```

**After:**
```
1. MainWindow.Search.cs ����
2. �˻� ���� ��� �ڵ尡 218�ٿ� ������!
3. �ٸ� ��ɰ� �и��Ǿ� ����
```

---

### **���� 3: �巡��&��� ���� ����**

**Before:**
```
1. MainWindow.xaml.cs ����
2. GridViewTile_Drop ã��
3. ���� �޼������ �������� ����� ����
```

**After:**
```
1. MainWindow.DragDrop.cs ����
2. �巡��&��� ���� ��� ������ �� ���Ͽ�!
3. GridViewItemsToRemove �ʵ嵵 ���� ���Ͽ�!
```

---

## ??? **����� ���λ���**

### **Using Directives:**

�� partial class ������ �ʿ��� using�� ����:

```csharp
// MainWindow.Tabs.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SLauncher.Controls.GridViewItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;

// MainWindow.Search.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SLauncher.Classes;
using SLauncher.Controls.GridViewItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
```

---

### **�ʵ� ����:**

��� partial class�� ���� �ʵ忡 ���� ����:

```csharp
// MainWindow.xaml.cs (���� ���Ͽ��� ����)
private Slider IconScaleSlider;
private SystemTrayIcon trayIcon;
private Dictionary<TabViewItem, Color> _tabColors;

// MainWindow.Tabs.cs (���� ����)
private void ChangeTabColor_Click(...)
{
    _tabColors[tab] = color;  // ? ���� ����!
}

// MainWindow.UI.cs (���� ����)
private void CreateIconScaleSlider()
{
    IconScaleSlider = new Slider();  // ? ���� ����!
}
```

---

### **�̺�Ʈ �ڵ鷯 ����:**

XAML�� �̺�Ʈ �ڵ鷯�� � partial class�� �־ �۵�:

```xml
<!-- MainWindow.xaml -->
<Button Click="AddFileBtn_Click" />
<!-- MainWindow.Items.cs�� �ִ� �޼��� -->

<AutoSuggestBox QuerySubmitted="SearchBox_QuerySubmitted" />
<!-- MainWindow.Search.cs�� �ִ� �޼��� -->

<TabView SelectionChanged="MainTabView_SelectionChanged" />
<!-- MainWindow.Tabs.cs�� �ִ� �޼��� -->
```

---

## ?? **���� ����**

### **������ �ð�:**
```
? ���� ����
- Partial class�� ������ �� �ϳ��� ������
- ���� IL �ڵ�� ����
```

### **��Ÿ�� ����:**
```
? ���� ����
- ��Ÿ�ӿ����� �ϳ��� Ŭ����
- �޼��� ȣ�� ������� ����
```

### **���� ����:**
```
? ũ�� ���!
- ���� �ڵ� Ž��
- ��Ȯ�� ����
- ���� ��������
- AI�� context �ľ� ����
```

---

## ?? **Best Practices**

### **1. ��Ȯ�� ���ϸ�:**
```
? MainWindow.Tabs.cs        (�� ����)
? MainWindow.DragDrop.cs    (�巡��&��� ����)
? MainWindow.Search.cs      (�˻� ����)

? MainWindow.Part1.cs (�ǹ� �Ҹ�Ȯ)
? MainWindow.Misc.cs      (�⵿���)
```

---

### **2. ���� �׷�ȭ:**
```
? ���� ��ɳ��� ����
   - �� ����/����/���� �� MainWindow.Tabs.cs
   - �˻�/�ڵ��ϼ� �� MainWindow.Search.cs

? ���ĺ������� ������
? �� ���� ��������� ������
```

---

### **3. ���� ���� �ּ�ȭ:**
```
? MainWindow.xaml.cs�� �ּ�������
   - �ʵ� ����
   - ������
   - �ٽ� �̺�Ʈ �ڵ鷯

? ��� �ڵ带 ���� ���Ͽ�
```

---

### **4. using �ּ�ȭ:**
```
? �� ���Ͽ� �ʿ��� using�� ����
   - MainWindow.Tabs.cs �� Windows.UI (Color ������)
   - MainWindow.Search.cs �� System.Diagnostics (Process ������)

? ��� ���Ͽ� ��� using ����
```

---

## ?? **���� Ȯ�� ���ɼ�**

### **�� ��� �߰� ��:**

```csharp
// �� ���� ����: MainWindow.Plugins.cs
namespace SLauncher
{
    /// <summary>
    /// Plugin management partial class for MainWindow
    /// </summary>
    public sealed partial class MainWindow
{
    private void LoadPlugins()
        {
            // Plugin loading logic
        }
   
     private void UnloadPlugins()
      {
          // Plugin unloading logic
        }
    }
}
```

**����:**
- ���� ���� ���� ���ʿ�
- ���������� ���� ����
- Git conflict ����

---

## ?? **�н� �ڷ�**

### **C# Partial Class ���� ����:**
```
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods
```

### **���� Partial Class�� ����ϳ�?**
```
? ū Ŭ������ �������� ���� ��
? ���� ���� ���� Ŭ���� �۾� ��
? Code generator�� ���� �ڵ� �и� ��
? XAML �����̳� �ڵ� �и� �� (WPF, WinUI)

? �ܼ��� ���� ũ�⸸ ���̷���
? ���� ���� ���� ����� ������ ���� ��
```

---

## ? **�Ϸ�!**

### **���� ����:**
```
? MainWindow.xaml.cs (324�� - ����)
? MainWindow.Tabs.cs        (358�� - �� ����)
? MainWindow.Items.cs       (204�� - ������)
? MainWindow.DragDrop.cs    (267�� - �巡��)
? MainWindow.Search.cs      (218�� - �˻�)
? MainWindow.UI.cs  (210�� - UI)
? MainWindow.Hotkeys.cs     (153�� - ����Ű)
```

### **����:**
```
? �ڵ� Ž�� �ӵ� 7�� ���
? ��ɺ��� ��Ȯ�� �и�
? ���� �� ���� Ȯ�� ����
? AI�� context �ľ� ����
? �� ���� �� conflict ����
? �� ��� �߰� ����
```

### **���� ���:**
```
? ���� ����!
? ��� ���� �۵�!
? ���� ���� ����!
```

---

## ?? **���� MainWindow.xaml.cs�� �ξ� �����ϱ� ���������ϴ�!**

�� ������ ��Ȯ�� å���� ������ �־:
- ?? **�ڵ� ã�� ����**
- ?? **�����ϱ� ����**
- ?? **AI�� �����ϱ� ����**
- ?? **�� ���� ����**

**��� ����� �Ϻ��ϰ� �۵��մϴ�!** ?
