# SLauncher ���� ���� �ذ� ���

## ���� ����
.NET 9 SDK�� Windows App SDK 1.6 ���� ȣȯ�� ������ `MrtCore.PriGen.targets` ������ �߻��մϴ�.

## ? �ذ� ���

### ��� 1: Visual Studio���� ���� (���� ?)

1. Visual Studio 2022���� �ַ�� ����
2. ��� �޴�: **���� > ���� ������**
3. ����:
   - **����**: Release
   - **�÷���**: x64
4. **���� > �ַ�� ����** (Ctrl+Shift+B)
5. ���� ����: `SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\`

### ��� 2: ��ġ ���� ���

```cmd
publish-vs.bat
```

�� ��ũ��Ʈ�� Visual Studio�� MSBuild�� �ڵ����� ã�� �����մϴ�.

### ��� 3: .NET 8 SDK ��� (���)

������Ʈ ��Ʈ�� `global.json` ���� ����:

```json
{
  "sdk": {
    "version": "8.0.100",
    "rollForward": "latestFeature"
  }
}
```

�� �� `dotnet build` ����.

## ?? ���� �� ����

���尡 �Ϸ�Ǹ�:

1. **���� ���� ��ġ:**
   ```
 SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\
   ```

2. **���� ��ü�� ZIP���� ����**
   - ���� �̸� ����: `SLauncher-v2.1.2-Portable.zip`

3. **����**
   - ZIP ������ ���� ����̺꿡 ����
   - ����ڵ��� ���� ���� �� `SLauncher.exe` ����

## ?? ��� PC �䱸����

����� PC�� ������ ��ġ�Ǿ� �־�� �մϴ�:

### .NET 8 Desktop Runtime
- �ٿ�ε�: https://dotnet.microsoft.com/download/dotnet/8.0
- ���� ��ũ: https://aka.ms/dotnet/8.0/windowsdesktop-runtime-win-x64.exe

### Windows App SDK Runtime
- �ٿ�ε�: https://aka.ms/windowsappsdk/1.6/latest/windowsappruntimeinstall-x64.exe

## ?? ���� �ذ�

### "�ý��ۿ��� ������ ������ ã�� �� �����ϴ�" ����
�� .NET 8 Desktop Runtime ��ġ

### "���� ������ �� �����ϴ�" ����
�� Windows App SDK Runtime ��ġ

### ���� ������ ���̷����� ������
�� ���̷��� ��� ���� ������ �߰�

## ?? ��

### �ϰ� ��ġ ��ũ��Ʈ ����
����ڵ��� ���� ��Ÿ�� ��ġ ��ũ��Ʈ�� �Բ� �����ϸ� ���մϴ�:

```batch
@echo off
echo .NET 8 Desktop Runtime ��ġ ��...
start /wait windowsdesktop-runtime-win-x64.exe /install /quiet /norestart

echo Windows App SDK Runtime ��ġ ��...
start /wait windowsappruntimeinstall-x64.exe

echo ��ġ �Ϸ�! SLauncher�� �����ϼ���.
pause
```

### ���� ��Ű�� ����
```
SLauncher-Portable/
������ SLauncher.exe
������ (��Ÿ DLL ���ϵ�)
������ Resources/
������ README.txt (��� ���)
������ Runtimes/
    ������ install-runtime.bat
    ������ windowsdesktop-runtime-win-x64.exe
    ������ windowsappruntimeinstall-x64.exe
```
