# ?? 탭 전환 시 아이템 분리 문제 해결!

## ? 문제

**증상:**
```
1. 기본 탭에 App1, App2 추가
2. "+" 버튼으로 탭 2 생성
3. 탭 2에 Game1, Game2 추가
4. 기본 탭으로 전환
5. ? 기본 탭에 Game1, Game2도 표시됨!
6. ? 두 탭이 같은 아이템 공유
```

**원인:**
```csharp
private void MainTabView_SelectionChanged(...)
{
    SaveCurrentTabItems();  // ? 현재 탭 저장 (이미 전환된 후)
    LoadCurrentTabItems();  // 새 탭 로드
}
```

문제: `SelectionChanged` 이벤트가 발생했을 때는 이미 새 탭으로 전환된 상태입니다!
- `MainTabView.SelectedItem` = 새로 선택한 탭 (탭 2)
- `ItemsGridView.Items` = 이전 탭의 아이템 (기본 탭)

따라서 새 탭(탭 2)의 Tag에 이전 탭(기본)의 아이템이 저장되어 버림!

---

## ? 해결 방법

### **핵심: 이전 탭 추적**

```csharp
// 이전에 선택되어 있던 탭을 추적
private Microsoft.UI.Xaml.Controls.TabViewItem _previousTab;
```

**동작 순서:**
```
1. 기본 탭 선택 중
   → _previousTab = 기본 탭
   → ItemsGridView = App1, App2

2. 탭 2 클릭 (SelectionChanged 발생)
   → SaveCurrentTabItems() 호출
   → _previousTab (기본 탭)의 Tag에 현재 ItemsGridView (App1, App2) 저장 ?
   → _previousTab = 탭 2로 업데이트
   → LoadCurrentTabItems() 호출
   → 탭 2의 Tag에서 아이템 로드 (비어있거나 Game1, Game2)

3. 기본 탭 클릭 (SelectionChanged 발생)
   → SaveCurrentTabItems() 호출
   → _previousTab (탭 2)의 Tag에 현재 ItemsGridView (Game1, Game2) 저장 ?
   → _previousTab = 기본 탭으로 업데이트
   → LoadCurrentTabItems() 호출
   → 기본 탭의 Tag에서 아이템 로드 (App1, App2) ?
```

---

## ?? 수정된 코드

### **1. 필드 추가**

```csharp
// Tab management - track previous tab for saving
private Microsoft.UI.Xaml.Controls.TabViewItem _previousTab;
```

---

### **2. InitializeTabs 수정**

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
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    
    // ? Set as previous tab
    _previousTab = defaultTab;
}
```

---

### **3. MainTabView_AddTabButtonClick 수정**

```csharp
private void MainTabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
{
    // ? Save current tab items BEFORE creating new tab
    SaveCurrentTabItems();
    
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
    
 // Clear items for new tab
    ItemsGridView.Items.Clear();
}
```

**변경 사항:**
- ? 새 탭 생성 전에 현재 탭 저장
- ? 명확한 주석 추가

---

### **4. MainTabView_SelectionChanged 수정 (핵심!)**

**Before (문제):**

```csharp
private void MainTabView_SelectionChanged(...)
{
    // ? 이미 새 탭으로 전환된 상태
SaveCurrentTabItems();  // 새 탭에 이전 탭 아이템 저장 ?
    
    // Load items for newly selected tab
    LoadCurrentTabItems();
}
```

**After (해결):**

```csharp
private void MainTabView_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
{
    // ? Save items from the PREVIOUS tab (before selection changed)
    if (_previousTab != null)
    {
      var items = new List<UserControl>();
        foreach (var item in ItemsGridView.Items)
        {
  if (item is UserControl control)
    {
           items.Add(control);
            }
        }
   _previousTab.Tag = items;  // ? 이전 탭에 저장
    }
    
    // ? Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem newTab)
    {
     _previousTab = newTab;
    }
    
  // Load items for newly selected tab
    LoadCurrentTabItems();
}
```

**핵심 차이:**
1. ? `_previousTab`을 사용하여 이전 탭에 저장
2. ? 저장 후 `_previousTab` 업데이트
3. ? 그 다음 새 탭 로드

---

### **5. MainTabView_TabCloseRequested 수정**

```csharp
private void MainTabView_TabCloseRequested(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabCloseRequestedEventArgs args)
{
    // Don't allow closing the last tab
    if (MainTabView.TabItems.Count <= 1)
    {
        return;
    }
    
    // ? Save current tab before closing
    SaveCurrentTabItems();
    
    // Remove the tab
    MainTabView.TabItems.Remove(args.Tab);

    // ? Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
    {
        _previousTab = selectedTab;
    }
}
```

**변경 사항:**
- ? 탭 닫기 전에 저장
- ? 닫은 후 `_previousTab` 업데이트

---

### **6. LoadCurrentTabItems 메서드 추가**

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
     // Re-register event handlers
  tile.Drop += GridViewTile_Drop;
      tile.DragEnter += GridViewTile_DragEnter;
   tile.DragLeave += GridViewTile_DragLeave;
         ItemsGridView.Items.Add(tile);
          }
      else if (item is GridViewTileGroup group)
         {
// Re-register event handlers
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

---

## ?? 동작 흐름 비교

### **Before (문제):**

```
[기본 탭 선택] App1, App2 추가
    ↓
[탭 2 클릭]
    ↓
SelectionChanged 발생
    ├─ MainTabView.SelectedItem = 탭 2 (이미 전환됨)
 ├─ ItemsGridView.Items = App1, App2 (아직 기본 탭 아이템)
    ├─ SaveCurrentTabItems() 호출
    │   → 탭 2.Tag = App1, App2 ? (잘못된 저장!)
    └─ LoadCurrentTabItems() 호출
    → 탭 2.Tag에서 로드 → App1, App2 표시 ?

[기본 탭 클릭]
    ↓
SelectionChanged 발생
    ├─ MainTabView.SelectedItem = 기본 탭
    ├─ ItemsGridView.Items = App1, App2 (탭 2 것)
    ├─ SaveCurrentTabItems() 호출
    │   → 기본 탭.Tag = App1, App2 ?
    └─ LoadCurrentTabItems() 호출
        → 기본 탭.Tag에서 로드 → App1, App2 표시 ?

결과: 두 탭 모두 같은 아이템 ?
```

---

### **After (해결):**

```
[기본 탭 선택] App1, App2 추가
    ├─ _previousTab = 기본 탭
 └─ ItemsGridView = App1, App2

[탭 2 클릭]
    ↓
SelectionChanged 발생
    ├─ _previousTab = 기본 탭 (아직 업데이트 안됨)
    ├─ ItemsGridView = App1, App2
    ├─ SaveCurrentTabItems() 호출
    │   → _previousTab (기본 탭).Tag = App1, App2 ? (올바른 저장!)
    ├─ _previousTab = 탭 2로 업데이트
    └─ LoadCurrentTabItems() 호출
        → 탭 2.Tag에서 로드 → 비어있음 ?

[탭 2에서 Game1, Game2 추가]
    ├─ _previousTab = 탭 2
    └─ ItemsGridView = Game1, Game2

[기본 탭 클릭]
    ↓
SelectionChanged 발생
    ├─ _previousTab = 탭 2
    ├─ ItemsGridView = Game1, Game2
    ├─ SaveCurrentTabItems() 호출
    │   → _previousTab (탭 2).Tag = Game1, Game2 ?
  ├─ _previousTab = 기본 탭으로 업데이트
    └─ LoadCurrentTabItems() 호출
        → 기본 탭.Tag에서 로드 → App1, App2 표시 ?

결과: 각 탭이 독립적인 아이템 관리 ?
```

---

## ?? 핵심 포인트

### **문제의 근본 원인:**

```csharp
// SelectionChanged 이벤트는 이미 선택이 변경된 AFTER에 발생
event SelectionChanged(...)
{
    // 이 시점에 MainTabView.SelectedItem은 이미 새 탭!
    // ItemsGridView.Items는 아직 이전 탭의 아이템!
}
```

### **해결책:**

```csharp
// 이전 탭을 추적하여, 현재 ItemsGridView를 이전 탭에 저장
private TabViewItem _previousTab;  // 항상 이전에 선택되었던 탭

SelectionChanged(...)
{
    // 1. _previousTab에 현재 ItemsGridView 저장 ?
    _previousTab.Tag = ItemsGridView.Items;
    
    // 2. _previousTab 업데이트
    _previousTab = 새로 선택한 탭;
    
    // 3. 새 탭 로드
    LoadFrom(새로 선택한 탭.Tag);
}
```

---

## ?? 테스트 시나리오

### **Test 1: 기본 시나리오**

```
1. 기본 탭에 App1, App2 추가 ?
2. "+" 버튼으로 탭 2 생성 ?
3. 탭 2 비어있음 확인 ?
4. 탭 2에 Game1, Game2 추가 ?
5. 기본 탭 클릭
6. App1, App2만 표시됨 ?
7. 탭 2 클릭
8. Game1, Game2만 표시됨 ?
```

---

### **Test 2: 여러 번 전환**

```
1. 기본 탭 → App1, App2
2. 탭 2 → Game1, Game2
3. 탭 3 → Music1, Music2

전환 테스트:
- 기본 → 탭 2 → 기본 ?
- 탭 2 → 탭 3 → 탭 2 ?
- 기본 → 탭 3 → 기본 ?

각 탭의 아이템이 유지됨 ?
```

---

### **Test 3: 탭 닫기**

```
1. 기본 탭 → App1, App2
2. 탭 2 → Game1, Game2
3. 탭 2 닫기
4. 기본 탭 선택됨 ?
5. App1, App2 표시됨 ?
```

---

### **Test 4: 아이템 추가/제거**

```
1. 기본 탭 → App1, App2
2. 탭 2 생성
3. 탭 2 → Game1, Game2
4. 기본 탭 전환
5. App3 추가
6. 탭 2 전환 → Game1, Game2만 있음 ?
7. 기본 탭 전환 → App1, App2, App3 있음 ?
```

---

## ?? 디버깅 팁

### **문제 확인 방법:**

```csharp
// SelectionChanged에 디버그 로그 추가
private void MainTabView_SelectionChanged(...)
{
    System.Diagnostics.Debug.WriteLine($"[Before Save] Previous Tab: {_previousTab?.Header}");
    System.Diagnostics.Debug.WriteLine($"[Before Save] Current Items: {ItemsGridView.Items.Count}");
    
    if (_previousTab != null)
    {
   // 저장...
        System.Diagnostics.Debug.WriteLine($"[After Save] Previous Tab Tag Count: {(_previousTab.Tag as List<UserControl>)?.Count}");
    }
    
    System.Diagnostics.Debug.WriteLine($"[After Update] New Previous Tab: {_previousTab?.Header}");
    
    LoadCurrentTabItems();
    System.Diagnostics.Debug.WriteLine($"[After Load] Items: {ItemsGridView.Items.Count}");
}
```

**출력 예시:**
```
[Before Save] Previous Tab: 기본
[Before Save] Current Items: 2 (App1, App2)
[After Save] Previous Tab Tag Count: 2
[After Update] New Previous Tab: 탭 2
[After Load] Items: 0 (비어있음)
```

---

## ?? 학습 포인트

### **1. 이벤트 타이밍**

```csharp
// SelectionChanged는 "이미 변경된 후" 발생
TabView.SelectedItem = newTab;  // 여기서 변경
    ↓
SelectionChanged 이벤트 발생  // 이미 newTab으로 변경됨
```

**따라서:**
- 이벤트 안에서 `SelectedItem`은 새 값
- 이전 값은 별도로 추적해야 함

---

### **2. 상태 추적의 중요성**

```csharp
// Bad: 현재 상태만 의존
SaveCurrent() → SelectedItem 사용 ?

// Good: 이전 상태 추적
_previousTab 필드로 추적 ?
SavePrevious() → _previousTab 사용 ?
```

---

### **3. Tag 속성 활용**

```csharp
// TabViewItem.Tag는 object 타입
// 어떤 데이터든 저장 가능

tab.Tag = List<UserControl>;  // ?
tab.Tag = Dictionary<...>;    // ?
tab.Tag = CustomClass;        // ?
```

**장점:**
- 간단한 데이터 저장
- 추가 클래스 불필요

**단점:**
- 타입 캐스팅 필요
- null 체크 필요

---

## ? 완료!

### **수정된 코드:**
- ? `_previousTab` 필드 추가
- ? `InitializeTabs()` 수정
- ? `MainTabView_AddTabButtonClick()` 수정
- ? `MainTabView_SelectionChanged()` 완전히 재작성 (핵심!)
- ? `MainTabView_TabCloseRequested()` 수정
- ? `LoadCurrentTabItems()` 메서드 추가

---

### **해결된 문제:**
- ? 탭 전환 시 아이템이 섞이는 문제
- ? 모든 탭이 같은 아이템을 표시하는 문제
- ? 탭 닫기 후 아이템 손실 문제

---

### **테스트:**

```
1. 기본 탭에 아이템 추가 ?
2. 새 탭 생성 ?
3. 새 탭에 다른 아이템 추가 ?
4. 탭 전환 ?
5. 각 탭이 독립적인 아이템 유지 ?
6. 여러 번 전환해도 문제없음 ?
```

---

## ?? 완료!

**이제 각 탭이 독립적으로 아이템을 관리합니다!**

**탭 전환 시 아이템이 섞이지 않습니다!** ?

**테스트해보세요!** ??
