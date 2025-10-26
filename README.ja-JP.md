# SLauncher

<div align="center">

**Windows用モダンマルチ言語アプリランチャー**

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)
[![WinUI 3](https://img.shields.io/badge/WinUI-3-0078D4)](https://microsoft.github.io/microsoft-ui-xaml/)

**言語:** [English](README.md) • [한국어](README.ko-KR.md) • [日本語](README.ja-JP.md)

[機能](#-機能) • [インストール](#-インストール) • [使い方](#-使い方) • [ソースからビルド](#%EF%B8%8F-ソースからビルド) • [貢献](#-貢献)

</div>

---

## 🌟 機能

### コア機能
- ⚡ **クイック起動** - お気に入りのアプリ、フォルダ、ウェブサイトを即座に起動
- 🔑 **グローバルホットキー** - どこからでもランチャーを呼び出し（デフォルト: `Ctrl + Space`）
- 📑 **タブ構成** - 複数のタブでアイテムを整理（名前と色をカスタマイズ可能）
- 🖱️ **ドラッグ＆ドロップ** - 直感的なアイテム管理
- 🔍 **スマート検索** - アイテム検索または直接ファイル/フォルダ/URLを開く
- 📐 **アイコンスケーリング** - スライダーまたは`Ctrl + マウスホイール`（0.25x - 6.00x）

### 多言語サポート
- 🇺🇸 **English**（英語）
- 🇰🇷 **한국어**（韓国語）
- 🇯🇵 **日本語**
- ⚡ リアルタイム言語切り替え - 再起動不要！

### モダンUI
- 🎨 **Windows 11デザイン** - Mica/Acrylic効果を使用したネイティブWinUI 3
- 🌓 **テーマサポート** - システムダーク/ライトテーマに自動追従
- 📺 **フルスクリーンモード** - 没入型ランチャー体験
- ⚖️ **グリッド配置** - 左または中央配置を選択
- 🎨 **タブカラー** - 8つのプリセットカラーでタブをカスタマイズ

### パフォーマンスと移植性
- 📦 **ポータブル** - 実行ファイルフォルダにすべてのデータを保存（`UserCache/`）
- 🚀 **高速起動** - キャッシュされたデータで即座にロード
- 💾 **ファビコンキャッシュ** - ウェブサイトアイコンをローカルにキャッシュ
- 🪶 **軽量** - 最小限のリソース使用

---

## 📦 インストール

### システム要件
- **Windows 10** バージョン1809（ビルド17763）以降
- **Windows 11**（最高の体験のために推奨）
- **.NET 8.0ランタイム**（自己完結型ビルドに含まれる）

### クイックインストール
1. [Releases](https://github.com/siriz/SLauncher/releases)から最新リリースをダウンロード
2. ZIPファイルを任意のフォルダに解凍
3. `SLauncher.exe`を実行
4. （オプション）設定で「Windowsと一緒に起動」を有効化

### ポータブルモード
すべての設定とデータは実行ファイルの隣の`UserCache`フォルダに保存されます:
```
SLauncher/
├── SLauncher.exe
└── UserCache/
    ├── Settings/    # ユーザー設定
    ├── Files/       # アイテムデータ
    └── FaviconCache/  # ウェブサイトアイコン
```

---

## 🎮 使い方

### アイテムの追加

#### 方法1: ボタン
- **ファイル追加**: ボタンをクリックして`.exe`、`.lnk`、または任意のファイルを選択
- **フォルダ追加**: ボタンをクリックしてフォルダを選択
- **ウェブサイト追加**: ボタンをクリックしてURLを入力（例: `https://github.com`）

#### 方法2: ドラッグ＆ドロップ
- ファイル、フォルダ、ショートカットをウィンドウに直接ドラッグ
- タブ間でドラッグしてアイテムを移動
- あるアイテムを別のアイテムにドラッグしてグループを作成

### アイテムの管理

| アクション | 方法 |
|-----------|------|
| **編集** | アイテムを右クリック → 編集 |
| **削除** | アイテムを右クリック → 削除または`Delete`キー |
| **グループ作成** | あるアイテムを別のアイテムにドラッグ |
| **並べ替え** | アイテムを新しい位置にドラッグ |

### タブ管理

#### タブの作成
- タブの隣の**+**ボタンをクリック
- 各タブは異なるアイテムと設定が可能

#### タブオプション（タブを右クリック）
- **名前変更** - タブにカスタム名を付ける
- **色変更** - 8つのプリセットカラーから選択
- **削除** - タブを削除（アイテムがある場合は確認が必要）

### 検索

検索ボックスは複数の入力タイプをサポートします:

| 入力タイプ | 例 | 結果 |
|-----------|-----|------|
| **アイテム名** | `メモ帳` | すべてのアイテムを検索 |
| **ファイルパス** | `C:\Windows\notepad.exe` | ファイルを直接開く |
| **フォルダパス** | `C:\Users\Documents` | エクスプローラーでフォルダを開く |
| **ウェブサイトURL** | `https://google.com` | デフォルトブラウザで開く |
| **検索クエリ** | `search:キーワード` | デフォルトブラウザで検索 |

### キーボードショートカット

| ショートカット | アクション |
|---------------|-----------|
| `Ctrl + Space` | ランチャー表示/非表示（設定で変更可能） |
| `Ctrl + マウスホイール` | アイコンサイズ調整（ウィンドウ全体で動作） |
| `Delete` | 選択したアイテムを削除 |
| `Esc` | ランチャーを閉じる（フルスクリーンモード） |
| `Enter` | 最初の検索結果を開く |

### アイコンスケーリング

次の方法でアイコンサイズを調整:
- 右下の**スライダー**
- ウィンドウのどこでも**Ctrl + マウスホイール**
- 範囲: 0.25x ～ 6.00x

---

## ⚙️ 設定

### 一般設定
- **フルスクリーン有効化** - フルスクリーンモードでランチャーを使用
- **グリッド配置** - 左（幅を埋める）または中央（固定幅）
- **Windowsと一緒に起動** - 起動時に自動実行
- **グローバルホットキー** - 表示/非表示ホットキーのカスタマイズ
  - 修飾キー: Ctrl、Alt、Shift、Ctrl+Shift、Ctrl+Alt
  - キー: Space、Tab、Enter、Esc、F1-F4
- **言語** - 優先言語を選択（英語、韓国語、日本語）

### キャッシュ管理
- **キャッシュサイズ表示** - ファビコンキャッシュ使用量を監視
- **キャッシュクリア** - すべてのキャッシュされたウェブサイトアイコンを削除
- **キャッシュフォルダを開く** - キャッシュディレクトリに直接アクセス
- **キャッシュ場所** - ポータブル: `UserCache\FaviconCache\`

---

## 🛠️ ソースからビルド

### 前提条件
- **Visual Studio 2022**（17.8以降）
  - ワークロード: 「.NETデスクトップ開発」
  - コンポーネント: 「Windows App SDK C#テンプレート」
- **Windows App SDK 1.5**以降
- **.NET 8.0 SDK**

### クローンとビルド

```bash
# リポジトリをクローン
git clone https://github.com/siriz/SLauncher.git
cd SLauncher

# NuGetパッケージを復元
dotnet restore

# ソリューションをビルド
dotnet build -c Release

# またはVisual Studioで開く
start SLauncher.sln
```

### プロジェクト構造

```
SLauncher/
├── SLauncher/   # メインWinUI 3プロジェクト
│   ├── Classes/      # コアクラス
│   │   ├── LocalizationManager.cs   # 多言語サポート
│   │   ├── UserSettingsClass.cs  # 設定管理
│   │   ├── GlobalHotkeyManager.cs # ホットキー登録
│   │   └── IconHelpers.cs # アイコン抽出とキャッシュ
│   ├── Controls/     # カスタムコントロール
│   │   ├── GridViewTile.xaml # アプリタイルコントロール
│   │   ├── GridViewTileGroup.xaml   # グループコントロール
│   │   └── AboutSectionControl.xaml # Aboutページ
│   ├── Strings/      # 多言語リソース
│ │   ├── en-US/Resources.resw     # 英語
│   │   ├── ko-KR/Resources.resw     # 韓国語
│ │   └── ja-JP/Resources.resw     # 日本語
│   ├── MainWindow*.cs    # メインウィンドウ（部分クラス）
│   │   ├── MainWindow.xaml.cs       # メインロジック
│   │   ├── MainWindow.UI.cs     # UI管理
│   │   ├── MainWindow.Tabs.cs       # タブ管理
│   │   ├── MainWindow.Items.cs      # アイテム管理
│   │   ├── MainWindow.DragDrop.cs   # ドラッグ＆ドロップ
│   │   ├── MainWindow.Search.cs     # 検索ロジック
│   │   └── MainWindow.Hotkeys.cs    # ホットキーとトレイ
│   └── SettingsWindow*.cs  # 設定ウィンドウ（部分クラス）
│  ├── SettingsWindow.xaml.cs       # メインロジック
│       ├── SettingsWindow.Localization.cs # 言語UI
│       ├── SettingsWindow.Cache.cs    # キャッシュ管理
│   ├── SettingsWindow.Hotkey.cs# ホットキー設定
│       └── SettingsWindow.Settings.cs   # 設定トグル
└── WinFormsClassLibrary/   # ヘルパーライブラリ（ファイルダイアログ）
```

### 部分クラスパターン

`MainWindow`と`SettingsWindow`の両方が、より良いコード構成のために部分クラスを使用します:
- 各部分クラスファイルが特定の機能領域を処理
- コードのナビゲーションと保守が容易
- 一貫性のために`MainWindow`と同じパターンに従う

---

## 🌐 多言語化

### サポートされている言語

| 言語 | コード | ステータス | リソース |
|------|--------|-----------|----------|
| 🇺🇸 English | en-US | ✅ 完了 | 90文字列 |
| 🇰🇷 한국어 | ko-KR | ✅ 完了 | 91文字列 |
| 🇯🇵 日本語 | ja-JP | ✅ 完了 | 91文字列 |

### 新しい言語の追加

1. **リソースファイルの作成**
   ```
   SLauncher/Strings/{言語コード}/Resources.resw
   ```

2. **テンプレートのコピー**
   ```bash
   cp SLauncher/Strings/en-US/Resources.resw SLauncher/Strings/{言語コード}/
   ```

3. **翻訳**
   - Visual Studioで`Resources.resw`を開く
   - `<value>`の内容を翻訳（`<data name>`は変更しない）
   - UIレイアウトをテスト（一部の言語は長い）

4. **設定に追加**
   ```xaml
   <!-- SettingsWindow.xaml -->
   <ComboBox x:Name="LanguageComboBox">
       <ComboBoxItem Content="あなたの言語名" Tag="{言語コード}" />
   </ComboBox>
   ```

5. **テスト**
   - ビルドして実行
   - 設定 → 言語で新しい言語を選択
   - すべてのUI要素が正しく表示されることを確認

### 翻訳ガイドライン
- プレースホルダーを保持: `{0}`、`{1}`（文字列フォーマットに使用）
- 改行とフォーマットを維持
- ネイティブの慣習を使用（句読点、引用符）
- 長い翻訳でテスト（レイアウトに影響する可能性あり）
- 技術用語の一貫性を保つ（例: 「キャッシュ」、「ホットキー」）

---

## 🤝 貢献

貢献を歓迎します！次のように協力できます:

### 貢献方法
- 🌍 **翻訳** - 新しい言語を追加または既存の言語を改善
- 🐛 **バグ報告** - [GitHub Issues](https://github.com/siriz/SLauncher/issues)で問題を報告
- ✨ **機能リクエスト** - 新機能を提案
- 💻 **コード** - プルリクエストを提出
- 📖 **ドキュメント** - READMEまたはコードコメントを改善

### 開発ワークフロー
1. **フォーク** リポジトリ
2. **クローン** フォーク
   ```bash
   git clone https://github.com/your-username/SLauncher.git
   ```
3. **作成** 機能ブランチ
   ```bash
   git checkout -b feature/AmazingFeature
   ```
4. **変更** して徹底的にテスト
5. **コミット** 明確なメッセージで
   ```bash
   git commit -m "feat: Add amazing feature"
   ```
6. **プッシュ** フォークへ
 ```bash
   git push origin feature/AmazingFeature
   ```
7. **開く** プルリクエスト

### コードスタイルガイドライン
- 既存のコードパターンに従う
- 意味のある変数/メソッド名を使用
- パブリックメソッドにXMLコメントを追加
- メソッドを集中的に保つ（単一責任）
- 大きなファイルには部分クラスを使用
- すべてのUIテキストに多言語化を追加

---

## 📄 ライセンス

このプロジェクトはMITライセンスの下でライセンスされています - 詳細は[LICENSE](LICENSE)ファイルを参照してください。

### サードパーティライブラリ
- **WinUI 3** - MITライセンス
- **CommunityToolkit.WinUI** - MITライセンス
- **WinUIEx** - MITライセンス
- **System.Drawing.Common** - MITライセンス

---

## 🙏 謝辞

- **ベース**: Lolle2000laの[LauncherX](https://github.com/Lolle2000la/LauncherX)
- **UIフレームワーク**: [WinUI 3](https://microsoft.github.io/microsoft-ui-xaml/)
- **Community Toolkit**: [Windows Community Toolkit](https://github.com/CommunityToolkit/Windows)
- **ウィンドウ管理**: [WinUIEx](https://github.com/dotMorten/WinUIEx)
- **アイコン**: [Segoe Fluent Icons](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)

---

## 📧 サポート

- **Issues**: [GitHub Issues](https://github.com/siriz/SLauncher/issues)
- **ディスカッション**: [GitHub Discussions](https://github.com/siriz/SLauncher/discussions)
- **メール**: your.email@example.com

---

## 📊 変更履歴

### v1.0.0（最新）
- ✨ 多言語サポート（英語、韓国語、日本語）
- ✨ リアルタイム言語切り替え
- ✨ より良いコード構成のための部分クラスリファクタリング
- ✨ ウィンドウ全体でCtrl+マウスホイールアイコンスケーリング
- 🐛 グリッド配置の多言語化を修正
- 🐛 キャッシュ管理サブテキストの多言語化を修正
- 📝 包括的な多言語化（言語ごとに90以上の文字列）

### 以前のバージョン
- 色付きタブ管理
- グローバルホットキーサポート
- システムトレイ統合
- ファビコンキャッシング

完全な変更履歴については[CHANGELOG.md](CHANGELOG.md)を参照してください。

---

<div align="center">

**Windowsパワーユーザーのために❤️で作られました**

[⬆ トップに戻る](#slauncher)

</div>
