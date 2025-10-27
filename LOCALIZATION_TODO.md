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
  <value>파일 열기</value>
</data>
<data name="ContextMenu_OpenFileLocation" xml:space="preserve">
  <value>파일 위치 열기</value>
</data>
<data name="ContextMenu_RemoveFileFromGroup" xml:space="preserve">
  <value>그룹에서 파일 제거</value>
</data>
<data name="ContextMenu_RemoveFileFromSLauncher" xml:space="preserve">
  <value>SLauncher에서 파일 제거</value>
</data>

<data name="ContextMenu_OpenFolder" xml:space="preserve">
  <value>폴더 열기</value>
</data>
<data name="ContextMenu_OpenFolderLocation" xml:space="preserve">
  <value>폴더 위치 열기</value>
</data>
<data name="ContextMenu_RemoveFolderFromGroup" xml:space="preserve">
  <value>그룹에서 폴더 제거</value>
</data>
<data name="ContextMenu_RemoveFolderFromSLauncher" xml:space="preserve">
  <value>SLauncher에서 폴더 제거</value>
</data>

<data name="ContextMenu_OpenWebsite" xml:space="preserve">
  <value>웹사이트 열기</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromGroup" xml:space="preserve">
  <value>그룹에서 웹사이트 제거</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromSLauncher" xml:space="preserve">
  <value>SLauncher에서 웹사이트 제거</value>
</data>

<data name="ContextMenu_RunAsAdmin" xml:space="preserve">
  <value>관리자 권한으로 실행 시도</value>
</data>
<data name="ContextMenu_EditItem" xml:space="preserve">
  <value>항목 편집</value>
</data>
```

### Edit Item Window
```xml
<data name="EditItem_WindowTitle.Title" xml:space="preserve">
  <value>항목 편집</value>
</data>

<data name="EditItem_EditIconButton.Content" xml:space="preserve">
  <value>아이콘 편집</value>
</data>

<data name="EditItem_ResetIconButton.Content" xml:space="preserve">
  <value>아이콘 초기화</value>
</data>

<data name="EditItem_DisplayTextLabel.Text" xml:space="preserve">
  <value>표시 텍스트:</value>
</data>
<data name="EditItem_DisplayTextLabel.ToolTip" xml:space="preserve">
  <value>SLauncher에서 표시되는 텍스트입니다. 클릭 시 실행되는 항목에는 영향을 주지 않습니다.</value>
</data>

<data name="EditItem_LinkLabel.Text" xml:space="preserve">
  <value>링크:</value>
</data>
<data name="EditItem_LinkLabel.ToolTip" xml:space="preserve">
  <value>실제로 실행될 경로 또는 URL입니다. 일반적으로 표시 텍스트와 동일합니다.</value>
</data>

<data name="EditItem_LaunchArgsLabel.Text" xml:space="preserve">
  <value>실행 인수:</value>
</data>

<data name="EditItem_CloseButton.Content" xml:space="preserve">
  <value>닫기</value>
</data>

<data name="EditItem_SaveButton.Content" xml:space="preserve">
  <value>저장하고 종료</value>
</data>
```

---

## Japanese Translations (ja-JP)

### Context Menu
```xml
<data name="ContextMenu_OpenFile" xml:space="preserve">
  <value>ファイルを開く</value>
</data>
<data name="ContextMenu_OpenFileLocation" xml:space="preserve">
  <value>ファイルの場所を開く</value>
</data>
<data name="ContextMenu_RemoveFileFromGroup" xml:space="preserve">
  <value>グル?プからファイルを削除</value>
</data>
<data name="ContextMenu_RemoveFileFromSLauncher" xml:space="preserve">
  <value>SLauncherからファイルを削除</value>
</data>

<data name="ContextMenu_OpenFolder" xml:space="preserve">
  <value>フォルダを開く</value>
</data>
<data name="ContextMenu_OpenFolderLocation" xml:space="preserve">
  <value>フォルダの場所を開く</value>
</data>
<data name="ContextMenu_RemoveFolderFromGroup" xml:space="preserve">
  <value>グル?プからフォルダを削除</value>
</data>
<data name="ContextMenu_RemoveFolderFromSLauncher" xml:space="preserve">
  <value>SLauncherからフォルダを削除</value>
</data>

<data name="ContextMenu_OpenWebsite" xml:space="preserve">
  <value>ウェブサイトを開く</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromGroup" xml:space="preserve">
  <value>グル?プからウェブサイトを削除</value>
</data>
<data name="ContextMenu_RemoveWebsiteFromSLauncher" xml:space="preserve">
  <value>SLauncherからウェブサイトを削除</value>
</data>

<data name="ContextMenu_RunAsAdmin" xml:space="preserve">
  <value>管理者として?行を試みる</value>
</data>
<data name="ContextMenu_EditItem" xml:space="preserve">
  <value>アイテムを編集</value>
</data>
```

### Edit Item Window
```xml
<data name="EditItem_WindowTitle.Title" xml:space="preserve">
  <value>アイテムを編集</value>
</data>

<data name="EditItem_EditIconButton.Content" xml:space="preserve">
  <value>アイコンを編集</value>
</data>

<data name="EditItem_ResetIconButton.Content" xml:space="preserve">
  <value>アイコンをリセット</value>
</data>

<data name="EditItem_DisplayTextLabel.Text" xml:space="preserve">
  <value>表示テキスト:</value>
</data>
<data name="EditItem_DisplayTextLabel.ToolTip" xml:space="preserve">
  <value>SLauncherで表示されるテキストです。クリック時に起動されるアイテムには影響しません。</value>
</data>

<data name="EditItem_LinkLabel.Text" xml:space="preserve">
  <value>リンク:</value>
</data>
<data name="EditItem_LinkLabel.ToolTip" xml:space="preserve">
  <value>?際に起動されるパスまたはURLです。通常は表示テキストと同じです。</value>
</data>

<data name="EditItem_LaunchArgsLabel.Text" xml:space="preserve">
  <value>起動引?:</value>
</data>

<data name="EditItem_CloseButton.Content" xml:space="preserve">
  <value>閉じる</value>
</data>

<data name="EditItem_SaveButton.Content" xml:space="preserve">
  <value>保存して終了</value>
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
