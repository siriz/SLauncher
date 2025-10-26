# ?? �� ���ؽ�Ʈ �޴� �߰� �Ϸ�!

## ? ������ ���

### ?? **�ٽ� ���:**

1. ? **�� ��Ŭ�� �� ���ؽ�Ʈ �޴�**
2. ? **�̸� ����** (�ؽ�Ʈ �Է� ���̾�α�)
3. ? **����** (Ȯ�� ���̾�α� + ������ ���� ǥ��)
4. ? **������ �� ���� ����**
5. ? **������ ���� ���** (1�� �̻��� ��)

---

## ?? ������ �ڵ�

### **1. InitializeTabs() - �⺻ �ǿ� ���ؽ�Ʈ �޴� �߰�**

```csharp
private void InitializeTabs()
{
    // Create default tab
    var defaultTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
    defaultTab.Header = "�⺻";
    defaultTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource 
    { 
   Symbol = Microsoft.UI.Xaml.Controls.Symbol.Home 
 };
 
    // ? Add context menu to tab
    AttachTabContextMenu(defaultTab);
   
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    
    // Set as previous tab
    _previousTab = defaultTab;
}
```

---

### **2. MainTabView_AddTabButtonClick() - �� �ǿ� ���ؽ�Ʈ �޴� �߰�**

```csharp
private void MainTabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
{
    // Save current tab items before creating new tab
    SaveCurrentTabItems();
    
  // Create new tab
    var newTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
    newTab.Header = $"�� {MainTabView.TabItems.Count + 1}";
    newTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource 
    { 
        Symbol = Microsoft.UI.Xaml.Controls.Symbol.Document 
    };
    
    // ? Add context menu to tab
    AttachTabContextMenu(newTab);
    
    // Add to TabView
    MainTabView.TabItems.Add(newTab);
    MainTabView.SelectedItem = newTab;
   
    // Clear items for new tab
    ItemsGridView.Items.Clear();
}
```

---

### **3. AttachTabContextMenu() - ���ؽ�Ʈ �޴� ����**

```csharp
/// <summary>
/// Attach context menu to a tab
/// </summary>
private void AttachTabContextMenu(Microsoft.UI.Xaml.Controls.TabViewItem tab)
{
    var contextMenu = new MenuFlyout();
    
    // ? Rename menu item
    var renameItem = new MenuFlyoutItem
    {
   Text = "�̸� ����",
  Icon = new SymbolIcon(Symbol.Rename)
    };
    renameItem.Click += (s, e) => RenameTab_Click(tab);
    contextMenu.Items.Add(renameItem);
    
    // ? Delete menu item
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

**�޴� �׸�:**
1. **�̸� ����** - ?? ������
2. **����** - ??? ������

---

### **4. RenameTab_Click() - �� �̸� ����**

```csharp
/// <summary>
/// Handle tab rename
/// </summary>
private async void RenameTab_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab)
{
    var textBox = new TextBox
    {
    Text = tab.Header?.ToString() ?? "",
        PlaceholderText = "�� �̸� �Է�",
  Width = 250
    };
    
    var dialog = new ContentDialog
    {
  Title = "�� �̸� ����",
Content = textBox,
        PrimaryButtonText = "Ȯ��",
      CloseButtonText = "���",
    DefaultButton = ContentDialogButton.Primary,
        XamlRoot = this.Content.XamlRoot
    };
   
    var result = await dialog.ShowAsync();
    
    if (result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(textBox.Text))
    {
        tab.Header = textBox.Text;  // ? �̸� ������Ʈ
    }
}
```

**����:**
1. ���� �� �̸��� TextBox�� ǥ��
2. ����ڰ� �� �̸� �Է�
3. "Ȯ��" Ŭ�� �� �� �̸� ����
4. "���" �Ǵ� �� �Է� �� ���� �ȵ�

---

### **5. DeleteTab_Click() - �� ���� (�ٽ�!)**

```csharp
/// <summary>
/// Handle tab delete
/// </summary>
private async void DeleteTab_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab)
{
    // ? 1. Don't allow deleting the last tab
    if (MainTabView.TabItems.Count <= 1)
  {
     var errorDialog = new ContentDialog
    {
    Title = "���� �Ұ�",
            Content = "������ ���� ������ �� �����ϴ�.",
            CloseButtonText = "Ȯ��",
         XamlRoot = this.Content.XamlRoot
        };
        await errorDialog.ShowAsync();
        return;
    }
    
    // ? 2. Count items in the tab
    int itemCount = 0;
    if (tab.Tag is List<UserControl> items)
    {
        itemCount = items.Count;
    }
    else if (tab == MainTabView.SelectedItem)
    {
        // If this is the current tab, count from ItemsGridView
        itemCount = ItemsGridView.Items.Count;
    }
    
    // ? 3. Show confirmation dialog with item count
    string message = itemCount > 0
        ? $"�� �ǿ��� {itemCount}���� �������� �ֽ��ϴ�.\n���� �����ϸ� ��� �����۵� �Բ� �����˴ϴ�.\n\n���� �����Ͻðڽ��ϱ�?"
    : "�� ���� �����Ͻðڽ��ϱ�?";
    
    var confirmDialog = new ContentDialog
  {
   Title = "�� ���� Ȯ��",
    Content = message,
 PrimaryButtonText = "����",
        CloseButtonText = "���",
   DefaultButton = ContentDialogButton.Close,  // ? �⺻�� ���
   XamlRoot = this.Content.XamlRoot
    };
    
    var result = await confirmDialog.ShowAsync();
    
    if (result == ContentDialogResult.Primary)
    {
  // ? 4. If deleting current tab, save it first
        if (tab == MainTabView.SelectedItem)
    {
        SaveCurrentTabItems();
    }
        
        // ? 5. Remove the tab
        MainTabView.TabItems.Remove(tab);
   
        // ? 6. Update previous tab reference
        if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
        {
            _previousTab = selectedTab;
      }
    }
}
```

**���� ����:**
1. ������ ������ Ȯ�� �� �������̸� ���� ���̾�α�
2. ���� ������ ���� Ȯ��
3. Ȯ�� ���̾�α� ǥ�� (������ ���� ����)
4. "����" Ŭ�� �� �� ����
5. ���� ������ �ڵ� ��ȯ

---

## ?? UI ����

### **1. �� ��Ŭ�� �� ���ؽ�Ʈ �޴�**

```
��������������������������������������
�� ?? �̸� ����    ��
��������������������������������������
�� ??? ����     ��
��������������������������������������
```

---

### **2. �̸� ���� ���̾�α�**

```
������������������������������������������������������������������������
�� �� �̸� ����       ��
������������������������������������������������������������������������
�� [�⺻   ]  �� ���� �̸���
�� ������������������������������������������������������������   ��
�� ��  �� �̸� �Է¦�   ��
�� ������������������������������������������������������������   ��
��   ��
��      [Ȯ��]  [���]          ��
������������������������������������������������������������������������
```

---

### **3. ���� Ȯ�� ���̾�α� (������ ���� ��)**

```
������������������������������������������������������������������������
�� �� ���� Ȯ��     ��
������������������������������������������������������������������������
�� �� ���� �����Ͻðڽ��ϱ�?  ��
��            ��
��           [����]  [���]  ��
������������������������������������������������������������������������
```

---

### **4. ���� Ȯ�� ���̾�α� (������ ���� ��) - ?? ���**

```
������������������������������������������������������������������������
�� �� ���� Ȯ��              ��
������������������������������������������������������������������������
�� �� �ǿ��� 5���� �������� �ֽ��ϴ�.��
�� ���� �����ϸ� ��� �����۵�       ��
�� �Բ� �����˴ϴ�.       ��
��            ��
�� ���� �����Ͻðڽ��ϱ�?        ��
�� ��
��           [����]  [���]       ��
������������������������������������������������������������������������
```

**Ư¡:**
- ?? ������ ���� ǥ��
- ?? "�Բ� �����˴ϴ�" ���
- ? �⺻ ��ư�� "���" (�Ǽ� ����)

---

### **5. ������ �� ���� �õ� - ����**

```
������������������������������������������������������������������������
�� ���� �Ұ�     ��
������������������������������������������������������������������������
�� ������ ���� ������ �� �����ϴ�.   ��
��              ��
��         [Ȯ��]   ��
������������������������������������������������������������������������
```

---

## ?? �׽�Ʈ �ó�����

### **Test 1: �� �̸� ����**

```
1. �� ��Ŭ�� ?
2. "�̸� ����" Ŭ�� ?
3. "����" �Է� ?
4. "Ȯ��" Ŭ�� ?
5. �� �̸��� "����"�� ����� ?
```

---

### **Test 2: �� �� ����**

```
1. �� �� ���� (�� 2) ?
2. ������ �߰� ���� ?
3. �� 2 ��Ŭ�� ?
4. "����" Ŭ�� ?
5. "�� ���� �����Ͻðڽ��ϱ�?" ���̾�α� ?
6. "����" Ŭ�� ?
7. �� 2 ���ŵ� ?
```

---

### **Test 3: ������ �ִ� �� ���� - ���**

```
1. �⺻ �ǿ� App1, App2, App3 �߰� ?
2. �⺻ �� ��Ŭ�� ?
3. "����" Ŭ�� ?
4. "�� �ǿ��� 3���� �������� �ֽ��ϴ�..." ���̾�α� ?
5. "���" Ŭ�� ?
6. �� ������ ?

�ٽ� �õ�:
7. �⺻ �� ��Ŭ�� ?
8. "����" Ŭ�� ?
9. "����" Ŭ�� (Ȯ��) ?
10. �⺻ �� ���ŵ� ?
11. App1, App2, App3�� �Բ� ������ ?
```

---

### **Test 4: ������ �� ���� �õ� - ����**

```
1. ���� 1���� ���� (�⺻ ��) ?
2. �⺻ �� ��Ŭ�� ?
3. "����" Ŭ�� ?
4. "������ ���� ������ �� �����ϴ�" ���� ���̾�α� ?
5. "Ȯ��" Ŭ�� ?
6. �� ������ ?
```

---

### **Test 5: ���� �� vs �ٸ� �� ����**

```
��Ȳ:
- �⺻ �� (App1, App2)
- �� 2 (Game1, Game2) �� ���� ���õ�

Test A: �ٸ� �� ���� (�⺻ ��)
1. �� 2 ���� �� ?
2. �⺻ �� ��Ŭ�� ?
3. "����" Ŭ�� ?
4. "�� �ǿ��� 2���� �������� �ֽ��ϴ�..." ?
5. "����" Ŭ�� ?
6. �⺻ �� ���ŵ� ?
7. �� 2 �״�� ���� (Game1, Game2) ?

Test B: ���� �� ���� (�� 2)
1. �� 2 ���� �� ?
2. �� 2 ��Ŭ�� ?
3. "����" Ŭ�� ?
4. "�� �ǿ��� 2���� �������� �ֽ��ϴ�..." ?
5. "����" Ŭ�� ?
6. �� 2 ���ŵ� ?
7. �ڵ����� �ٸ� �� ���õ� ?
```

---

## ?? ��� ���λ���

### **1. ������ ���� Ȯ�� ����**

```csharp
int itemCount = 0;

// Case 1: �ٸ� �� (Tag�� �����)
if (tab.Tag is List<UserControl> items)
{
    itemCount = items.Count;
}
// Case 2: ���� �� (ItemsGridView�� ǥ�� ��)
else if (tab == MainTabView.SelectedItem)
{
    itemCount = ItemsGridView.Items.Count;
}
```

**�� ���� ���:**
1. **�ٸ� ��:** Tag�� ����� ������ ����
2. **���� ��:** ItemsGridView�� ������ ����

---

### **2. ��� �޽��� ����**

```csharp
string message = itemCount > 0
    ? $"�� �ǿ��� {itemCount}���� �������� �ֽ��ϴ�.\n" +
    $"���� �����ϸ� ��� �����۵� �Բ� �����˴ϴ�.\n\n" +
      $"���� �����Ͻðڽ��ϱ�?"
 : "�� ���� �����Ͻðڽ��ϱ�?";
```

**���Ǻ� �޽���:**
- **������ ����:** ���� + ���
- **������ ����:** ������ Ȯ��

---

### **3. �⺻ ��ư ����**

```csharp
DefaultButton = ContentDialogButton.Close  // ? ��Ұ� �⺻
```

**����:**
- �Ǽ��� Enter ������ ����
- ��������� "����" Ŭ�� �ʿ�

---

### **4. ���ؽ�Ʈ �޴� ������**

```csharp
// �̸� ����
Icon = new SymbolIcon(Symbol.Rename)  // ??

// ����
Icon = new SymbolIcon(Symbol.Delete)  // ???
```

**�ð��� ��Ʈ:**
- ���������� ��� ������ ǥ��
- WinUI ǥ�� �ɺ� ���

---

## ?? ����� �ó�����

### **�ó����� 1: ������Ʈ�� �� ����**

```
�ʱ� ����:
[�⺻] [�� 2] [�� 3]

�۾�:
1. �⺻ �� ��Ŭ�� �� �̸� ���� �� "����"
2. �� 2 ��Ŭ�� �� �̸� ���� �� "������"
3. �� 3 ��Ŭ�� �� �̸� ���� �� "����"

���:
[����] [������] [����] ?
```

---

### **�ó����� 2: ���ʿ��� �� ����**

```
�ʱ� ����:
[����] [����] [�ӽ�] [�׽�Ʈ]

�۾�:
1. �ӽ� �� ��Ŭ�� �� ���� (������ ����)
2. "����" Ȯ��
3. �׽�Ʈ �� ��Ŭ�� �� ���� (������ 2��)
4. ��� Ȯ�� �� "����"

���:
[����] [����] ?
```

---

### **�ó����� 3: �Ǽ� ����**

```
��Ȳ:
[����] �ǿ� �߿��� ������Ʈ 20��

�۾�:
1. ���� �� ��Ŭ�� �� ����
2. "�� �ǿ��� 20���� �������� �ֽ��ϴ�..." ??
3. "���" Ŭ�� (�Ǽ� ������)

���:
�� ������ ? (������ ������)
```

---

## ?? �ٸ� �� ��ɰ��� ����

### **���� ���:**
1. ? �� �߰� ("+" ��ư)
2. ? �� ��ȯ (Ŭ��)
3. ? �� �ݱ� (X ��ư)

### **�� ���:**
4. ? �� �̸� ���� (���ؽ�Ʈ �޴�)
5. ? �� ���� (���ؽ�Ʈ �޴�)

### **���� ����:**

```
X ��ư (TabCloseRequested):
- ��� ���� (Ȯ�� ����)
- ������ �� ��ȣ ?

���ؽ�Ʈ �޴� "����":
- Ȯ�� ���̾�α� ?
- ������ ���� ��� ?
- ������ �� ��ȣ ?
```

**������:**
- **X ��ư:** ���� ����
- **���ؽ�Ʈ �޴�:** ������ ���� (��� ����)

---

## ?? ���� ����Ʈ

### **1. ���ٽ����� �� ���� ����**

```csharp
renameItem.Click += (s, e) => RenameTab_Click(tab);
deleteItem.Click += (s, e) => DeleteTab_Click(tab);
```

**����:**
- �� �Ǹ��� ������ �ڵ鷯 �ʿ�
- ���ٷ� tab ���� ĸó

---

### **2. async/await ���̾�α�**

```csharp
private async void RenameTab_Click(...)
{
    var result = await dialog.ShowAsync();
  // ���̾�α� ���� ������ ���
}
```

**����:**
- UI ���� ����
- ����� �ڵ�

---

### **3. ���Ǻ� �޽���**

```csharp
string message = itemCount > 0
    ? $"��� �޽��� (������ {itemCount}��)"
    : "�ܼ� Ȯ�� �޽���";
```

**ȿ��:**
- ��Ȳ�� �´� �޽���
- ����� ģȭ��

---

### **4. ���� �� vs �ٸ� �� ó��**

```csharp
if (tab == MainTabView.SelectedItem)
{
    SaveCurrentTabItems();  // ���� �Ǹ� ����
}
```

**����:**
- �ٸ� ���� �̹� Tag�� �����
- ���� �Ǹ� ItemsGridView�� ����

---

## ? �Ϸ�!

### **�߰��� ���:**
- ? �� ���ؽ�Ʈ �޴� (��Ŭ��)
- ? �̸� ���� ���
- ? ���� ��� (Ȯ�� ���̾�α�)
- ? ������ ���� ���
- ? ������ �� ���� ����

---

### **����� ����:**
- ? `SLauncher/MainWindow.xaml.cs`
  - `InitializeTabs()` ����
  - `MainTabView_AddTabButtonClick()` ����
  - `AttachTabContextMenu()` �� �޼���
  - `RenameTab_Click()` �� �޼���
  - `DeleteTab_Click()` �� �޼���

---

### **�׽�Ʈ:**

```
1. �� ��Ŭ�� ?
2. ���ؽ�Ʈ �޴� ǥ�� ?
3. "�̸� ����" Ŭ�� �� ���̾�α� ?
4. �̸� ���� ���� ?
5. "����" Ŭ�� �� Ȯ�� ���̾�α� ?
6. ������ ���� ǥ�� ?
7. ���� ���� ?
8. ������ �� ���� ���� ?
```

---

## ?? �Ϸ�!

**�� ���ؽ�Ʈ �޴��� ���������� �߰��Ǿ����ϴ�!**

**���� ���� ��Ŭ���Ͽ� �̸��� �����ϰų� ������ �� �ֽ��ϴ�!** ?

**�������� �ִ� ���� �����ϸ� ��� �޽����� ǥ�õ˴ϴ�!** ??

**������ ���� ������ �� �����ϴ�!** ???

**�׽�Ʈ�غ�����!** ??
