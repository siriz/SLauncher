# SLauncher ��Ÿ�� ��ġ ���̵�

## ȸ�� PC�� ��Ÿ�� ��ġ (������ ���� �ʿ�)

���ͺ� ����(�淮)�� ����Ϸ��� �� PC�� ��Ÿ���� �� ���� ��ġ�ϸ� �˴ϴ�.

### 1. .NET 8 Desktop Runtime ��ġ

**�ٿ�ε�:**
- ���� ��ũ: https://dotnet.microsoft.com/download/dotnet/8.0
- �Ǵ� ���� �ٿ�ε�: https://aka.ms/dotnet/8.0/windowsdesktop-runtime-win-x64.exe

**��ġ ���:**
```cmd
windowsdesktop-runtime-win-x64.exe /install /quiet /norestart
```

### 2. Windows App SDK Runtime ��ġ

**�ٿ�ε�:**
- ���� �ٿ�ε�: https://aka.ms/windowsappsdk/1.6/latest/windowsappruntimeinstall-x64.exe

**��ġ ���:**
```cmd
windowsappruntimeinstall-x64.exe
```

---

## ���� �ɼ� ��

### �ɼ� 1: ���ͺ� ���� (����)
- **����:** `publish-portable.bat` ����
- **�뷮:** �� 10-30MB
- **����:** �ſ� ������, ������Ʈ ����
- **����:** ��Ÿ�� ���� ��ġ �ʿ�
- **�뵵:** ȸ�� ���� ���� ����

### �ɼ� 2: Self-Contained ����
- **����:** `publish-selfcontained.bat` ����
- **�뷮:** �� 100-150MB
- **����:** ��ġ ���� �ٷ� ����
- **����:** �뷮�� ŭ
- **�뵵:** �ܺ� ���� �Ǵ� �ӽ� ���

---

## �ϰ� ��ġ ��ũ��Ʈ (�����ڿ�)

ȸ�� PC�� ��Ÿ���� �ϰ� ��ġ�Ϸ���:

```batch
@echo off
echo .NET 8 Desktop Runtime ��ġ ��...
windowsdesktop-runtime-win-x64.exe /install /quiet /norestart

echo Windows App SDK Runtime ��ġ ��...
windowsappruntimeinstall-x64.exe

echo ��ġ �Ϸ�!
pause
```

---

## ���� �� üũ����Ʈ

- [ ] ��� PC: Windows 11 64��Ʈ
- [ ] ��Ÿ�� ��ġ �Ϸ� (���ͺ� ���� ��� ��)
- [ ] publish ���� ��ü ����
- [ ] ���� ���� Ȯ��
- [ ] ��ȭ�� ���� ���� (�ʿ� ��)

---

## ���� �ذ�

### "��Ÿ���� ã�� �� �����ϴ�" ����
�� .NET 8 Desktop Runtime ��ġ

### "Windows App SDK�� ã�� �� �����ϴ�" ����
�� Windows App SDK Runtime ��ġ

### ������ �� ��
�� Windows 11 64��Ʈ Ȯ��
�� ���̷��� ��� ���� ���� Ȯ��
