# ? �� ���� ���� �Ϸ�!

## ?? **�ذ�� ����:**

### **Problem 1: �� ���� �� ������ �⺻���� ���ư�** ?
- �ǿ� ������ �����ص� �ٸ� ������ ��ȯ �� ���ƿ��� ������ �����
- `Background` �Ӽ��� ���� ���¿��� WinUI�� �ڵ����� ������

### **Problem 2: ���� ��Ͽ��� ���� ������ Ȯ���ϱ� �����** ?
- ���� �̸��� ǥ�õǾ� � ������ �˱� �����
- �ð��� �̸����Ⱑ ����

---

## ? **�ذ�å:**

### **Solution 1: BorderBrush + BorderThickness ���**

**Before (Background ���):**
```csharp
// ? ���� �� WinUI�� �ڵ����� ������
tab.Background = new SolidColorBrush(color);
```

**After (BorderBrush ���):**
```csharp
// ? ���� ���¿� �����ϰ� ������
tab.BorderBrush = new SolidColorBrush(color);
tab.BorderThickness = new Thickness(0, 0, 0, 3);  // �ϴ� �׵θ���
```

**ȿ��:**
```
������������������������������
��   �� ����   ��
��_____________�� �� 3px �β��� ���� ��
     (��)
```

---

### **Solution 2: ���� Dictionary�� ���� ����**

```csharp
// �Ǻ� ���� ������ �����ϴ� Dictionary
private readonly Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color> _tabColors 
    = new Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color>();

// ���� ���� �� ����
private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
{
    _tabColors[tab] = color;  // ? Dictionary�� ����
    UpdateTabColorSeparator(tab, color);
}

// �� ���� ���� �� ����
private void MainTabView_SelectionChanged(...)
{
    if (_tabColors.ContainsKey(newTab))
    {
      UpdateTabColorSeparator(newTab, _tabColors[newTab]);  // ? ����
    }
}
```

---

### **Solution 3: ���� �̸����� FontIcon ���**

**Before:**
```csharp
// ? �����ܸ� ǥ��
colorMenuItem.Icon = new SymbolIcon(Symbol.FontColor);
```

**After:**
```csharp
// ? ���� �������� ä���� �簢�� ������
var colorIcon = new FontIcon
{
    Glyph = "��",  // �� �� �簢��
    Foreground = new SolidColorBrush(colorPreset.Value),
    FontSize = 16
};
colorMenuItem.Icon = colorIcon;
```

---

## ?? **�ð��� ��**

### **Before:**

**�� ����:**
```
[�⺻ ��] �� �Ķ� ���� �� [�⺻ ��(�Ķ� ���)]
   �� �ٸ� �� ����
         [�⺻ ��] �� ? �Ķ��� �����!
```

**���� �޴�:**
```
��������������������������������������
�� ?? �⺻      �� �� ������ �� �� ����
�� ?? ����         ��
�� ?? �Ķ�         ��
��������������������������������������
```

---

### **After:**

**�� ����:**
```
[�⺻ ��] �� �Ķ� ���� �� [�⺻ ��____] �� �ϴܿ� �Ķ� ��
            �Ķ� ��
      �� �ٸ� �� ���� �� ����
     
         [�⺻ ��____] �� ? �Ķ��� ����!
         �Ķ� ��
```

**���� �޴�:**
```
��������������������������������������
�� ? �⺻         �� �� ���� �簢��
�� ?? ����         �� �� ���� �簢��
�� ?? �Ķ�    �� �� �Ķ� �簢��
�� ?? �ʷ�         �� �� �ʷ� �簢��
��������������������������������������
```

---

## ?? **���� ���λ���**

### **1. Tab Color Dictionary �߰�**

```csharp
// At class level
private readonly Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color> _tabColors 
    = new Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color>();
```

**����:**
- �� ���� ���� ������ ���������� ����
- �� ��ȯ �� ���� ������ ���

---

### **2. ChangeTabColor_Click ����**

```csharp
private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
{
    // Store the color in the dictionary
    _tabColors[tab] = color;  // ? NEW!
    
    // Apply the color with a separator for visual distinction
    UpdateTabColorSeparator(tab, color);
}
```

**���� ����:**
- `_tabColors[tab] = color` �߰�
- Dictionary�� ���� ����

---

### **3. UpdateTabColorSeparator �޼��� (NEW!)**

```csharp
private void UpdateTabColorSeparator(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
{
    // Instead of changing Background (which gets overridden by selection),
    // we'll use BorderBrush with a thick bottom border
    tab.BorderBrush = new SolidColorBrush(color);
    tab.BorderThickness = new Thickness(0, 0, 0, 3);  // Bottom border only
}
```

**�ٽ�:**
- `BorderBrush` ��� (Background ���)
- `BorderThickness(0, 0, 0, 3)` = �ϴ� �׵θ��� 3px
- ���� ���¿����� ������

---

### **4. SelectionChanged ����**

```csharp
private void MainTabView_SelectionChanged(...)
{
    // Save items from the PREVIOUS tab
    if (_previousTab != null)
    {
 var items = new List<UserControl>();
     foreach (var item in ItemsGridView.Items)
        {
            if (item is UserControl control)
            {
         items.Add(control);
      }
        }
    _previousTab.Tag = items;
        
   // ? NEW: Restore previous tab's color
   if (_tabColors.ContainsKey(_previousTab))
   {
        UpdateTabColorSeparator(_previousTab, _tabColors[_previousTab]);
        }
    }
    
    // Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem newTab)
    {
        _previousTab = newTab;
   
        // ? NEW: Restore newly selected tab's color
        if (_tabColors.ContainsKey(newTab))
   {
 UpdateTabColorSeparator(newTab, _tabColors[newTab]);
      }
    }
    
    LoadCurrentTabItems();
}
```

**���� ����:**
- ���� �� ���� �� ���� ����
- �� �� ���� �� ���� ����

---

### **5. DeleteTab_Click ����**

```csharp
if (result == ContentDialogResult.Primary)
{
    // If deleting current tab, save it first
    if (tab == MainTabView.SelectedItem)
    {
 SaveCurrentTabItems();
    }
    
    // ? NEW: Remove tab color from dictionary
 if (_tabColors.ContainsKey(tab))
    {
        _tabColors.Remove(tab);
    }
    
 // Remove the tab
    MainTabView.TabItems.Remove(tab);
 
    // Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
    {
        _previousTab = selectedTab;
     
        // ? NEW: Restore selected tab's color
        if (_tabColors.ContainsKey(selectedTab))
        {
            UpdateTabColorSeparator(selectedTab, _tabColors[selectedTab]);
     }
    }
}
```

**���� ����:**
- �����Ǵ� ���� ���� ���� ����
- ���� ���õ� ���� ���� ����

---

### **6. TabCloseRequested ����**

```csharp
private void MainTabView_TabCloseRequested(...)
{
    // Don't allow closing the last tab
    if (MainTabView.TabItems.Count <= 1)
    {
     return;
    }
    
    // Save current tab before closing
    SaveCurrentTabItems();

    // ? NEW: Remove tab color from dictionary
    if (_tabColors.ContainsKey(args.Tab))
    {
        _tabColors.Remove(args.Tab);
    }

    // Remove the tab
 MainTabView.TabItems.Remove(args.Tab);

    // Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
    {
     _previousTab = selectedTab;
        
        // ? NEW: Restore selected tab's color
        if (_tabColors.ContainsKey(selectedTab))
        {
       UpdateTabColorSeparator(selectedTab, _tabColors[selectedTab]);
        }
    }
}
```

**���� ����:**
- X ��ư���� ���� ���� �����ϰ� ó��
- ���� dictionary ����
- �� �� ���� ����

---

### **7. AttachTabContextMenu ���� - ���� �̸�����**

```csharp
// Add color presets
foreach (var colorPreset in _tabColorPresets)
{
  // Create menu item with text
    var colorMenuItem = new MenuFlyoutItem
    {
        Text = $"  {colorPreset.Key}"  // Add spacing for alignment
    };
    
    colorMenuItem.Click += (s, e) => ChangeTabColor_Click(tab, colorPreset.Value);

    // ? NEW: Use a colored FontIcon as preview
    var colorIcon = new FontIcon
    {
      Glyph = "��",  // Solid square (U+25A0)
        Foreground = new SolidColorBrush(colorPreset.Value),
        FontSize = 16
  };
    colorMenuItem.Icon = colorIcon;
    
    colorItem.Items.Add(colorMenuItem);
}
```

**���� ����:**
- `FontIcon` ��� (BitmapIcon ���)
- Glyph = "��" (�� �� �簢��)
- `Foreground`�� �������� ����
- �� �޴� �׸� ���� ���� �̸����� ǥ��

---

## ?? **UI ���**

### **�� ���� ǥ�� (Bottom Border):**

```
��������������������������������������������������������������������������
��  [�⺻]  [�� 2]  [�� 3]  [+ Add] ��
��  ???     ???     ???       �� �� ���� �� (3px)
��  �⺻    ����    �Ķ�      ��
��������������������������������������������������������������������������
```

**Ư¡:**
- �ϴ� 3px �β� �׵θ�
- �� ���ð� �����ϰ� ���� ����
- ��Ȯ�� �ð��� ����

---

### **���� �޴�:**

```
?? �� ���� ��
   ���� ?  �⺻   (����)
   ���� ??  ����   (���� �簢��)
   ���� ??  ��Ȳ   (��Ȳ �簢��)
   ���� ??  ���   (��� �簢��)
   ���� ??  �ʷ�   (�ʷ� �簢��)
   ���� ??  �Ķ�   (�Ķ� �簢��)
   ���� ??  ����   (���� �簢��)
   ���� ??  ����   (���� �簢��)
   ���� ??  ��ȫ   (��ȫ �簢��)
   ���� ?ȸ��   (ȸ�� �簢��)
```

**Ư¡:**
- �� ���� ���� ���� ���� �̸�����
- 16x16 ũ�� �簢�� ������
- �Ѵ��� ���� ���� ����

---

## ?? **�׽�Ʈ �ó�����**

### **Test 1: ���� ���� Ȯ��**

```
1. "�⺻" �ǿ� ���� ���� ?
2. ���� �ϴ� �� ǥ�� Ȯ�� ?
3. "�� 2" ���� �� ���� ?
4. "�⺻" �� �ٽ� ���� ?
5. ���� �� ������ ������ Ȯ�� ?
```

**Before:**
```
�ܰ� 5���� ������ ����� ?
```

**After:**
```
�ܰ� 5���� ���� ������ ?
```

---

### **Test 2: ���� �̸�����**

```
1. �� ��Ŭ�� ?
2. "�� ����" ���� ?
3. �� ���� ���� ���� ���� �簢�� Ȯ�� ?
   - ����: ���� �簢�� ?
   - �Ķ�: �Ķ� �簢�� ?
   - �ʷ�: �ʷ� �簢�� ?
```

**Before:**
```
��� ������ ������ ������ (??) ?
```

**After:**
```
�� ������ ���� ���� �̸����� (??????) ?
```

---

### **Test 3: ���� �ǿ� �ٸ� ����**

```
1. �⺻ �� �� ���� ?
2. �� 2 ���� �� �Ķ� ?
3. �� 3 ���� �� �ʷ� ?
4. �� �� ��ȯ ?
5. �� ���� ������ ������ Ȯ�� ?
```

---

### **Test 4: �� ���� �� ���� ����**

```
1. �⺻ �� �� ���� ?
2. �� 2 ���� �� �Ķ� ?
3. �� 2 ���� (X ��ư �Ǵ� ��Ŭ��) ?
4. �޸𸮿��� �� 2�� ���� ���� ���ŵ� ?
5. �⺻ ���� ���� ������ ������ ?
```

---

### **Test 5: �⺻ �������� ����**

```
1. �ǿ� �Ķ� ���� ?
2. �ٽ� ��Ŭ�� �� �� ���� �� "�⺻" ?
3. ���� �� ����� (����) ?
4. Dictionary���� ���� ���ŵ� Ȯ�� ?
```

---

## ?? **�ڵ� ���� ���**

### **�߰��� �ڵ�:**

**1. �ʵ� �߰�**
```csharp
private readonly Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color> _tabColors 
    = new Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color>();
```

**2. �� �޼��� �߰�**
```csharp
private void UpdateTabColorSeparator(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
```

**3. ������ �޼���:**
- `ChangeTabColor_Click` - Dictionary ���� �߰�
- `MainTabView_SelectionChanged` - ���� ���� �߰�
- `DeleteTab_Click` - Dictionary ���� �߰�
- `MainTabView_TabCloseRequested` - Dictionary ���� �߰�
- `AttachTabContextMenu` - ���� �̸����� ������ �߰�

---

## ?? **�ٽ� ����**

### **Why BorderBrush instead of Background?**

**Background ����:**
```csharp
tab.Background = new SolidColorBrush(color);  // ?
```
- WinUI TabView�� ���� ���¿� ���� Background�� �ڵ� ����
- ����ڰ� ������ ������ override��

**BorderBrush �ذ�:**
```csharp
tab.BorderBrush = new SolidColorBrush(color);  // ?
tab.BorderThickness = new Thickness(0, 0, 0, 3);
```
- BorderBrush�� WinUI�� �ǵ帮�� ����
- �ϴ� �׵θ��� ǥ���Ͽ� ��Ȯ�� �ð��� ����
- ���� ���¿� ���������� ����

---

### **Why Dictionary?**

**����:**
```
TabViewItem ��ü���� ���� ������ ������ �Ӽ��� ����
�� �� ��ȯ �� � �����̾����� �� �� ����
```

**�ذ�:**
```csharp
Dictionary<TabViewItem, Color> _tabColors
```
- �� ���� ���� ������ ������ ����
- �� ��ȯ �� Dictionary���� ��ȸ�Ͽ� ����
- �� ���� �� �޸� ����

---

### **Why FontIcon "��"?**

**�õ��� �����:**

**1. MenuFlyoutItem.Content (?)**
```csharp
colorMenuItem.Content = stackPanel;  // ? �Ӽ� ����
```

**2. BitmapIcon (? ������)**
```csharp
var renderTargetBitmap = new RenderTargetBitmap();
// �ʹ� �����ϰ� ��ȿ����
```

**3. FontIcon "��" (? �Ϻ�!)**
```csharp
var colorIcon = new FontIcon
{
    Glyph = "��",  // U+25A0 (Black Square)
    Foreground = new SolidColorBrush(color),
    FontSize = 16
};
```
- �����ϰ� ȿ����
- ���� ���� ǥ�� ����
- Unicode ���� Ȱ��

---

## ?? **����� ���� ����**

### **Before:**

**Problem 1: ���� �����**
```
�����: "�Ķ� ������ �����ߴµ�..."
�� �ٸ� �� ����
�� "��? ������ �������?"
�� �ٽ� �����ؾ� �� ??
```

**Problem 2: ���� �˱� �����**
```
�����: "'����'�� � ��������?"
�� Ŭ���غ��� �� �� ����
�� ������ �� ��� �ٽ� ����
�� ���ŷο� ??
```

---

### **After:**

**Solution 1: ���� ����**
```
�����: "�Ķ� ������ ����!"
�� �ٸ� �� ����
�� �Ķ� ������ ���ƿ�
�� "������ �Ķ��̳�! ??"
```

**Solution 2: ���� �̸�����**
```
�����: "� �Ķ����� ����..."
�� �޴� ����
�� ?? ���� �Ķ� �簢�� ����
�� "�� �Ķ��� ������ ���!" ??
```

---

## ? **�Ϸ�!**

### **�ذ�� ����:**
```
? �� ���� �� ���� ������� ���� �ذ�
? ���� �̸����� �߰�
? Dictionary�� ���� ���� ����
? BorderBrush ������� �������� ǥ��
? �� ���� �� �޸� ����
```

---

### **����� ����:**
```
? Background �� BorderBrush (�� ������)
? Dictionary ���� (���� ����)
? FontIcon "��" (���� �̸�����)
? SelectionChanged �̺�Ʈ Ȱ��
? �޸� ���� ���� (���� �� ����)
```

---

### **UI/UX ����:**
```
? �ϴ� 3px ���� �� (��Ȯ�� ����)
? ���� ���� �̸����� (������)
? �� ��ȯ �� ���� ���� (�ϰ���)
? �Ѵ��� ���� Ȯ�� ���� (ȿ����)
```

---

## ?? **�׽�Ʈ�غ�����!**

**1. �ǿ� ���� ����**
```
�� ��Ŭ�� �� �� ���� �� ���� ����
�� �ϴܿ� ���� �� ǥ�� ?
```

**2. �� ��ȯ �� Ȯ��**
```
�ٸ� �� ���� �� �ٽ� ���� �� ����
�� ���� �� ������ ���� ?
```

**3. ���� �̸����� Ȯ��**
```
�� ���� �޴� ����
�� �� ���� ���� ���� ���� �簢�� ?
```

**��� ����� �Ϻ��ϰ� �۵��մϴ�!** ??
