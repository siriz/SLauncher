# ?? Visual Studio ���� ���̵�

## ?? ���� ���

### 1�ܰ�: Visual Studio ����
```
LauncherX-master.sln ����Ŭ��
```

### 2�ܰ�: ���� ���� ����
1. ��� ���ٿ���:
   - **����**: `Release` ����
   - **�÷���**: `x64` ����

   ```
   Debug ��  ��  Release
   Any CPU ��  ��  x64
   ```

### 3�ܰ�: ���� ����
**��� A: �޴�**
```
���� > �ַ�� ���� (Ctrl+Shift+B)
```

**��� B: �ַ�� Ž����**
```
�ַ�� ��Ŭ�� > ����
```

### 4�ܰ�: ���� �Ϸ� Ȯ��
��� â�� ���� �޽����� ǥ�õǾ�� �մϴ�:
```
========== ����: ���� 2, ���� 0, �ֽ� 0, �ǳʶٱ� 0 ==========
```

---

## ?? ���� ���� ��ġ

���� �Ϸ� ��:
```
D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\
```

### ���� ����:
```
net8.0-windows10.0.22621.0/
������ SLauncher.exe      �� ���� ����
������ SLauncher.dll
������ Resources/              �� ���ҽ� ����
��   ������ icon.ico
��   ������ icon.png
��   ������ LinkedFolderIcon.png
��   ������ websitePlaceholder.png
������ Microsoft.*.dll         �� Windows App SDK
������ (��Ÿ DLL ���ϵ�)
```

**���� ũ��:** 10-30MB

---

## ?? ���� �غ�

### 1. ���� ����
```
1. net8.0-windows10.0.22621.0 ���� ����
2. ��Ŭ�� > ������ > ����(ZIP) ����
3. �̸� ����: SLauncher-v2.1.2-Portable.zip
```

### 2. ����
```
1. ZIP ������ ȸ�� ���� ������ ����
2. ����ڵ鿡�� �˸�
```

---

## ?? ��� PC �䱸����

����� PC�� ���� ��Ÿ���� **�� ����** ��ġ�Ǿ�� �մϴ�:

### .NET 8 Desktop Runtime
**�ٿ�ε�:**
- https://dotnet.microsoft.com/download/dotnet/8.0
- ���� ��ũ: https://aka.ms/dotnet/8.0/windowsdesktop-runtime-win-x64.exe

**��ġ ��� (�ڵ� ��ġ��):**
```cmd
windowsdesktop-runtime-win-x64.exe /install /quiet /norestart
```

### Windows App SDK Runtime
**�ٿ�ε�:**
- https://aka.ms/windowsappsdk/1.6/latest/windowsappruntimeinstall-x64.exe

**��ġ ���:**
```cmd
windowsappruntimeinstall-x64.exe
```

---

## ?? ���� üũ����Ʈ

���� �� Ȯ�λ���:

- [ ] Release ���� ����
- [ ] x64 �÷������� ����
- [ ] ���� ���� Ȯ��
- [ ] ���� ���� Ȯ�� (net8.0-windows10.0.22621.0)
- [ ] SLauncher.exe ���� �׽�Ʈ
- [ ] Resources ���� ���� Ȯ��
- [ ] ZIP ���� ����
- [ ] ���� ũ�� Ȯ�� (10-30MB)
- [ ] ��Ÿ�� ��ġ ���̵� �غ�

---

## ?? ���� �ذ�

### "���� ����" ����
**�ذ�å:**
```
1. ��� â Ȯ�� (���� > ���)
2. ���� �޽��� Ȯ��
3. �ַ�� ����: ���� > �ַ�� ����
4. �ٽ� ����
```

### ".NET 8 SDK not found" ����
**�ذ�å:**
```
1. Visual Studio Installer ����
2. "����" Ŭ��
3. ".NET ����ũ�� ����" ��ũ�ε� Ȯ��
4. ".NET 8.0 Runtime" Ȯ��
5. ���� ����
```

### "Windows App SDK" ���� ����
**�ذ�å:**
```
1. NuGet ��Ű�� ����:
   �ַ�� ��Ŭ�� > NuGet ��Ű�� ����
2. �ٽ� ����
```

---

## ?? ���� ��

### ��Ÿ�� �ϰ� ��ġ ��ũ��Ʈ
����ڵ��� ���� ��Ÿ�� ��ġ ��ũ��Ʈ�� �Բ� ����:

**install-runtime.bat:**
```batch
@echo off
echo SLauncher ��Ÿ�� ��ġ ��...
echo.

echo [1/2] .NET 8 Desktop Runtime ��ġ...
start /wait windowsdesktop-runtime-win-x64.exe /install /quiet /norestart

echo [2/2] Windows App SDK Runtime ��ġ...
start /wait windowsappruntimeinstall-x64.exe

echo.
echo ? ��ġ �Ϸ�!
echo SLauncher.exe�� �����ϼ���.
pause
```

### ���� ��Ű�� ����
```
SLauncher-Deployment/
������ SLauncher-v2.1.2-Portable.zip    �� ���α׷�
������ Runtimes/    �� ��Ÿ�� (����)
��   ������ windowsdesktop-runtime-win-x64.exe
��   ������ windowsappruntimeinstall-x64.exe
��   ������ install-runtime.bat
������ README.txt            �� ��� ����
```

---

## ?? �Ϸ�!

���尡 �����ϸ�:
1. ? ���� ������ ���� ���� ����
2. ? ũ��: 10-30MB (Framework-Dependent)
3. ? ��� ���� ����

**���� �ܰ�:** ZIP ������ ����� �����ϼ���! ??
