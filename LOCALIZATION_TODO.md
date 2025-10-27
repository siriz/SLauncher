# Localization TODO: Context Menu and Edit Item Window

This document lists all the resource strings that need to be added to support multi-language context menus and Edit Item window.

## Files to Update

Add the following strings to **all three** resource files:
- `SLauncher/Strings/en-US/Resources.resw`
- `SLauncher/Strings/ko-KR/Resources.resw`
- `SLauncher/Strings/ja-JP/Resources.resw`

## Context Menu Strings

### File Context Menu
```xml
<data name="ContextMenu_OpenFile" xml:space="preserve">
  <value>Open file</value>
</data>
<data name="ContextMenu_OpenFileLocation" xml:space="preserve">
  <value>Open file location</value>
</data>
<data name="ContextMenu_RemoveFileFromGroup" xml:space="preserve">
  <value>Remove file from group</value>
</data>
<data name="ContextMenu_RemoveFileFromSLauncher" xml:space="preserve">
  <value>Remove file from SLauncher</value>
</data>
```

### Folder Context Menu
```xml
<data name="ContextMenu_OpenFolder" xml:space="preserve">
<value>Open folder</value>
</data>
<data name="ContextMenu_OpenFolderLocation" xml:space="preserve">
  <value>Open folder location</value>
</data>
<data name="ContextMenu_RemoveFolderFromGroup" xml:space="preserve">
  <value>Remove folder from group</value>
</data>
<data name="ContextMenu_RemoveFolderFromSLauncher" xml:space="preserve">
  <value>Remove folder from SLauncher</value>
</data>
```

### Website Context Menu
```xml
<data name="ContextMenu_OpenWebsite" xml:space="preserve">
  <value>Open website</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromGroup" xml:space="preserve">
  <value>Remove website from group</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromSLauncher" xml:space="preserve">
  <value>Remove website from SLauncher</value>
</data>
```

### Common Context Menu
```xml
<data name="ContextMenu_RunAsAdmin" xml:space="preserve">
  <value>Attempt to run as administrator</value>
</data>
<data name="ContextMenu_EditItem" xml:space="preserve">
  <value>Edit item</value>
</data>
```

## Edit Item Window Strings

```xml
<data name="EditItem_WindowTitle.Title" xml:space="preserve">
  <value>Edit item</value>
</data>

<data name="EditItem_EditIconButton.Content" xml:space="preserve">
  <value>Edit icon</value>
</data>

<data name="EditItem_ResetIconButton.Content" xml:space="preserve">
  <value>Reset icon</value>
</data>

<data name="EditItem_DisplayTextLabel.Text" xml:space="preserve">
  <value>Display Text:</value>
</data>
<data name="EditItem_DisplayTextLabel.ToolTip" xml:space="preserve">
  <value>The text that is displayed on this item in SLauncher. Does not affect what is launched when this item is clicked.</value>
</data>

<data name="EditItem_LinkLabel.Text" xml:space="preserve">
  <value>Link:</value>
</data>
<data name="EditItem_LinkLabel.ToolTip" xml:space="preserve">
  <value>The actual path or URL that will be launched. Usually same as the display text.</value>
</data>

<data name="EditItem_LaunchArgsLabel.Text" xml:space="preserve">
  <value>Launch arguments:</value>
</data>

<data name="EditItem_CloseButton.Content" xml:space="preserve">
  <value>Close</value>
</data>

<data name="EditItem_SaveButton.Content" xml:space="preserve">
  <value>Save and Exit</value>
</data>
```

---

## Korean Translations (ko-KR)

### Context Menu
```xml
<data name="ContextMenu_OpenFile" xml:space="preserve">
  <value>���� ����</value>
</data>
<data name="ContextMenu_OpenFileLocation" xml:space="preserve">
  <value>���� ��ġ ����</value>
</data>
<data name="ContextMenu_RemoveFileFromGroup" xml:space="preserve">
  <value>�׷쿡�� ���� ����</value>
</data>
<data name="ContextMenu_RemoveFileFromSLauncher" xml:space="preserve">
  <value>SLauncher���� ���� ����</value>
</data>

<data name="ContextMenu_OpenFolder" xml:space="preserve">
  <value>���� ����</value>
</data>
<data name="ContextMenu_OpenFolderLocation" xml:space="preserve">
  <value>���� ��ġ ����</value>
</data>
<data name="ContextMenu_RemoveFolderFromGroup" xml:space="preserve">
  <value>�׷쿡�� ���� ����</value>
</data>
<data name="ContextMenu_RemoveFolderFromSLauncher" xml:space="preserve">
  <value>SLauncher���� ���� ����</value>
</data>

<data name="ContextMenu_OpenWebsite" xml:space="preserve">
  <value>������Ʈ ����</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromGroup" xml:space="preserve">
  <value>�׷쿡�� ������Ʈ ����</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromSLauncher" xml:space="preserve">
  <value>SLauncher���� ������Ʈ ����</value>
</data>

<data name="ContextMenu_RunAsAdmin" xml:space="preserve">
  <value>������ �������� ���� �õ�</value>
</data>
<data name="ContextMenu_EditItem" xml:space="preserve">
  <value>�׸� ����</value>
</data>
```

### Edit Item Window
```xml
<data name="EditItem_WindowTitle.Title" xml:space="preserve">
  <value>�׸� ����</value>
</data>

<data name="EditItem_EditIconButton.Content" xml:space="preserve">
  <value>������ ����</value>
</data>

<data name="EditItem_ResetIconButton.Content" xml:space="preserve">
  <value>������ �ʱ�ȭ</value>
</data>

<data name="EditItem_DisplayTextLabel.Text" xml:space="preserve">
  <value>ǥ�� �ؽ�Ʈ:</value>
</data>
<data name="EditItem_DisplayTextLabel.ToolTip" xml:space="preserve">
  <value>SLauncher���� ǥ�õǴ� �ؽ�Ʈ�Դϴ�. Ŭ�� �� ����Ǵ� �׸񿡴� ������ ���� �ʽ��ϴ�.</value>
</data>

<data name="EditItem_LinkLabel.Text" xml:space="preserve">
  <value>��ũ:</value>
</data>
<data name="EditItem_LinkLabel.ToolTip" xml:space="preserve">
  <value>������ ����� ��� �Ǵ� URL�Դϴ�. �Ϲ������� ǥ�� �ؽ�Ʈ�� �����մϴ�.</value>
</data>

<data name="EditItem_LaunchArgsLabel.Text" xml:space="preserve">
  <value>���� �μ�:</value>
</data>

<data name="EditItem_CloseButton.Content" xml:space="preserve">
  <value>�ݱ�</value>
</data>

<data name="EditItem_SaveButton.Content" xml:space="preserve">
  <value>�����ϰ� ����</value>
</data>
```

---

## Japanese Translations (ja-JP)

### Context Menu
```xml
<data name="ContextMenu_OpenFile" xml:space="preserve">
  <value>�ի�������Ҫ�</value>
</data>
<data name="ContextMenu_OpenFileLocation" xml:space="preserve">
  <value>�ի��������ᶪ��Ҫ�</value>
</data>
<data name="ContextMenu_RemoveFileFromGroup" xml:space="preserve">
  <value>����?�ת���ի���������</value>
</data>
<data name="ContextMenu_RemoveFileFromSLauncher" xml:space="preserve">
  <value>SLauncher����ի���������</value>
</data>

<data name="ContextMenu_OpenFolder" xml:space="preserve">
  <value>�ի�������Ҫ�</value>
</data>
<data name="ContextMenu_OpenFolderLocation" xml:space="preserve">
  <value>�ի��������ᶪ��Ҫ�</value>
</data>
<data name="ContextMenu_RemoveFolderFromGroup" xml:space="preserve">
  <value>����?�ת���ի���������</value>
</data>
<data name="ContextMenu_RemoveFolderFromSLauncher" xml:space="preserve">
  <value>SLauncher����ի���������</value>
</data>

<data name="ContextMenu_OpenWebsite" xml:space="preserve">
  <value>�����֫����Ȫ��Ҫ�</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromGroup" xml:space="preserve">
  <value>����?�ת��髦���֫����Ȫ����</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromSLauncher" xml:space="preserve">
  <value>SLauncher���髦���֫����Ȫ����</value>
</data>

<data name="ContextMenu_RunAsAdmin" xml:space="preserve">
  <value>η���Ȫ���?�����˪ߪ�</value>
</data>
<data name="ContextMenu_EditItem" xml:space="preserve">
  <value>�����ƫ�����</value>
</data>
```

### Edit Item Window
```xml
<data name="EditItem_WindowTitle.Title" xml:space="preserve">
  <value>�����ƫ�����</value>
</data>

<data name="EditItem_EditIconButton.Content" xml:space="preserve">
  <value>������������</value>
</data>

<data name="EditItem_ResetIconButton.Content" xml:space="preserve">
  <value>���������꫻�ë�</value>
</data>

<data name="EditItem_DisplayTextLabel.Text" xml:space="preserve">
  <value>���ƫƫ�����:</value>
</data>
<data name="EditItem_DisplayTextLabel.ToolTip" xml:space="preserve">
  <value>SLauncher�����ƪ����ƫ����ȪǪ�������ë��������Ѫ���뫢���ƫ�˪���ª��ު���</value>
</data>

<data name="EditItem_LinkLabel.Text" xml:space="preserve">
  <value>���:</value>
</data>
<data name="EditItem_LinkLabel.ToolTip" xml:space="preserve">
  <value>?�����Ѫ����ѫ��ު���URL�Ǫ������Ȫ����ƫƫ����Ȫ��Ҫ��Ǫ���</value>
</data>

<data name="EditItem_LaunchArgsLabel.Text" xml:space="preserve">
  <value>������?:</value>
</data>

<data name="EditItem_CloseButton.Content" xml:space="preserve">
  <value>�ͪ���</value>
</data>

<data name="EditItem_SaveButton.Content" xml:space="preserve">
  <value>����������</value>
</data>
```

---

## How to Add to Resources.resw Files

1. Open each `.resw` file in a text editor (or Visual Studio Resource Editor)
2. Add the `<data>` blocks above before the closing `</root>` tag
3. Make sure the `xml:space="preserve"` attribute is included
4. Save all files
5. Rebuild the project

## Files to Edit

1. `SLauncher/Strings/en-US/Resources.resw` - Add English strings
2. `SLauncher/Strings/ko-KR/Resources.resw` - Add Korean strings
3. `SLauncher/Strings/ja-JP/Resources.resw` - Add Japanese strings
