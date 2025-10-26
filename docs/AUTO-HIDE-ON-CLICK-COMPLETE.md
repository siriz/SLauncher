# ?? Auto Hide to Tray on Item Click - ���� �Ϸ�!

## ? ������ ���

**����/����/��ũ Ŭ�� �� �ڵ����� Ʈ���̷� �̵�:**

```
1. ������ Ŭ�� (�Ǵ� ��Ŭ�� �޴����� Open)
2. ����/����/��ũ ����
3. �ڵ����� Ʈ���̷� ���� ?
```

**Ư¡:**
- ? ����Ŭ������ ���� �� �ڵ� ����
- ? ��Ŭ�� �� Open �� �ڵ� ����
- ? ��Ŭ�� �� Run as Administrator �� �ڵ� ����
- ? MinimizeToTray ������ ���� ����
- ? �׷� �� �����۵� �����ϰ� �۵�

---

## ?? ������ ����

### **GridViewTile.xaml.cs**

#### 1. GridViewTileControl_Tapped (����Ŭ��):

**Before:**
```csharp
private async void GridViewTileControl_Tapped(object sender, TappedRoutedEventArgs e)
{
    await StartAssociatedProcess();
}
```

**After:**
```csharp
private async void GridViewTileControl_Tapped(object sender, TappedRoutedEventArgs e)
{
    await StartAssociatedProcess();
    
    // ? Hide main window to tray if MinimizeToTray is enabled
    if (UserSettingsClass.MinimizeToTray)
    {
      App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
      App.MainWindow.AppWindow.Hide();
    });
    }
}
```

---

#### 2. MenuOpenOption_Click (��Ŭ�� �� Open):

**Before:**
```csharp
private async void MenuOpenOption_Click(object sender, RoutedEventArgs e)
{
    // Start the process
    await StartAssociatedProcess();
}
```

**After:**
```csharp
private async void MenuOpenOption_Click(object sender, RoutedEventArgs e)
{
    // Start the process
await StartAssociatedProcess();
    
    // ? Hide main window to tray if MinimizeToTray is enabled
    if (UserSettingsClass.MinimizeToTray)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            App.MainWindow.AppWindow.Hide();
        });
    }
}
```

---

#### 3. MenuAdminOption_Click (��Ŭ�� �� Run as Administrator):

**Before:**
```csharp
private async void MenuAdminOption_Click(object sender, RoutedEventArgs e)
{
    // Start the process as admin
    await StartAssociatedProcess(true);
}
```

**After:**
```csharp
private async void MenuAdminOption_Click(object sender, RoutedEventArgs e)
{
    // Start the process as admin
    await StartAssociatedProcess(true);
  
    // ? Hide main window to tray if MinimizeToTray is enabled
    if (UserSettingsClass.MinimizeToTray)
    {
  App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
      App.MainWindow.AppWindow.Hide();
 });
    }
}
```

---

## ?? �۵� ����

### **���� �帧:**

```
1. ����ڰ� ������ Ŭ��
    ��
2. StartAssociatedProcess() ����
    ��
3. ����/����/��ũ ����
    ��
4. MinimizeToTray ���� Ȯ��
    ��
5. true�� �� AppWindow.Hide()
��
6. â�� Ʈ���̷� ������ ?
```

---

### **DispatcherQueue ��� ����:**

```csharp
App.MainWindow.DispatcherQueue.TryEnqueue(() =>
{
    App.MainWindow.AppWindow.Hide();
});
```

**����:**
- `StartAssociatedProcess()`�� �񵿱� �޼���
- UI ������Ʈ�� UI �����忡���� ����
- `DispatcherQueue`�� UI �����忡 �۾� ����
- �����ϰ� Ȯ���� ���� ����

---

## ?? ��� �ó�����

### **�ó����� 1: ���� ����**

```
1. Word ���� ����Ŭ��
2. Word ����
3. SLauncher �ڵ����� Ʈ���̷� ?
4. Word �۾�
5. Ctrl+Space�� �ٽ� SLauncher ����
```

### **�ó����� 2: ������Ʈ ����**

```
1. GitHub ��ũ Ŭ��
2. ���������� GitHub ����
3. SLauncher �ڵ����� Ʈ���̷� ?
4. ������
5. Ctrl+Space�� �ٽ� SLauncher ����
```

### **�ó����� 3: ������ ���� ����**

```
1. ��ġ ���� ��Ŭ��
2. "Run as Administrator" ����
3. UAC ������Ʈ ǥ��
4. ���� �� ��ġ ���� ����
5. SLauncher �ڵ����� Ʈ���̷� ?
```

### **�ó����� 4: �׷� �� ������**

```
1. �׷� Ŭ�� �� �׷� ���̾�α� ����
2. �׷� �� ���� Ŭ��
3. ���� ����
4. �׷� ���̾�α� ����
5. SLauncher �ڵ����� Ʈ���̷� ?
```

---

## ?? �������� ����

### **MinimizeToTray ����:**

```csharp
if (UserSettingsClass.MinimizeToTray)
{
    // Ʈ���̷� ����
}
```

**����:**
- `MinimizeToTray = true` �� ������ ���� �� Ʈ���̷�
- `MinimizeToTray = false` �� ������ ���� �� â ����

---

### **Settings���� ����:**

```
Settings �� System �� Minimize to Tray
[Yes/No] ���
```

**����� ����:**
- **Yes:** ������ ���� �� �ڵ����� Ʈ���̷� (�⺻��)
- **No:** ������ ���� �Ŀ��� â ����

---

## ?? ����� ����

### **Before (���� ��):**

```
?? �����: "������ ���� SLauncher�� ��� ������"
?? �����: "�Ź� �������� �ݾƾ� �ϳ���?"
?? �����: "�����ؿ�..."
```

### **After (���� ��):**

```
?? �����: "������ ���� SLauncher�� �ڵ����� �������!"
?? �����: "�۾� ������ ����������!"
?�����: "Ctrl+Space�� �ٷ� �ٽ� �� �� �־ ���ؿ�!"
```

---

## ?? �ٸ� ��ó�� ��

### **Alfred (macOS):**
```
������ ���� �� �ڵ����� ���� ?
```

### **Wox (Windows):**
```
������ ���� �� �ڵ����� ���� ?
```

### **PowerToys Run:**
```
������ ���� �� �ڵ����� ���� ?
```

### **SLauncher (����):**
```
������ ���� �� �ڵ����� Ʈ���̷� ??
```

**SLauncher ����:**
- ? ������ ������ �ʰ� Ʈ���̿� ����
- ? Ctrl+Space�� ��� ����
- ? ��׶��忡�� ���
- ? ���� ������

---

## ?? �߰� ���� ���� (���û���)

### **1. ������ ����**

������ ���� �� â�� ����� ���� �ణ�� ������:

```csharp
private async void GridViewTileControl_Tapped(object sender, TappedRoutedEventArgs e)
{
    await StartAssociatedProcess();
    
    if (UserSettingsClass.MinimizeToTray)
    {
        // 500ms ������ (���� ���� Ȯ��)
      await Task.Delay(500);
    
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
  App.MainWindow.AppWindow.Hide();
        });
    }
}
```

**����:**
- ������ ������ ���ȴ��� Ȯ��
- ���� �޽����� �� �ð� Ȯ��

### **2. �ִϸ��̼� ȿ��**

â�� Ʈ���̷� ������ �� ���̵� �ƿ� ȿ��:

```csharp
private async Task FadeOutAndHide()
{
    var opacity = 1.0;
  while (opacity > 0)
    {
  opacity -= 0.1;
        App.MainWindow.Opacity = opacity;
        await Task.Delay(20);
    }
    App.MainWindow.AppWindow.Hide();
    App.MainWindow.Opacity = 1.0;  // ���� �ø� ����
}
```

### **3. ���� �ɼ� �߰�**

```xml
<!-- SettingsWindow.xaml -->
<wct:SettingsCard
    Header="Auto hide on item click"
 Description="Automatically hide to tray when opening an item">
    <ToggleSwitch x:Name="AutoHideToggle" />
</wct:SettingsCard>
```

```csharp
// UserSettingsClass.cs
public static bool AutoHideOnClick = true;

// GridViewTile.xaml.cs
if (UserSettingsClass.MinimizeToTray && UserSettingsClass.AutoHideOnClick)
{
    // Hide to tray
}
```

### **4. Ư�� ������ ����**

Ư�� �������� ���� �Ŀ��� â ����:

```csharp
// GridViewTile�� �Ӽ� �߰�
public bool KeepWindowOpen { get; set; } = false;

// EditItemWindow�� üũ�ڽ� �߰�
<CheckBox x:Name="KeepWindowOpenCheckBox"
          Content="Keep SLauncher window open after execution"
     IsChecked="{x:Bind KeepWindowOpen, Mode=TwoWay}"/>

// ���� �� üũ
if (UserSettingsClass.MinimizeToTray && !this.KeepWindowOpen)
{
    // Hide to tray
}
```

---

## ?? ���� ������ ����

### **���� 1: ���� ���̾�αװ� �� ����**

**����:**
- ���� ���� ���� �� ���� �޽����� Ʈ���̷� ������ �� ����

**�ذ�:**
```csharp
public async Task StartAssociatedProcess(bool runAsAdmin = false)
{
    try
    {
        // ... ���� ���� ...
    }
    catch
    {
        // ���� �߻� �� â ������ ����
     await ShowErrorDialog();
        return;  // ���⼭ �����Ͽ� Hide() ���� �� ��
    }
    
  // ���� �ø� ����
}
```

### **���� 2: UAC ������Ʈ �ڿ� â ������**

**����:**
- ������ ���� ���� �� UAC ������Ʈ�� ǥ�õǱ� ���� â�� ������

**�ذ�:**
```csharp
private async void MenuAdminOption_Click(object sender, RoutedEventArgs e)
{
    await StartAssociatedProcess(true);
    
    // UAC �Ϸ� ���
    await Task.Delay(1000);
    
    if (UserSettingsClass.MinimizeToTray)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
      {
            App.MainWindow.AppWindow.Hide();
        });
    }
}
```

### **���� 3: �׷� ���̾�αװ� ���� ����**

**����:**
- �׷� �� ������ ���� �� �׷� ���̾�α׵� ���� ����

**���� ����:**
- �׷� ���̾�α״� MainWindow�� �ڽ��̹Ƿ�
- MainWindow�� �������� �ڵ����� ����
- **�̴� �ǵ��� �����Դϴ�** ?

**��� (���ϸ�):**
```csharp
// �׷� ���̾�α� ���� ������ â ������ ����
if (UserSettingsClass.MinimizeToTray && !IsGroupDialogOpen)
{
    // Hide to tray
}
```

---

## ?? ���� ����

### **�ڵ� ������ �߻��ϴ� ���:**

1. ? ������ ����Ŭ��
2. ? ��Ŭ�� �� Open
3. ? ��Ŭ�� �� Run as Administrator
4. ? �׷� �� ������ Ŭ��

### **�ڵ� ������ �߻����� �ʴ� ���:**

1. ? ��Ŭ�� �� Open File Location
2. ? ��Ŭ�� �� Edit
3. ? ��Ŭ�� �� Remove
4. ? �巡�� �� ��� (������)
5. ? MinimizeToTray = false �� ��

---

## ?? �׽�Ʈ �ó�����

### **Test 1: ���� ����**
```
1. Settings �� Minimize to Tray = Yes
2. Word ���� ����Ŭ��
3. Word ���� ?
4. SLauncher Ʈ���̷� �̵� ?
```

### **Test 2: ������Ʈ ����**
```
1. GitHub ��ũ Ŭ��
2. ���������� ���� ?
3. SLauncher Ʈ���̷� �̵� ?
```

### **Test 3: ������ ����**
```
1. ��ġ ���� ��Ŭ��
2. "Run as Administrator"
3. UAC ����
4. ��ġ ���� ���� ?
5. SLauncher Ʈ���̷� �̵� ?
```

### **Test 4: MinimizeToTray = No**
```
1. Settings �� Minimize to Tray = No
2. ���� Ŭ��
3. ���� ���� ?
4. SLauncher â ���� ?
```

### **Test 5: �׷� �� ������**
```
1. �׷� Ŭ��
2. �׷� ���̾�α� ����
3. �׷� �� ���� Ŭ��
4. ���� ���� ?
5. �׷� ���̾�α� ���� ?
6. SLauncher Ʈ���̷� �̵� ?
```

### **Test 6: ���� ó��**
```
1. ������ ���� Ŭ��
2. ���� ���̾�α� ǥ�� ?
3. SLauncher â ���� ? (Hide �� ��)
```

---

## ? ���� �Ϸ�!

### **����� ����:**
- ? `GridViewTile.xaml.cs`
  - `GridViewTileControl_Tapped` ����
  - `MenuOpenOption_Click` ����
  - `MenuAdminOption_Click` ����

### **����:**
- ? ����/����/��ũ Ŭ�� �� �ڵ� Ʈ���� �̵�
- ? MinimizeToTray ���� ����
- ? DispatcherQueue�� ������ UI ������Ʈ
- ? ��� ���� ����� ����

### **Ư¡:**
- ? �������� ����
- ? �ٸ� ��ó�� ������ UX
- ? �������� ���� ����
- ? Ctrl+Space�� ���� ����

---

## ?? ���� �� �׽�Ʈ

### **����:**
```
Visual Studio �� Rebuild Solution �� F5
```

### **�׽�Ʈ:**
```
1. ���� �ϳ� �߰�
2. Settings �� Minimize to Tray = Yes Ȯ��
3. ���� ����Ŭ��
4. ���� ���� + SLauncher Ʈ���̷� ?
5. Ctrl+Space�� ���� ?
6. �Ϻ�! ??
```

---

## ?? �Ϸ�!

**����/����/��ũ Ŭ�� �� �ڵ����� Ʈ���̷� �̵��ϴ� ����� �����Ǿ����ϴ�!**

**����:**
- ? ������ ���� �� �ڵ����� Ʈ���̷�
- ? �۾� ���� ����ϰ� ����
- ? Ctrl+Space�� ��� ����
- ? Alfred, Wox�� ���� UX

**�Ϻ��� ��ó ������ �����մϴ�!** ?

**�׽�Ʈ�غ�����!** ??
