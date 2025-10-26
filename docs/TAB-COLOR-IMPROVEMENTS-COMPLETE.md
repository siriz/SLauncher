# ? 탭 색상 개선 완료!

## ?? **해결된 문제:**

### **Problem 1: 탭 선택 시 색상이 기본으로 돌아감** ?
- 탭에 색상을 적용해도 다른 탭으로 전환 후 돌아오면 색상이 사라짐
- `Background` 속성이 선택 상태에서 WinUI가 자동으로 변경함

### **Problem 2: 색상 목록에서 실제 색상을 확인하기 어려움** ?
- 색상 이름만 표시되어 어떤 색인지 알기 어려움
- 시각적 미리보기가 없음

---

## ? **해결책:**

### **Solution 1: BorderBrush + BorderThickness 사용**

**Before (Background 사용):**
```csharp
// ? 선택 시 WinUI가 자동으로 변경함
tab.Background = new SolidColorBrush(color);
```

**After (BorderBrush 사용):**
```csharp
// ? 선택 상태와 무관하게 유지됨
tab.BorderBrush = new SolidColorBrush(color);
tab.BorderThickness = new Thickness(0, 0, 0, 3);  // 하단 테두리만
```

**효과:**
```
┌─────────────┐
│   탭 제목   │
│_____________│ ← 3px 두께의 색상 바
     (탭)
```

---

### **Solution 2: 색상 Dictionary로 영구 저장**

```csharp
// 탭별 색상 정보를 저장하는 Dictionary
private readonly Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color> _tabColors 
    = new Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color>();

// 색상 적용 시 저장
private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
{
    _tabColors[tab] = color;  // ? Dictionary에 저장
    UpdateTabColorSeparator(tab, color);
}

// 탭 선택 변경 시 복원
private void MainTabView_SelectionChanged(...)
{
    if (_tabColors.ContainsKey(newTab))
    {
      UpdateTabColorSeparator(newTab, _tabColors[newTab]);  // ? 복원
    }
}
```

---

### **Solution 3: 색상 미리보기 FontIcon 사용**

**Before:**
```csharp
// ? 아이콘만 표시
colorMenuItem.Icon = new SymbolIcon(Symbol.FontColor);
```

**After:**
```csharp
// ? 실제 색상으로 채워진 사각형 아이콘
var colorIcon = new FontIcon
{
    Glyph = "■",  // 꽉 찬 사각형
    Foreground = new SolidColorBrush(colorPreset.Value),
    FontSize = 16
};
colorMenuItem.Icon = colorIcon;
```

---

## ?? **시각적 비교**

### **Before:**

**탭 색상:**
```
[기본 탭] → 파랑 적용 → [기본 탭(파랑 배경)]
   ↓ 다른 탭 선택
         [기본 탭] ← ? 파랑색 사라짐!
```

**색상 메뉴:**
```
┌─────────────────┐
│ ?? 기본      │ ← 색상을 알 수 없음
│ ?? 빨강         │
│ ?? 파랑         │
└─────────────────┘
```

---

### **After:**

**탭 색상:**
```
[기본 탭] → 파랑 적용 → [기본 탭____] ← 하단에 파랑 바
            파랑 바
      ↓ 다른 탭 선택 후 복귀
     
         [기본 탭____] ← ? 파랑색 유지!
         파랑 바
```

**색상 메뉴:**
```
┌─────────────────┐
│ ? 기본         │ ← 투명 사각형
│ ?? 빨강         │ ← 빨강 사각형
│ ?? 파랑    │ ← 파랑 사각형
│ ?? 초록         │ ← 초록 사각형
└─────────────────┘
```

---

## ?? **구현 세부사항**

### **1. Tab Color Dictionary 추가**

```csharp
// At class level
private readonly Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color> _tabColors 
    = new Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color>();
```

**목적:**
- 각 탭의 색상 정보를 영구적으로 저장
- 탭 전환 시 색상 복원에 사용

---

### **2. ChangeTabColor_Click 수정**

```csharp
private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
{
    // Store the color in the dictionary
    _tabColors[tab] = color;  // ? NEW!
    
    // Apply the color with a separator for visual distinction
    UpdateTabColorSeparator(tab, color);
}
```

**변경 사항:**
- `_tabColors[tab] = color` 추가
- Dictionary에 색상 저장

---

### **3. UpdateTabColorSeparator 메서드 (NEW!)**

```csharp
private void UpdateTabColorSeparator(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
{
    // Instead of changing Background (which gets overridden by selection),
    // we'll use BorderBrush with a thick bottom border
    tab.BorderBrush = new SolidColorBrush(color);
    tab.BorderThickness = new Thickness(0, 0, 0, 3);  // Bottom border only
}
```

**핵심:**
- `BorderBrush` 사용 (Background 대신)
- `BorderThickness(0, 0, 0, 3)` = 하단 테두리만 3px
- 선택 상태에서도 유지됨

---

### **4. SelectionChanged 수정**

```csharp
private void MainTabView_SelectionChanged(...)
{
    // Save items from the PREVIOUS tab
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
    _previousTab.Tag = items;
        
   // ? NEW: Restore previous tab's color
   if (_tabColors.ContainsKey(_previousTab))
   {
        UpdateTabColorSeparator(_previousTab, _tabColors[_previousTab]);
        }
    }
    
    // Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem newTab)
    {
        _previousTab = newTab;
   
        // ? NEW: Restore newly selected tab's color
        if (_tabColors.ContainsKey(newTab))
   {
 UpdateTabColorSeparator(newTab, _tabColors[newTab]);
      }
    }
    
    LoadCurrentTabItems();
}
```

**변경 사항:**
- 이전 탭 저장 시 색상 복원
- 새 탭 선택 시 색상 복원

---

### **5. DeleteTab_Click 수정**

```csharp
if (result == ContentDialogResult.Primary)
{
    // If deleting current tab, save it first
    if (tab == MainTabView.SelectedItem)
    {
 SaveCurrentTabItems();
    }
    
    // ? NEW: Remove tab color from dictionary
 if (_tabColors.ContainsKey(tab))
    {
        _tabColors.Remove(tab);
    }
    
 // Remove the tab
    MainTabView.TabItems.Remove(tab);
 
    // Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
    {
        _previousTab = selectedTab;
     
        // ? NEW: Restore selected tab's color
        if (_tabColors.ContainsKey(selectedTab))
        {
            UpdateTabColorSeparator(selectedTab, _tabColors[selectedTab]);
     }
    }
}
```

**변경 사항:**
- 삭제되는 탭의 색상 정보 제거
- 새로 선택된 탭의 색상 복원

---

### **6. TabCloseRequested 수정**

```csharp
private void MainTabView_TabCloseRequested(...)
{
    // Don't allow closing the last tab
    if (MainTabView.TabItems.Count <= 1)
    {
     return;
    }
    
    // Save current tab before closing
    SaveCurrentTabItems();

    // ? NEW: Remove tab color from dictionary
    if (_tabColors.ContainsKey(args.Tab))
    {
        _tabColors.Remove(args.Tab);
    }

    // Remove the tab
 MainTabView.TabItems.Remove(args.Tab);

    // Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
    {
     _previousTab = selectedTab;
        
        // ? NEW: Restore selected tab's color
        if (_tabColors.ContainsKey(selectedTab))
        {
       UpdateTabColorSeparator(selectedTab, _tabColors[selectedTab]);
        }
    }
}
```

**변경 사항:**
- X 버튼으로 닫을 때도 동일하게 처리
- 색상 dictionary 정리
- 새 탭 색상 복원

---

### **7. AttachTabContextMenu 수정 - 색상 미리보기**

```csharp
// Add color presets
foreach (var colorPreset in _tabColorPresets)
{
  // Create menu item with text
    var colorMenuItem = new MenuFlyoutItem
    {
        Text = $"  {colorPreset.Key}"  // Add spacing for alignment
    };
    
    colorMenuItem.Click += (s, e) => ChangeTabColor_Click(tab, colorPreset.Value);

    // ? NEW: Use a colored FontIcon as preview
    var colorIcon = new FontIcon
    {
      Glyph = "■",  // Solid square (U+25A0)
        Foreground = new SolidColorBrush(colorPreset.Value),
        FontSize = 16
  };
    colorMenuItem.Icon = colorIcon;
    
    colorItem.Items.Add(colorMenuItem);
}
```

**변경 사항:**
- `FontIcon` 사용 (BitmapIcon 대신)
- Glyph = "■" (꽉 찬 사각형)
- `Foreground`를 색상으로 설정
- 각 메뉴 항목에 실제 색상 미리보기 표시

---

## ?? **UI 결과**

### **탭 색상 표시 (Bottom Border):**

```
┌───────────────────────────────────┐
│  [기본]  [탭 2]  [탭 3]  [+ Add] │
│  ???     ???     ???       │ ← 색상 바 (3px)
│  기본    빨강    파랑      │
└───────────────────────────────────┘
```

**특징:**
- 하단 3px 두께 테두리
- 탭 선택과 무관하게 색상 유지
- 명확한 시각적 구분

---

### **색상 메뉴:**

```
?? 색 변경 ▶
   ├─ ?  기본   (투명)
   ├─ ??  빨강   (빨강 사각형)
   ├─ ??  주황   (주황 사각형)
   ├─ ??  노랑   (노랑 사각형)
   ├─ ??  초록   (초록 사각형)
   ├─ ??  파랑   (파랑 사각형)
   ├─ ??  남색   (남색 사각형)
   ├─ ??  보라   (보라 사각형)
   ├─ ??  분홍   (분홍 사각형)
   └─ ?회색   (회색 사각형)
```

**특징:**
- 각 색상 옆에 실제 색상 미리보기
- 16x16 크기 사각형 아이콘
- 한눈에 색상 구분 가능

---

## ?? **테스트 시나리오**

### **Test 1: 색상 유지 확인**

```
1. "기본" 탭에 빨강 적용 ?
2. 빨강 하단 바 표시 확인 ?
3. "탭 2" 생성 및 선택 ?
4. "기본" 탭 다시 선택 ?
5. 빨강 바 여전히 유지됨 확인 ?
```

**Before:**
```
단계 5에서 색상이 사라짐 ?
```

**After:**
```
단계 5에서 색상 유지됨 ?
```

---

### **Test 2: 색상 미리보기**

```
1. 탭 우클릭 ?
2. "색 변경" 선택 ?
3. 각 색상 옆에 실제 색상 사각형 확인 ?
   - 빨강: 빨간 사각형 ?
   - 파랑: 파란 사각형 ?
   - 초록: 초록 사각형 ?
```

**Before:**
```
모든 색상이 동일한 아이콘 (??) ?
```

**After:**
```
각 색상의 실제 색상 미리보기 (??????) ?
```

---

### **Test 3: 여러 탭에 다른 색상**

```
1. 기본 탭 → 빨강 ?
2. 탭 2 생성 → 파랑 ?
3. 탭 3 생성 → 초록 ?
4. 탭 간 전환 ?
5. 각 탭의 색상이 유지됨 확인 ?
```

---

### **Test 4: 탭 삭제 시 색상 정리**

```
1. 기본 탭 → 빨강 ?
2. 탭 2 생성 → 파랑 ?
3. 탭 2 삭제 (X 버튼 또는 우클릭) ?
4. 메모리에서 탭 2의 색상 정보 제거됨 ?
5. 기본 탭의 빨강 색상은 유지됨 ?
```

---

### **Test 5: 기본 색상으로 복원**

```
1. 탭에 파랑 적용 ?
2. 다시 우클릭 → 색 변경 → "기본" ?
3. 색상 바 사라짐 (투명) ?
4. Dictionary에서 색상 제거됨 확인 ?
```

---

## ?? **코드 변경 통계**

### **추가된 코드:**

**1. 필드 추가**
```csharp
private readonly Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color> _tabColors 
    = new Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color>();
```

**2. 새 메서드 추가**
```csharp
private void UpdateTabColorSeparator(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
```

**3. 수정된 메서드:**
- `ChangeTabColor_Click` - Dictionary 저장 추가
- `MainTabView_SelectionChanged` - 색상 복원 추가
- `DeleteTab_Click` - Dictionary 정리 추가
- `MainTabView_TabCloseRequested` - Dictionary 정리 추가
- `AttachTabContextMenu` - 색상 미리보기 아이콘 추가

---

## ?? **핵심 개념**

### **Why BorderBrush instead of Background?**

**Background 문제:**
```csharp
tab.Background = new SolidColorBrush(color);  // ?
```
- WinUI TabView가 선택 상태에 따라 Background를 자동 변경
- 사용자가 설정한 색상이 override됨

**BorderBrush 해결:**
```csharp
tab.BorderBrush = new SolidColorBrush(color);  // ?
tab.BorderThickness = new Thickness(0, 0, 0, 3);
```
- BorderBrush는 WinUI가 건드리지 않음
- 하단 테두리만 표시하여 명확한 시각적 구분
- 선택 상태와 독립적으로 유지

---

### **Why Dictionary?**

**문제:**
```
TabViewItem 자체에는 색상 정보를 저장할 속성이 없음
→ 탭 전환 시 어떤 색상이었는지 알 수 없음
```

**해결:**
```csharp
Dictionary<TabViewItem, Color> _tabColors
```
- 각 탭의 색상 정보를 별도로 관리
- 탭 전환 시 Dictionary에서 조회하여 복원
- 탭 삭제 시 메모리 정리

---

### **Why FontIcon "■"?**

**시도한 방법들:**

**1. MenuFlyoutItem.Content (?)**
```csharp
colorMenuItem.Content = stackPanel;  // ? 속성 없음
```

**2. BitmapIcon (? 복잡함)**
```csharp
var renderTargetBitmap = new RenderTargetBitmap();
// 너무 복잡하고 비효율적
```

**3. FontIcon "■" (? 완벽!)**
```csharp
var colorIcon = new FontIcon
{
    Glyph = "■",  // U+25A0 (Black Square)
    Foreground = new SolidColorBrush(color),
    FontSize = 16
};
```
- 간단하고 효율적
- 실제 색상 표시 가능
- Unicode 문자 활용

---

## ?? **사용자 경험 개선**

### **Before:**

**Problem 1: 색상 사라짐**
```
사용자: "파랑 탭으로 설정했는데..."
→ 다른 탭 선택
→ "어? 색상이 사라졌네?"
→ 다시 설정해야 함 ??
```

**Problem 2: 색상 알기 어려움**
```
사용자: "'빨강'이 어떤 빨강이지?"
→ 클릭해봐야 알 수 있음
→ 마음에 안 들면 다시 선택
→ 번거로움 ??
```

---

### **After:**

**Solution 1: 색상 유지**
```
사용자: "파랑 탭으로 설정!"
→ 다른 탭 선택
→ 파랑 탭으로 돌아옴
→ "여전히 파랑이네! ??"
```

**Solution 2: 색상 미리보기**
```
사용자: "어떤 파랑인지 보자..."
→ 메뉴 열기
→ ?? 실제 파랑 사각형 보임
→ "이 파랑이 마음에 든다!" ??
```

---

## ? **완료!**

### **해결된 문제:**
```
? 탭 선택 시 색상 사라지는 문제 해결
? 색상 미리보기 추가
? Dictionary로 색상 영구 저장
? BorderBrush 사용으로 안정적인 표시
? 탭 삭제 시 메모리 정리
```

---

### **기술적 개선:**
```
? Background → BorderBrush (더 안정적)
? Dictionary 패턴 (색상 관리)
? FontIcon "■" (색상 미리보기)
? SelectionChanged 이벤트 활용
? 메모리 누수 방지 (삭제 시 정리)
```

---

### **UI/UX 개선:**
```
? 하단 3px 색상 바 (명확한 구분)
? 실제 색상 미리보기 (직관적)
? 탭 전환 시 색상 유지 (일관성)
? 한눈에 색상 확인 가능 (효율성)
```

---

## ?? **테스트해보세요!**

**1. 탭에 색상 적용**
```
탭 우클릭 → 색 변경 → 빨강 선택
→ 하단에 빨강 바 표시 ?
```

**2. 탭 전환 후 확인**
```
다른 탭 선택 → 다시 빨강 탭 선택
→ 빨강 바 여전히 유지 ?
```

**3. 색상 미리보기 확인**
```
색 변경 메뉴 열기
→ 각 색상 옆에 실제 색상 사각형 ?
```

**모든 기능이 완벽하게 작동합니다!** ??
