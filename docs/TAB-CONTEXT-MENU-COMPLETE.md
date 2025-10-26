# ?? 탭 컨텍스트 메뉴 추가 완료!

## ? 구현된 기능

### ?? **핵심 기능:**

1. ? **탭 우클릭 → 컨텍스트 메뉴**
2. ? **이름 변경** (텍스트 입력 다이얼로그)
3. ? **삭제** (확인 다이얼로그 + 아이템 개수 표시)
4. ? **마지막 탭 삭제 방지**
5. ? **아이템 개수 경고** (1개 이상일 때)

---

## ?? 수정된 코드

### **1. InitializeTabs() - 기본 탭에 컨텍스트 메뉴 추가**

```csharp
private void InitializeTabs()
{
    // Create default tab
    var defaultTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
    defaultTab.Header = "기본";
    defaultTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource 
    { 
   Symbol = Microsoft.UI.Xaml.Controls.Symbol.Home 
 };
 
    // ? Add context menu to tab
    AttachTabContextMenu(defaultTab);
   
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    
    // Set as previous tab
    _previousTab = defaultTab;
}
```

---

### **2. MainTabView_AddTabButtonClick() - 새 탭에 컨텍스트 메뉴 추가**

```csharp
private void MainTabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
{
    // Save current tab items before creating new tab
    SaveCurrentTabItems();
    
  // Create new tab
    var newTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
    newTab.Header = $"탭 {MainTabView.TabItems.Count + 1}";
    newTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource 
    { 
        Symbol = Microsoft.UI.Xaml.Controls.Symbol.Document 
    };
    
    // ? Add context menu to tab
    AttachTabContextMenu(newTab);
    
    // Add to TabView
    MainTabView.TabItems.Add(newTab);
    MainTabView.SelectedItem = newTab;
   
    // Clear items for new tab
    ItemsGridView.Items.Clear();
}
```

---

### **3. AttachTabContextMenu() - 컨텍스트 메뉴 생성**

```csharp
/// <summary>
/// Attach context menu to a tab
/// </summary>
private void AttachTabContextMenu(Microsoft.UI.Xaml.Controls.TabViewItem tab)
{
    var contextMenu = new MenuFlyout();
    
    // ? Rename menu item
    var renameItem = new MenuFlyoutItem
    {
   Text = "이름 변경",
  Icon = new SymbolIcon(Symbol.Rename)
    };
    renameItem.Click += (s, e) => RenameTab_Click(tab);
    contextMenu.Items.Add(renameItem);
    
    // ? Delete menu item
    var deleteItem = new MenuFlyoutItem
    {
   Text = "삭제",
   Icon = new SymbolIcon(Symbol.Delete)
    };
    deleteItem.Click += (s, e) => DeleteTab_Click(tab);
    contextMenu.Items.Add(deleteItem);
    
    tab.ContextFlyout = contextMenu;
}
```

**메뉴 항목:**
1. **이름 변경** - ?? 아이콘
2. **삭제** - ??? 아이콘

---

### **4. RenameTab_Click() - 탭 이름 변경**

```csharp
/// <summary>
/// Handle tab rename
/// </summary>
private async void RenameTab_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab)
{
    var textBox = new TextBox
    {
    Text = tab.Header?.ToString() ?? "",
        PlaceholderText = "탭 이름 입력",
  Width = 250
    };
    
    var dialog = new ContentDialog
    {
  Title = "탭 이름 변경",
Content = textBox,
        PrimaryButtonText = "확인",
      CloseButtonText = "취소",
    DefaultButton = ContentDialogButton.Primary,
        XamlRoot = this.Content.XamlRoot
    };
   
    var result = await dialog.ShowAsync();
    
    if (result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(textBox.Text))
    {
        tab.Header = textBox.Text;  // ? 이름 업데이트
    }
}
```

**동작:**
1. 현재 탭 이름을 TextBox에 표시
2. 사용자가 새 이름 입력
3. "확인" 클릭 시 탭 이름 변경
4. "취소" 또는 빈 입력 시 변경 안됨

---

### **5. DeleteTab_Click() - 탭 삭제 (핵심!)**

```csharp
/// <summary>
/// Handle tab delete
/// </summary>
private async void DeleteTab_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab)
{
    // ? 1. Don't allow deleting the last tab
    if (MainTabView.TabItems.Count <= 1)
  {
     var errorDialog = new ContentDialog
    {
    Title = "삭제 불가",
            Content = "마지막 탭은 삭제할 수 없습니다.",
            CloseButtonText = "확인",
         XamlRoot = this.Content.XamlRoot
        };
        await errorDialog.ShowAsync();
        return;
    }
    
    // ? 2. Count items in the tab
    int itemCount = 0;
    if (tab.Tag is List<UserControl> items)
    {
        itemCount = items.Count;
    }
    else if (tab == MainTabView.SelectedItem)
    {
        // If this is the current tab, count from ItemsGridView
        itemCount = ItemsGridView.Items.Count;
    }
    
    // ? 3. Show confirmation dialog with item count
    string message = itemCount > 0
        ? $"이 탭에는 {itemCount}개의 아이템이 있습니다.\n탭을 삭제하면 모든 아이템도 함께 삭제됩니다.\n\n정말 삭제하시겠습니까?"
    : "이 탭을 삭제하시겠습니까?";
    
    var confirmDialog = new ContentDialog
  {
   Title = "탭 삭제 확인",
    Content = message,
 PrimaryButtonText = "삭제",
        CloseButtonText = "취소",
   DefaultButton = ContentDialogButton.Close,  // ? 기본은 취소
   XamlRoot = this.Content.XamlRoot
    };
    
    var result = await confirmDialog.ShowAsync();
    
    if (result == ContentDialogResult.Primary)
    {
  // ? 4. If deleting current tab, save it first
        if (tab == MainTabView.SelectedItem)
    {
        SaveCurrentTabItems();
    }
        
        // ? 5. Remove the tab
        MainTabView.TabItems.Remove(tab);
   
        // ? 6. Update previous tab reference
        if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
        {
            _previousTab = selectedTab;
      }
    }
}
```

**동작 순서:**
1. 마지막 탭인지 확인 → 마지막이면 에러 다이얼로그
2. 탭의 아이템 개수 확인
3. 확인 다이얼로그 표시 (아이템 개수 포함)
4. "삭제" 클릭 시 탭 제거
5. 다음 탭으로 자동 전환

---

## ?? UI 예시

### **1. 탭 우클릭 → 컨텍스트 메뉴**

```
┌─────────────────┐
│ ?? 이름 변경    │
├─────────────────┤
│ ??? 삭제     │
└─────────────────┘
```

---

### **2. 이름 변경 다이얼로그**

```
┌──────────────────────────────────┐
│ 탭 이름 변경       │
├──────────────────────────────────┤
│ [기본   ]  ← 현재 이름│
│ ┌────────────────────────────┐   │
│ │  탭 이름 입력│   │
│ └────────────────────────────┘   │
│   │
│      [확인]  [취소]          │
└──────────────────────────────────┘
```

---

### **3. 삭제 확인 다이얼로그 (아이템 없을 때)**

```
┌──────────────────────────────────┐
│ 탭 삭제 확인     │
├──────────────────────────────────┤
│ 이 탭을 삭제하시겠습니까?  │
│            │
│           [삭제]  [취소]  │
└──────────────────────────────────┘
```

---

### **4. 삭제 확인 다이얼로그 (아이템 있을 때) - ?? 경고**

```
┌──────────────────────────────────┐
│ 탭 삭제 확인              │
├──────────────────────────────────┤
│ 이 탭에는 5개의 아이템이 있습니다.│
│ 탭을 삭제하면 모든 아이템도       │
│ 함께 삭제됩니다.       │
│            │
│ 정말 삭제하시겠습니까?        │
│ │
│           [삭제]  [취소]       │
└──────────────────────────────────┘
```

**특징:**
- ?? 아이템 개수 표시
- ?? "함께 삭제됩니다" 경고
- ? 기본 버튼은 "취소" (실수 방지)

---

### **5. 마지막 탭 삭제 시도 - 에러**

```
┌──────────────────────────────────┐
│ 삭제 불가     │
├──────────────────────────────────┤
│ 마지막 탭은 삭제할 수 없습니다.   │
│              │
│         [확인]   │
└──────────────────────────────────┘
```

---

## ?? 테스트 시나리오

### **Test 1: 탭 이름 변경**

```
1. 탭 우클릭 ?
2. "이름 변경" 클릭 ?
3. "개발" 입력 ?
4. "확인" 클릭 ?
5. 탭 이름이 "개발"로 변경됨 ?
```

---

### **Test 2: 빈 탭 삭제**

```
1. 새 탭 생성 (탭 2) ?
2. 아이템 추가 안함 ?
3. 탭 2 우클릭 ?
4. "삭제" 클릭 ?
5. "이 탭을 삭제하시겠습니까?" 다이얼로그 ?
6. "삭제" 클릭 ?
7. 탭 2 제거됨 ?
```

---

### **Test 3: 아이템 있는 탭 삭제 - 경고**

```
1. 기본 탭에 App1, App2, App3 추가 ?
2. 기본 탭 우클릭 ?
3. "삭제" 클릭 ?
4. "이 탭에는 3개의 아이템이 있습니다..." 다이얼로그 ?
5. "취소" 클릭 ?
6. 탭 유지됨 ?

다시 시도:
7. 기본 탭 우클릭 ?
8. "삭제" 클릭 ?
9. "삭제" 클릭 (확인) ?
10. 기본 탭 제거됨 ?
11. App1, App2, App3도 함께 삭제됨 ?
```

---

### **Test 4: 마지막 탭 삭제 시도 - 에러**

```
1. 탭이 1개만 남음 (기본 탭) ?
2. 기본 탭 우클릭 ?
3. "삭제" 클릭 ?
4. "마지막 탭은 삭제할 수 없습니다" 에러 다이얼로그 ?
5. "확인" 클릭 ?
6. 탭 유지됨 ?
```

---

### **Test 5: 현재 탭 vs 다른 탭 삭제**

```
상황:
- 기본 탭 (App1, App2)
- 탭 2 (Game1, Game2) ← 현재 선택됨

Test A: 다른 탭 삭제 (기본 탭)
1. 탭 2 선택 중 ?
2. 기본 탭 우클릭 ?
3. "삭제" 클릭 ?
4. "이 탭에는 2개의 아이템이 있습니다..." ?
5. "삭제" 클릭 ?
6. 기본 탭 제거됨 ?
7. 탭 2 그대로 유지 (Game1, Game2) ?

Test B: 현재 탭 삭제 (탭 2)
1. 탭 2 선택 중 ?
2. 탭 2 우클릭 ?
3. "삭제" 클릭 ?
4. "이 탭에는 2개의 아이템이 있습니다..." ?
5. "삭제" 클릭 ?
6. 탭 2 제거됨 ?
7. 자동으로 다른 탭 선택됨 ?
```

---

## ?? 기능 세부사항

### **1. 아이템 개수 확인 로직**

```csharp
int itemCount = 0;

// Case 1: 다른 탭 (Tag에 저장됨)
if (tab.Tag is List<UserControl> items)
{
    itemCount = items.Count;
}
// Case 2: 현재 탭 (ItemsGridView에 표시 중)
else if (tab == MainTabView.SelectedItem)
{
    itemCount = ItemsGridView.Items.Count;
}
```

**두 가지 경우:**
1. **다른 탭:** Tag에 저장된 아이템 개수
2. **현재 탭:** ItemsGridView의 아이템 개수

---

### **2. 경고 메시지 생성**

```csharp
string message = itemCount > 0
    ? $"이 탭에는 {itemCount}개의 아이템이 있습니다.\n" +
    $"탭을 삭제하면 모든 아이템도 함께 삭제됩니다.\n\n" +
      $"정말 삭제하시겠습니까?"
 : "이 탭을 삭제하시겠습니까?";
```

**조건부 메시지:**
- **아이템 있음:** 개수 + 경고
- **아이템 없음:** 간단한 확인

---

### **3. 기본 버튼 설정**

```csharp
DefaultButton = ContentDialogButton.Close  // ? 취소가 기본
```

**이유:**
- 실수로 Enter 눌러도 안전
- 명시적으로 "삭제" 클릭 필요

---

### **4. 컨텍스트 메뉴 아이콘**

```csharp
// 이름 변경
Icon = new SymbolIcon(Symbol.Rename)  // ??

// 삭제
Icon = new SymbolIcon(Symbol.Delete)  // ???
```

**시각적 힌트:**
- 아이콘으로 기능 직관적 표시
- WinUI 표준 심볼 사용

---

## ?? 사용자 시나리오

### **시나리오 1: 프로젝트별 탭 정리**

```
초기 상태:
[기본] [탭 2] [탭 3]

작업:
1. 기본 탭 우클릭 → 이름 변경 → "개발"
2. 탭 2 우클릭 → 이름 변경 → "디자인"
3. 탭 3 우클릭 → 이름 변경 → "문서"

결과:
[개발] [디자인] [문서] ?
```

---

### **시나리오 2: 불필요한 탭 정리**

```
초기 상태:
[업무] [개발] [임시] [테스트]

작업:
1. 임시 탭 우클릭 → 삭제 (아이템 없음)
2. "삭제" 확인
3. 테스트 탭 우클릭 → 삭제 (아이템 2개)
4. 경고 확인 → "삭제"

결과:
[업무] [개발] ?
```

---

### **시나리오 3: 실수 방지**

```
상황:
[개발] 탭에 중요한 프로젝트 20개

작업:
1. 개발 탭 우클릭 → 삭제
2. "이 탭에는 20개의 아이템이 있습니다..." ??
3. "취소" 클릭 (실수 깨달음)

결과:
탭 유지됨 ? (아이템 보존됨)
```

---

## ?? 다른 탭 기능과의 통합

### **기존 기능:**
1. ? 탭 추가 ("+" 버튼)
2. ? 탭 전환 (클릭)
3. ? 탭 닫기 (X 버튼)

### **새 기능:**
4. ? 탭 이름 변경 (컨텍스트 메뉴)
5. ? 탭 삭제 (컨텍스트 메뉴)

### **통합 동작:**

```
X 버튼 (TabCloseRequested):
- 즉시 삭제 (확인 없음)
- 마지막 탭 보호 ?

컨텍스트 메뉴 "삭제":
- 확인 다이얼로그 ?
- 아이템 개수 경고 ?
- 마지막 탭 보호 ?
```

**차이점:**
- **X 버튼:** 빠른 삭제
- **컨텍스트 메뉴:** 안전한 삭제 (경고 포함)

---

## ?? 구현 포인트

### **1. 람다식으로 탭 참조 전달**

```csharp
renameItem.Click += (s, e) => RenameTab_Click(tab);
deleteItem.Click += (s, e) => DeleteTab_Click(tab);
```

**이유:**
- 각 탭마다 고유한 핸들러 필요
- 람다로 tab 참조 캡처

---

### **2. async/await 다이얼로그**

```csharp
private async void RenameTab_Click(...)
{
    var result = await dialog.ShowAsync();
  // 다이얼로그 닫힐 때까지 대기
}
```

**장점:**
- UI 차단 안함
- 깔끔한 코드

---

### **3. 조건부 메시지**

```csharp
string message = itemCount > 0
    ? $"경고 메시지 (아이템 {itemCount}개)"
    : "단순 확인 메시지";
```

**효과:**
- 상황에 맞는 메시지
- 사용자 친화적

---

### **4. 현재 탭 vs 다른 탭 처리**

```csharp
if (tab == MainTabView.SelectedItem)
{
    SaveCurrentTabItems();  // 현재 탭만 저장
}
```

**이유:**
- 다른 탭은 이미 Tag에 저장됨
- 현재 탭만 ItemsGridView에 있음

---

## ? 완료!

### **추가된 기능:**
- ? 탭 컨텍스트 메뉴 (우클릭)
- ? 이름 변경 기능
- ? 삭제 기능 (확인 다이얼로그)
- ? 아이템 개수 경고
- ? 마지막 탭 삭제 방지

---

### **변경된 파일:**
- ? `SLauncher/MainWindow.xaml.cs`
  - `InitializeTabs()` 수정
  - `MainTabView_AddTabButtonClick()` 수정
  - `AttachTabContextMenu()` 새 메서드
  - `RenameTab_Click()` 새 메서드
  - `DeleteTab_Click()` 새 메서드

---

### **테스트:**

```
1. 탭 우클릭 ?
2. 컨텍스트 메뉴 표시 ?
3. "이름 변경" 클릭 → 다이얼로그 ?
4. 이름 변경 성공 ?
5. "삭제" 클릭 → 확인 다이얼로그 ?
6. 아이템 개수 표시 ?
7. 삭제 성공 ?
8. 마지막 탭 삭제 방지 ?
```

---

## ?? 완료!

**탭 컨텍스트 메뉴가 성공적으로 추가되었습니다!**

**이제 탭을 우클릭하여 이름을 변경하거나 삭제할 수 있습니다!** ?

**아이템이 있는 탭을 삭제하면 경고 메시지가 표시됩니다!** ??

**마지막 탭은 삭제할 수 없습니다!** ???

**테스트해보세요!** ??
