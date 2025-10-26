# SLauncher

<div align="center">

**Windows�� ��� ��Ƽ ��� �� ��ó**

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)
[![WinUI 3](https://img.shields.io/badge/WinUI-3-0078D4)](https://microsoft.github.io/microsoft-ui-xaml/)

**[English](README.md)** | **[�ѱ���](README.ko-KR.md)** | **[������](README.ja-JP.md)**

[���](#-���) ? [��ġ](#-��ġ) ? [����](#-����) ? [���](#?-�ҽ�-���) ? [�⿩](#-�⿩�ϱ�)

</div>

---

## ?? ���

### �ٽ� ���
- ? **���� ����** - ��� ����ϴ� ��, ����, ������Ʈ�� ��� ����
- ?? **���� ����Ű** - ��𼭳� ��ó ȣ�� (�⺻: `Ctrl + Space`)
- ?? **�� ����** - ���� ������ �׸� ���� (�̸� �� ���� ���� ����)
- ??? **�巡�� �� ���** - �������� �׸� ����
- ?? **����Ʈ �˻�** - �׸� �˻� �Ǵ� ���� ����/����/URL ����
- ?? **������ ũ�� ����** - �����̴� �Ǵ� `Ctrl + ���콺 ��` (0.25x - 6.00x)

### �ٱ��� ����
- ???? **English** (����)
- ???? **�ѱ���**
- ???? **������** (�Ϻ���)
- ? �ǽð� ��� ��ȯ - ����� ���ʿ�!

### ��� UI
- ?? **Windows 11 ������** - Mica/Acrylic ȿ���� ����� ����Ƽ�� WinUI 3
- ?? **�׸� ����** - �ý��� ��ũ/����Ʈ �׸� �ڵ� ����
- ?? **��üȭ�� ���** - ������ ��ó ����
- ?? **�׸��� ����** - ���� �Ǵ� ��� ���� ����
- ?? **�� ����** - 8���� ������ �������� �� Ŀ���͸���¡

### ���� & �̽ļ�
- ?? **���ͺ�** - ���� ���� ������ ��� ������ ���� (`UserCache/`)
- ?? **���� ����** - ĳ�õ� �����ͷ� ��� �ε�
- ?? **�ĺ��� ĳ��** - ������Ʈ ������ ���� ĳ��
- ?? **������** - �ּ����� ���ҽ� ���

---

## ?? ��ġ

### �ý��� �䱸����
- **Windows 10** ���� 1809 (��� 17763) �̻�
- **Windows 11** (�ֻ��� ������ ���� ����)
- **.NET 8.0 ��Ÿ��** (��ü ���� ��忡 ����)

### ���� ��ġ
1. [Releases](https://github.com/yourusername/SLauncher/releases)���� �ֽ� ������ �ٿ�ε�
2. ZIP ������ ���ϴ� ������ ���� ����
3. `SLauncher.exe` ����
4. (���û���) �������� "Windows ���� �� ����" Ȱ��ȭ

### ���ͺ� ���
��� ������ �����ʹ� ���� ���� �� `UserCache` ������ ����˴ϴ�:
```
SLauncher/
������ SLauncher.exe
������ UserCache/
    ������ Settings/      # ����� ����
    ������ Files/         # �׸� ������
    ������ FaviconCache/  # ������Ʈ ������
```

---

## ?? ����

### �׸� �߰�

#### ��� 1: ��ư
- **���� �߰�**: ��ư Ŭ�� �� `.exe`, `.lnk` �Ǵ� ��� ���� ����
- **���� �߰�**: ��ư Ŭ�� �� ���� ����
- **������Ʈ �߰�**: ��ư Ŭ�� �� URL �Է� (��: `https://github.com`)

#### ��� 2: �巡�� �� ���
- ����, ����, �ٷΰ��⸦ â�� ���� �巡��
- �� �� �巡�׷� �׸� �̵�
- �� �׸��� �ٸ� �׸� ���� �巡���Ͽ� �׷� ����

### �׸� ����

| �۾� | ��� |
|------|------|
| **����** | �׸� ��Ŭ�� �� ���� |
| **����** | �׸� ��Ŭ�� �� ���� �Ǵ� `Delete` Ű |
| **�׷� ����** | �� �׸��� �ٸ� �׸� ���� �巡�� |
| **���� ����** | �׸��� �� ��ġ�� �巡�� |

### �� ����

#### �� ����
- �� �� **+** ��ư Ŭ��
- �� ���� ���� �ٸ� �׸�� ���� ����

#### �� �ɼ� (�� ��Ŭ��)
- **�̸� ����** - �ǿ� ����� ���� �̸� �ο�
- **���� ����** - 8���� ������ ���� �� ����
- **����** - �� ���� (�׸��� ������ Ȯ�� �ʿ�)

### �˻�

�˻�â�� ���� �Է� ������ �����մϴ�:

| �Է� ���� | ���� | ��� |
|-----------|------|------|
| **�׸� �̸�** | `�޸���` | ��� �׸� �˻� |
| **���� ���** | `C:\Windows\notepad.exe` | ���� ���� ���� |
| **���� ���** | `C:\Users\Documents` | Ž���⿡�� ���� ���� |
| **������Ʈ URL** | `https://google.com` | �⺻ �������� ���� |
| **�˻� ����** | `search:Ű����` | �⺻ �������� �˻� |

### Ű���� ����Ű

| ����Ű | ���� |
|--------|------|
| `Ctrl + Space` | ��ó ǥ��/���� (�������� ���� ����) |
| `Ctrl + ���콺 ��` | ������ ũ�� ���� (â ��ü���� �۵�) |
| `Delete` | ������ �׸� ���� |
| `Esc` | ��ó �ݱ� (��üȭ�� ���) |
| `Enter` | ù ��° �˻� ��� ���� |

### ������ ũ�� ����

���� ������� ������ ũ�� ����:
- ������ �ϴ��� **�����̴�**
- â ��𼭳� **Ctrl + ���콺 ��**
- ����: 0.25x ~ 6.00x

---

## ?? ����

### �Ϲ� ����
- **��üȭ�� Ȱ��ȭ** - ��üȭ�� ���� ��ó ���
- **�׸��� ����** - ���� (�ʺ� ä��) �Ǵ� ��� (���� �ʺ�)
- **Windows ���� �� ����** - ���� �� �ڵ� ����
- **���� ����Ű** - ǥ��/���� ����Ű Ŀ���͸���¡
  - ���ľ�: Ctrl, Alt, Shift, Ctrl+Shift, Ctrl+Alt
  - Ű: Space, Tab, Enter, Esc, F1-F4
- **���** - ��ȣ ��� ���� (����, �ѱ���, �Ϻ���)

### ĳ�� ����
- **ĳ�� ũ�� ����** - �ĺ��� ĳ�� ��뷮 ����͸�
- **ĳ�� �����** - ��� ĳ�õ� ������Ʈ ������ ����
- **ĳ�� ���� ����** - ĳ�� ���丮 ���� ����
- **ĳ�� ��ġ** - ���ͺ�: `UserCache\FaviconCache\`

---

## ??? �ҽ� ���

### �ʿ� ����
- **Visual Studio 2022** (17.8 �̻�)
  - ��ũ�ε�: ".NET ����ũ�� ����"
  - ���� ���: "Windows App SDK C# ���ø�"
- **Windows App SDK 1.5** �̻�
- **.NET 8.0 SDK**

### Ŭ�� �� ���

```bash
# ����� Ŭ��
git clone https://github.com/yourusername/SLauncher.git
cd SLauncher

# NuGet ��Ű�� ����
dotnet restore

# �ַ�� ���
dotnet build -c Release

# �Ǵ� Visual Studio���� ����
start SLauncher.sln
```

### ������Ʈ ����

```
SLauncher/
������ SLauncher/              # ���� WinUI 3 ������Ʈ
��   ������ Classes/            # �ٽ� Ŭ����
��   ��   ������ LocalizationManager.cs   # �ٱ��� ����
��   ��   ������ UserSettingsClass.cs     # ���� ����
��   ��   ������ GlobalHotkeyManager.cs   # ����Ű ���
��   ��   ������ IconHelpers.cs           # ������ ���� & ĳ��
��   ������ Controls/# ����� ���� ��Ʈ��
��   ��   ������ GridViewTile.xaml        # �� Ÿ�� ��Ʈ��
��   ��   ������ GridViewTileGroup.xaml   # �׷� ��Ʈ��
��   ��   ������ AboutSectionControl.xaml # ���� ������
�������� Strings/     # �ٱ��� ���ҽ�
��   ��   ������ en-US/Resources.resw     # ����
��   ��   ������ ko-KR/Resources.resw     # �ѱ���
��   ��   ������ ja-JP/Resources.resw     # �Ϻ���
��   ������ MainWindow*.cs      # ���� â (�κ� Ŭ����)
��   ��   ������ MainWindow.xaml.cs       # ���� ����
��   ��   ������ MainWindow.UI.cs         # UI ����
��   ��   ������ MainWindow.Tabs.cs       # �� ����
��   ��   ������ MainWindow.Items.cs      # �׸� ����
��   �� ������ MainWindow.DragDrop.cs   # �巡�� �� ���
��   ��   ������ MainWindow.Search.cs     # �˻� ����
��   ��   ������ MainWindow.Hotkeys.cs    # ����Ű & Ʈ����
��   ������ SettingsWindow*.cs  # ���� â (�κ� Ŭ����)
��  ������ SettingsWindow.xaml.cs         # ���� ����
��       ������ SettingsWindow.Localization.cs # ��� UI
�� ������ SettingsWindow.Cache.cs        # ĳ�� ����
��       ������ SettingsWindow.Hotkey.cs       # ����Ű ����
��       ������ SettingsWindow.Settings.cs     # ���� ���
������ WinFormsClassLibrary/   # ���� ���̺귯�� (���� ��ȭ����)
```

### �κ� Ŭ���� ����

`MainWindow`�� `SettingsWindow` ��� �� ���� �ڵ� ������ ���� �κ� Ŭ������ ����մϴ�:
- �� �κ� Ŭ���� ������ Ư�� ��� ������ ó��
- �ڵ� Ž�� �� ���� ������ ����
- �ϰ����� ���� `MainWindow`�� ������ ���� ����

---

## ?? �ٱ��� ����

### ���� ���

| ��� | �ڵ� | ���� | ���ҽ� |
|------|------|------|--------|
| ???? English | en-US | ? �Ϸ� | 90 ���ڿ� |
| ???? �ѱ��� | ko-KR | ? �Ϸ� | 91 ���ڿ� |
| ???? ������ | ja-JP | ? �Ϸ� | 91 ���ڿ� |

### �� ��� �߰�

1. **���ҽ� ���� ����**
   ```
   SLauncher/Strings/{���-�ڵ�}/Resources.resw
   ```

2. **���ø� ����**
   ```bash
   cp SLauncher/Strings/en-US/Resources.resw SLauncher/Strings/{���-�ڵ�}/
   ```

3. **����**
   - Visual Studio���� `Resources.resw` ����
 - `<value>` ���� ���� (`<data name>`�� �������� ����)
   - UI ���̾ƿ� �׽�Ʈ (�Ϻ� ���� �� ��)

4. **������ �߰�**
   ```xaml
   <!-- SettingsWindow.xaml -->
   <ComboBox x:Name="LanguageComboBox">
       <ComboBoxItem Content="������ ��� �̸�" Tag="{���-�ڵ�}" />
 </ComboBox>
   ```

5. **�׽�Ʈ**
   - ��� �� ����
   - ���� �� ���� �� ��� ����
   - ��� UI ��Ұ� �ùٸ��� ǥ�õǴ��� Ȯ��

### ���� ���̵����
- �÷��̽�Ȧ�� ����: `{0}`, `{1}` (���ڿ� ���Ŀ� ���)
- �� �ٲ� �� ���� ����
- ���� ���� ��� (������, ����ǥ)
- �� �������� �׽�Ʈ (���̾ƿ�� ������ �� �� ����)
- ��� ��� �ϰ��� ���� (��: "ĳ��", "����Ű")

---

## ?? �⿩�ϱ�

�⿩�� ȯ���մϴ�! ������ ���� ���� �� �ֽ��ϴ�:

### �⿩ ���
- ?? **����** - �� ��� �߰� �Ǵ� ���� ��� ����
- ?? **���� ����Ʈ** - [GitHub Issues](https://github.com/yourusername/SLauncher/issues)�� ���� ����
- ? **��� ����** - �� ��� ����
- ?? **�ڵ�** - Ǯ ������Ʈ ����
- ?? **����** - README �Ǵ� �ڵ� �ּ� ����

### ���� ��ũ�÷�
1. **��ũ** �����
2. **Ŭ��** ��ũ
   ```bash
   git clone https://github.com/your-username/SLauncher.git
   ```
3. **����** ��� �귣ġ
   ```bash
   git checkout -b feature/AmazingFeature
   ```
4. **����** �� ö���� �׽�Ʈ
5. **Ŀ��** ��Ȯ�� �޽����� �Բ�
   ```bash
   git commit -m "feat: Add amazing feature"
   ```
6. **Ǫ��** ��ũ��
   ```bash
   git push origin feature/AmazingFeature
   ```
7. **����** Ǯ ������Ʈ

### �ڵ� ��Ÿ�� ���̵����
- ���� �ڵ� ���� ������
- �ǹ� �ִ� ����/�޼��� �̸� ���
- ���� �޼��忡 XML �ּ� �߰�
- �޼��� ���� ���� (���� å��)
- ū ���Ͽ� �κ� Ŭ���� ���
- ��� UI �ؽ�Ʈ�� �ٱ��� ���� �߰�

---

## ?? ���̼���

�� ������Ʈ�� MIT ���̼����� ���� ���̼����� �ο��˴ϴ� - �ڼ��� ������ [LICENSE](LICENSE) ������ �����ϼ���.

### ������Ƽ ���̺귯��
- **WinUI 3** - MIT ���̼���
- **CommunityToolkit.WinUI** - MIT ���̼���
- **WinUIEx** - MIT ���̼���
- **System.Drawing.Common** - MIT ���̼���

---

## ?? ������ ��

- **���**: Lolle2000la�� [LauncherX](https://github.com/Lolle2000la/LauncherX)
- **UI �����ӿ�ũ**: [WinUI 3](https://microsoft.github.io/microsoft-ui-xaml/)
- **Community Toolkit**: [Windows Community Toolkit](https://github.com/CommunityToolkit/Windows)
- **â ����**: [WinUIEx](https://github.com/dotMorten/WinUIEx)
- **������**: [Segoe Fluent Icons](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)

---

## ?? ����

- **Issues**: [GitHub Issues](https://github.com/yourusername/SLauncher/issues)
- **���**: [GitHub Discussions](https://github.com/yourusername/SLauncher/discussions)
- **�̸���**: your.email@example.com

---

## ?? ���� �α�

### v1.0.0 (�ֽ�)
- ? �ٱ��� ���� (����, �ѱ���, �Ϻ���)
- ? �ǽð� ��� ��ȯ
- ? �� ���� �ڵ� ������ ���� �κ� Ŭ���� �����丵
- ? â ��ü���� Ctrl+���콺�� ������ ũ�� ����
- ?? �׸��� ���� �ٱ��� ����
- ?? ĳ�� ���� �����ؽ�Ʈ �ٱ��� ����
- ?? �������� �ٱ��� ���� (���� 90�� �̻� ���ڿ�)

### ���� ����
- ������ �ִ� �� ����
- ���� ����Ű ����
- �ý��� Ʈ���� ����
- �ĺ��� ĳ��

��ü ���� �α״� [CHANGELOG.md](CHANGELOG.md)�� �����ϼ���.

---

<div align="center">

**Windows �Ŀ� ������ ���� ??�� ��������ϴ�**

[? �� ����](#slauncher)

</div>
