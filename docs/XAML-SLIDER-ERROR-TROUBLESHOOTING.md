# ?? XAML Slider Error - Troubleshooting Guide

## ? ����

```
Failed to assign to property 'Microsoft.UI.Xaml.Controls.Primitives.RangeBase.Minimum'
```

�� ������ **Slider �Ӽ� �ʱ�ȭ ����** �����Դϴ�.

---

## ? �ذ� �õ� (������� ����)

### 1?? **Visual Studio ���� �����**
```
1. Visual Studio ����
2. Task Manager���� MSBuild.exe, VBCSCompiler.exe ����
3. Visual Studio �ٽ� ����
```

### 2?? **Clean & Rebuild**
```
Visual Studio����:
1. Build �� Clean Solution
2. Build �� Rebuild Solution
3. F5 (����� ����)
```

### 3?? **��� ĳ�� ����**
```powershell
cd "D:\Works\Playground\C#\SLauncher"

# obj ���� ����
Remove-Item -Path "SLauncher\obj" -Recurse -Force
Remove-Item -Path "WinFormsClassLibrary\obj" -Recurse -Force

# bin ���� ����
Remove-Item -Path "SLauncher\bin" -Recurse -Force
Remove-Item -Path "WinFormsClassLibrary\bin" -Recurse -Force

# Visual Studio ĳ�� ����
Remove-Item -Path ".vs" -Recurse -Force
```

### 4?? **NuGet ĳ�� Ŭ����**
```powershell
dotnet nuget locals all --clear
dotnet restore
```

### 5?? **�ӽ� �ذ�: Slider ����**

MainWindow.xaml���� IconScaleSlider�� �ӽ÷� �ּ� ó��:

```xml
<!-- Icon Scale Slider (temporarily disabled for troubleshooting) -->
<!--
<StackPanel ...>
 <FontIcon ... />
    <Slider x:Name="IconScaleSlider" ... />
    <TextBlock x:Name="ScaleValueText" ... />
</StackPanel>
-->
```

MainWindow.xaml.cs���� ���� �ڵ� �ּ� ó��:

```csharp
// Initialize icon scale slider
// IconScaleSlider.Value = UserSettingsClass.GridScale;
// ScaleValueText.Text = $"{UserSettingsClass.GridScale:F2}x";
```

```csharp
// Icon Scale Slider event handler (disabled)
/*
private void IconScaleSlider_ValueChanged(...)
{
    // ...
}
*/
```

---

## ?? ���� �м�

### XAML ���� ���� Ȯ��
```
SLauncher\obj\x64\Debug\net8.0-windows10.0.22621.0\win-x64\MainWindow.g.i.cs
```

�� ������ Slider�� �ʱ�ȭ�ϴ� �ڵ带 Ȯ���ؾ� �մϴ�.

### �������� �ʱ�ȭ ����:
```csharp
// Generated code should look like:
slider.Minimum = 0.25;
slider.Maximum = 6.00;
slider.Value = 1.0;
```

### ������ �Ǵ� ����:
```csharp
// BAD - causes error:
slider.Value = 1.0;       // �� Error if Minimum hasn't been set!
slider.Minimum = 0.25;
slider.Maximum = 6.00;
```

---

## ?? üũ����Ʈ

### XAML ���� ����:
```xml
<Slider
    x:Name="IconScaleSlider"
    Width="150"
    VerticalAlignment="Center"
    Minimum="0.25"    �� 1����
  Maximum="6.00"          �� 2����
    Value="1.0"   �� 3����
    ... />
```

**�߿�:** `Minimum`, `Maximum`, `Value` ���� **�ʼ�**!

### �ٸ� Slider Ȯ��:
```powershell
# ������Ʈ ��ü���� Slider �˻�
Get-ChildItem -Path "SLauncher" -Filter "*.xaml" -Recurse | 
    Select-String -Pattern "<Slider"
```

### WinUI 3 ���� Ȯ��:
```xml
<PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
```

�ֽ� �������� Ȯ��!

---

## ?? ��� �ذ� ���

### �ɼ� A: Slider�� SettingsWindow�� ����

MainWindow���� ������ ��ӵǸ�, ������� SettingsWindow�� �δ� ���� �����մϴ�.

### �ɼ� B: �ڵ忡�� ���� ����

XAML ��� �ڵ忡�� Slider ����:

```csharp
// MainWindow.xaml.cs - Container_Loaded
private void Container_Loaded(object sender, RoutedEventArgs e)
{
  // ...existing code...
    
 // Create slider programmatically
    CreateIconScaleSlider();
}

private void CreateIconScaleSlider()
{
    var stackPanel = new StackPanel
    {
        Orientation = Orientation.Horizontal,
        Spacing = 10,
        HorizontalAlignment = HorizontalAlignment.Right,
        VerticalAlignment = VerticalAlignment.Bottom,
 Margin = new Thickness(0, 0, 20, 20)
    };

    var icon = new FontIcon
    {
   Glyph = "\xE71E",
        FontSize = 16,
    VerticalAlignment = VerticalAlignment.Center,
 Opacity = 0.7
    };

    var slider = new Slider
    {
     Name = "IconScaleSlider",
        Width = 150,
      VerticalAlignment = VerticalAlignment.Center,
      Minimum = 0.25,
 Maximum = 6.00,
        Value = UserSettingsClass.GridScale,
        SmallChange = 0.05,
        StepFrequency = 0.05
    };
    slider.ValueChanged += IconScaleSlider_ValueChanged;

    var textBlock = new TextBlock
    {
    Name = "ScaleValueText",
      Text = $"{UserSettingsClass.GridScale:F2}x",
        VerticalAlignment = VerticalAlignment.Center,
      FontSize = 12,
  Opacity = 0.7,
        MinWidth = 40
    };

    stackPanel.Children.Add(icon);
    stackPanel.Children.Add(slider);
    stackPanel.Children.Add(textBlock);

    Grid.SetRow(stackPanel, 0);
    GridViewBackground.Children.Add(stackPanel);
}
```

---

## ?? ��õ ����

1. Visual Studio ����� + Clean & Rebuild
2. ������ ����? �� ��� ĳ�� ����
3. ������ ����? �� Slider �ӽ� ����
4. ������ ����? �� �ڵ忡�� ���� ����

---

## ?? �߰� �����

���� ���� ��� ����� �����ϸ�:

1. **������ ���� Ȯ��:**
```
SLauncher\obj\x64\Debug\net8.0-windows10.0.22621.0\win-x64\MainWindow.g.i.cs
```

2. **�ش� ���Ͽ��� Slider �ʱ�ȭ �ڵ� �˻�**

3. **�ʱ�ȭ ������ �߸��Ǿ� �ִٸ� XAML �����Ϸ� ����**

---

**�� ������� �õ��غ��ð� ����� �˷��ּ���!** ??
