# ?? System.Drawing.Common ���� ȣȯ�� ���� �ذ�

## ? �߰� ���� �߰�

### ����:
```
? System.Drawing.Common 9.0.10 ��ġ �Ŀ��� �� ���� �� ��
? ����: 0xc000027b (STATUS_DLL_NOT_FOUND)
? ���� �浹 ��� �߻�
```

---

## ?? �ٺ� ����

### ���� 1: .NET ���� ����ġ
```
������Ʈ: .NET 8.0 (net8.0-windows10.0.22621.0)
System.Drawing.Common: 9.0.10 (.NET 9 ����)
�� ȣȯ���� ����!
```

### ���� 2: ���Ӽ� �浹
```
System.Collections.Immutable:
- .NET 8: Version 8.0.0.0
- System.Drawing.Common 9.0: Version 9.0.0.0
�� ���� �浹!

System.Reflection.Metadata:
- .NET 8: Version 8.0.0.0
- System.Drawing.Common 9.0: Version 9.0.0.0
�� ���� �浹!
```

### ���� ��� �α�:
```
warning MSB3277: 
�ذ��� �� ���� "System.Collections.Immutable"�� �ٸ� ���� �� �浹�� �߰ߵǾ����ϴ�.
"System.Collections.Immutable, Version=8.0.0.0"��(��) "Version=9.0.0.0" ���̿� �浹�� �߻��߽��ϴ�.
```

---

## ? �ذ� ���

### 1. System.Drawing.Common �ٿ�׷��̵�

**�߸��� ���� (9.0.10):**
```bash
dotnet add package System.Drawing.Common
# �� �ֽ� ���� 9.0.10 ��ġ (NET 9 ����)
```

**�ùٸ� ���� (8.0.11):**
```bash
# ���� ��Ű�� ����
dotnet remove package System.Drawing.Common

# .NET 8 ȣȯ ���� ��ġ
dotnet add package System.Drawing.Common --version 8.0.11
```

---

## ?? ���� ��Ű�� ����

### SLauncher.csproj
```xml
<ItemGroup>
  <!-- WinUI 3 Core -->
  <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
  <PackageReference Include="WinUIEx" Version="2.5.0" />
  
  <!-- UI Components -->
  <PackageReference Include="CommunityToolkit.WinUI.Controls.SettingsControls" Version="8.1.240916" />
  <PackageReference Include="CommunityToolkit.WinUI.Extensions" Version="8.1.240916" />
  
  <!-- System Libraries -->
  <PackageReference Include="System.Drawing.Common" Version="8.0.11" />  �� 8.0.11 �ʼ�!
  <PackageReference Include="System.Text.Json" Version="9.0.0" />
  
  <!-- Build Tools -->
  <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.26100.1742" />
</ItemGroup>
```

### ���� ȣȯ�� ���̺�

| ��Ű�� | .NET 8 ���� | .NET 9 ���� | SLauncher ��� |
|--------|------------|------------|---------------|
| System.Drawing.Common | ? **8.0.11** | 9.0.10 | **8.0.11** |
| System.Text.Json | 8.0.x | ? **9.0.0** | 9.0.0 (ȣȯ��) |
| Microsoft.Win32.SystemEvents | ? **8.0.0** | 9.0.0 | 8.0.0 (�ڵ�) |

---

## ?? ���� Ȯ�� ���

### 1. ��ġ�� ��Ű�� Ȯ��
```bash
cd "D:\Works\Playground\C#\SLauncher\SLauncher"
dotnet list package
```

**��� ����:**
```
�ֻ��� ��Ű��
   [net8.0-windows10.0.22621.0]: 
   ��Ű��   ��û�� �׸�    �ذ��  
   -----------------------------------------------------------  --------  -------
   CommunityToolkit.WinUI.Controls.SettingsControls          8.1.240916  8.1.240916
   CommunityToolkit.WinUI.Extensions   8.1.240916  8.1.240916
   Microsoft.Windows.SDK.BuildTools              10.0.26100  10.0.26100.1742
   Microsoft.WindowsAppSDK    1.6.241114  1.6.241114003
   System.Drawing.Common           8.0.11      8.0.11  �� Ȯ��!
   System.Text.Json           9.0.09.0.0
   WinUIEx              2.5.0   2.5.0
```

### 2. ���Ӽ� Ʈ�� Ȯ��
```bash
dotnet list package --include-transitive | findstr "System.Drawing"
```

**���:**
```
System.Drawing.Common 8.0.11 8.0.11
Microsoft.Win32.SystemEvents           (����)      8.0.0
```

---

## ?? .NET ������ ���ǻ���

### .NET 8 ������Ʈ
```xml
<TargetFramework>net8.0-windows10.0.22621.0</TargetFramework>
```
**��� ���� ��Ű��:**
- ? System.Drawing.Common **8.0.x**
- ? System.Drawing.Common 9.0.x (ȣȯ �� ��)

### .NET 9 ������Ʈ
```xml
<TargetFramework>net9.0-windows10.0.22621.0</TargetFramework>
```
**��� ���� ��Ű��:**
- ? System.Drawing.Common **9.0.x**
- ? System.Drawing.Common 8.0.x (���� ȣȯ)

---

## ?? ����� ��

### 1. ���� ���� Ȱ��ȭ
```
Visual Studio:
1. Debug �� Windows �� Exception Settings (Ctrl+Alt+E)
2. "Common Language Runtime Exceptions" üũ
3. �� ���� �� ��Ȯ�� ���� ��ġ Ȯ��
```

### 2. Fusion Log Ȱ��ȭ
```
������Ʈ��:
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Fusion
- ForceLog = 1 (DWORD)
- LogFailures = 1 (DWORD)
- LogPath = "C:\FusionLogs"
```

### 3. DLL ���Ӽ� Ȯ��
```powershell
# Dependencies.exe �ٿ�ε�
# https://github.com/lucasg/Dependencies
Dependencies.exe "D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\SLauncher.exe"
```

---

## ?? ��� ����

### 1. ������Ʈ ���Ͽ� ����� ���� ����
```xml
<ItemGroup>
  <!-- .NET 8 ȣȯ ������ ��� -->
  <PackageReference Include="System.Drawing.Common" Version="8.0.11" />
  
  <!-- ���� �������� �������� ���� ���� -->
  <!-- <PackageReference Include="System.Drawing.Common" Version="[8.0.11,9.0.0)" /> -->
</ItemGroup>
```

### 2. Global.json ���
```json
{
  "sdk": {
    "version": "8.0.404",
    "rollForward": "latestMinor"
  },
  "msbuild-sdks": {
    "Microsoft.Build.NoTargets": "3.7.0"
  }
}
```

### 3. Directory.Build.props ���
```xml
<!-- Directory.Build.props -->
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
</Project>

<!-- Directory.Packages.props -->
<Project>
  <ItemGroup>
  <PackageVersion Include="System.Drawing.Common" Version="8.0.11" />
  </ItemGroup>
</Project>
```

---

## ?? üũ����Ʈ

### ��ġ �� Ȯ��
```
? .NET SDK ���� Ȯ�� (dotnet --version)
? ������Ʈ TargetFramework Ȯ��
? ��Ű�� ���� ȣȯ�� Ȯ��
```

### ��ġ �� Ȯ��
```
? dotnet list package ����
? ���� ��� Ȯ��
? �� ���� �׽�Ʈ
```

### ���� �߻� ��
```
? ��Ű�� ĳ�� Ŭ���� (dotnet nuget locals all --clear)
? obj/bin ���� ����
? dotnet restore �����
? Visual Studio �����
```

---

## ? ���� Ȯ��

### Before:
```
? System.Drawing.Common 9.0.10 (NET 9)
? .NET 8 ������Ʈ�� ȣȯ �� ��
? ���� �浹 ���
? �� ���� ����
```

### After:
```
? System.Drawing.Common 8.0.11 (NET 8)
? .NET 8 ������Ʈ�� �Ϻ� ȣȯ
? ���� �浹 ����
? ���� ����
? �� ���� ���� ����
```

---

## ?? �ٽ� ����

### 1. **��Ű�� ���� ������ �߿伺**
```
�ֽ� ���� �� �ּ��� ����
������Ʈ .NET ������ ȣȯ�Ǵ� ��Ű�� ���� ���� �ʼ�
```

### 2. **����� ���� ����**
```bash
# ���� ��
dotnet add package System.Drawing.Common

# ���� ��
dotnet add package System.Drawing.Common --version 8.0.11
```

### 3. **���� ��� ���� ����**
```
���� ������ ������ ��ȣ
Ư�� ���� �浹 ���� �ݵ�� �ذ�
```

---

## ?? ���� �ܰ�

### 1. �� ���� �׽�Ʈ
```
1. Visual Studio���� F5 (����� ����)
2. �������� ���������� �ε�Ǵ��� Ȯ��
3. ����/���� �߰� ��� �׽�Ʈ
```

### 2. Release ���� �׽�Ʈ
```bash
dotnet build --configuration Release
cd bin\x64\Release\net8.0-windows10.0.22621.0\win-x64
SLauncher.exe
```

### 3. ���� �غ�
```
? System.Drawing.Common 8.0.11 ���� Ȯ��
? Microsoft.Win32.SystemEvents 8.0.0 ���� Ȯ��
? ��� ���Ӽ� DLL ���� Ȯ��
```

---

## ?? ���� �ڷ�

### Microsoft ����
- [System.Drawing.Common ���� ȣȯ��](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only)
- [NuGet ��Ű�� ���� ����](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning)
- [.NET ���� ȣȯ��](https://learn.microsoft.com/en-us/dotnet/standard/frameworks)

### NuGet ��Ű��
- [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common/)
- [Microsoft.Win32.SystemEvents](https://www.nuget.org/packages/Microsoft.Win32.SystemEvents/)

---

**���� System.Drawing.Common 8.0.11�� ��ġ�Ǿ� .NET 8�� �Ϻ��ϰ� ȣȯ�˴ϴ�!** ?

**���� �����غ��ð�, ������ �ִٸ� ����� ��� �α׳� ���� �޽����� �������ּ���!** ??
