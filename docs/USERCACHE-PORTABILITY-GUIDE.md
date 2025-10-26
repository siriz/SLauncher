# ?? UserCache ���� �� ���� ���̵�

## ? UserCache ������ 100% ���ͺ��Դϴ�!

UserCache ������ �ٸ� �����/PC�� �����ϸ� **��� ������ �����Ͱ� �״��** �۵��մϴ�.

---

## ?? UserCache ����

```
SLauncher\
������ UserCache\
    ������ userSettings.json   �� ����� ����
    ������ Files\  �� �߰��� ������
    ��   ������ 0.json       (���� ����)
    �� ������ 1.json      (���� ����)
    ��   ������ 2.json    (������Ʈ ����)
  ��   ������ 3\  (�׷�)
    ��  ������ props.json
��       ������ 0.json
  ������ FaviconCache\    �� ������Ʈ ������
        ������ github.com.png
        ������ google.com.png
      ������ intranet.company.com.png
```

---

## ?? ���� �ó�����

### 1?? ���� ���/����

**���:**
```
C:\Tools\SLauncher\UserCache\
�� ����
D:\Backup\SLauncher-UserCache-2025-01-25\
```

**����:**
```
D:\Backup\SLauncher-UserCache-2025-01-25\
�� ����
C:\Tools\SLauncher\UserCache\
```

**���:** ? ��� ������ ������ ������

---

### 2?? ���� PC���� �����ϰ� ���

```
����ũ��: C:\SLauncher\UserCache\
�� ����
��Ʈ��: D:\Apps\SLauncher\UserCache\
�� ����
USB ����̺�: E:\Portable\SLauncher\UserCache\
```

**���:** ? ��� PC���� ������ ȯ��

---

### 3?? �����鿡�� ǥ�� ���� ����

**�غ� (������):**
```
1. SLauncher ��ġ �� ����
2. ȸ�� ������Ʈ �߰�:
 - ��Ʈ���
   - ���� ����
   - ���� ����
3. ���� ����:
   - ��� �ؽ�Ʈ: "ȸ�� ���� ����"
   - ������ ũ��: 1.2
4. UserCache ������ ZIP���� ����
```

**���� (������):**
```
1. SLauncher-v2.1.2-Portable.zip ���� ����
2. UserCache-Template.zip ���� �����Ͽ� UserCache ������ ����
3. SLauncher.exe ����
4. ��� ��� ����! ?
```

---

## ?? ���ԵǴ� ����

### ? userSettings.json
```json
{
  "headerText": "My files, folders, and websites",
  "gridScale": 1.2,
  "useFullscreen": false,
  "gridPosition": "Center"
}
```
**���� ��:** ������ UI ����

---

### ? Files\ ����
```
Files\
������ 0.json    �� ����: C:\Work\document.docx
������ 1.json    �� ����: \\Server\Share\Projects
������ 2.json    �� ������Ʈ: https://intranet.company.com
������ 3\    �� �׷�: "���ã��"
    ������ props.json
������ 0.json
    ������ 1.json
```
**���� ��:** ������ ������ ���

---

### ? FaviconCache\ ����
```
FaviconCache\
������ github.com.png
������ google.com.png
������ intranet.company.com.png
```
**���� ��:** ������ ��� ǥ�� (�ٿ�ε� ���ʿ�)

---

## ?? ���ǻ���

### 1. ���� ��� ����

**���� ���� ���:**
```json
// ? ���� �߻� ����
{
  "executingPath": "C:\\Users\\UserA\\Documents\\work.docx"
}
```
�� User B���� �����ϸ� ��� ����

**���� �ذ�å:**
```json
// ? ��Ʈ��ũ ��� ���
{
  "executingPath": "\\\\Server\\Share\\work.docx"
}

// ? ���� ���� ���
{
"executingPath": "C:\\Public\\Documents\\work.docx"
}
```

---

### 2. ���� ����

**�ó�����:**
```
User A: ������ �������� �߰��� ����
User B: �Ϲ� ����� �� ���� �Ұ�
```

**�ذ�å:**
- ���� ���� ���
- ��Ʈ��ũ ���� ���
- ���� Ȯ��

---

### 3. ����̺� ���� ����

**����:**
```
User A: D:\Projects\...
User B: D: ����̺� ����
```

**�ذ�å:**
- UNC ��� ���: `\\Server\Share\...`
- ��� ��� ��� (������ ���)

---

## ?? ���� ����

### Strategy 1: �ּ� ���� (����)
```
���� ��Ű��:
������ SLauncher-v2.1.2-Portable.zip

�����:
- �ڽ��� ����/���� ���� �߰�
- ����ȭ�� ȯ��
```

---

### Strategy 2: ǥ�� ���� ����
```
���� ��Ű��:
������ SLauncher-v2.1.2-Portable.zip
������ UserCache-Standard\
    ������ userSettings.json        (ǥ�� ����)
 ������ Files\
        ������ 0.json              (ȸ�� ��Ʈ���)
     ������ 1.json   (���� ����)
        ������ 2.json     (���� ����)

�����:
1. SLauncher ���� ����
2. UserCache-Standard ���� (���û���)
3. �߰� ������ ����ȭ
```

---

### Strategy 3: ���� ǥ��ȭ ����
```
���� ��Ű��:
������ SLauncher-Preconfigured.zip
    ������ SLauncher.exe
������ (��� ���� ����)
    ������ UserCache\   (�̸� ������)
        ������ userSettings.json
     ������ Files\
        ������ FaviconCache\

�����:
1. ���� ����
2. ����
3. ��!
```

---

## ?? ���� üũ����Ʈ

### ���� ��� ��:
- [ ] UserCache ���� ��ü ����
- [ ] ��� ��¥ ���
- [ ] �ֱ��� ��� (����)

### �� ���� ��:
- [ ] ���� ��� ������ Ȯ��
- [ ] ��Ʈ��ũ ��� ��� Ȯ��
- [ ] ���� ���� ������ �׽�Ʈ
- [ ] �ٸ� PC���� �׽�Ʈ
- [ ] README �ۼ�

### ���� PC ����ȭ ��:
- [ ] �ֽ� ���� Ȯ��
- [ ] �浹 ������ Ȯ��
- [ ] �ֱ��� ����ȭ ��ȹ

---

## ?? ���� ����

### ���� 1: USB ����̺�� �̵�
```
1. ����ũ�鿡�� SLauncher ��ġ �� ����
2. UserCache ������ USB�� ����
3. ��Ʈ�Ͽ��� SLauncher ��ġ
4. USB�� UserCache�� ��Ʈ�Ͽ� ����
5. ��Ʈ�Ͽ��� SLauncher ���� �� ������ ȯ��!
```

---

### ���� 2: ȸ�� ǥ�� ȯ�� ����
```bash
# ������ �۾�
1. ǥ�� SLauncher ����:
   - ȸ�� ��Ʈ���
   - ���� �ý��� ��ũ
- ���� ����
   - ǥ�� ���� (������ ũ�� ��)

2. UserCache ������ ���� ������ ��ġ:
   \\Server\Software\SLauncher\UserCache-Standard\

# ���� �۾�
1. SLauncher ��ġ
2. ���� ������ UserCache-Standard�� ����
3. �ʿ信 ���� ���� ������ �߰�
```

---

### ���� 3: �� ������Ʈ ����
```
������Ʈ ��:
- ���� ������Ʈ ����
- ���� ���� ��ũ
- ���� ������Ʈ

UserCache ����:
1. ������ ǥ�� UserCache ����
2. �� ���� ������ ��ġ
3. �������� �����Ͽ� ���
4. ���� ������ �߰� ����
```

---

## ? ���

### UserCache�� ������ ���ͺ��մϴ�!

**���� ����:**
- ? �ٸ� ����ڿ���
- ? �ٸ� PC��
- ? USB ����̺꿡
- ? ��Ʈ��ũ ����̺꿡

**�۵� ����:**
- ? ���� ����
- ? ������ ����
- ? ������ ĳ�� ����

**���ǻ���:**
- ?? ���� ��� Ȯ�� (���� vs ����)
- ?? ���� Ȯ��
- ?? ����̺� ���� ���� ���

---

## ?? Best Practice

```
1. ���� ���:
   �� UserCache�� ���������� ���

2. �� ���:
   �� ǥ�� UserCache ���ø� ����
   �� ����ȭ �����ϵ��� �ȳ�

3. ȸ�� ����:
   �� ��Ʈ��ũ ��� ���
   �� ���� ���ҽ��� ����
   �� ���� �߰��� �����Ӱ�
```

**SLauncher�� ���ͺ� ���� ���п� UserCache�� ������ ��𼭵� ������ ȯ���� ����� �� �ֽ��ϴ�!** ??
