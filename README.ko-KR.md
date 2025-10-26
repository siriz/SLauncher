# SLauncher

<div align="center">

**Windows용 모던 멀티 언어 앱 런처**

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)
[![WinUI 3](https://img.shields.io/badge/WinUI-3-0078D4)](https://microsoft.github.io/microsoft-ui-xaml/)

**언어:** [English](README.md) ? [한국어](README.ko-KR.md) ? 日本語 (준비중)

[기능](#-기능) ? [설치](#-설치) ? [사용법](#-사용법) ? [소스 빌드](#%EF%B8%8F-소스에서-빌드) ? [기여](#-기여하기)

</div>

---

## ?? 기능

### 핵심 기능
- ? **빠른 실행** - 즐겨 사용하는 앱, 폴더, 웹사이트를 즉시 실행
- ?? **전역 단축키** - 어디서나 런처 호출 (기본: `Ctrl + Space`)
- ?? **탭 구성** - 여러 탭으로 항목 정리 (이름 및 색상 변경 가능)
- ??? **드래그 앤 드롭** - 직관적인 항목 관리
- ?? **스마트 검색** - 항목 검색 또는 직접 파일/폴더/URL 열기
- ?? **아이콘 크기 조절** - 슬라이더 또는 `Ctrl + 마우스 휠` (0.25x - 6.00x)

### 다국어 지원
- ???? **English** (영어)
- ???? **한국어**
- ???? **日本語** (일본어)
- ? 실시간 언어 전환 - 재시작 불필요!

### 모던 UI
- ?? **Windows 11 디자인** - Mica/Acrylic 효과를 사용한 네이티브 WinUI 3
- ?? **테마 지원** - 시스템 다크/라이트 테마 자동 추종
- ?? **전체화면 모드** - 몰입형 런처 경험
- ?? **그리드 정렬** - 왼쪽 또는 가운데 정렬 선택
- ?? **탭 색상** - 8가지 프리셋 색상으로 탭 커스터마이징

### 성능 및 이식성
- ?? **포터블** - 실행 파일 폴더에 모든 데이터 저장 (`UserCache/`)
- ?? **빠른 시작** - 캐시된 데이터로 즉시 로딩
- ?? **파비콘 캐시** - 웹사이트 아이콘 로컬 캐싱
- ?? **가벼움** - 최소한의 리소스 사용

---

## ?? 설치

### 시스템 요구사항
- **Windows 10** 버전 1809 (빌드 17763) 이상
- **Windows 11** (최상의 경험을 위해 권장)
- **.NET 8.0 런타임** (자체 포함 빌드에 포함)

### 빠른 설치
1. [Releases](https://github.com/siriz/SLauncher/releases)에서 최신 릴리스 다운로드
2. ZIP 파일을 원하는 폴더에 압축 해제
3. `SLauncher.exe` 실행
4. (선택사항) 설정에서 "Windows 시작 시 실행" 활성화

### 포터블 모드
모든 설정과 데이터는 실행 파일 옆 `UserCache` 폴더에 저장됩니다:
```
SLauncher/
├── SLauncher.exe
└── UserCache/
    ├── Settings/      # 사용자 설정
    ├── Files/         # 항목 데이터
    └── FaviconCache/  # 웹사이트 아이콘
```

---

## ?? 사용법

### 항목 추가

#### 방법 1: 버튼
- **파일 추가**: 버튼 클릭 후 `.exe`, `.lnk` 또는 모든 파일 선택
- **폴더 추가**: 버튼 클릭 후 폴더 선택
- **웹사이트 추가**: 버튼 클릭 후 URL 입력 (예: `https://github.com`)

#### 방법 2: 드래그 앤 드롭
- 파일, 폴더, 바로가기를 창에 직접 드래그
- 탭 간 드래그로 항목 이동
- 한 항목을 다른 항목 위에 드래그하여 그룹 생성

### 항목 관리

| 작업 | 방법 |
|------|------|
| **편집** | 항목 우클릭 → 편집 |
| **삭제** | 항목 우클릭 → 삭제 또는 `Delete` 키 |
| **그룹 생성** | 한 항목을 다른 항목 위에 드래그 |
| **순서 변경** | 항목을 새 위치로 드래그 |

### 탭 관리

#### 탭 생성
- 탭 옆 **+** 버튼 클릭
- 각 탭은 서로 다른 항목과 설정 가능

#### 탭 옵션 (탭 우클릭)
- **이름 변경** - 탭에 사용자 지정 이름 부여
- **색상 변경** - 8가지 프리셋 색상 중 선택
- **삭제** - 탭 제거 (항목이 있으면 확인 필요)

### 검색

검색창은 여러 입력 유형을 지원합니다:

| 입력 유형 | 예시 | 결과 |
|-----------|------|------|
| **항목 이름** | `메모장` | 모든 항목 검색 |
| **파일 경로** | `C:\Windows\notepad.exe` | 파일 직접 열기 |
| **폴더 경로** | `C:\Users\Documents` | 탐색기에서 폴더 열기 |
| **웹사이트 URL** | `https://google.com` | 기본 브라우저로 열기 |
| **검색 쿼리** | `search:키워드` | 기본 브라우저로 검색 |

### 키보드 단축키

| 단축키 | 동작 |
|--------|------|
| `Ctrl + Space` | 런처 표시/숨김 (설정에서 변경 가능) |
| `Ctrl + 마우스 휠` | 아이콘 크기 조절 (창 전체에서 작동) |
| `Delete` | 선택한 항목 삭제 |
| `Esc` | 런처 닫기 (전체화면 모드) |
| `Enter` | 첫 번째 검색 결과 열기 |

### 아이콘 크기 조절

다음 방법으로 아이콘 크기 조절:
- 오른쪽 하단의 **슬라이더**
- 창 어디서나 **Ctrl + 마우스 휠**
- 범위: 0.25x ~ 6.00x

---

## ?? 설정

### 일반 설정
- **전체화면 활성화** - 전체화면 모드로 런처 사용
- **그리드 정렬** - 왼쪽 (너비 채움) 또는 가운데 (고정 너비)
- **Windows 시작 시 실행** - 시작 시 자동 실행
- **전역 단축키** - 표시/숨김 단축키 커스터마이징
  - 수식어: Ctrl, Alt, Shift, Ctrl+Shift, Ctrl+Alt
  - 키: Space, Tab, Enter, Esc, F1-F4
- **언어** - 선호 언어 선택 (영어, 한국어, 일본어)

### 캐시 관리
- **캐시 크기 보기** - 파비콘 캐시 사용량 모니터링
- **캐시 지우기** - 모든 캐시된 웹사이트 아이콘 제거
- **캐시 폴더 열기** - 캐시 디렉토리 직접 접근
- **캐시 위치** - 포터블: `UserCache\FaviconCache\`

---

## ??? 소스에서 빌드

### 필요 사항
- **Visual Studio 2022** (17.8 이상)
  - 워크로드: ".NET 데스크톱 개발"
  - 구성 요소: "Windows App SDK C# 템플릿"
- **Windows App SDK 1.5** 이상
- **.NET 8.0 SDK**

### 클론 및 빌드

```bash
# 저장소 클론
git clone https://github.com/siriz/SLauncher.git
cd SLauncher

# NuGet 패키지 복원
dotnet restore

# 솔루션 빌드
dotnet build -c Release

# 또는 Visual Studio에서 열기
start SLauncher.sln
```

### 프로젝트 구조

```
SLauncher/
├── SLauncher/         # 메인 WinUI 3 프로젝트
│   ├── Classes/            # 핵심 클래스
│   │   ├── LocalizationManager.cs   # 다국어 지원
│   │   ├── UserSettingsClass.cs     # 설정 관리
│   │   ├── GlobalHotkeyManager.cs   # 단축키 등록
│   │   └── IconHelpers.cs  # 아이콘 추출 및 캐시
│   ├── Controls/    # 사용자 지정 컨트롤
│   │   ├── GridViewTile.xaml        # 앱 타일 컨트롤
│   │   ├── GridViewTileGroup.xaml   # 그룹 컨트롤
│   │   └── AboutSectionControl.xaml # 정보 페이지
│ ├── Strings/            # 다국어 리소스
│   │   ├── en-US/Resources.resw     # 영어
│ │   ├── ko-KR/Resources.resw     # 한국어
│   │   └── ja-JP/Resources.resw   # 일본어
│   ├── MainWindow*.cs      # 메인 창 (부분 클래스)
│   │   ├── MainWindow.xaml.cs       # 메인 로직
│   │   ├── MainWindow.UI.cs         # UI 관리
│   │   ├── MainWindow.Tabs.cs       # 탭 관리
│   │   ├── MainWindow.Items.cs      # 항목 관리
│   │   ├── MainWindow.DragDrop.cs   # 드래그 앤 드롭
│   │   ├── MainWindow.Search.cs     # 검색 로직
│   │   └── MainWindow.Hotkeys.cs    # 단축키 및 트레이
│   └── SettingsWindow*.cs  # 설정 창 (부분 클래스)
│       ├── SettingsWindow.xaml.cs      # 메인 로직
│       ├── SettingsWindow.Localization.cs # 언어 UI
│     ├── SettingsWindow.Cache.cs        # 캐시 관리
│       ├── SettingsWindow.Hotkey.cs       # 단축키 설정
│       └── SettingsWindow.Settings.cs     # 설정 토글
└── WinFormsClassLibrary/   # 헬퍼 라이브러리 (파일 대화상자)
```

### 부분 클래스 패턴

`MainWindow`와 `SettingsWindow` 모두 더 나은 코드 구성을 위해 부분 클래스를 사용합니다:
- 각 부분 클래스 파일이 특정 기능 영역을 처리
- 코드 탐색 및 유지 관리가 쉬움
- 일관성을 위해 `MainWindow`와 동일한 패턴 따름

---

## ?? 다국어 지원

### 지원 언어

| 언어 | 코드 | 상태 | 리소스 |
|------|------|------|--------|
| ???? English | en-US | ? 완료 | 90 문자열 |
| ???? 한국어 | ko-KR | ? 완료 | 91 문자열 |
| ???? 日本語 | ja-JP | ? 완료 | 91 문자열 |

### 새 언어 추가

1. **리소스 파일 생성**
   ```
   SLauncher/Strings/{언어-코드}/Resources.resw
   ```

2. **템플릿 복사**
   ```bash
   cp SLauncher/Strings/en-US/Resources.resw SLauncher/Strings/{언어-코드}/
   ```

3. **번역**
   - Visual Studio에서 `Resources.resw` 열기
   - `<value>` 내용 번역 (`<data name>`은 변경하지 않음)
   - UI 레이아웃 테스트 (일부 언어는 더 김)

4. **설정에 추가**
   ```xaml
   <!-- SettingsWindow.xaml -->
   <ComboBox x:Name="LanguageComboBox">
       <ComboBoxItem Content="귀하의 언어 이름" Tag="{언어-코드}" />
   </ComboBox>
   ```

5. **테스트**
   - 빌드 및 실행
   - 설정 → 언어에서 새 언어 선택
   - 모든 UI 요소가 올바르게 표시되는지 확인

### 번역 가이드라인
- 플레이스홀더 유지: `{0}`, `{1}` (문자열 형식에 사용)
- 줄 바꿈 및 형식 유지
- 고유 관례 사용 (구두점, 따옴표)
- 긴 번역으로 테스트 (레이아웃에 영향을 줄 수 있음)
- 기술 용어 일관성 유지 (예: "캐시", "단축키")

---

## ?? 기여하기

기여를 환영합니다! 다음과 같이 도울 수 있습니다:

### 기여 방법
- ?? **번역** - 새 언어 추가 또는 기존 언어 개선
- ?? **버그 리포트** - [GitHub Issues](https://github.com/siriz/SLauncher/issues)에 문제 보고
- ? **기능 제안** - 새 기능 제안
- ?? **코드** - 풀 리퀘스트 제출
- ?? **문서** - README 또는 코드 주석 개선

### 개발 워크플로
1. **포크** 저장소
2. **클론** 포크
   ```bash
   git clone https://github.com/your-username/SLauncher.git
   ```
3. **생성** 기능 브랜치
 ```bash
   git checkout -b feature/AmazingFeature
   ```
4. **변경** 및 철저히 테스트
5. **커밋** 명확한 메시지와 함께
   ```bash
   git commit -m "feat: Add amazing feature"
   ```
6. **푸시** 포크로
   ```bash
   git push origin feature/AmazingFeature
   ```
7. **열기** 풀 리퀘스트

### 코드 스타일 가이드라인
- 기존 코드 패턴 따르기
- 의미 있는 변수/메서드 이름 사용
- 공개 메서드에 XML 주석 추가
- 메서드 집중 유지 (단일 책임)
- 큰 파일에 부분 클래스 사용
- 모든 UI 텍스트에 다국어 지원 추가

---

## ?? 라이선스

이 프로젝트는 MIT 라이선스에 따라 라이선스가 부여됩니다 - 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하세요.

### 서드파티 라이브러리
- **WinUI 3** - MIT 라이선스
- **CommunityToolkit.WinUI** - MIT 라이선스
- **WinUIEx** - MIT 라이선스
- **System.Drawing.Common** - MIT 라이선스

---

## ?? 감사의 말

- **기반**: Lolle2000la의 [LauncherX](https://github.com/Lolle2000la/LauncherX)
- **UI 프레임워크**: [WinUI 3](https://microsoft.github.io/microsoft-ui-xaml/)
- **Community Toolkit**: [Windows Community Toolkit](https://github.com/CommunityToolkit/Windows)
- **창 관리**: [WinUIEx](https://github.com/dotMorten/WinUIEx)
- **아이콘**: [Segoe Fluent Icons](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)

---

## ?? 지원

- **Issues**: [GitHub Issues](https://github.com/siriz/SLauncher/issues)
- **토론**: [GitHub Discussions](https://github.com/siriz/SLauncher/discussions)
- **이메일**: your.email@example.com

---

## ?? 변경 로그

### v1.0.0 (최신)
- ? 다국어 지원 (영어, 한국어, 일본어)
- ? 실시간 언어 전환
- ? 더 나은 코드 구성을 위한 부분 클래스 리팩토링
- ? 창 전체에서 Ctrl+마우스휠 아이콘 크기 조절
- ?? 그리드 정렬 다국어 수정
- ?? 캐시 관리 서브텍스트 다국어 수정
- ?? 포괄적인 다국어 지원 (언어당 90개 이상 문자열)

### 이전 버전
- 색상이 있는 탭 관리
- 전역 단축키 지원
- 시스템 트레이 통합
- 파비콘 캐싱

전체 변경 로그는 [CHANGELOG.md](CHANGELOG.md)를 참조하세요.

---

<div align="center">

**Windows 파워 유저를 위해 ??로 만들었습니다**

[? 맨 위로](#slauncher)

</div>
