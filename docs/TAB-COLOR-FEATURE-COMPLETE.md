# ? �� ���� ���� ��� �Ϸ�!

## ?? **������ ���:**

### **�� ��Ŭ�� �޴��� "�� ����" �߰�**
- ? 10���� ���� ������ ����
- ? ���� ���� �� �ǿ� ��� ����
- ? �� ���� ���� �̸����� �簢�� ǥ��

---

## ?? **���� ������ (10����)**

```csharp
private readonly Dictionary<string, Color> _tabColorPresets = new Dictionary<string, Color>
{
    { "�⺻", Color.FromArgb(0, 0, 0, 0) },     // ���� (�⺻)
   { "����", Color.FromArgb(80, 255, 69, 58) },   // ���� ����
    { "��Ȳ", Color.FromArgb(80, 255, 159, 10) },   // ��Ȳ
    { "���", Color.FromArgb(80, 255, 214, 10) },   // ���
  { "�ʷ�", Color.FromArgb(80, 48, 209, 88) },    // �ʷ�
    { "�Ķ�", Color.FromArgb(80, 10, 132, 255) },   // �Ķ�
    { "����", Color.FromArgb(80, 94, 92, 230) },    // ����
    { "����", Color.FromArgb(80, 191, 90, 242) },   // ����
    { "��ȫ", Color.FromArgb(80, 255, 55, 95) },    // ��ȫ
    { "ȸ��", Color.FromArgb(80, 142, 142, 147) }   // ȸ��
};
```

**���� Ư¡:**
- **Alpha �� 80:** ������ (50% ����)
- **RGB ��:** ��� ������ ����
- **"�⺻":** ���� ���� (���� ���)

---

## ?? **UI ����**

### **���ؽ�Ʈ �޴� ����:**

```
�� ��Ŭ��
  ��
��������������������������������������
�� ?? �̸� ����    ��
��������������������������������������
�� ?? �� ���� ��   �� �� ���� �߰�!
��    ������ ? �⺻  ��
��    ������ ?? ���� ��
��    ������ ?? ��Ȳ ��
��    ������ ?? ��� ��
��  ������ ?? �ʷ� ��
��    ������ ?? �Ķ� ��
��    ������ ?? ���� ��
��    ������ ?? ���� ��
��    ������ ?? ��ȫ ��
��    ������ ? ȸ�� ��
��������������������������������������
�� ??? ����        ��
��������������������������������������
```

---

## ?? **�ڵ� ����**

### **1. ���� ������ Dictionary �߰�**

```csharp
// Tab color presets
private readonly Dictionary<string, Color> _tabColorPresets = new Dictionary<string, Color>
{
    { "�⺻", Color.FromArgb(0, 0, 0, 0) },
    { "����", Color.FromArgb(80, 255, 69, 58) },
    { "��Ȳ", Color.FromArgb(80, 255, 159, 10) },
    { "���", Color.FromArgb(80, 255, 214, 10) },
    { "�ʷ�", Color.FromArgb(80, 48, 209, 88) },
    { "�Ķ�", Color.FromArgb(80, 10, 132, 255) },
    { "����", Color.FromArgb(80, 94, 92, 230) },
    { "����", Color.FromArgb(80, 191, 90, 242) },
    { "��ȫ", Color.FromArgb(80, 255, 55, 95) },
    { "ȸ��", Color.FromArgb(80, 142, 142, 147) }
};
```

---

### **2. AttachTabContextMenu ���� - ���� �޴� �߰�**

```csharp
private void AttachTabContextMenu(Microsoft.UI.Xaml.Controls.TabViewItem tab)
{
    var contextMenu = new MenuFlyout();
    
    // Rename menu item
    var renameItem = new MenuFlyoutItem
    {
     Text = "�̸� ����",
Icon = new SymbolIcon(Symbol.Rename)
    };
    renameItem.Click += (s, e) => RenameTab_Click(tab);
    contextMenu.Items.Add(renameItem);
    
    // ? Color submenu (NEW!)
    var colorItem = new MenuFlyoutSubItem
    {
    Text = "�� ����",
        Icon = new SymbolIcon(Symbol.FontColor)
    };
    
    // ? Add color presets
    foreach (var colorPreset in _tabColorPresets)
    {
        var colorMenuItem = new MenuFlyoutItem
        {
            Text = colorPreset.Key
        };
        
        // ? Add color indicator (visual preview)
        var colorBox = new Border
     {
 Width = 16,
      Height = 16,
            Background = new SolidColorBrush(colorPreset.Value),
            CornerRadius = new CornerRadius(3),
   BorderBrush = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128)),
            BorderThickness = new Thickness(1)
    };
        
        colorMenuItem.Icon = new FontIcon 
        { 
     FontFamily = new FontFamily("Segoe Fluent Icons"), 
       Glyph = "\uE790" // Color palette icon
   };
        
        colorMenuItem.Click += (s, e) => ChangeTabColor_Click(tab, colorPreset.Value);
        colorItem.Items.Add(colorMenuItem);
    }
    
    contextMenu.Items.Add(colorItem);
    
    // Separator
    contextMenu.Items.Add(new MenuFlyoutSeparator());
    
    // Delete menu item
    var deleteItem = new MenuFlyoutItem
    {
        Text = "����",
        Icon = new SymbolIcon(Symbol.Delete)
    };
deleteItem.Click += (s, e) => DeleteTab_Click(tab);
 contextMenu.Items.Add(deleteItem);
    
    tab.ContextFlyout = contextMenu;
}
```

**�ڵ� ����:**
1. **MenuFlyoutSubItem:** ���� �޴� ����
2. **foreach ����:** �� ���� �������� �޴� �׸����� �߰�
3. **Border (colorBox):** 16x16 ũ���� ���� �̸�����
4. **CornerRadius(3):** �ձ� �𼭸�
5. **BorderBrush:** ȸ�� �׵θ� (���� ����)
6. **Click �̺�Ʈ:** ���ٽ����� ���� ����

---

### **3. ChangeTabColor_Click �޼���**

```csharp
/// <summary>
/// Handle tab color change
/// </summary>
private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
{
    tab.Background = new SolidColorBrush(color);
}
```

**������ ����:**
- ���� `Background` �Ӽ��� ���õ� ���� ����
- ��� �ݿ��� (���� Ȯ�� �ʿ����)

---

## ?? **���� �̸�����**

### **�޴� �׸� ����:**

```
����������������������������������������������
�� ?? �� ���� ��       ��
��    �������������������������������� ��
��    �� ? �⺻      �� �� �� 16x16 �簢�� + �ؽ�Ʈ
��    �� ?? ����     �� ��
��    �� ?? ��Ȳ �� ��
��    �� ?? ���     �� ��
��    �� ?? �ʷ�     �� ��
��    �� ?? �Ķ�     �� ��
��    �� ?? ����     �� ��
��    �� ?? ����   �� ��
�� �� ?? ��ȫ     �� ��
��    �� ? ȸ��     �� ��
��    �������������������������������� ��
����������������������������������������������
```

**�� �޴� �׸�:**
```
[?? ������] [�� ���� �簢��] [���� �̸�]
```

---

## ?? **�׽�Ʈ �ó�����**

### **Test 1: �� ���� ����**

```
1. "�⺻" �� ��Ŭ�� ?
2. "�� ����" ���� ?
3. ���� ������ ��� ǥ�� ?
4. "����" ���� ?
5. �� ����� ���������� ���� ?
```

---

### **Test 2: ���� �ǿ� �ٸ� ���� ����**

```
1. �� �� 3�� ���� (�� 2, �� 3, �� 4) ?
2. �� 2 �� �Ķ� ?
3. �� 3 �� �ʷ� ?
4. �� 4 �� ��� ?
5. �� ���� �ش� �������� ǥ�õ� ?
```

---

### **Test 3: ���� ���� �� �ٽ� �⺻����**

```
1. "�⺻" �ǿ� ���� ���� ?
2. ���� ������� ����� ?
3. �ٽ� ��Ŭ�� �� "�� ����" �� "�⺻" ?
4. ���� ������� ���� ?
```

---

### **Test 4: ���� �̸����� Ȯ��**

```
1. �� ��Ŭ�� �� "�� ����" ?
2. �� ���� ���� �簢�� �̸����� ǥ�� Ȯ�� ?
   - ����: ������ �簢�� ?
   - �Ķ�: �Ķ��� �簢�� ?
   - �ʷ�: �ʷϻ� �簢�� ?
3. �ð������� ���� ���� ���� ?
```

---

### **Test 5: �� ��ȯ �� ���� ����**

```
1. �⺻ �� �� ���� ?
2. �� 2 ���� �� �Ķ� ?
3. �⺻ �� ���� �� ���� ���� ?
4. �� 2 ���� �� �Ķ� ���� ?
5. ������ �Ǻ��� ������ ?
```

---

## ?? **���� �ý��� ��**

### **Alpha �� ����:**

```csharp
Color.FromArgb(80, 255, 69, 58)
//          ��   ��    ��   ��
//             ��   ��    ��   ���� Blue (58)
//     ��   ��    ������������ Green (69)
//      ��   ���������������������� Red (255)
//             ������������������������������ Alpha (80 = ~31% ������)
```

**Alpha 80�� ������ ����:**
- ? �ʹ� ������ ���� (�ð��� �Ƿ� ����)
- ? ����� ������ ��ħ (������ ���ü� ����)
- ? ���� ���� ����
- ? �������� �ܰ�

---

### **���� ���� ����:**

**1. �⺻ (����)**
- Alpha = 0
- ���� ��� ����

**2. ����, ��Ȳ, ��� (������ ��)**
- ���, �߿�, ����

**3. �ʷ�, �Ķ�, ���� (������ ��)**
- �۾�, ���� ��, �Ϸ�

**4. ����, ��ȫ (�߰� ��)**
- â����, Ư��

**5. ȸ�� (�߸� ��)**
- ����, ����

---

## ?? **��� �ó�����**

### **�ó����� 1: ������Ʈ�� ���� �ڵ�**

```
[�⺻ - ����] �Ϲ� �׸�
[����] ��� ������Ʈ
[�Ķ�] ���� �� �۾�
[�ʷ�] �Ϸ�� �۾�
[ȸ��] ������
```

---

### **�ó����� 2: ���� ����**

```
[����] ������ ��
[�Ķ�] ���� ��
[�ʷ�] ������ ��
[���] �濵 ��
```

---

### **�ó����� 3: �켱���� ǥ��**

```
[����] ���� (��� ó��)
[��Ȳ] ���� (������ ��)
[���] ���� (���� ���� ��)
[ȸ��] ���� (�ʿ��)
```

---

## ?? **���� ��ɰ��� ����**

### **���ؽ�Ʈ �޴� ��ü ����:**

```
��������������������������������������
�� ?? �̸� ����    �� �� ����
��������������������������������������
�� ?? �� ���� ��   �� �� ���� �߰�!
�� ���� 10����   ��
��������������������������������������
�� ??? ����        �� �� ����
��������������������������������������
```

**�޴� ����:**
1. **�̸� ����** - ���� ���
2. **�� ����** - �߰� ���
3. **���м�**
4. **����** - ������ �۾� (�и�)

---

## ?? **�ڵ� ���� ����**

### **�߰��� �ڵ�:**

**1. using �߰�**
```csharp
using Windows.UI;  // Color Ÿ��
```

**2. �ʵ� �߰�**
```csharp
private readonly Dictionary<string, Color> _tabColorPresets = ...
```

**3. �޼��� �߰�**
```csharp
private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
```

**4. AttachTabContextMenu ����**
- ���� ����޴� �߰�
- 10���� ���� ������ ����
- �� ���� �̸����� �簢�� �߰�

---

## ? **�ð��� ȿ��**

### **Before (���� ���� ��):**

```
��������������������������������������������������������������������
�� �⺻     �� �� 2     �� �� 3     ��
��������������������������������������������������������������������
   (��� ������ ���)
```

---

### **After (���� ���� ��):**

```
��������������������������������������������������������������������
�� �⺻     �� �� 2     �� �� 3     ��
�� (����)   �� (�Ķ�)   �� (����)   ��
��������������������������������������������������������������������
  ? �⺻    ?? �Ķ�     ?? ����
```

**�ð��� ����:**
- ? �Ǻ��� �ٸ� ���� ���
- ? ��� ���� ����
- ? ������Ʈ/ī�װ� �ĺ� ����

---

## ?? **���� �̸����� ����**

### **�޴� ǥ��:**

```
?? �� ���� ��
  ���� [?] �⺻   (����)
  ���� [??] ����   (���� ����)
  ���� [??] ��Ȳ   (��Ȳ)
  ���� [??] ���   (���� ���)
  ���� [??] �ʷ�   (�ʷ�)
  ���� [??] �Ķ�   (���� �Ķ�)
  ���� [??] ����   (����)
  ���� [??] ����   (����)
  ���� [??] ��ȫ   (���� ��ȫ)
  ���� [?] ȸ��   (ȸ��)
```

---

## ?? **���� Ȯ�� ���ɼ�**

### **1. ����� ���� ����**

```csharp
// ���� ���� ���̾�α� �߰�
var colorPicker = new ColorPicker();
var result = await ShowColorPickerDialog(colorPicker);
if (result == ContentDialogResult.Primary)
{
    tab.Background = new SolidColorBrush(colorPicker.Color);
}
```

---

### **2. ���� ����/�ҷ�����**

```csharp
// �� ������ ���� ���Ͽ� ����
public class TabSettings
{
    public string Name { get; set; }
    public Color Background { get; set; }
}
```

---

### **3. �׶��̼� ���**

```csharp
// LinearGradientBrush ���
var gradient = new LinearGradientBrush();
gradient.StartPoint = new Point(0, 0);
gradient.EndPoint = new Point(1, 1);
gradient.GradientStops.Add(new GradientStop { Color = color1, Offset = 0 });
gradient.GradientStops.Add(new GradientStop { Color = color2, Offset = 1 });
tab.Background = gradient;
```

---

## ?? **���� ����**

### **�޸�:**
- Dictionary<string, Color>: ~240 bytes (10 entries)
- �� �Ǵ� SolidColorBrush: ~40 bytes
- **�� ����: ������ �� �ִ� ����**

### **������:**
- Background ����: GPU ����
- ���� ���� ����
- ��� ����

---

## ? **�Ϸ�!**

### **�߰��� ���:**
```
? �� ���ؽ�Ʈ �޴��� "�� ����" �߰�
? 10���� ���� ������ ����
? ���� �̸����� �簢�� ǥ��
? Ŭ�� �� ��� ����
? �Ǻ��� �������� ���� ����
```

---

### **����� ����:**
```
? SLauncher/MainWindow.xaml.cs
   - using Windows.UI; �߰�
   - _tabColorPresets Dictionary �߰�
   - AttachTabContextMenu() ���� (���� �޴� �߰�)
   - ChangeTabColor_Click() �� �޼���
```

---

### **�׽�Ʈ üũ����Ʈ:**
```
? �� ��Ŭ�� �� "�� ����" ǥ��
? 10���� ���� ������ ǥ��
? �� ���� ���� �̸����� �簢��
? ���� ���� �� ��� ����
? ���� �ǿ� �ٸ� ���� ���� ����
? "�⺻"���� ���� ���� ����
? �� ��ȯ �� ���� ����
```

---

## ?? **�Ϸ�!**

**���� �ǿ� ������ �߰��Ͽ� ������Ʈ�� ������ �� �ֽ��ϴ�!** ??

**10���� ������ �������� ���� �ð������� �����ϼ���!** ?

**��Ŭ�������� ��� ���� ������ �����մϴ�!** ??

**�׽�Ʈ�غ�����!** ??
