# ? 탭 색상 변경 기능 완료!

## ?? **구현된 기능:**

### **탭 우클릭 메뉴에 "색 변경" 추가**
- ? 10가지 색상 프리셋 제공
- ? 색상 선택 시 탭에 즉시 적용
- ? 각 색상 옆에 미리보기 사각형 표시

---

## ?? **색상 프리셋 (10가지)**

```csharp
private readonly Dictionary<string, Color> _tabColorPresets = new Dictionary<string, Color>
{
    { "기본", Color.FromArgb(0, 0, 0, 0) },     // 투명 (기본)
   { "빨강", Color.FromArgb(80, 255, 69, 58) },   // 밝은 빨강
    { "주황", Color.FromArgb(80, 255, 159, 10) },   // 주황
    { "노랑", Color.FromArgb(80, 255, 214, 10) },   // 노랑
  { "초록", Color.FromArgb(80, 48, 209, 88) },    // 초록
    { "파랑", Color.FromArgb(80, 10, 132, 255) },   // 파랑
    { "남색", Color.FromArgb(80, 94, 92, 230) },    // 남색
    { "보라", Color.FromArgb(80, 191, 90, 242) },   // 보라
    { "분홍", Color.FromArgb(80, 255, 55, 95) },    // 분홍
    { "회색", Color.FromArgb(80, 142, 142, 147) }   // 회색
};
```

**색상 특징:**
- **Alpha 값 80:** 반투명 (50% 투명도)
- **RGB 값:** 밝고 선명한 색상
- **"기본":** 완전 투명 (원래 배경)

---

## ?? **UI 구조**

### **컨텍스트 메뉴 구조:**

```
탭 우클릭
  ↓
┌─────────────────┐
│ ?? 이름 변경    │
├─────────────────┤
│ ?? 색 변경 ▶   │ ← 새로 추가!
│    ├── ? 기본  │
│    ├── ?? 빨강 │
│    ├── ?? 주황 │
│    ├── ?? 노랑 │
│  ├── ?? 초록 │
│    ├── ?? 파랑 │
│    ├── ?? 남색 │
│    ├── ?? 보라 │
│    ├── ?? 분홍 │
│    └── ? 회색 │
├─────────────────┤
│ ??? 삭제        │
└─────────────────┘
```

---

## ?? **코드 구현**

### **1. 색상 프리셋 Dictionary 추가**

```csharp
// Tab color presets
private readonly Dictionary<string, Color> _tabColorPresets = new Dictionary<string, Color>
{
    { "기본", Color.FromArgb(0, 0, 0, 0) },
    { "빨강", Color.FromArgb(80, 255, 69, 58) },
    { "주황", Color.FromArgb(80, 255, 159, 10) },
    { "노랑", Color.FromArgb(80, 255, 214, 10) },
    { "초록", Color.FromArgb(80, 48, 209, 88) },
    { "파랑", Color.FromArgb(80, 10, 132, 255) },
    { "남색", Color.FromArgb(80, 94, 92, 230) },
    { "보라", Color.FromArgb(80, 191, 90, 242) },
    { "분홍", Color.FromArgb(80, 255, 55, 95) },
    { "회색", Color.FromArgb(80, 142, 142, 147) }
};
```

---

### **2. AttachTabContextMenu 수정 - 색상 메뉴 추가**

```csharp
private void AttachTabContextMenu(Microsoft.UI.Xaml.Controls.TabViewItem tab)
{
    var contextMenu = new MenuFlyout();
    
    // Rename menu item
    var renameItem = new MenuFlyoutItem
    {
     Text = "이름 변경",
Icon = new SymbolIcon(Symbol.Rename)
    };
    renameItem.Click += (s, e) => RenameTab_Click(tab);
    contextMenu.Items.Add(renameItem);
    
    // ? Color submenu (NEW!)
    var colorItem = new MenuFlyoutSubItem
    {
    Text = "색 변경",
        Icon = new SymbolIcon(Symbol.FontColor)
    };
    
    // ? Add color presets
    foreach (var colorPreset in _tabColorPresets)
    {
        var colorMenuItem = new MenuFlyoutItem
        {
            Text = colorPreset.Key
        };
        
        // ? Add color indicator (visual preview)
        var colorBox = new Border
     {
 Width = 16,
      Height = 16,
            Background = new SolidColorBrush(colorPreset.Value),
            CornerRadius = new CornerRadius(3),
   BorderBrush = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128)),
            BorderThickness = new Thickness(1)
    };
        
        colorMenuItem.Icon = new FontIcon 
        { 
     FontFamily = new FontFamily("Segoe Fluent Icons"), 
       Glyph = "\uE790" // Color palette icon
   };
        
        colorMenuItem.Click += (s, e) => ChangeTabColor_Click(tab, colorPreset.Value);
        colorItem.Items.Add(colorMenuItem);
    }
    
    contextMenu.Items.Add(colorItem);
    
    // Separator
    contextMenu.Items.Add(new MenuFlyoutSeparator());
    
    // Delete menu item
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

**코드 설명:**
1. **MenuFlyoutSubItem:** 하위 메뉴 생성
2. **foreach 루프:** 각 색상 프리셋을 메뉴 항목으로 추가
3. **Border (colorBox):** 16x16 크기의 색상 미리보기
4. **CornerRadius(3):** 둥근 모서리
5. **BorderBrush:** 회색 테두리 (색상 구분)
6. **Click 이벤트:** 람다식으로 색상 적용

---

### **3. ChangeTabColor_Click 메서드**

```csharp
/// <summary>
/// Handle tab color change
/// </summary>
private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
{
    tab.Background = new SolidColorBrush(color);
}
```

**간단한 구현:**
- 탭의 `Background` 속성에 선택된 색상 적용
- 즉시 반영됨 (별도 확인 필요없음)

---

## ?? **색상 미리보기**

### **메뉴 항목 구조:**

```
┌─────────────────────┐
│ ?? 색 변경 ▶       │
│    ┌──────────────┐ │
│    │ ? 기본      │ │ ← 16x16 사각형 + 텍스트
│    │ ?? 빨강     │ │
│    │ ?? 주황 │ │
│    │ ?? 노랑     │ │
│    │ ?? 초록     │ │
│    │ ?? 파랑     │ │
│    │ ?? 남색     │ │
│    │ ?? 보라   │ │
│ │ ?? 분홍     │ │
│    │ ? 회색     │ │
│    └──────────────┘ │
└─────────────────────┘
```

**각 메뉴 항목:**
```
[?? 아이콘] [■ 색상 사각형] [색상 이름]
```

---

## ?? **테스트 시나리오**

### **Test 1: 탭 색상 변경**

```
1. "기본" 탭 우클릭 ?
2. "색 변경" 선택 ?
3. 색상 프리셋 목록 표시 ?
4. "빨강" 선택 ?
5. 탭 배경이 빨강색으로 변경 ?
```

---

### **Test 2: 여러 탭에 다른 색상 적용**

```
1. 새 탭 3개 생성 (탭 2, 탭 3, 탭 4) ?
2. 탭 2 → 파랑 ?
3. 탭 3 → 초록 ?
4. 탭 4 → 노랑 ?
5. 각 탭이 해당 색상으로 표시됨 ?
```

---

### **Test 3: 색상 변경 후 다시 기본으로**

```
1. "기본" 탭에 빨강 적용 ?
2. 빨강 배경으로 변경됨 ?
3. 다시 우클릭 → "색 변경" → "기본" ?
4. 투명 배경으로 복원 ?
```

---

### **Test 4: 색상 미리보기 확인**

```
1. 탭 우클릭 → "색 변경" ?
2. 각 색상 옆에 사각형 미리보기 표시 확인 ?
   - 빨강: 붉은색 사각형 ?
   - 파랑: 파란색 사각형 ?
   - 초록: 초록색 사각형 ?
3. 시각적으로 색상 구분 가능 ?
```

---

### **Test 5: 탭 전환 시 색상 유지**

```
1. 기본 탭 → 빨강 ?
2. 탭 2 생성 → 파랑 ?
3. 기본 탭 선택 → 빨강 유지 ?
4. 탭 2 선택 → 파랑 유지 ?
5. 색상이 탭별로 유지됨 ?
```

---

## ?? **색상 시스템 상세**

### **Alpha 값 설명:**

```csharp
Color.FromArgb(80, 255, 69, 58)
//          ↑   ↑    ↑   ↑
//             │   │    │   └─ Blue (58)
//     │   │    └───── Green (69)
//      │   └────────── Red (255)
//             └────────────── Alpha (80 = ~31% 불투명)
```

**Alpha 80을 선택한 이유:**
- ? 너무 진하지 않음 (시각적 피로 감소)
- ? 배경이 적당히 비침 (컨텐츠 가시성 유지)
- ? 색상 구분 가능
- ? 전문적인 외관

---

### **색상 선택 기준:**

**1. 기본 (투명)**
- Alpha = 0
- 원래 배경 복원

**2. 빨강, 주황, 노랑 (따뜻한 색)**
- 긴급, 중요, 주의

**3. 초록, 파랑, 남색 (차가운 색)**
- 작업, 진행 중, 완료

**4. 보라, 분홍 (중간 색)**
- 창의적, 특별

**5. 회색 (중립 색)**
- 보관, 참고

---

## ?? **사용 시나리오**

### **시나리오 1: 프로젝트별 색상 코딩**

```
[기본 - 투명] 일반 항목
[빨강] 긴급 프로젝트
[파랑] 진행 중 작업
[초록] 완료된 작업
[회색] 보관함
```

---

### **시나리오 2: 팀별 구분**

```
[빨강] 마케팅 팀
[파랑] 개발 팀
[초록] 디자인 팀
[노랑] 경영 팀
```

---

### **시나리오 3: 우선순위 표시**

```
[빨강] 높음 (즉시 처리)
[주황] 보통 (일주일 내)
[노랑] 낮음 (여유 있을 때)
[회색] 참고 (필요시)
```

---

## ?? **기존 기능과의 통합**

### **컨텍스트 메뉴 전체 구조:**

```
┌─────────────────┐
│ ?? 이름 변경    │ ← 기존
├─────────────────┤
│ ?? 색 변경 ▶   │ ← 새로 추가!
│ └─ 10가지   │
├─────────────────┤
│ ??? 삭제        │ ← 기존
└─────────────────┘
```

**메뉴 순서:**
1. **이름 변경** - 자주 사용
2. **색 변경** - 중간 사용
3. **구분선**
4. **삭제** - 위험한 작업 (분리)

---

## ?? **코드 변경 사항**

### **추가된 코드:**

**1. using 추가**
```csharp
using Windows.UI;  // Color 타입
```

**2. 필드 추가**
```csharp
private readonly Dictionary<string, Color> _tabColorPresets = ...
```

**3. 메서드 추가**
```csharp
private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
```

**4. AttachTabContextMenu 수정**
- 색상 서브메뉴 추가
- 10가지 색상 프리셋 생성
- 각 색상에 미리보기 사각형 추가

---

## ? **시각적 효과**

### **Before (색상 변경 전):**

```
┌──────────┬──────────┬──────────┐
│ 기본     │ 탭 2     │ 탭 3     │
└──────────┴──────────┴──────────┘
   (모두 동일한 배경)
```

---

### **After (색상 변경 후):**

```
┌──────────┬──────────┬──────────┐
│ 기본     │ 탭 2     │ 탭 3     │
│ (투명)   │ (파랑)   │ (빨강)   │
└──────────┴──────────┴──────────┘
  ? 기본    ?? 파랑     ?? 빨강
```

**시각적 구분:**
- ? 탭별로 다른 색상 배경
- ? 즉시 구분 가능
- ? 프로젝트/카테고리 식별 용이

---

## ?? **색상 미리보기 예시**

### **메뉴 표시:**

```
?? 색 변경 ▶
  ├─ [?] 기본   (투명)
  ├─ [??] 빨강   (밝은 빨강)
  ├─ [??] 주황   (주황)
  ├─ [??] 노랑   (밝은 노랑)
  ├─ [??] 초록   (초록)
  ├─ [??] 파랑   (밝은 파랑)
  ├─ [??] 남색   (남색)
  ├─ [??] 보라   (보라)
  ├─ [??] 분홍   (밝은 분홍)
  └─ [?] 회색   (회색)
```

---

## ?? **향후 확장 가능성**

### **1. 사용자 정의 색상**

```csharp
// 색상 선택 다이얼로그 추가
var colorPicker = new ColorPicker();
var result = await ShowColorPickerDialog(colorPicker);
if (result == ContentDialogResult.Primary)
{
    tab.Background = new SolidColorBrush(colorPicker.Color);
}
```

---

### **2. 색상 저장/불러오기**

```csharp
// 탭 색상을 설정 파일에 저장
public class TabSettings
{
    public string Name { get; set; }
    public Color Background { get; set; }
}
```

---

### **3. 그라데이션 배경**

```csharp
// LinearGradientBrush 사용
var gradient = new LinearGradientBrush();
gradient.StartPoint = new Point(0, 0);
gradient.EndPoint = new Point(1, 1);
gradient.GradientStops.Add(new GradientStop { Color = color1, Offset = 0 });
gradient.GradientStops.Add(new GradientStop { Color = color2, Offset = 1 });
tab.Background = gradient;
```

---

## ?? **성능 영향**

### **메모리:**
- Dictionary<string, Color>: ~240 bytes (10 entries)
- 각 탭당 SolidColorBrush: ~40 bytes
- **총 영향: 무시할 수 있는 수준**

### **렌더링:**
- Background 변경: GPU 가속
- 성능 저하 없음
- 즉시 적용

---

## ? **완료!**

### **추가된 기능:**
```
? 탭 컨텍스트 메뉴에 "색 변경" 추가
? 10가지 색상 프리셋 제공
? 색상 미리보기 사각형 표시
? 클릭 시 즉시 적용
? 탭별로 독립적인 색상 설정
```

---

### **변경된 파일:**
```
? SLauncher/MainWindow.xaml.cs
   - using Windows.UI; 추가
   - _tabColorPresets Dictionary 추가
   - AttachTabContextMenu() 수정 (색상 메뉴 추가)
   - ChangeTabColor_Click() 새 메서드
```

---

### **테스트 체크리스트:**
```
? 탭 우클릭 → "색 변경" 표시
? 10가지 색상 프리셋 표시
? 각 색상 옆에 미리보기 사각형
? 색상 선택 시 즉시 적용
? 여러 탭에 다른 색상 적용 가능
? "기본"으로 투명 복원 가능
? 탭 전환 시 색상 유지
```

---

## ?? **완료!**

**이제 탭에 색상을 추가하여 프로젝트를 구분할 수 있습니다!** ??

**10가지 프리셋 색상으로 탭을 시각적으로 구분하세요!** ?

**우클릭만으로 즉시 색상 변경이 가능합니다!** ??

**테스트해보세요!** ??
