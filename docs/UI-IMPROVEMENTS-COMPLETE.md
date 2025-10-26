# ? UI Improvements - Always On Top & Icon Scale

## ?? ���� �Ϸ�

�� ���� �ֿ� UI ���������� ���������� �����Ǿ����ϴ�!

---

## ?? ������ ���

### ? **1. Always On Top (�׻� ���� ǥ��)**
- Settings���� ��۷� On/Off ����
- �ٸ� �ۺ��� �׻� ������ ǥ��
- ���� ��� ����
- �⺻��: `false` (��Ȱ��ȭ)

### ? **2. Icon Scale (������ ũ�� ����)**
- Settings���� MainWindow ������ �Ʒ��� �̵�
- �ǽð����� ������ ũ�� ���� ����
- �Ŀ�����Ʈ ��Ÿ���� �������̽�
- ���� ���� ǥ�� (��: "1.20x")

---

## ??? ���� ����

```
SLauncher/
������ Classes/
��   ������ UserSettingsClass.cs        �� alwaysOnTop ���� �߰�
��
������ MainWindow.xaml     �� Icon Scale Slider �߰�
������ MainWindow.xaml.cs          �� �ǽð� ũ�� ����
��
������ SettingsWindow.xaml             �� Always On Top �ɼ� �߰�
������ SettingsWindow.xaml.cs       �� �̺�Ʈ �ڵ鷯 �߰�
```

---

## ?? �ڵ� ��

### 1?? **Always On Top ���**

#### UserSettingsClass.cs
```csharp
public class UserSettingsJson
{
    // ...���� ������...
    
    // Window settings
  public bool alwaysOnTop { get; set; } = false;
}

public static class UserSettingsClass
{
    /// <summary>
    /// Variable which stores whether window should always be on top
    /// </summary>
    public static bool AlwaysOnTop = false;
    
    public static void WriteSettingsFile()
    {
      var userSettingsJson = new UserSettingsJson
   {
            // ...
      alwaysOnTop = AlwaysOnTop
        };
        // ...
    }
    
    public static void TryReadSettingsFile()
    {
// ...
        AlwaysOnTop = userSettingsJson.alwaysOnTop;
    }
}
```

#### SettingsWindow.xaml
```xml
<!--  Always on top  -->
<wct:SettingsCard
    Margin="0,5,0,0"
    Description="Keep SLauncher window always on top of other windows."
    Header="Always on top">
    <wct:SettingsCard.HeaderIcon>
        <FontIcon Glyph="&#xE8A7;" />
    </wct:SettingsCard.HeaderIcon>
    <ToggleSwitch
        x:Name="AlwaysOnTopToggleSwitch"
        OffContent="No"
    OnContent="Yes"
        Toggled="AlwaysOnTopToggleSwitch_Toggled" />
</wct:SettingsCard>
```

#### SettingsWindow.xaml.cs
```csharp
private void Container_Loaded(object sender, RoutedEventArgs e)
{
    // ...
    AlwaysOnTopToggleSwitch.IsOn = UserSettingsClass.AlwaysOnTop;
}

private void AlwaysOnTopToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
    UserSettingsClass.AlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
    UserSettingsClass.WriteSettingsFile();
    
    // Update main window's always on top state
    App.MainWindow.IsAlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
}
```

#### MainWindow.xaml.cs
```csharp
private async void Container_Loaded(object sender, RoutedEventArgs e)
{
    // ...
    // Set always on top if enabled
    this.IsAlwaysOnTop = UserSettingsClass.AlwaysOnTop;
}
```

---

### 2?? **Icon Scale ���**

#### MainWindow.xaml
```xml
<!--  Icon Scale Slider (bottom right)  -->
<StackPanel
    Grid.Row="0"
    Margin="0,0,20,20"
    HorizontalAlignment="Right"
    VerticalAlignment="Bottom"
    Orientation="Horizontal"
Spacing="10">
  
    <!-- Icon -->
    <FontIcon 
        Glyph="&#xE71E;"
    FontSize="16"
        VerticalAlignment="Center"
        Opacity="0.7"
        ToolTipService.ToolTip="Icon Scale" />
    
    <!-- Slider -->
  <Slider
      x:Name="IconScaleSlider"
    Width="150"
        VerticalAlignment="Center"
        Maximum="6.00"
        Minimum="0.25"
        SmallChange="0.05"
        StepFrequency="0.05"
 Value="1.0"
        ValueChanged="IconScaleSlider_ValueChanged"
        ToolTipService.ToolTip="Adjust icon scale" />
    
    <!-- Scale Value Display -->
    <TextBlock
     x:Name="ScaleValueText"
        VerticalAlignment="Center"
        FontSize="12"
        Opacity="0.7"
        Text="1.00x"
        MinWidth="40"
        ToolTipService.ToolTip="Current scale" />
</StackPanel>
```

#### MainWindow.xaml.cs
```csharp
private async void Container_Loaded(object sender, RoutedEventArgs e)
{
    // ...
    // Initialize icon scale slider
    IconScaleSlider.Value = UserSettingsClass.GridScale;
    ScaleValueText.Text = $"{UserSettingsClass.GridScale:F2}x";
}

private void IconScaleSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
{
    // Update scale value text
    double scale = Math.Round(IconScaleSlider.Value, 2);
    ScaleValueText.Text = $"{scale:F2}x";
    
    // Update UserSettingsClass
    UserSettingsClass.GridScale = scale;
 UserSettingsClass.WriteSettingsFile();
    
    // Update all item sizes in real-time
    foreach (var gridViewItem in ItemsGridView.Items)
    {
        if (gridViewItem is GridViewTile)
        {
            ((GridViewTile)gridViewItem).Size = scale;
        }
        else if (gridViewItem is GridViewTileGroup)
        {
      GridViewTileGroup gridViewTileGroup = gridViewItem as GridViewTileGroup;
          gridViewTileGroup.Size = scale;
         
            foreach (GridViewTile gridViewTile in gridViewTileGroup.Items)
            {
          gridViewTile.Size = scale;
            }
        }
    }
    
    // Re-align the GridView
    if (UserSettingsClass.GridPosition == "Left")
    {
        AlignGridViewLeft();
    }
    else if (UserSettingsClass.GridPosition == "Center")
    {
        AlignGridViewCenter();
    }
}
```

---

## ?? UI ���̾ƿ�

### Settings Window
```
Settings ����������������������������������������������������
������ Change header text
������ Enable fullscreen
������ Grid alignment
������ Start with Windows
������ Always on top    �� �ű�

Cache Management ������������������������������������
������ Favicon Cache
������ Cache Location

About ��������������������������������������������������������
������ About SLauncher
```

### Main Window
```
��������������������������������������������������������������������������������������
��  [Search]        [Settings] [X]    ��
��           ��
��  My files, folders, and websites         ��
��  [Add file] [Add folder] [Add website]��
����������������������������������������������������������������������������������������
��    ��
��  [Icons...            ]        ��
��  [Icons...     ]          ��
��  [Icons...           ]          ��
��             ��
��                ��
��  [??����������?] 1.20x�� �� Icon Scale
����������������������������������������������������������������������������������������
```

---

## ?? ����� �ó�����

### Scenario 1: Always On Top Ȱ��ȭ
```
1. Settings ����
2. "Always on top" ��� On
3. Settings â �ݱ�
4. SLauncher�� �׻� �ٸ� â ���� ǥ�õ� ?
```

### Scenario 2: Icon Scale �ǽð� ����
```
1. ���� ȭ�� ������ �Ʒ� �����̴� Ȯ��
2. �����̴��� ���������� �巡��
3. �������� �ǽð����� Ŀ�� ?
4. "1.50x" ǥ�� Ȯ��
5. ������ �ڵ����� ����� ?
```

### Scenario 3: PowerPoint ��Ÿ�� ���
```
1. �������� ���鼭 �����̴� ����
2. ���ϴ� ũ�� ã��
3. �����̴����� �� ����
4. ���� �ڵ� ���� ?
```

---

## ?? ��� ��

### Before (Settings�� ���� ��)
```
? Icon Scale �����Ϸ��� Settings ����� ��
? ���� ���� �� Ȯ���Ϸ��� Settings �ݾƾ� ��
? �ǽð� �̸����� �Ұ�
```

### After (Main Window�� ���� ��)
```
? Icon Scale�� �׻� ����
? �ǽð����� ũ�� ���� Ȯ��
? PowerPoint ��Ÿ�� �������̽�
? �� �������� UX
```

---

## ?? ����� ���� ����

### 1. **Always On Top**
```
Before:
- SLauncher�� �ٸ� â�� ������
- ���� ã�ƾ� ��

After:
- �׻� �ֻ����� ǥ��
- ���� ���� ����
- ���꼺 ���
```

### 2. **Icon Scale**
```
Before (Settings):
1. Settings ����
2. Slider ����
3. Settings �ݱ�
4. ��� Ȯ��
5. ������ �� ��� �ٽ� Settings ����

After (Main Window):
1. Slider ����
2. �ǽð� Ȯ��
3. ��!
```

---

## ?? ���� ���� ����

### userSettings.json
```json
{
  "headerText": "My files, folders, and websites",
  "gridScale": 1.2,
  "useFullscreen": false,
  "gridPosition": "Center",
  "startWithWindows": true,
  "alwaysOnTop": false
}
```

---

## ?? UI/UX ��Ģ ����

### 1. **���� ���� (Direct Manipulation)**
```
? Icon Scale�� ���� ȭ�鿡 ����
? �����̴��� �巡���ϸ� ��� �ݿ�
? �ǵ���� �ﰢ��
```

### 2. **���ü� (Visibility)**
```
? Icon Scale ��Ʈ���� �׻� ����
? ���� ���� ǥ�� (1.20x)
? ���������� ��� ��Ȯ�� ǥ��
```

### 3. **�ϰ��� (Consistency)**
```
? PowerPoint/Office ��Ÿ�ϰ� ����
? ������ �Ʒ� = ���� ��Ʈ�� ��ġ
? ����ڿ��� �ͼ��� ����
```

### 4. **ȿ���� (Efficiency)**
```
? Settings �� �ʿ� ����
? Ŭ�� Ƚ�� ����
? �۾� �ð� ����
```

---

## ?? �׽�Ʈ �ó�����

### Test 1: Always On Top On
```
1. Settings ����
2. "Always on top" On
3. �ٸ� �� ���� (��: Chrome)
4. SLauncher�� Chrome ���� ǥ�õ� ?
```

### Test 2: Always On Top Off
```
1. Settings ����
2. "Always on top" Off
3. �ٸ� �� ����
4. SLauncher�� �Ϲ� âó�� ���� ?
```

### Test 3: Icon Scale �ǽð� ����
```
1. MainWindow���� ������ �Ʒ� �����̴� Ȯ��
2. �����̴��� 1.5�� �̵�
3. �������� ��� Ŀ�� ?
4. "1.50x" ǥ�� Ȯ�� ?
5. �� ����� �� ũ�� ���� ?
```

### Test 4: Icon Scale ���� ����
```
1. Slider�� 2.0���� �̵�
2. SLauncher ����
3. SLauncher �����
4. �������� 2.0 ������ ǥ�õ� ?
5. Slider�� 2.0 ��ġ�� ���� ?
```

### Test 5: Icon Scale �ּ�/�ִ�
```
1. Slider�� 0.25 (�ּ�)�� �̵�
2. �������� �ſ� �۾��� ?
3. Slider�� 6.00 (�ִ�)�� �̵�
4. �������� �ſ� Ŀ�� ?
```

---

## ?? �ð��� ������

### Icon Scale Slider
```
����������������������������������������������������������������������
��        ��
��          ��
��   ��
��        [??����������?] 1.20x��
����������������������������������������������������������������������
     ��������������������������  ��������������  ����������
    ������    �����̴�  ���� ǥ��
```

**���� ���:**
- ?? ������ - ��� �ĺ�
- Slider - 0.25 ~ 6.00 ����
- "1.20x" - ���� ���� ǥ��

**��ġ:**
- ������ �ϴ�
- GridView ���� ��ġ
- �׻� ����
- �ٸ� ��� ���� �� ��

---

## ? ���� ����

### ���� ����
```
? ���� ����
? ��� 0��
? ���� 0��
```

### ��� Ȯ��
```
? Always On Top ��� �۵�
? Icon Scale Slider ǥ��
? �ǽð� ũ�� ���� �۵�
? ���� ǥ�� ������Ʈ
? ���� ����/�ε� �۵�
? �� ����� �� ���� ����
```

### UI/UX Ȯ��
```
? �������� ��ġ (������ �Ʒ�)
? �ǽð� �ǵ��
? PowerPoint ��Ÿ��
? ����� ģȭ��
```

---

## ?? ���� ���� ���ɻ���

### 1. **Keyboard Shortcuts**
```
Ctrl + Plus  : Icon ũ��
Ctrl + Minus : Icon �۰�
Ctrl + 0     : �⺻ ũ��� ����
```

### 2. **Preset Buttons**
```
[ 0.5x ] [ 1.0x ] [ 1.5x ] [ 2.0x ]
  �� Ŭ���ϸ� ��� ����
```

### 3. **Zoom Percentage Input**
```
[??����������?] [120%] �� ���� �Է� ����
```

### 4. **Animation**
```
ũ�� ���� �� �ε巯�� �ִϸ��̼� ȿ��
```

---

## ?? �Ϸ�!

**UI ���� ������ ���������� �����Ǿ����ϴ�!**

### �ֿ� ����:
- ? Always On Top ��� �߰�
- ? Icon Scale�� MainWindow�� �̵�
- ? �ǽð� ũ�� ���� ����
- ? PowerPoint ��Ÿ�� �������̽�
- ? ����� ���� ���� ���

### ����� ����:
- ? �� ���� ������ ũ�� ����
- ? �ǽð� �̸�����
- ? Settings �� �ʿ� ����
- ? ���꼺 ���

**SLauncher v2.1.2 with Enhanced UI! ??**
