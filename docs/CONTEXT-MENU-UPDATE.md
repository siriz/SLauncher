# ?? Context Menu Update - UI Polish

## ? �Ϸ�� �۾�

### 1. **LauncherX �� SLauncher �귣�� ������Ʈ**
��� ���ؽ�Ʈ �޴����� "LauncherX" �� "SLauncher"�� ����

#### Before:
```
Remove website from LauncherX
Remove folder from LauncherX
Remove file from LauncherX
Unlink parent folder from LauncherX
```

#### After:
```
Remove website from SLauncher
Remove folder from SLauncher
Remove file from SLauncher
Unlink parent folder from SLauncher
```

---

### 2. **Remove �ɼ� ���ġ �� ����**

#### Before:
```
[Open]
[Run as admin]
[Open Location]
[Remove from group]
[Remove from LauncherX]  �� �߰��� ��ġ
[Unlink folder]
[Edit item]
```

#### After: ?
```
[Open]
[Run as admin]
[Open Location]
[Edit item]
[Remove from group]
[Unlink folder]
����������������������������������  �� Separator �߰�
[Remove from SLauncher]  �� �� �Ʒ� + ������ ����
```

---

## ?? UI ���� ����

### �޴� ����:
```
���: �Ϲ� �۾�
������ Open
������ Run as administrator (���ϸ�)
������ Open Location

�߰�: ���� �۾�
������ Edit item
������ Remove from group (�׷� �� �����۸�)
������ Unlink folder (��ũ ������)

������������������������������ Separator

�ϴ�: ������ �۾�
������ Remove from SLauncher (������)
```

---

## ?? Remove �ɼ� ����

### XAML ����:
```xml
<MenuFlyoutSeparator />

<MenuFlyoutItem
    x:Name="MenuRemoveOption"
    Click="MenuRemoveOption_Click"
    Foreground="Red"
    Text="Remove from SLauncher">
    <MenuFlyoutItem.Icon>
    <FontIcon Foreground="Red" Glyph="&#xE74D;" />
    </MenuFlyoutItem.Icon>
</MenuFlyoutItem>
```

**�ð��� ȿ��:**
- ? ������ �ؽ�Ʈ
- ? ������ ���� ������
- ? ���м����� �и�
- ? �� �Ʒ� ��ġ

---

## ?? ���� �ؽ�Ʈ ������Ʈ

### C# �ڵ�:
```csharp
private void GridViewTileControl_RightTapped(...)
{
    if (ExecutingPath.StartsWith("http://") || ExecutingPath.StartsWith("https://"))
    {
     // Website
        MenuRemoveOption.Text = "Remove website from SLauncher";
    }
    else if (IsPathDirectory(ExecutingPath))
    {
    // Folder
        MenuRemoveOption.Text = "Remove folder from SLauncher";
    }
    else
    {
 // File
  MenuRemoveOption.Text = "Remove file from SLauncher";
    }
}
```

---

## ?? ����� ���� ���

### Before (ȥ��):
```
- LauncherX vs SLauncher? (�귣�� ����ġ)
- Remove �ɼ��� �߰��� ��ġ (�Ǽ��� Ŭ�� ����)
- �ٸ� �ɼǰ� ���� �� ��
```

### After (��Ȯ): ?
```
- �ϰ��� SLauncher �귣��
- Remove �ɼ��� �� �Ʒ� (����)
- ������ ������ ���輺 ǥ��
- Separator�� ��Ȯ�� ����
```

---

## ?? Before & After ��

### ������Ʈ ���ؽ�Ʈ �޴�:

#### Before:
```
Open website
Open Location
Remove from group
Remove website from LauncherX  �� ����
Unlink folder
Edit item
```

#### After:
```
Open website
Edit item
����������������������������������
Remove website from SLauncher  �� ������
```

---

### ���� ���ؽ�Ʈ �޴�:

#### Before:
```
Open file
Run as administrator
Open file location
Remove from group
Remove file from LauncherX  �� ����
Unlink folder
Edit item
```

#### After:
```
Open file
Run as administrator
Open file location
Edit item
Remove from group
����������������������������������
Remove file from SLauncher  �� ������
```

---

### ���� ���ؽ�Ʈ �޴�:

#### Before:
```
Open folder
Open folder location
Remove from group
Remove folder from LauncherX  �� ����
Unlink folder
Edit item
```

#### After:
```
Open folder
Open folder location
Edit item
Remove from group
����������������������������������
Remove folder from SLauncher  �� ������
```

---

## ?? ���� ���� ����

### 1. XAML ���� ����:
- `GridViewTile.xaml`:
  - MenuRemoveOption�� �� �Ʒ��� �̵�
  - MenuFlyoutSeparator �߰�
  - Foreground="Red" �߰�
  - FontIcon�� Foreground="Red" �߰�
  - Icon�� Delete (&#xE74D;)�� ����

### 2. C# ���� ����:
- `GridViewTile.xaml.cs`:
  - "LauncherX" �� "SLauncher" (3��)
  - MenuRemoveOption.Text ���� ����

---

## ? �׽�Ʈ �ó�����

### Test 1: ���� ��Ŭ��
```
1. ���� ������ ��Ŭ��
2. �޴� Ȯ��:
   ? "Remove file from SLauncher" ǥ��
   ? �� �Ʒ� ��ġ
   ? ������ �ؽ�Ʈ �� ������
   ? Separator�� ���е�
```

### Test 2: ���� ��Ŭ��
```
1. ���� ������ ��Ŭ��
2. �޴� Ȯ��:
   ? "Remove folder from SLauncher" ǥ��
   ? �� �Ʒ� ��ġ
   ? ������ ����
```

### Test 3: ������Ʈ ��Ŭ��
```
1. ������Ʈ ������ ��Ŭ��
2. �޴� Ȯ��:
   ? "Remove website from SLauncher" ǥ��
   ? �� �Ʒ� ��ġ
   ? ������ ����
```

### Test 4: �Ǽ� ����
```
1. ������ ��Ŭ��
2. Remove �ɼ��� �� �Ʒ�:
   ? �Ǽ��� Ŭ���ϱ� �����
   ? �ǵ������� ��ũ���ؾ� ��
   ? ���������� ���
```

---

## ?? UI/UX ��Ģ ����

### 1. **�ϰ��� (Consistency)**
```
? ��� �޴����� "SLauncher" ���
? ������ ���̾ƿ� ����
? �ϰ��� ������ ��Ÿ��
```

### 2. **������ (Safety)**
```
? ������ �۾��� �� �Ʒ�
? Separator�� �ð��� �и�
? ���������� ��� ǥ��
```

### 3. **��Ȯ�� (Clarity)**
```
? "Remove website from SLauncher" (��Ȯ)
? ������ + �ؽ�Ʈ ����
? ���ؽ�Ʈ�� �´� ���� �ؽ�Ʈ
```

### 4. **��뼺 (Usability)**
```
? ���� ���� �ɼ��� ����
? ������ �ɼ��� �Ʒ���
? ������ �׷�ȭ
```

---

## ?? ������ ö��

### Microsoft Fluent Design ��Ģ:
```
1. Light (�淮)
   �� ���ʿ��� �ɼ� �ּ�ȭ

2. Depth (����)
   �� Separator�� ���� ����

3. Motion (������)
   �� (�ڵ� ����)

4. Material (����)
   �� ���������� ���輺 ǥ��

5. Scale (ũ��)
   �� (�⺻ ũ�� ����)
```

---

## ?? ����� ���̵� ������Ʈ

### ���ؽ�Ʈ �޴� ����:
```
1. ������ ��Ŭ��
2. �޴� ����:
 
   [�Ϲ� �۾�]
   - Open: ����/����/������Ʈ ����
   - Run as admin: �����ڷ� ���� (���ϸ�)
   - Open Location: ��ġ ����
   
   [���� �۾�]
   - Edit item: ������ ����
   - Remove from group: �׷쿡�� ����
   
   [������ �۾�] �� ������
   - Remove from SLauncher: ������ ����
```

---

## ? ���� �� ����

### ���� ���:
```
���: 0��
����: 0��
����: ���� ?
```

### ���� �غ�:
```
? UI ���� �Ϸ�
? �귣�� �ϰ��� Ȯ��
? ����� ������ ���
? �׽�Ʈ �Ϸ�
? ���� �ۼ� �Ϸ�
```

---

## ?? �Ϸ�!

**���ؽ�Ʈ �޴� UI ���� �Ϸ�!**

### ���� ����:
- ? SLauncher �귣�� �ϰ���
- ? Remove �ɼ� ������ ��ġ�� �̵�
- ? ������ ������ ���輺 ǥ��
- ? Separator�� �ð��� ����
- ? ����� ���� ���

**��� ��������� ���������� ����Ǿ����ϴ�!** ??
