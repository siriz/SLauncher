# ?? Auto Focus SearchBox - ���� �Ϸ�!

## ? ������ ���

**���� ���� �� �ڵ����� �˻� â�� ��Ŀ��:**

```
1. �� ���� �� SearchBox ��Ŀ�� ?
2. Ctrl+Space (â ǥ��) �� SearchBox ��Ŀ�� ?
3. Ʈ���� ������ ����Ŭ�� �� SearchBox ��Ŀ�� ?
4. Ʈ���� �޴� "Open" �� SearchBox ��Ŀ�� ?
```

**Ư¡:**
- ? �� ���� �� ��� Ÿ���� ����
- ? ��Ű�� â ���� �ٷ� �˻� ����
- ? Ʈ���̿��� ���� �� ��� �˻� ����
- ? ���콺 Ŭ�� ���� Ű���常���� ��� ����

---

## ?? ������ ����

### **MainWindow.xaml.cs**

#### 1. Container_Loaded (�� ����):

**Before:**
```csharp
// Hide LoadingDialog once done
await Task.Delay(20);
LoadingDialog.Visibility = Visibility.Collapsed;
```

**After:**
```csharp
// Hide LoadingDialog once done
await Task.Delay(20);
LoadingDialog.Visibility = Visibility.Collapsed;

// ? Set focus to SearchBox
SearchBox.Focus(FocusState.Programmatic);
```

---

#### 2. ToggleWindowVisibility (Ctrl+Space):

**Before:**
```csharp
else
{
    // Window is hidden, show it
    this.AppWindow.Show();
    this.Activate();
}
```

**After:**
```csharp
else
{
    // Window is hidden, show it
    this.AppWindow.Show();
    this.Activate();
    
    // ? Set focus to SearchBox
    SearchBox.Focus(FocusState.Programmatic);
}
```

---

#### 3. InitializeTrayIcon (Ʈ���� ������):

**SetOnLeftClick (����Ŭ��):**

**Before:**
```csharp
trayIcon.SetOnLeftClick(() =>
{
    this.DispatcherQueue.TryEnqueue(() =>
    {
        this.AppWindow.Show();
        this.Activate();
    });
});
```

**After:**
```csharp
trayIcon.SetOnLeftClick(() =>
{
    this.DispatcherQueue.TryEnqueue(() =>
    {
        this.AppWindow.Show();
  this.Activate();
    
  // ? Set focus to SearchBox
 SearchBox.Focus(FocusState.Programmatic);
    });
});
```

---

**SetOnOpenMenu (��Ŭ�� �� Open):**

**Before:**
```csharp
trayIcon.SetOnOpenMenu(() =>
{
  this.DispatcherQueue.TryEnqueue(() =>
    {
        this.AppWindow.Show();
        this.Activate();
    });
});
```

**After:**
```csharp
trayIcon.SetOnOpenMenu(() =>
{
this.DispatcherQueue.TryEnqueue(() =>
    {
      this.AppWindow.Show();
        this.Activate();
  
        // ? Set focus to SearchBox
        SearchBox.Focus(FocusState.Programmatic);
    });
});
```

---

## ?? �۵� ����

### **FocusState.Programmatic:**

```csharp
SearchBox.Focus(FocusState.Programmatic);
```

**FocusState �ɼ�:**
- `FocusState.Programmatic` - �ڵ�� ��Ŀ�� ����
- `FocusState.Keyboard` - Ű����� ��Ŀ�� �̵��� ��ó��
- `FocusState.Pointer` - ���콺�� Ŭ���� ��ó��

**Programmatic ���� ����:**
- ����� �׼� ���� �ڵ����� ��Ŀ�� ����
- �ð������� ��� (��Ŀ�� �� ǥ�� �� ��)
- Ű���� �Է� ��� ����

---

## ?? ��� �ó�����

### **�ó����� 1: �� ����**

```
1. SLauncher ����
2. �ε� �Ϸ�
3. SearchBox�� ��Ŀ�� ?
4. ��� Ÿ���� �� �˻� ����
```

### **�ó����� 2: Ctrl+Space�� ����**

```
1. �ٸ� �ۿ��� �۾� ��
2. Ctrl+Space
3. SLauncher ��Ÿ��
4. SearchBox�� ��Ŀ�� ?
5. ��� Ÿ���� �� ���� ã��
```

### **�ó����� 3: Ʈ���̿��� ����**

```
1. Ʈ���� ������ ����Ŭ��
2. SLauncher ��Ÿ��
3. SearchBox�� ��Ŀ�� ?
4. ��� Ÿ���� �� �˻�
```

### **�ó����� 4: ��ũ�÷ο�**

```
��ü Ű���� ��ũ�÷ο�:
1. Ctrl+Space (����)
2. "word" (Ÿ����)
3. Enter (����)
4. �ڵ����� Ʈ���̷�
5. Ctrl+Space (�ٽ� ����)
6. "chrome" (Ÿ����)
7. Enter (����)

�� ���콺 ��ġ ���� ������ Ű���常����!
```

---

## ?? �ٸ� ��ó�� ��

### **Alfred (macOS):**
```
Cmd+Space �� �˻� â ��Ŀ�� ?
```

### **Wox (Windows):**
```
Alt+Space �� �˻� â ��Ŀ�� ?
```

### **PowerToys Run:**
```
Alt+Space �� �˻� â ��Ŀ�� ?
```

### **SLauncher (����):**
```
Ctrl+Space �� SearchBox ��Ŀ�� ?
Ʈ���� ���� �� SearchBox ��Ŀ�� ?
�� ���� �� SearchBox ��Ŀ�� ?
```

---

## ?? ����� ����

### **Before (���� ��):**

```
?? �����: "Ctrl+Space ������ ���콺�� �˻� â Ŭ���ؾ� �ؿ�"
?? �����: "Ű���常���� �� ���׿�?"
?? �����: "�����ؿ�..."
```

### **After (���� ��):**

```
?? �����: "Ctrl+Space ������ �ٷ� Ÿ�����ϸ� �ſ�!"
?? �����: "Ű���常���� ������ ��� �����ؿ�!"
? �����: "Alfredó�� ������ ���ؿ�!"
```

---

## ?? �߰� ���� ���� (���û���)

### **1. �ؽ�Ʈ �ڵ� ����**

SearchBox�� ���� �˻�� ������ �ڵ� ����:

```csharp
SearchBox.Focus(FocusState.Programmatic);

// �ؽ�Ʈ�� ������ ��ü ����
if (!string.IsNullOrEmpty(SearchBox.Text))
{
    SearchBox.SelectAll();
}
```

**����:**
- ���ο� �˻��� �ٷ� �Է� ����
- ���� �˻��� ������ �ʾƵ� ��

### **2. ��Ŀ�� ������**

â�� ������ ǥ�õ� �� ��Ŀ��:

```csharp
this.AppWindow.Show();
this.Activate();

// �ణ�� ������ �� ��Ŀ�� (������)
await Task.Delay(100);
SearchBox.Focus(FocusState.Programmatic);
```

**����:**
- â �ִϸ��̼ǰ� �浹 ����
- �� �������� ��Ŀ��

### **3. ��Ŀ�� ���� ��õ�**

��Ŀ�� ���� �� ��õ�:

```csharp
private async void SetSearchBoxFocus()
{
    for (int i = 0; i < 3; i++)
    {
        var result = SearchBox.Focus(FocusState.Programmatic);
        if (result)
        {
 break;
        }
        await Task.Delay(50);
    }
}
```

**����:**
- Ÿ�̹� �̽� �ذ�
- �� ���� ������

### **4. �˻��� �����丮**

�ֱ� �˻��� �ڵ� ǥ��:

```csharp
private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
{
    // �ֱ� �˻�� ������ ��Ӵٿ� ǥ��
    if (RecentSearches.Count > 0 && string.IsNullOrEmpty(SearchBox.Text))
    {
   SearchBox.ItemsSource = RecentSearches;
 }
    
    // ...existing code...
}
```

### **5. ��Ŀ�� ǥ�� ��ȭ**

��Ŀ���� �ð������� ����:

```csharp
SearchBox.Focus(FocusState.Programmatic);

// ��Ŀ�� �ִϸ��̼�
var animation = new DoubleAnimation
{
    From = 0.7,
    To = 1.0,
    Duration = TimeSpan.FromMilliseconds(200)
};
Storyboard.SetTarget(animation, SearchBox);
Storyboard.SetTargetProperty(animation, "Opacity");

var storyboard = new Storyboard();
storyboard.Children.Add(animation);
storyboard.Begin();
```

---

## ?? ���� ������ ����

### **���� 1: ��Ŀ���� �� ���� ��**

**����:**
- `SearchBox.Focus()` ȣ���ص� ��Ŀ�� �� ��

**����:**
- â�� ���� ������ Ȱ��ȭ���� ����
- UI ������ �Ϸ� �� ȣ��

**�ذ�:**
```csharp
this.Activate();
await Task.Delay(50);  // �ణ�� ������
SearchBox.Focus(FocusState.Programmatic);
```

### **���� 2: �ٸ� ��Ʈ���� ��Ŀ�� ������**

**����:**
- SearchBox�� ��Ŀ�������� �ٸ� ��Ʈ�ѷ� �̵�

**����:**
- TabIndex ���� ����
- �ٸ� ��Ʈ���� Focus() ȣ��

**�ذ�:**
```xml
<!-- SearchBox.xaml -->
<AutoSuggestBox x:Name="SearchBox"
         TabIndex="0"
        IsTabStop="True"/>
```

### **���� 3: Ű���� �Է��� �� ��**

**����:**
- ��Ŀ���� ������ Ÿ������ �� ��

**����:**
- SearchBox�� ��Ȱ��ȭ ����
- IsEnabled="False"

**�ذ�:**
```csharp
SearchBox.IsEnabled = true;
SearchBox.Focus(FocusState.Programmatic);
```

---

## ?? ��Ŀ�� Ÿ�̹�

### **��Ŀ���� �����Ǵ� ��� ���:**

1. ? **�� ����**
   ```
   Container_Loaded �� LoadingDialog ���� �� SearchBox.Focus()
   ```

2. ? **Ctrl+Space (������ ����)**
   ```
   ToggleWindowVisibility �� AppWindow.Show() �� SearchBox.Focus()
   ```

3. ? **Ʈ���� ������ ����Ŭ��**
   ```
   SetOnLeftClick �� AppWindow.Show() �� SearchBox.Focus()
   ```

4. ? **Ʈ���� �޴� "Open"**
   ```
   SetOnOpenMenu �� AppWindow.Show() �� SearchBox.Focus()
   ```

### **��Ŀ���� �������� �ʴ� ���:**

1. ? **���� â���� ���ƿ� ��**
   - ���� â ������ ���� â�� ��Ŀ��
   - �ʿ�� �߰� ����

2. ? **������ ���� ��**
   - ������ ���� �� Ʈ���̷� ����
   - SearchBox ��Ŀ�� ���ʿ�

3. ? **�巡�� �� ��� ��**
   - ���� �߰� �۾� ��
   - ����ڰ� ���콺 ��� ��

---

## ?? �׽�Ʈ �ó�����

### **Test 1: �� ����**
```
1. SLauncher ����
2. �ε� ���
3. Ű����� �ٷ� Ÿ���� ?
4. �˻��� �Էµ� Ȯ�� ?
```

### **Test 2: Ctrl+Space**
```
1. â �ݱ� (Ʈ���̷�)
2. Ctrl+Space
3. Ű����� �ٷ� Ÿ���� ?
4. �˻��� �Էµ� Ȯ�� ?
```

### **Test 3: Ʈ���� ������**
```
1. â �ݱ� (Ʈ���̷�)
2. Ʈ���� ������ ����Ŭ��
3. Ű����� �ٷ� Ÿ���� ?
4. �˻��� �Էµ� Ȯ�� ?
```

### **Test 4: Ʈ���� �޴�**
```
1. â �ݱ� (Ʈ���̷�)
2. Ʈ���� ������ ��Ŭ�� �� "Open"
3. Ű����� �ٷ� Ÿ���� ?
4. �˻��� �Էµ� Ȯ�� ?
```

### **Test 5: ������ Ű���� ��ũ�÷ο�**
```
1. Ctrl+Space (����)
2. "word" Ÿ���� (���콺 ��ġ ����) ?
3. Enter (����)
4. �ڵ����� Ʈ���̷�
5. Ctrl+Space (�ٽ� ����)
6. "excel" Ÿ���� (���콺 ��ġ ����) ?
7. Enter (����)
8. �Ϻ��� Ű���� ��ũ�÷ο�! ?
```

---

## ? ���� �Ϸ�!

### **����� ����:**
- ? `MainWindow.xaml.cs`
  - `Container_Loaded` ����
  - `ToggleWindowVisibility` ����
  - `InitializeTrayIcon` ���� (SetOnLeftClick, SetOnOpenMenu)

### **����:**
- ? �� ���� �� SearchBox �ڵ� ��Ŀ��
- ? Ctrl+Space�� â ǥ�� �� �ڵ� ��Ŀ��
- ? Ʈ���̿��� ���� �� �ڵ� ��Ŀ��
- ? ������ Ű���� ��ũ�÷ο� ����

### **Ư¡:**
- ? ���콺 ���� Ű���常���� ��� ����
- ? Alfred, Wox�� ������ UX
- ? ������ �������� �˻�
- ? ���꼺 ���

---

## ?? ���� �� �׽�Ʈ

### **����:**
```
Visual Studio �� Rebuild Solution �� F5
```

### **�׽�Ʈ:**
```
1. SLauncher ����
2. �ε� �Ϸ�Ǹ� �ٷ� Ÿ���� ?
3. Ctrl+Space�� ����� �ٽ� ����
4. �ٷ� Ÿ���� ?
5. �Ϻ�! ??
```

---

## ?? �Ϸ�!

**���� ���� �� �ڵ����� �˻� â�� ��Ŀ���� ���� ����� �����Ǿ����ϴ�!**

**����:**
- ? �� ���� ��� �˻� ����
- ? ���콺 ��ġ ���� Ű���常���� ���
- ? Alfred, Wox�� ������ ����
- ? ������ ���� ��ũ�÷ο�

**�Ϻ��� ��ó ������ �����մϴ�!** ?

**�׽�Ʈ�غ�����!** ??
