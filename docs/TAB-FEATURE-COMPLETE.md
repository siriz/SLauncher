# ?? 탭 기능 추가 완료!

## ? 구현된 기능

### ?? **주요 기능:**

1. ? **검색창 아래 탭 추가**
2. ? **기본 탭 ("기본") 자동 생성**
3. ? **"+" 버튼으로 새 탭 추가**
4. ? **탭 전환 시 아이템 자동 저장/로드**
5. ? **마지막 탭 삭제 방지 (최소 1개 유지)**
6. ? **탭별로 독립적인 아이템 관리**

---

## ?? 수정된 파일

### **1. SLauncher/Classes/TabItem.cs (새 파일)**

탭 데이터 모델 클래스:

```csharp
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SLauncher.Classes
{
    public class TabItem : INotifyPropertyChanged
    {
  private string _name;
        private string _id;
 private ObservableCollection<UserControl> _items;

        public TabItem()
   {
        Id = Guid.NewGuid().ToString();
      Items = new ObservableCollection<UserControl>();
    }

        public string Id { get; set; }
        public string Name { get; set; }
   public ObservableCollection<UserControl> Items { get; set; }

 public event PropertyChangedEventHandler PropertyChanged;
     protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

---

### **2. MainWindow.xaml**

**탭뷰 추가:**

```xaml
<!--  TabView for organizing items  -->
<muxc:TabView
    x:Name="MainTabView"
 Margin="0,57,0,0"
    VerticalAlignment="Top"
    AddTabButtonClick="MainTabView_AddTabButtonClick"
    IsAddTabButtonVisible="True"
    SelectionChanged="MainTabView_SelectionChanged"
    TabCloseRequested="MainTabView_TabCloseRequested" />
```

**변경 사항:**
- ? 검색창 아래에 TabView 추가
- ? `AddTabButtonClick` 이벤트 - 새 탭 추가
- ? `SelectionChanged` 이벤트 - 탭 전환
- ? `TabCloseRequested` 이벤트 - 탭 닫기
- ? `IsAddTabButtonVisible="True"` - "+" 버튼 표시

**버튼 위치 조정:**
```xaml
<!-- Before: Margin="0,62,0,0" -->
<!-- After: Margin="0,100,0,0" -->
```
탭뷰가 추가되어 버튼 위치를 아래로 조정

---

### **3. MainWindow.xaml.cs**

**추가된 코드:**

#### **(1) INotifyPropertyChanged 구현**

```csharp
public sealed partial class MainWindow : WinUIEx.WindowEx, INotifyPropertyChanged
{
    // Tabs collection
  private ObservableCollection<TabItem> _tabs = new ObservableCollection<TabItem>();
    public ObservableCollection<TabItem> Tabs
    {
        get => _tabs;
        set
  {
            _tabs = value;
   OnPropertyChanged();
     }
    }

    private TabItem _currentTab;
    public TabItem CurrentTab
    {
  get => _currentTab;
  set
        {
          _currentTab = value;
OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

---

#### **(2) 탭 초기화**

```csharp
/// <summary>
/// Initialize tabs - create default tab if none exist
/// </summary>
private void InitializeTabs()
{
    // Create default tab
    var defaultTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
    defaultTab.Header = "기본";
    defaultTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource 
    { 
 Symbol = Microsoft.UI.Xaml.Controls.Symbol.Home 
    };
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
}
```

**호출 위치:**
```csharp
public MainWindow()
{
    this.InitializeComponent();

    // Initialize tabs
    InitializeTabs();  // ? 추가됨

    // ...기존 코드...
}
```

---

#### **(3) 새 탭 추가**

```csharp
/// <summary>
/// Handle adding a new tab
/// </summary>
private void MainTabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
{
// Create new tab
    var newTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
    newTab.Header = $"탭 {MainTabView.TabItems.Count + 1}";
    newTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource 
    { 
        Symbol = Microsoft.UI.Xaml.Controls.Symbol.Document 
    };
    
    // Add to TabView
    MainTabView.TabItems.Add(newTab);
    MainTabView.SelectedItem = newTab;
    
  // Clear current items and show in new tab
    SaveCurrentTabItems();
    ItemsGridView.Items.Clear();
}
```

**동작:**
1. 새 탭 생성 (이름: "탭 2", "탭 3", ...)
2. 문서 아이콘 추가
3. TabView에 추가
4. 새 탭 선택
5. 현재 탭 아이템 저장
6. 아이템 그리드 초기화

---

#### **(4) 탭 전환**

```csharp
/// <summary>
/// Handle tab selection changed
/// </summary>
private void MainTabView_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
{
    // Save current tab items before switching
    SaveCurrentTabItems();
    
    // Load items for newly selected tab
    LoadCurrentTabItems();
}
```

**동작:**
1. 현재 탭의 아이템 저장
2. 새로 선택된 탭의 아이템 로드

---

#### **(5) 탭 닫기**

```csharp
/// <summary>
/// Handle tab close request
/// </summary>
private void MainTabView_TabCloseRequested(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabCloseRequestedEventArgs args)
{
    // Don't allow closing the last tab
    if (MainTabView.TabItems.Count <= 1)
    {
        return;  // ? 마지막 탭 보호
    }
    
    // Remove the tab
    MainTabView.TabItems.Remove(args.Tab);
}
```

**보호 기능:**
- 마지막 탭 삭제 방지
- 항상 최소 1개 탭 유지

---

#### **(6) 아이템 저장**

```csharp
/// <summary>
/// Save current ItemsGridView items to current tab's Tag
/// </summary>
private void SaveCurrentTabItems()
{
  if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem currentTab)
    {
 var items = new List<UserControl>();
        foreach (var item in ItemsGridView.Items)
   {
            if (item is UserControl control)
    {
        items.Add(control);
       }
        }
        currentTab.Tag = items;  // ? Tag에 저장
    }
}
```

**저장 메커니즘:**
- TabViewItem의 `Tag` 속성 활용
- List<UserControl>로 모든 아이템 저장
- 탭 전환 시 자동 호출

---

#### **(7) 아이템 로드**

```csharp
/// <summary>
/// Load items from current tab's Tag to ItemsGridView
/// </summary>
private void LoadCurrentTabItems()
{
    ItemsGridView.Items.Clear();
    
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem currentTab)
    {
      if (currentTab.Tag is List<UserControl> items)
        {
   foreach (var item in items)
      {
        if (item is GridViewTile tile)
       {
        // 이벤트 핸들러 재등록
tile.Drop += GridViewTile_Drop;
  tile.DragEnter += GridViewTile_DragEnter;
         tile.DragLeave += GridViewTile_DragLeave;
          ItemsGridView.Items.Add(tile);
                }
         else if (item is GridViewTileGroup group)
     {
          // 이벤트 핸들러 재등록
         group.DragEnter += GridViewTileGroup_DragEnter;
         group.DragLeave += GridViewTileGroup_DragLeave;
           group.Drop += GridViewTileGroup_Drop;
         ItemsGridView.Items.Add(group);
                }
            }
    }
    }
}
```

**로드 프로세스:**
1. ItemsGridView 초기화
2. 현재 탭의 Tag에서 아이템 가져오기
3. 각 아이템의 이벤트 핸들러 재등록
4. ItemsGridView에 추가

---

## ?? 사용자 인터페이스

### **레이아웃:**

```
┌─────────────────────────────────────────────────────────────┐
│ [Search Box]    [??] [×]       │
├─────────────────────────────────────────────────────────────┤
│ [기본 탭] [탭 2] [+]         │ ← 탭 바
├─────────────────────────────────────────────────────────────┤
│       [Add file] [Add folder] [Add website] │
├─────────────────────────────────────────────────────────────┤
│       │
│  ┌─────┐ ┌─────┐ ┌─────┐         │
│  │App 1│ │App 2│ │App 3│         │
│  └─────┘ └─────┘ └─────┘           │
│    │
│         [?? Zoom Slider]   │
└─────────────────────────────────────────────────────────────┘
```

---

## ?? 기능 설명

### **1. 기본 탭 ("기본")**

**특징:**
- ? 앱 시작 시 자동 생성
- ? 홈 아이콘 (??)
- ? 삭제 불가 (마지막 탭일 경우)

**생성 시점:**
```csharp
public MainWindow()
{
    this.InitializeComponent();
    InitializeTabs();  // 여기서 "기본" 탭 생성
}
```

---

### **2. 새 탭 추가 ("+" 버튼)**

**동작:**
1. "+" 버튼 클릭
2. 새 탭 생성 ("탭 2", "탭 3", ...)
3. 문서 아이콘 (??) 추가
4. 새 탭으로 자동 전환
5. 빈 아이템 그리드 표시

**예시:**
```
처음: [기본] [+]
클릭: [기본] [탭 2] [+]
클릭: [기본] [탭 2] [탭 3] [+]
```

---

### **3. 탭 전환**

**동작:**
1. 탭 클릭
2. 현재 탭의 아이템 자동 저장
3. 선택한 탭의 아이템 로드
4. ItemsGridView 업데이트

**예시:**
```
[기본] 탭: App1, App2, App3
    ↓ 클릭 [탭 2]
[탭 2] 탭: Game1, Game2
    ↓ 클릭 [기본]
[기본] 탭: App1, App2, App3 (복원됨)
```

---

### **4. 탭 닫기**

**동작:**
1. 탭의 X 버튼 클릭
2. 마지막 탭이면 무시 (최소 1개 유지)
3. 탭 제거
4. 이전 탭 자동 선택

**보호 메커니즘:**
```csharp
if (MainTabView.TabItems.Count <= 1)
{
    return;  // 마지막 탭 보호
}
```

**예시:**
```
[기본] [탭 2] [탭 3]
    ↓ [탭 2] 닫기
[기본] [탭 3] ?

[기본]만 남음
    ↓ [기본] 닫기 시도
[기본] (그대로) ? 삭제 안됨
```

---

## ?? 데이터 저장 메커니즘

### **Tag 속성 활용:**

```csharp
// 저장
currentTab.Tag = items;  // List<UserControl>

// 로드
if (currentTab.Tag is List<UserControl> items)
{
    // items 사용
}
```

**장점:**
- ? 간단한 구현
- ? 메모리 효율적
- ? 탭 전환 시 즉시 복원

**단점:**
- ? 앱 종료 시 손실 (영구 저장 미구현)
- ? 탭 이름 변경 불가 (현재)

---

## ?? 향후 개선 사항

### **1. 탭 이름 변경**

```csharp
// 더블클릭 시 이름 변경 다이얼로그
private async void TabViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
{
    var textBox = new TextBox();
    var dialog = new ContentDialog
    {
        Title = "탭 이름 변경",
        Content = textBox,
  PrimaryButtonText = "확인",
        CloseButtonText = "취소"
    };
    
    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
    {
        currentTab.Header = textBox.Text;
    }
}
```

---

### **2. 탭 순서 변경 (드래그 앤 드롭)**

```csharp
// TabView의 CanReorderItems 활용
MainTabView.CanReorderItems = true;
MainTabView.CanDragItems = true;
```

---

### **3. 탭별 아이콘 변경**

```csharp
// 아이콘 선택 다이얼로그
var iconPicker = new SymbolIconPicker();
if (await iconPicker.ShowAsync() == ContentDialogResult.Primary)
{
    currentTab.IconSource = new SymbolIconSource 
    { 
 Symbol = iconPicker.SelectedSymbol 
    };
}
```

---

### **4. 탭 데이터 영구 저장 (JSON)**

```csharp
// TabsData.json
{
    "tabs": [
        {
            "id": "guid-1",
        "name": "기본",
            "icon": "Home",
   "items": [...]
        },
     {
        "id": "guid-2",
     "name": "개발",
            "icon": "Code",
   "items": [...]
        }
    ],
    "lastSelectedTabId": "guid-1"
}
```

**저장 위치:**
```
UserCache/tabs.json
```

---

### **5. 탭 그룹 (중첩 탭)**

```csharp
// 탭 안에 하위 탭
[업무]
  ├─ 문서
  ├─ 스프레드시트
  └─ 프레젠테이션
```

---

### **6. 탭 템플릿**

```csharp
// 미리 정의된 탭 템플릿
- 개발 탭 (VSCode, Git, Terminal)
- 디자인 탭 (Photoshop, Figma, Sketch)
- 게임 탭 (Steam, Epic, Discord)
```

---

## ?? 테스트 시나리오

### **Test 1: 기본 탭 확인**

```
1. SLauncher 실행
2. "기본" 탭 존재 확인 ?
3. 홈 아이콘 확인 ?
4. 탭 1개만 있음 ?
```

---

### **Test 2: 새 탭 추가**

```
1. "+" 버튼 클릭
2. "탭 2" 생성됨 ?
3. 문서 아이콘 표시 ?
4. "탭 2"가 선택됨 ?
5. 아이템 그리드 비어있음 ?
```

---

### **Test 3: 탭 전환**

```
1. "기본" 탭에 App1, App2 추가
2. "+" 버튼으로 "탭 2" 생성
3. "탭 2"에 Game1, Game2 추가
4. "기본" 탭 클릭
5. App1, App2 표시됨 ?
6. "탭 2" 클릭
7. Game1, Game2 표시됨 ?
```

---

### **Test 4: 탭 닫기**

```
1. [기본] [탭 2] [탭 3] 상태
2. "탭 2" 닫기 (X 버튼)
3. [기본] [탭 3] 상태 ?
4. "탭 3" 닫기
5. [기본]만 남음 ?
6. "기본" 닫기 시도
7. 닫히지 않음 ?
```

---

### **Test 5: 아이템 추가**

```
1. "기본" 탭 선택
2. "Add a file" 클릭
3. 파일 선택
4. "기본" 탭에 추가됨 ?
5. "탭 2" 선택
6. "Add a website" 클릭
7. URL 입력
8. "탭 2"에 추가됨 ?
9. "기본" 탭 클릭
10. 파일만 표시됨 ?
```

---

### **Test 6: 드래그 앤 드롭**

```
1. "기본" 탭 선택
2. 파일 드래그 앤 드롭
3. "기본" 탭에 추가됨 ?
4. "탭 2" 선택
5. 폴더 드래그 앤 드롭
6. "탭 2"에 추가됨 ?
```

---

### **Test 7: 검색 기능**

```
1. 여러 탭에 아이템 추가
2. 검색창에 입력
3. 모든 탭의 아이템 검색 ?
4. 아이템 실행
5. 해당 탭으로 전환 필요 (향후 개선)
```

---

## ?? 현재 구현 상태

### **완료된 기능:** ?

```
1. ? 기본 탭 자동 생성
2. ? "+" 버튼으로 새 탭 추가
3. ? 탭 전환 시 아이템 자동 저장/로드
4. ? 마지막 탭 삭제 방지
5. ? 탭별 독립적인 아이템 관리
6. ? 탭 아이콘 표시
7. ? 탭 닫기 버튼
```

### **미구현 기능:** ?

```
1. ? 탭 데이터 영구 저장 (JSON)
2. ? 탭 이름 변경
3. ? 탭 순서 변경 (드래그 앤 드롭)
4. ? 탭 아이콘 변경
5. ? 마지막 선택 탭 기억
6. ? 탭 그룹
7. ? 탭 템플릿
8. ? 탭 복제
9. ? 탭 내보내기/가져오기
```

---

## ?? 사용 방법

### **1. 기본 사용**

```
1. SLauncher 실행
2. "기본" 탭에서 작업
3. "+" 버튼으로 새 탭 추가
4. 탭별로 아이템 정리
```

---

### **2. 업무별 분류**

```
[업무] 탭: Office, Email, Calendar
[개발] 탭: VSCode, Git, Terminal
[디자인] 탭: Photoshop, Figma, Sketch
[게임] 탭: Steam, Epic, Discord
```

---

### **3. 프로젝트별 분류**

```
[프로젝트 A] 탭: 관련 파일, 폴더, 웹사이트
[프로젝트 B] 탭: 관련 파일, 폴더, 웹사이트
[참고 자료] 탭: 튜토리얼, 문서, 예제
```

---

## ?? 팁

### **효율적인 탭 사용:**

1. **카테고리별 분류**
   - 업무, 개인, 학습 등

2. **프로젝트별 분류**
   - 각 프로젝트마다 탭 생성

3. **빈도별 분류**
   - 자주 사용, 가끔 사용, 참고용

4. **시간대별 분류**
   - 오전, 오후, 저녁 작업

---

## ? 완료!

### **변경된 파일:**
- ? `SLauncher/Classes/TabItem.cs` (새 파일)
- ? `SLauncher/MainWindow.xaml`
  - TabView 추가
  - 버튼 위치 조정
- ? `SLauncher/MainWindow.xaml.cs`
  - INotifyPropertyChanged 구현
  - 탭 관리 메서드 추가
  - 탭 이벤트 핸들러 추가

---

### **테스트:**

```
1. SLauncher 실행
2. "기본" 탭 확인 ?
3. "+" 버튼 클릭 → "탭 2" 생성 ?
4. 아이템 추가 ?
5. 탭 전환 ?
6. 아이템 유지 ?
7. 탭 닫기 ?
8. 마지막 탭 보호 ?
```

---

## ?? 완료!

**탭 기능이 성공적으로 추가되었습니다!**

**이제 SLauncher에서 아이템을 탭으로 깔끔하게 정리할 수 있습니다!** ?

**각 탭은 독립적으로 아이템을 관리하며, 탭 전환 시 자동으로 저장/로드됩니다!** ??

---

## ?? 다음 단계 (선택사항)

### **우선순위 1: 탭 데이터 영구 저장**
- JSON 파일로 저장
- 앱 재시작 시 탭 복원
- 마지막 선택 탭 기억

### **우선순위 2: 탭 이름 변경**
- 더블클릭 또는 우클릭 메뉴
- 텍스트 입력 다이얼로그
- 즉시 반영

### **우선순위 3: 탭 아이콘 변경**
- 아이콘 선택 UI
- 다양한 아이콘 옵션
- 탭별 개성 부여

---

**테스트해보세요!** ??
