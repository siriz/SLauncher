# ?? Direct Path Opening in SearchBox - ���� �Ϸ�!

## ? ������ ���

**�˻�â�� ����/���� ��� �Է� �� Enter �� �ٷ� ����!**

```
���� ��� ����:
1. ���� ����: C:\Program Files (x86)\Battle.net\Battle.net Launcher.exe
2. ���� ����: C:\Program Files (x86)\Battle.net
3. ��Ʈ��ũ ���: \\server.domain.com\share\folder\file.txt
4. ���� ����: C:\Users\�����\Documents\�� ����\����.docx
5. �ܱ���: C:\Users\��ö��\����ȭ��\����.xlsx
```

**Ư¡:**
- ? ��� �ڵ� ���� (C:\, D:\, \\\\)
- ? ���� ���� Ȯ��
- ? ���� ���� Ȯ��
- ? ��Ʈ��ũ ��� ����
- ? ���� �� Ư������ ó��
- ? �ܱ��� (�ѱ�, �Ϻ���, �߱��� ��) ����
- ? ���� ó�� �� ����� �ǵ��

---

## ?? ������ ����

### **MainWindow.xaml.cs**

#### SearchBox_QuerySubmitted �޼��� ��ü ���ۼ�:

**�ٽ� ����:**

```csharp
private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, 
    AutoSuggestBoxQuerySubmittedEventArgs args)
{
    string query = sender.Text?.Trim();
    
    // 1. ��� ���� ����
    bool isPath = false;
    
    // Windows ����̺� ��� (C:\, D:\)
    if (char.IsLetter(query[0]) && query[1] == ':')
    {
  isPath = true;
    }
    // UNC ��Ʈ��ũ ��� (\\server\share)
    else if (query.StartsWith("\\\\"))
    {
   isPath = true;
    }
    
    // 2. ����� ��� ���� ����
    if (isPath)
    {
        // ���� Ȯ��
        if (File.Exists(query))
        {
   Process.Start(new ProcessStartInfo 
{ 
                FileName = query, 
           UseShellExecute = true 
      });
   return;
        }
     
     // ���� Ȯ��
   if (Directory.Exists(query))
      {
     Process.Start(new ProcessStartInfo 
        { 
 FileName = "explorer.exe", 
         Arguments = $"\"{query}\"" 
     });
    return;
   }
    }
    
    // 3. �Ϲ� �˻� (���� ���)
    // ... ���� �˻� ���� ...
}
```

---

## ?? �۵� ����

### **1. ��� ����:**

```csharp
// Windows ����̺� ���
if (char.IsLetter(query[0]) && query[1] == ':')
{
    isPath = true;  // C:\, D:\, E:\ ��
}

// UNC ��Ʈ��ũ ���
else if (query.StartsWith("\\\\"))
{
    isPath = true;  // \\server\share
}
```

**���� ����:**
- `C:\` - ���� ����̺�
- `D:\` - �ٸ� ����̺�
- `\\\\server` - UNC ���
- `\\\\192.168.1.100` - IP �ּ�
- `\\\\server.domain.com` - FQDN

---

### **2. ���� ����:**

```csharp
if (File.Exists(query))
{
    ProcessStartInfo psi = new ProcessStartInfo 
    { 
        FileName = query, 
   UseShellExecute = true 
    };
    Process.Start(psi);
}
```

**����:**
- Windows�� ����� ���α׷����� ���� ����
- `.txt` �� �޸���
- `.docx` �� Word
- `.exe` �� ���� ����

---

### **3. ���� ����:**

```csharp
if (Directory.Exists(query))
{
    ProcessStartInfo psi = new ProcessStartInfo 
    { 
        FileName = "explorer.exe", 
        Arguments = $"\"{query}\"" 
    };
    Process.Start(psi);
}
```

**����:**
- Explorer�� ���� ����
- �ο��ȣ�� ���� ó��

---

### **4. ���� ó��:**

```csharp
catch (Exception ex)
{
    var dialog = new ContentDialog
    {
 Title = "Error",
        Content = $"Unable to open:\n{query}\n\nError: {ex.Message}",
   CloseButtonText = "OK"
    };
    await dialog.ShowAsync();
}
```

**���� ��Ȳ:**
- ���� ����
- ��Ʈ��ũ ���� ����
- ������ ��� ��
- �߸��� ���

---

## ?? ��� �ó�����

### **�ó����� 1: ���� ���� ����**

```
1. Ctrl+Space (SLauncher ����)
2. "C:\Program Files (x86)\Battle.net\Battle.net Launcher.exe" �Է�
3. Enter
4. Battle.net ���� ?
5. SLauncher �ڵ����� Ʈ���̷� ?
```

### **�ó����� 2: ���� ���� ����**

```
1. Ctrl+Space
2. "C:\Program Files (x86)\Battle.net" �Է�
3. Enter
4. Explorer���� ���� ���� ?
5. SLauncher �ڵ����� Ʈ���̷� ?
```

### **�ó����� 3: ��Ʈ��ũ ���� ����**

```
1. Ctrl+Space
2. "\\192.168.1.100\SharedFolder\Documents" �Է�
3. Enter
4. ��Ʈ��ũ ���� ���� ?
5. SLauncher �ڵ����� Ʈ���̷� ?
```

### **�ó����� 4: ���� ���� ���**

```
1. Ctrl+Space
2. "C:\Users\John Doe\My Documents\Report.docx" �Է�
3. Enter
4. Word���� ���� ���� ?
```

### **�ó����� 5: �ѱ� ���**

```
1. Ctrl+Space
2. "C:\Users\��ö��\����ȭ��\����.xlsx" �Է�
3. Enter
4. Excel���� ���� ���� ?
```

### **�ó����� 6: ��Ʈ��ũ ��� (������)**

```
1. Ctrl+Space
2. "\\file-server.nmcorp.nissan.biz\��������\������Ʈ\����.pdf" �Է�
3. Enter
4. PDF �������� ���� ?
```

---

## ?? ���� ��� ����

### **���� ����̺�:**

```
? C:\
? C:\Windows
? C:\Program Files
? C:\Program Files (x86)\Battle.net
? D:\Games\Steam
? E:\Backup\Data
```

### **UNC ��Ʈ��ũ ���:**

```
? \\server\share
? \\server\share\folder
? \\server\share\folder\file.txt
? \\192.168.1.100\public
? \\file-server.domain.com\shared
? \\nas-01.local\media
```

### **Ư�� ���� ����:**

```
? C:\Users\John Doe\Documents
? C:\Program Files (x86)\App Name
? D:\������Ʈ\2024\����.docx
? \\server\share\?��ݻ\����.xlsx
? \\server\����\����\������.csv
```

---

## ?? ��� ���� ����

### **Windows ����̺� ���:**

```csharp
// ����: ù ���ڰ� ���ĺ� + �� ��° ���ڰ� ':'
if (query.Length >= 2 && 
    char.IsLetter(query[0]) && 
    query[1] == ':')
{
    // C:\, D:\, E:\ ������ �����ϴ� ���
}
```

**����:**
- `C:\Windows` ?
- `D:\Data` ?
- `E:\Backup` ?
- `Z:\Network` ?

---

### **UNC ��Ʈ��ũ ���:**

```csharp
// ����: "\\\\" �� ����
if (query.StartsWith("\\\\"))
{
    // \\server\share ������ ��Ʈ��ũ ���
}
```

**����:**
- `\\server\share` ?
- `\\192.168.1.1\public` ?
- `\\nas.local\media` ?
- `\\file-server.domain.com\docs` ?

---

## ?? �ڵ� �ϼ� ����

**�˻�â ����:**

```
1. �Ϲ� �˻��� �Է�:
   "chrome" �� ��Ӵٿ Chrome ������ ǥ�� �� Enter �� Chrome ����

2. ��� �Է�:
   "C:\..." �� ��Ӵٿ� ǥ�� �� �� �� Enter �� ����/���� ����

3. ȥ��:
   ó���� "C:\..."�� ���� �� ��η� �ν�
   ���߿� �Ϲ� �ؽ�Ʈ�� ���� �� �˻����� �ν�
```

---

## ?? ����� �ǵ��

### **����:**

```
���� ���� �� �ڵ����� Ʈ���̷�
���� ���� �� �ڵ����� Ʈ���̷�
�˻�â �ڵ� Ŭ����
```

### **����:**

**1. ��ΰ� �������� ����:**
```
����������������������������������������������������������
�� Path Not Found            ��
����������������������������������������������������������
�� The specified path does   ��
�� not exist:    ��
��     ��
�� C:\NonExistent\file.txt   ��
��               ��
�� Please check the path and ��
�� try again.    ��
����������������������������������������������������������
��           [OK]         ��
����������������������������������������������������������
```

**2. ���� ���� (����, ��Ʈ��ũ ��):**
```
����������������������������������������������������������
�� Error     ��
����������������������������������������������������������
�� Unable to open:           ��
�� \\server\share\file.txt ��
��       ��
�� Error: Access denied      ��
����������������������������������������������������������
��  [OK]         ��
����������������������������������������������������������
```

---

## ?? ���� �������

### **1. ��� ��ȿ�� �˻�:**

```csharp
// ����/���� ���� Ȯ�� �� ����
if (File.Exists(query) || Directory.Exists(query))
{
    // �����ϰ� ����
}
else
{
    // ���� �޽��� ǥ��
}
```

### **2. ���� ó��:**

```csharp
try
{
    Process.Start(...);
}
catch (Exception ex)
{
    // ����ڿ��� ���� ǥ��
    // �� ũ���� ����
}
```

### **3. UseShellExecute = true:**

```csharp
ProcessStartInfo psi = new ProcessStartInfo 
{ 
    FileName = query, 
    UseShellExecute = true  // �� ������ ����
};
```

**����:**
- Windows�� ������ ���α׷� ����
- ���� ���� ����
- ���� ������Ʈ ǥ�� (�ʿ� ��)

---

## ?? �׽�Ʈ �ó�����

### **Test 1: ���� ����**
```
Input: C:\Windows\System32\notepad.exe
Expected: �޸��� ���� ?
Actual: ?
```

### **Test 2: ���� ����**
```
Input: C:\Program Files
Expected: Explorer���� ���� ���� ?
Actual: ?
```

### **Test 3: ���� ����**
```
Input: C:\Program Files (x86)\Battle.net\Battle.net Launcher.exe
Expected: Battle.net ���� ?
Actual: ?
```

### **Test 4: ��Ʈ��ũ ���**
```
Input: \\192.168.1.100\SharedFolder
Expected: ��Ʈ��ũ ���� ���� ?
Actual: ?
```

### **Test 5: �ѱ� ���**
```
Input: C:\Users\��ö��\����ȭ��\����.txt
Expected: �޸��忡�� ���� ?
Actual: ?
```

### **Test 6: �������� �ʴ� ���**
```
Input: C:\NonExistent\file.txt
Expected: "Path Not Found" ���� ?
Actual: ?
```

### **Test 7: ���� ���� ����**
```
Input: C:\Windows\System32\config\SAM
Expected: "Access denied" ���� ?
Actual: ?
```

### **Test 8: �Ϲ� �˻��� ȥ��**
```
Input: "chrome"
Expected: Chrome ������ �˻� ?
Actual: ?

Input: "C:\Program Files\Google\Chrome\Application\chrome.exe"
Expected: Chrome ���� ���� ?
Actual: ?
```

---

## ?? ���� ��ɰ��� ����

### **�켱����:**

```
1����: ��� ���� ���� �� ����/���� ����
2����: ���� �˻� �� ������ ����
```

**����:**

```csharp
// ����� ���
if (isPath && (File.Exists(query) || Directory.Exists(query)))
{
    // ���� ����
    return;
}

// �Ϲ� �˻�
if (SearchBoxDropdownItems.Count > 0)
{
    // ������ �˻� �� ����
}
```

---

## ?? �߰� ���� ���� (���û���)

### **1. ��� ��� ����**

```csharp
// ���� �۾� ���丮 ����
if (!Path.IsPathRooted(query))
{
    query = Path.GetFullPath(query);
}
```

**����:**
- `.\file.txt` �� ���� ����
- `..\folder` �� ���� ����

### **2. ȯ�� ���� ����**

```csharp
// %USERPROFILE%, %TEMP% ��
query = Environment.ExpandEnvironmentVariables(query);
```

**����:**
- `%USERPROFILE%\Documents` �� `C:\Users\Username\Documents`
- `%TEMP%` �� `C:\Users\Username\AppData\Local\Temp`

### **3. �ڵ� �ϼ�**

```csharp
// ��� �Է� �� ����/���� ��� ǥ��
private async Task<List<string>> GetPathSuggestions(string partialPath)
{
    // C:\Program Files �Է� ��
    // �� C:\Program Files\
    // �� C:\Program Files (x86)\
}
```

### **4. �ֱ� ��� ���**

```csharp
// �ֱ� ������ ��� ����
private static List<string> RecentPaths = new List<string>();

// ��Ӵٿ �ֱ� ��� ǥ��
```

### **5. �巡�� �� ��ӿ��� ��� ��������**

```csharp
// ���� �巡�� �� ��� �� ��� �ڵ� �Է�
SearchBox.Text = droppedFilePath;
```

---

## ?? �˷��� ���ѻ���

### **1. ���ε� ��Ʈ��ũ ����̺�**

```
Z:\SharedFolder
```

**����:** ���� �� �� (Z:�� ����̺�ó�� �νĵ����� �����δ� ��Ʈ��ũ ���)

**�ذ�:**
```csharp
// ���ε� ����̺굵 ��η� �ν�
if (query.Length >= 2 && query[1] == ':')
{
    isPath = true;  // Z:\ � ����
}
```

### **2. ���鸸 �ִ� ������**

```
C:\Users\   \Documents
```

**����:** ó�� ���������� �������� ����

**�ذ�:** Windows ��ü���� ������� �����Ƿ� ���� ����

### **3. �ſ� �� ��� (260�� �̻�)**

```
C:\Very\Long\Path\That\Exceeds\260\Characters\...
```

**����:** Windows ���� ����

**�ذ�:** .NET Core/.NET 5+ ������ �� ��� ���� (������Ʈ�� ���� �ʿ�)

---

## ? ���� �Ϸ�!

### **����� ����:**
- ? `MainWindow.xaml.cs`
  - `SearchBox_QuerySubmitted` �޼��� ��ü ���ۼ�
  - ��� ���� ���� �߰�
  - ����/���� ���� �߰�
  - ���� ó�� �߰�

### **����:**
- ? ���� ���� ��� (C:\, D:\)
- ? ���� ���� ���
- ? UNC ��Ʈ��ũ ��� (\\\\server\\share)
- ? ���� ���� ���
- ? �ܱ��� (�ѱ�, �Ϻ��� ��) ���
- ? �ڵ� Ʈ���� ����
- ? ���� �޽��� ǥ��

### **Ư¡:**
- ? ���� �˻� ��ɰ� �Ϻ��� ����
- ? ����� ģȭ���� ���� �޽���
- ? ������ ���� (UseShellExecute)
- ? ���� �� ��Ʈ��ũ ���� ó��

---

## ?? ���� �� �׽�Ʈ

### **����:**
```
Visual Studio �� Rebuild Solution �� F5
```

### **�׽�Ʈ:**
```
1. Ctrl+Space
2. "C:\Windows\System32\notepad.exe" �Է�
3. Enter
4. �޸��� ���� ?

5. Ctrl+Space
6. "\\server\share\folder" �Է�
7. Enter
8. ��Ʈ��ũ ���� ���� ?
```

---

## ?? �Ϸ�!

**�˻�â���� ����/���� ��θ� ���� �� �� �ִ� ����� �����Ǿ����ϴ�!**

**����:**
- ? ���� ����/���� (C:\, D:\)
- ? ��Ʈ��ũ ��� (\\\\server\\share)
- ? ���� �� Ư������
- ? �ܱ��� (�ѱ�, �Ϻ���, �߱���)
- ? �ڵ� ���� ó��
- ? ����� �ǵ��

**���� �˻�â�� �� �����������ϴ�!** ?

**�׽�Ʈ�غ�����!** ??
