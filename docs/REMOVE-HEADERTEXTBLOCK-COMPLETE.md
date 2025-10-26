# ??? HeaderTextBlock ���� ���� - �Ϸ�!

## ? ���ŵ� ���

**HeaderTextBlock ���� ����:**
```xaml
<!-- ���ŵ� -->
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

**����:**
- ���ʿ��� �ؽ�Ʈ ���
- ���� ����
- ��ư������ ����� UI
- �� �̴ϸ��� ������

---

## ?? ������ ����

### **MainWindow.xaml**

**Before (���� ��):**

```xaml
<!--  Header Text + Buttons  -->
<Grid
    Height="32"
    Margin="0,62,0,0"
    HorizontalAlignment="Stretch"
  VerticalAlignment="Top">
    <Grid.ColumnDefinitions>
   <ColumnDefinition Width="*" />
        <ColumnDefinition Width="365" />
    </Grid.ColumnDefinitions>

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

    <StackPanel
      Grid.Column="1"
        HorizontalAlignment="Right"
        VerticalAlignment="Stretch"
  Orientation="Horizontal">
        <Button
            x:Name="AddFileBtn"
        Width="105"
            Height="32"
            Click="AddFileBtn_Click"
     Content="Add a file"
 Style="{ThemeResource AccentButtonStyle}" />
        <Button
       x:Name="AddFolderBtn"
            Width="110"
         Height="32"
   Margin="10,0,0,0"
          Click="AddFolderBtn_Click"
            Content="Add a folder" />
    <Button
      x:Name="AddWebsiteBtn"
      Width="120"
     Height="32"
            Margin="10,0,0,0"
         Click="AddWebsiteBtn_Click"
        Content="Add a website" />
    </StackPanel>
</Grid>
```

---

**After (���� ��):**

```xaml
<!--  Header Text + Buttons  -->
<Grid
    Height="32"
    Margin="0,62,0,0"
    HorizontalAlignment="Stretch"
    VerticalAlignment="Top">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*" />
        <ColumnDefinition Width="auto" />
    </Grid.ColumnDefinitions>

    <StackPanel
   Grid.Column="1"
    HorizontalAlignment="Right"
        VerticalAlignment="Stretch"
 Orientation="Horizontal">
  <Button
        x:Name="AddFileBtn"
            Width="105"
   Height="32"
   Click="AddFileBtn_Click"
       Content="Add a file"
            Style="{ThemeResource AccentButtonStyle}" />
        <Button
        x:Name="AddFolderBtn"
            Width="110"
            Height="32"
 Margin="10,0,0,0"
            Click="AddFolderBtn_Click"
          Content="Add a folder" />
<Button
     x:Name="AddWebsiteBtn"
    Width="120"
     Height="32"
   Margin="10,0,0,0"
     Click="AddWebsiteBtn_Click"
            Content="Add a website" />
    </StackPanel>
</Grid>
```

**���� ����:**
- ? HeaderTextBlock ���� ����
- ? Grid.ColumnDefinitions ����
  - Column 0: `Width="*"` (�� ����)
  - Column 1: `Width="365"` �� `Width="auto"` (��ư ũ�⿡ ����)
- ? StackPanel�� `Grid.Column="1"`�� �̵�
- ? �ּ��� ���� (�ڵ� ������)

---

## ?? ���̾ƿ� ����

### **Before (���� ��):**

```
������������������������������������������������������������������������������������������������������������������������������
�� [Search Box]     [??] [��]         ��
������������������������������������������������������������������������������������������������������������������������������
��    ��
�� My apps and shortcuts     [Add file] [Add folder] [Add web]  ��
��     ��
������������������������������������������������������������������������������������������������������������������������������
��         ��
��      [�����۵�]           ��
��       ��
������������������������������������������������������������������������������������������������������������������������������
```

**������:**
- ? "My apps and shortcuts" �ؽ�Ʈ ���� ����
- ? ��ư�� �ؽ�Ʈ�� �и��Ǿ� ����
- ? ���ʿ��� ����

---

### **After (���� ��):**

```
������������������������������������������������������������������������������������������������������������������������������
�� [Search Box]      [??] [��]              ��
������������������������������������������������������������������������������������������������������������������������������
����
��    [Add file] [Add folder] [Add web]  ��
��         ��
������������������������������������������������������������������������������������������������������������������������������
��                ��
�� [�����۵�]        ��
��             ��
������������������������������������������������������������������������������������������������������������������������������
```

**���� ����:**
- ? �� ����� ���̾ƿ�
- ? ��ư�� �����ʿ� ����
- ? ���ʿ��� �ؽ�Ʈ ����
- ? ���� Ȱ�� ����
- ? �̴ϸ��� ������

---

## ?? ���� ���

### **���ŵ� �ڵ�:**

```xaml
1. TextBlock (HeaderTextBlock) - 10��
2. Grid.ColumnDefinitions ����
   - Column 1: Width="365" �� Width="auto"
```

**�� ����: ~10��**

---

### **���� �ڵ�:**

```xaml
1. Grid (��� ����) ?
2. StackPanel (��ư��) ?
3. 3�� ��ư ?
   - Add a file
   - Add a folder
   - Add a website
```

---

## ?? Grid ���̾ƿ� ����

### **���ο� Grid ����:**

```xaml
<Grid.ColumnDefinitions>
    <ColumnDefinition Width="*" />      <!-- �� ���� (����) -->
    <ColumnDefinition Width="auto" />   <!-- ��ư�� (������) -->
</Grid.ColumnDefinitions>
```

**����:**
- Column 0: `Width="*"` - ���� ���� ��� ����
- Column 1: `Width="auto"` - ��ư���� ũ�⸸ŭ�� ����

**���:**
- ��ư���� �����ʿ� ���ĵ� ?
- ������ ������� (�����) ?
- ������ ���̾ƿ� ���� ?

---

## ?? ������ ö��

### **Before:**

```
�ؽ�Ʈ + ��ư = ������
"My apps and shortcuts" - ���ʿ��� ����
����ڰ� �̹� �˰� �ִ� ����
```

### **After:**

```
��ư�� = ������
"Add a file", "Add a folder", "Add a website" - ��Ȯ�� �׼�
����ڰ� ���ϴ� ��: ���� ����
```

**���:**
- ? �̴ϸ� ������
- ? ��� �߽�
- ? ���� ���ټ�

---

## ?? �ٸ� ��ó�� ��

### **Alfred (macOS):**
```
��������������������������������������������������������
�� [Search]   ��
��������������������������������������������������������
```
**Ư¡:** �˻� �߽�, �ּ����� UI

### **Wox (Windows):**
```
��������������������������������������������������������
�� [Search]  ��
�� [�����]                 ��
��������������������������������������������������������
```
**Ư¡:** �˻� �߽�, ������

### **PowerToys Run:**
```
��������������������������������������������������������
�� [Search]         ��
�� [�����]��
��������������������������������������������������������
```
**Ư¡:** �˻� �߽�, �̴ϸ�

### **SLauncher (����):**
```
����������������������������������������������������������������������������������������
�� [Search]      [??]         ��
����������������������������������������������������������������������������������������
��  [Add file] [Add folder] ... ��
����������������������������������������������������������������������������������������
�� [�����۵�]   ��
����������������������������������������������������������������������������������������
```

**Ư¡:**
- ? �˻� + �ð��� �׸���
- ? ��ư���� ���� �߰�
- ? ����� ���̾ƿ�
- ? ��ɰ� �̴ϸ��� ����

---

## ?? �׽�Ʈ �ó�����

### **Test 1: ���̾ƿ� Ȯ��**

```
1. SLauncher ����
2. ��� ���� Ȯ��
3. "My apps and shortcuts" �ؽ�Ʈ ���� ?
4. ��ư 3���� ������ ���� ?
5. ����� ���̾ƿ� ?
```

---

### **Test 2: ��ư ����**

```
1. "Add a file" Ŭ��
2. ���� ���� ���̾�α� ���� ?
3. "Add a folder" Ŭ��
4. ���� ���� ���̾�α� ���� ?
5. "Add a website" Ŭ��
6. ������Ʈ �Է� ���̾�α� ���� ?
```

---

### **Test 3: ������ ���̾ƿ�**

```
1. â ũ�� ���� (�۰�)
2. ��ư�� ������ ���� ?
3. â ũ�� ���� (ũ��)
4. ��ư�� ������ ���� ?
5. �׻� �ϰ��� ��ġ ?
```

---

### **Test 4: ��ü ȭ�� ���**

```
1. Settings �� Enable fullscreen
2. ��ü ȭ�� ����
3. ��ư�� ������ ���� ���� ?
4. ��� �ؽ�Ʈ ���� ?
5. ����� ���̾ƿ� ?
```

---

## ?? ���� Ȱ��

### **Before (��� �ؽ�Ʈ ���� ��):**

```
Grid Layout:
��������������������������������������������������������������������������������������
�� My apps and shortcuts ��   [Buttons]     ��
�� (300px �̻� ����)      ��   (365px)      ��
��������������������������������������������������������������������������������������

�� �ʺ�: 665px �̻�
```

---

### **After (��� �ؽ�Ʈ ���� ��):**

```
Grid Layout:
������������������������������������������������������������������������������������
��        (�� ����)     ��[Buttons] ��
��  (*)           ��  (auto)  ��
������������������������������������������������������������������������������������

�� �ʺ�: ��ư ũ�⸸ŭ�� (345px)
```

**����:**
- ? ���� ���� ����
- ? ��ư�� �׻� ������
- ? ������ ���̾ƿ�
- ? ����� ����

---

## ?? ���� ���� ���� (���û���)

### **1. ��ư ������ �߰�**

```xaml
<Button Content="Add a file">
    <Button.Content>
        <StackPanel Orientation="Horizontal">
       <FontIcon Glyph="&#xE8E5;" FontSize="14" Margin="0,0,5,0"/>
  <TextBlock Text="Add a file"/>
   </StackPanel>
    </Button.Content>
</Button>
```

**����:**
- �ð������� �� ��Ȯ
- �� �������� ����

---

### **2. ��ư ũ�� ���̱�**

```xaml
<Button Width="90" Height="28" ... />  <!-- �� �۰� -->
```

**����:**
- �� ���� ���� Ȯ��
- �� ����Ʈ�� UI

---

### **3. ��Ӵٿ� ��ư���� ����**

```xaml
<DropDownButton Content="Add Item">
    <DropDownButton.Flyout>
        <MenuFlyout>
            <MenuFlyoutItem Text="Add a file" />
      <MenuFlyoutItem Text="Add a folder" />
     <MenuFlyoutItem Text="Add a website" />
      </MenuFlyout>
  </DropDownButton.Flyout>
</DropDownButton>
```

**����:**
- ���� ���� ����
- �� ����� UI
- Ȯ�� ���ɼ�

---

### **4. Tooltip �߰�**

```xaml
<Button 
    Content="Add a file"
    ToolTipService.ToolTip="Add a new file to SLauncher (Ctrl+F)" />
```

**����:**
- ����Ű �ȳ�
- �� ���� UX

---

## ?? ���� ����

### **���̾ƿ�:**

```
������������������������������������������������������������������������������������������������������������������������������
�� [Search Box]       [??] [��]          ��
������������������������������������������������������������������������������������������������������������������������������
��         ��
��[Add file] [Add folder] [Add web]  ��
��            ��
������������������������������������������������������������������������������������������������������������������������������
��   ��
��  �������������� �������������� �������������� ��
��  ��App 1�� ��App 2�� ��App 3��          ��
��  �������������� �������������� ��������������         ��
��                   ��
��  �������������� �������������� ��������������   ��
��  ��App 4�� ��App 5�� ��App 6��          ��
��  �������������� �������������� ��������������   ��
��   ��
��           [?? Zoom Slider]      ��
������������������������������������������������������������������������������������������������������������������������������
```

**Ư¡:**
- ? �˻� â (���)
- ? �߰� ��ư�� (������ ����)
- ? ������ �׸��� (�߾�)
- ? �� �����̴� (���ϴ�)
- ? ����ϰ� ������ UI

---

## ? �Ϸ�!

### **����� ����:**
- ? `MainWindow.xaml`
  - HeaderTextBlock ���� ����
  - Grid.ColumnDefinitions ���� (Width="365" �� Width="auto")
  - StackPanel Grid.Column ����

---

### **���:**
- ? HeaderTextBlock ���� ���ŵ�
- ? ��ư�鸸 ������ ����
- ? �� ����� ���̾ƿ�
- ? ���� Ȱ�� ����
- ? �̴ϸ� ������
- ? ���� ����

---

## ?? �׽�Ʈ

```
1. SLauncher ����
2. ��� ���� Ȯ��
3. "My apps and shortcuts" �ؽ�Ʈ ���� ?
4. ��ư 3�� ������ ���� ?
5. ����� UI ?
6. ��� ��� ���� �۵� ?
```

---

## ?? �߰� ���� ����

### **MainWindow.xaml.cs������ ���� ����:**

���� �ڵ忡�� HeaderTextBlock ������ �����Ƿ� �߰� ���� ���ʿ� ?

**�̹� �Ϸ�� ����:**
- ? `UpdateUIFromSettings()`���� HeaderTextBlock.Text ���ŵ�
- ? HeaderTextBlock ���� �ڵ� ����
- ? ������ ���ŵ�

---

## ?? �Ϸ�!

**HeaderTextBlock�� ������ ���ŵǾ����ϴ�!**

**���� SLauncher�� ���� ����ϰ� �̴ϸ��� �������� ������ �Ǿ����ϴ�!** ?

**����� �״�� �����ϸ鼭 UI�� �� �����������ϴ�!** ??

**�׽�Ʈ�غ�����!** ??
