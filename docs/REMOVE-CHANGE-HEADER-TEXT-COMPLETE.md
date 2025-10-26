# ??? "Change Header Text" ��� ���� - �Ϸ�!

## ? ���ŵ� ���

**"Change header text" ����:**
```
Settings �� Change header text
�� ��� �ؽ�Ʈ ���� TextBox
�� UserSettingsClass.HeaderText ����
�� MainWindow HeaderTextBlock ������Ʈ
```

**����:**
- ���ʿ��� Ŀ���͸���¡
- ��� �� ����
- UI �ϰ��� ������ ����

---

## ?? ������ ����

### **1. SettingsWindow.xaml**

**Before (���� ��):**

```xaml
<!--  Header text  -->
<wct:SettingsCard
    Margin="0,10,0,0"
    Description="Modify the text shown below the search box."
    Header="Change header text">
    <wct:SettingsCard.HeaderIcon>
        <FontIcon Glyph="&#xE70F;" />
    </wct:SettingsCard.HeaderIcon>
    <TextBox x:Name="ChangeHeaderTextBox" Width="220" />
</wct:SettingsCard>

<!--  Fullscreen  -->
<wct:SettingsCard
    Margin="0,5,0,0"
    ...
```

**After (���� ��):**

```xaml
<!--  Settings section  -->
<TextBlock
    FontSize="20"
    FontWeight="Bold"
    Text="Settings" />
<TextBlock
    Margin="0,5,0,0"
    FontSize="13"
    FontStyle="Italic"
    Opacity="0.7"
    Text="Settings are automatically saved once modified." />

<!--  Fullscreen  -->
<wct:SettingsCard
    Margin="0,10,0,0"
    Description="Use SLauncher in fullscreen mode."
    Header="Enable fullscreen">
    ...
```

**���� ����:**
- ? `ChangeHeaderTextBox` SettingsCard ����
- ? Fullscreen�� ù ��° �������� �̵�
- ? Margin ���� (0,5,0,0 �� 0,10,0,0)

---

### **2. SettingsWindow.xaml.cs**

**Before (���� ��):**

```csharp
// Update the textbox and slider to show correct values
ChangeHeaderTextBox.Text = UserSettingsClass.HeaderText;
FullscreenToggleSwitch.IsOn = UserSettingsClass.UseFullscreen;
GridAlignComboBox.SelectedItem = UserSettingsClass.GridPosition;

// ...

// Create event handlers
ChangeHeaderTextBox.TextChanged += ChangeHeaderTextBox_TextChanged;
FullscreenToggleSwitch.Toggled += FullscreenToggleSwitch_Toggled;
GridAlignComboBox.SelectionChanged += GridAlignComboBox_SelectionChanged;

// Make sure to unsubscribe
ChangeHeaderTextBox.Unloaded += (s, e) => ChangeHeaderTextBox.TextChanged -= ChangeHeaderTextBox_TextChanged;
FullscreenToggleSwitch.Unloaded += (s, e) => ...
GridAlignComboBox.Unloaded += (s, e) => ...
```

```csharp
private void ChangeHeaderTextBox_TextChanged(object sender, TextChangedEventArgs e)
{
    // Update UserSettingsClass
    UserSettingsClass.HeaderText = ChangeHeaderTextBox.Text;
    UserSettingsClass.WriteSettingsFile();
}
```

**After (���� ��):**

```csharp
// Update the textbox and slider to show correct values
FullscreenToggleSwitch.IsOn = UserSettingsClass.UseFullscreen;
GridAlignComboBox.SelectedItem = UserSettingsClass.GridPosition;

// Set startup toggle
StartWithWindowsToggleSwitch.IsOn = UserSettingsClass.StartWithWindows;

// Set always on top toggle
AlwaysOnTopToggleSwitch.IsOn = UserSettingsClass.AlwaysOnTop;

// Update hotkey button text
UpdateHotkeyButtonText();

// Update cache information
UpdateCacheInfo();

// Create event handlers
FullscreenToggleSwitch.Toggled += FullscreenToggleSwitch_Toggled;
GridAlignComboBox.SelectionChanged += GridAlignComboBox_SelectionChanged;

// Make sure to unsubscribe
FullscreenToggleSwitch.Unloaded += (s, e) => FullscreenToggleSwitch.Toggled -= FullscreenToggleSwitch_Toggled;
GridAlignComboBox.Unloaded += (s, e) => GridAlignComboBox.SelectionChanged -= GridAlignComboBox_SelectionChanged;
```

**���� ����:**
- ? `ChangeHeaderTextBox` �ʱ�ȭ ����
- ? `ChangeHeaderTextBox_TextChanged` �̺�Ʈ �ڵ鷯 ����
- ? `ChangeHeaderTextBox_TextChanged` �޼��� ����
- ? Unsubscribe �ڵ� ����

---

### **3. MainWindow.xaml**

**Before (���� ��):**

```xaml
<TextBlock
    x:Name="HeaderTextBlock"
    Grid.Column="0"
    HorizontalAlignment="Stretch"
    VerticalAlignment="Center"
    FontSize="20"
    FontWeight="Bold"
    Text="My files, folders, and websites"
    TextTrimming="CharacterEllipsis"
    TextWrapping="NoWrap" />
```

**After (���� ��):**

```xaml
<TextBlock
    x:Name="HeaderTextBlock"
    Grid.Column="0"
    HorizontalAlignment="Stretch"
    VerticalAlignment="Center"
    FontSize="20"
    FontWeight="Bold"
  Text="My apps and shortcuts"
    TextTrimming="CharacterEllipsis"
    TextWrapping="NoWrap" />
```

**���� ����:**
- ? �ؽ�Ʈ�� ���� ������ ����
- ? "My files, folders, and websites" �� "My apps and shortcuts"
- ? �� �����ϰ� ��Ȯ�� ǥ��

---

### **4. MainWindow.xaml.cs**

**Before (���� ��):**

```csharp
private void UpdateUIFromSettings()
{
    // Set header text (Update from HeaderText)
    HeaderTextBlock.Text = UserSettingsClass.HeaderText;

    // Adjust the size of items in ItemsGridView (Update from GridScale)
    foreach (var gridViewItem in ItemsGridView.Items)
    {
        // ...
    }
    
    // ...
}
```

**After (���� ��):**

```csharp
private void UpdateUIFromSettings()
{
    // Adjust the size of items in ItemsGridView (Update from GridScale)
    foreach (var gridViewItem in ItemsGridView.Items)
    {
        if (gridViewItem is GridViewTile)
        {
      ((GridViewTile)gridViewItem).Size = UserSettingsClass.GridScale;
        }
 else if (gridViewItem is GridViewTileGroup)
      {
     // ...
}
    }
    
    // ...
}
```

**���� ����:**
- ? `HeaderTextBlock.Text` ������Ʈ �ڵ� ����
- ? �ּ� ����
- ? `UpdateUIFromSettings()`�� �� ��������

---

## ?? ���� ���

### **���ŵ� �ڵ�:**

```
1. SettingsWindow.xaml
   - SettingsCard (Change header text) - 10��

2. SettingsWindow.xaml.cs
   - ChangeHeaderTextBox.Text �ʱ�ȭ - 1��
   - TextChanged �̺�Ʈ �ڵ鷯 ��� - 1��
   - Unloaded �̺�Ʈ �ڵ鷯 - 1��
   - ChangeHeaderTextBox_TextChanged �޼��� - 5��

3. MainWindow.xaml
   - Text �Ӽ� �� ���� (���� �� ����)

4. MainWindow.xaml.cs
   - HeaderTextBlock.Text ������Ʈ - 1��
   - �ּ� - 1��

�� ����: ~20��
```

---

## ?? ��� �ؽ�Ʈ ����

### **���ο� ���� �ؽ�Ʈ:**

```
"My apps and shortcuts"
```

**����:**
- ? �� ������
- ? ���� ������ ��Ȯ�� ǥ��
- ? "files, folders, and websites" �� "apps and shortcuts"
- ? �Ϲ����� ��� ���

**�ٸ� �ɼǵ�:**
```
? "My files, folders, and websites" - �ʹ� ��� ��ü��
? "My apps and shortcuts" - �����ϰ� ������
? "Quick Launch" - ��� �߽�
? "Favorites" - ���ã�� ����
? "My Stuff" - �ʹ� ĳ�־�
```

---

## ?? UserSettingsClass.HeaderText ó��

### **�ɼ� 1: ������ ����**

```csharp
// UserSettingsClass.cs
public static string HeaderText { get; set; } = "My apps and shortcuts";  // �� ����
```

**����:**
- �ڵ� ������ ����
- ���� ���Ͽ����� ����

**����:**
- ���� ������� ���� ���Ͽ� ��������
- ���̱׷��̼� �ʿ�

---

### **�ɼ� 2: �����ϵ� ��� �� �� (����)**

```csharp
// UserSettingsClass.cs
public static string HeaderText { get; set; } = "My apps and shortcuts";  // �� ����
```

**����:**
- ���� ����� ȣȯ��
- ���� ���� ���� ����
- ���߿� ��Ȱ��ȭ ����

**����:**
- ������� �ʴ� �ڵ� ����

**���� ����: �ɼ� 2** ?

---

## ?? Settings â ����

### **Before (���� ��):**

```
����������������������������������������������������������������������������������������������
�� Settings         ��
����������������������������������������������������������������������������������������������
�� �������������������������������������������������������������������������������������� ��
�� �� Change header text          �� ��
�� �� Modify the text shown below...          �� ��
�� �� [_____________________]       �� ��
�� �������������������������������������������������������������������������������������� ��
��           ��
�� �������������������������������������������������������������������������������������� ��
�� �� Enable fullscreen       �� ��
�� �� Use SLauncher in fullscreen mode.      �� ��
�� ��          [Toggle]     �� ��
�� �������������������������������������������������������������������������������������� ��
��      ��
�� ...       ��
����������������������������������������������������������������������������������������������
```

---

### **After (���� ��):**

```
����������������������������������������������������������������������������������������������
�� Settings         ��
����������������������������������������������������������������������������������������������
�� �������������������������������������������������������������������������������������� ��
�� �� Enable fullscreen    �� ��
�� �� Use SLauncher in fullscreen mode.      �� ��
�� ��     [Toggle]     �� ��
�� �������������������������������������������������������������������������������������� ��
��       ��
�� �������������������������������������������������������������������������������������� ��
�� �� Grid alignment   �� ��
�� �� Choose how to align the grid...    �� ��
�� ��             [Left ��]          �� ��
�� �������������������������������������������������������������������������������������� ��
��   ��
�� ...     ��
����������������������������������������������������������������������������������������������
```

**���� ����:**
- ? �� ����� ���̾ƿ�
- ? ���ʿ��� �ɼ� ����
- ? �ٽ� ��ɿ� ����

---

## ?? �׽�Ʈ �ó�����

### **Test 1: Settings â ����**

```
1. Settings ��ư Ŭ��
2. Settings â ����
3. "Change header text" ���� ?
4. "Enable fullscreen"�� ù ��° ���� ?
```

---

### **Test 2: ��� �ؽ�Ʈ Ȯ��**

```
1. ���� â Ȯ��
2. ��� �ؽ�Ʈ: "My apps and shortcuts" ?
3. ������ �ؽ�Ʈ ?
4. ���� �Ұ� ?
```

---

### **Test 3: ���� ���� ����**

```
1. ���� settings.json ���� ����
2. "HeaderText": "My custom text" ����
3. SLauncher ����
4. ��� �ؽ�Ʈ: "My apps and shortcuts" ?
5. ���� ���� ���õ� ?
6. ���� ���� ?
```

---

### **Test 4: �� ��ġ**

```
1. ù ����
2. settings.json ����
3. ��� �ؽ�Ʈ: "My apps and shortcuts" ?
4. ���� �Ұ� ?
```

---

## ?? ���� Settings �׸�

### **���� Settings:**

```
Settings:
1. ? Enable fullscreen
2. ? Grid alignment
3. ? Start with Windows
4. ? Always on top
5. ? Global Hotkey

Cache Management:
6. ? Favicon Cache
7. ? Cache Location

About:
8. ? About SLauncher
```

**�� 8�� ���� (����� ������)** ?

---

## ?? ���� �߰� ������ ����

### **�׸� ����:**

```
? Change header text (���ŵ�)
? Dark/Light/System theme (�߰� ����)
? Accent color (�߰� ����)
? Background transparency (�߰� ����)
```

### **���� ����:**

```
? Minimize to tray (�̹� ������)
? Close to tray (���� ����)
? Start minimized (���� ����)
```

### **�˻� ����:**

```
? Search placeholder text (���� ����)
? Show recent searches (���� ����)
? Search hotkey (�̹� ������)
```

**����� �ٽ� ��ɿ� ����!** ?

---

## ?? �ڵ� ����

### **���ŵ� UserSettingsClass ���:**

**Before:**
```csharp
// Settings���� ����
UserSettingsClass.HeaderText = ChangeHeaderTextBox.Text;
UserSettingsClass.WriteSettingsFile();

// MainWindow���� ���
HeaderTextBlock.Text = UserSettingsClass.HeaderText;
```

**After:**
```csharp
// MainWindow.xaml���� ���� ��
Text="My apps and shortcuts"

// �ڵ忡�� �ǵ帮�� ����
// UserSettingsClass.HeaderText�� ���� (ȣȯ��)
```

---

## ? �Ϸ�!

### **����� ����:**
- ? `SettingsWindow.xaml`
  - SettingsCard (Change header text) ����
  - Fullscreen Margin ����

- ? `SettingsWindow.xaml.cs`
  - ChangeHeaderTextBox �ʱ�ȭ ����
  - TextChanged �̺�Ʈ �ڵ鷯 ����
  - ChangeHeaderTextBox_TextChanged �޼��� ����

- ? `MainWindow.xaml`
  - HeaderTextBlock Text ���� ������ ����
  - "My files, folders, and websites" �� "My apps and shortcuts"

- ? `MainWindow.xaml.cs`
  - HeaderTextBlock.Text ������Ʈ �ڵ� ����

---

### **���:**
- ? "Change header text" ��� ������ ����
- ? ��� �ؽ�Ʈ ����: "My apps and shortcuts"
- ? Settings â �� �������
- ? ���ʿ��� Ŀ���͸���¡ ����
- ? �ڵ� ����ȭ
- ? ���� ����

---

## ?? �׽�Ʈ

```
1. SLauncher ����
2. ��� �ؽ�Ʈ Ȯ��: "My apps and shortcuts" ?
3. Settings ��ư Ŭ��
4. "Change header text" ���� ?
5. �ٸ� ������ ���� �۵� ?
```

---

## ?? �Ϸ�!

**"Change header text" ����� ���������� ���ŵǾ����ϴ�!**

**���� SLauncher�� �� �����ϰ� ����� ���� ȭ���� �����մϴ�!** ?

**�ٽ� ��ɿ� �����Ͽ� ����� ������ ���Ǿ����ϴ�!** ??
