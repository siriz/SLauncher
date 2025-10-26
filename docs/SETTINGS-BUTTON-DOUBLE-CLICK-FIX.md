# ?? Settings ��ư �ߺ� Ŭ�� ���� - �ذ� �Ϸ�!

## ? ����

**����:**
```
1. Settings ��ư Ŭ��
2. Settings â ����
3. Settings ��ư ������ �ٽ� Ŭ��
4. Settings â 2�� ���� ?
5. ȥ�������� ����
```

**����:**
- ù ��° Ŭ������ Settings â�� ������ ����
- �� ��° Ŭ���� ó����
- `CreateModalWindow`�� �� �� ȣ���
- �ߺ� Ŭ�� ���� ���� ����

---

## ? �ذ� ���

### **2�ܰ� ���:**

**1. �÷��� Ȯ��**
```csharp
private bool isSettingsWindowOpen = false;

if (isSettingsWindowOpen)
{
    return;  // �̹� ���� ������ ����
}
```

**2. ��ư ��Ȱ��ȭ**
```csharp
SettingsButton.IsEnabled = false;  // ��ư ��Ȱ��ȭ
```

---

## ?? ������ �ڵ�

### **MainWindow.xaml.cs**

**Before (����):**

```csharp
private void SettingsButton_Click(object sender, RoutedEventArgs e)
{
    // Open Settings Window as a modal window
    SettingsWindow settingsWindow = new SettingsWindow();
  UIFunctionsClass.CreateModalWindow(settingsWindow, this);

    // Update the UI once the SettingsWindow is closed
    settingsWindow.Closed += (s, e) => UpdateUIFromSettings();
}
```

**������:**
- �ߺ� Ŭ�� �� ���� â ����
- ��ư�� ��� Ȱ��ȭ ����
- ����ڿ��� �ǵ�� ����

---

**After (�ذ�):**

```csharp
// Track if settings window is already open
private bool isSettingsWindowOpen = false;

private void SettingsButton_Click(object sender, RoutedEventArgs e)
{
    // ? Prevent opening multiple settings windows
    if (isSettingsWindowOpen)
    {
  return;
    }

    isSettingsWindowOpen = true;
  
    // ? Disable settings button
    SettingsButton.IsEnabled = false;

    // Open Settings Window as a modal window
    SettingsWindow settingsWindow = new SettingsWindow();
    UIFunctionsClass.CreateModalWindow(settingsWindow, this);

    // Update the UI once the SettingsWindow is closed
    settingsWindow.Closed += (s, e) =>
    {
        UpdateUIFromSettings();
   isSettingsWindowOpen = false;
        
 // ? Re-enable settings button
      SettingsButton.IsEnabled = true;
    };
}
```

**���� ����:**
- ? �÷��׷� �ߺ� ����
- ? ��ư ��Ȱ��ȭ�� �ð��� �ǵ��
- ? â ���� �� ���� ����

---

## ?? �۵� ����

### **�ó����� 1: ���� ����Ŭ�� (���� ��Ȳ)**

**Before:**
```
Click 1 �� CreateModalWindow() ����
  �� (100ms �ҿ�)
Click 2 �� CreateModalWindow() �� ���� ?
  ��
Settings â 2�� ���� ?
```

**After:**
```
Click 1 �� isSettingsWindowOpen = true
        �� SettingsButton.IsEnabled = false
        �� CreateModalWindow() ����
  ��
Click 2 �� if (isSettingsWindowOpen) return; ?
    �� ���õ� ?
  ��
Settings â 1���� ���� ?
```

---

### **�ó����� 2: ���� Ŭ��**

**����:**
```
1. Settings ��ư Ŭ��
   �� isSettingsWindowOpen = true
   �� ��ư ��Ȱ��ȭ (ȸ��)

2. Settings â ����
   �� ����� ���� ����

3. Settings â �ݱ�
   �� UpdateUIFromSettings()
   �� isSettingsWindowOpen = false
   �� ��ư Ȱ��ȭ (�ٽ� Ŭ�� ����)

4. Settings ��ư �ٽ� Ŭ�� ���� ?
```

---

## ?? �÷��� vs ��ư ��Ȱ��ȭ

### **�÷��׸� ����� ���:**

```csharp
if (isSettingsWindowOpen)
{
    return;  // ����
}
```

**����:**
- ? �ߺ� ���� Ȯ��
- ? ������ ����

**����:**
- ? ����ڿ��� �ǵ�� ����
- ? ��ư�� ������ Ŭ�� ������ ����
- ? �� �� �Ǵ��� ��

---

### **��ư ��Ȱ��ȭ�� �Բ�:**

```csharp
SettingsButton.IsEnabled = false;
```

**����:**
- ? �ð��� �ǵ�� (ȸ������ ����)
- ? ����ڰ� ��ư�� Ŭ���� �� ����
- ? "�̹� ���� �ֱ���" ���� ����
- ? �� ���� UX

**����:**
- ����

**���: �� �� ����ϴ� ���� �ּ�!** ?

---

## ?? ����� ����

### **Before (����):**

```
?? �����: "Settings ��ư�� ������ 2�� �������� â�� 2�� �����"
?? �����: "��� â�� �ݾƾ� ����?"
?? �����: "�����ΰ���?"
```

### **After (�ذ�):**

```
?? �����: "Settings ��ư�� �������� ��ư�� ȸ������ ���߾��"
?? �����: "��, �̹� ���� �ֱ���!"
? �����: "�������̿���!"
```

---

## ?? �ߺ� ���� ����

### **���� 1: �÷��� (Flag)**

```csharp
private bool isOperationInProgress = false;

private void Button_Click(object sender, RoutedEventArgs e)
{
    if (isOperationInProgress)
    {
        return;
    }

    isOperationInProgress = true;
    
    // ... �۾� ���� ...
    
    // �۾� �Ϸ� ��
    isOperationInProgress = false;
}
```

**��� ���:**
- ��� â ����
- ���� ����/�ε�
- ��Ʈ��ũ ��û
- �� �۾� (�ε�)

---

### **���� 2: ��ư ��Ȱ��ȭ**

```csharp
private async void Button_Click(object sender, RoutedEventArgs e)
{
    Button.IsEnabled = false;
    
    // ... �۾� ���� ...
    
    Button.IsEnabled = true;
}
```

**��� ���:**
- ����� �Է� ����
- �ð��� �ǵ��
- Form ����
- �ߺ� ���� ����

---

### **���� 3: ��ٿ�� (Debouncing)**

```csharp
private DateTime lastClickTime = DateTime.MinValue;
private TimeSpan debounceInterval = TimeSpan.FromMilliseconds(500);

private void Button_Click(object sender, RoutedEventArgs e)
{
    DateTime now = DateTime.Now;
    
    if (now - lastClickTime < debounceInterval)
    {
        return;  // �ʹ� ���� Ŭ����
    }
    
    lastClickTime = now;
    
    // ... �۾� ���� ...
}
```

**��� ���:**
- �˻� �Է�
- �ڵ� ����
- API ��û ����
- ��ũ�� �̺�Ʈ

---

### **���� 4: �������� (Semaphore)**

```csharp
private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

private async void Button_Click(object sender, RoutedEventArgs e)
{
    if (!await semaphore.WaitAsync(0))  // ��� �õ�
    {
     return;  // �̹� ���� ��
    }
    
    try
    {
    // ... �۾� ���� ...
    }
    finally
    {
        semaphore.Release();
    }
}
```

**��� ���:**
- ��Ƽ������ ȯ��
- �񵿱� �۾�
- ������ ����ȭ
- ���ҽ� ����

---

## ?? �츮�� ����: �÷��� + ��ư ��Ȱ��ȭ

**��?**

```
1. ������ ?
   - �÷��� 1��
   - IsEnabled �Ӽ��� ����

2. ȿ���� ?
   - �ߺ� ���� Ȯ��
   - �ð��� �ǵ��

3. ���� ?
   - ������� ����
   - ��� ����

4. �������� ?
   - �ڵ� �����ϱ� ����
   - ���� ���ɼ� ����
```

**�Ϻ��� ����!** ??

---

## ?? �׽�Ʈ �ó�����

### **Test 1: ���� ����Ŭ��**

```
1. Settings ��ư Ŭ��
2. ��� �ٽ� Ŭ��
3. ���: Settings â 1���� ���� ?
4. ��ư ��Ȱ��ȭ ���� ?
```

### **Test 2: Ʈ���� Ŭ��**

```
1. Settings ��ư Ŭ��
2. �������� 2�� �� Ŭ��
3. ���: Settings â 1���� ���� ?
4. �߰� Ŭ�� ���õ� ?
```

### **Test 3: â �ݰ� �ٽ� ����**

```
1. Settings ��ư Ŭ��
2. Settings â ����
3. Settings â �ݱ�
4. Settings ��ư �ٽ� Ŭ�� ���� ?
5. Settings â �ٽ� ���� ?
```

### **Test 4: ��ư ��Ȱ��ȭ Ȯ��**

```
1. Settings ��ư Ŭ��
2. ��ư�� ȸ������ ���� ?
3. Settings â �ݱ�
4. ��ư�� �ٽ� Ȱ��ȭ ?
```

### **Test 5: ���� �� �ݺ�**

```
1. Settings ���� �� �ݱ�
2. Settings ���� �� �ݱ�
3. Settings ���� �� �ݱ�
4. ��� ���� �۵� ?
5. â 1������ ���� ?
```

---

## ?? ���� ������ ����

### **���� 1: â�� ������ ���� ��**

**����:**
```
Alt+F4�� �۾� �����ڷ� Settings â ���� ����
�� isSettingsWindowOpen = true �״��
�� ��ư�� ��Ȱ��ȭ ���·� ����
```

**�ذ�:**
```csharp
settingsWindow.Closed += (s, e) =>
{
    // Closed �̺�Ʈ�� ��� ���� ������� �߻�
    // Alt+F4, X��ư, �۾� ������ ��� OK ?
    UpdateUIFromSettings();
    isSettingsWindowOpen = false;
    SettingsButton.IsEnabled = true;
};
```

**���� �ڵ�� �̹� ������!** ?

---

### **���� 2: ���� �߻� ��**

**����:**
```
Settings â ���� �� ���� �߻�
�� isSettingsWindowOpen = true �״��
�� ��ư ���� �� ��
```

**�ذ�:**
```csharp
private void SettingsButton_Click(object sender, RoutedEventArgs e)
{
    if (isSettingsWindowOpen)
    {
        return;
    }

  isSettingsWindowOpen = true;
    SettingsButton.IsEnabled = false;

    try
    {
    SettingsWindow settingsWindow = new SettingsWindow();
        UIFunctionsClass.CreateModalWindow(settingsWindow, this);

        settingsWindow.Closed += (s, e) =>
        {
    UpdateUIFromSettings();
     isSettingsWindowOpen = false;
            SettingsButton.IsEnabled = true;
        };
    }
    catch (Exception ex)
    {
        // ���� �߻� �� ���� ����
        isSettingsWindowOpen = false;
        SettingsButton.IsEnabled = true;
    
        // ���� ǥ��
        Debug.WriteLine($"Error opening settings: {ex}");
}
}
```

**�ʿ� �� �߰� ����** (����� ���� ���ɼ� ����)

---

### **���� 3: �޸� ����**

**����:**
```
Settings â�� ���� �� ���� ����
�� �̺�Ʈ �ڵ鷯 ����?
�� �޸� ��뷮 ����?
```

**Ȯ��:**
```csharp
settingsWindow.Closed += (s, e) =>
{
    // ���ٴ� settingsWindow �ν��Ͻ��� �����
    // â�� ������ �̺�Ʈ �ڵ鷯�� GC��
    // �޸� ���� ���� ?
};
```

**���� �ڵ�� ������!** ?

---

## ?? �Ϸ�!

### **����� ����:**
- ? `MainWindow.xaml.cs`
  - `isSettingsWindowOpen` �÷��� �߰�
  - `SettingsButton_Click` �޼��� ����
  - ��ư ��Ȱ��ȭ/Ȱ��ȭ ���� �߰�

### **�ذ�� ����:**
- ? Settings ��ư �ߺ� Ŭ�� ����
- ? Settings â 1���� ����
- ? �ð��� �ǵ�� (��ư ��Ȱ��ȭ)
- ? ����� ���� ����

### **Ư¡:**
- ? �����ϰ� ȿ������ ����
- ? ���� ������� ����
- ? ��� ���� ������� ����
- ? �������� ����

---

## ?? �׽�Ʈ ���

```
1. SLauncher ����

2. Settings ��ư ������ 2�� Ŭ��
   �� Settings â 1���� ���� ?
   �� ��ư ��Ȱ��ȭ ���� ?

3. Settings â �ݱ�
   �� ��ư �ٽ� Ȱ��ȭ ?

4. Settings ��ư �ٽ� Ŭ��
 �� Settings â ���� ���� ?

5. ���� �� �ݺ� �׽�Ʈ
   �� ��� ���� �۵� ?
```

---

## ?? �ٸ� ��ư���� ���� ����

### **Add File / Add Folder / Add Website ��ư:**

������ ���� ���� ���� (�ʿ� ��):

```csharp
private bool isAddFileDialogOpen = false;

private async void AddFileBtn_Click(object sender, RoutedEventArgs e)
{
    if (isAddFileDialogOpen)
    {
        return;
  }

    isAddFileDialogOpen = true;
    AddFileBtn.IsEnabled = false;

 try
    {
   AddFileDialog addFileDialog = new AddFileDialog()
        {
            XamlRoot = Container.XamlRoot
   };

    ContentDialogResult result = await addFileDialog.ShowAsync();

        // ... ���� ���� ...
    }
    finally
    {
        isAddFileDialogOpen = false;
        AddFileBtn.IsEnabled = true;
    }
}
```

**����� ContentDialog�̹Ƿ� �ߺ� ���� ����** (Modal�̶� �ڵ� ���ܵ�)

---

## ?? �н� ����Ʈ

### **1. �ߺ� Ŭ�� ������ �߿��ϴ�**
```
����ڴ� ������ Ŭ���� �� ����
�� ��� ���� �ʼ�
```

### **2. �÷��� + UI �ǵ�� ������ �ּ�**
```
�÷���: ���� ����
UI �ǵ��: ����� ����
```

### **3. �̺�Ʈ �ڵ鷯���� ���� ����**
```
Closed �̺�Ʈ���� �÷��� false
�� ��� ���� ��� Ŀ��
```

### **4. ������ �ذ�å�� �ּ�**
```
������ ����ȭ ���ʿ�
�� �÷��׸����� ���
```

---

**�Ϻ��ϰ� �ذ�Ǿ����ϴ�!** ??

**���� Settings ��ư�� �ƹ��� ���� ������ â�� 1���� �����ϴ�!** ?

**�׽�Ʈ�غ�����!** ??
