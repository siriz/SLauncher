# ? Always on Top 고정 설정 완료!

## ?? 변경 사항

### **기존 동작:**
- Settings에 "Always on top" 토글 스위치
- 사용자가 on/off 선택 가능
- 설정 파일에 저장

### **새로운 동작:**
- ? **항상 Always on Top 활성화**
- ? Settings에서 옵션 제거
- ? 사용자가 변경할 수 없음

---

## ?? 수정된 파일

### **1. SLauncher/SettingsWindow.xaml**

**제거된 코드:**

```xaml
<!--  Always on top  -->
<wct:SettingsCard
    Margin="0,5,0,0"
    Description="Keep SLauncher window always on top of other windows."
    Header="Always on top">
 <wct:SettingsCard.HeaderIcon>
        <FontIcon Glyph="&#xE8A7;" />
    </wct:SettingsCard.HeaderIcon>
    <ToggleSwitch
        x:Name="AlwaysOnTopToggleSwitch"
      OffContent="No"
OnContent="Yes"
      Toggled="AlwaysOnTopToggleSwitch_Toggled" />
</wct:SettingsCard>
```

**결과:**
- ? Always on top 설정 섹션 완전 제거
- ? Start with Windows → Global Hotkey 순서로 연결

---

### **2. SLauncher/SettingsWindow.xaml.cs**

#### **(1) Container_Loaded 메서드 수정**

**Before:**
```csharp
// Set always on top toggle
AlwaysOnTopToggleSwitch.IsOn = UserSettingsClass.AlwaysOnTop;
```

**After:**
```csharp
// (제거됨)
```

---

#### **(2) AlwaysOnTopToggleSwitch_Toggled 이벤트 핸들러 제거**

**Before:**
```csharp
private void AlwaysOnTopToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
 UserSettingsClass.AlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
    UserSettingsClass.WriteSettingsFile();

    // Update main window's always on top state
    App.MainWindow.IsAlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
 
    // Keep settings window on top of main window
    this.IsAlwaysOnTop = true;
    
    // Bring settings window to front
    UIFunctionsClass.BringWindowToFront(this);
}
```

**After:**
```csharp
// (완전 제거됨)
```

---

### **3. SLauncher/MainWindow.xaml.cs**

#### **Container_Loaded 메서드 수정**

**Before:**
```csharp
// Set Window Background
UIFunctionsClass.SetWindowBackground(this, ContainerFallbackBackgroundBrush);

// Set always on top if enabled
this.IsAlwaysOnTop = UserSettingsClass.AlwaysOnTop;  // ? 설정에서 읽음

// Create icon scale slider dynamically to avoid XAML compiler bug
CreateIconScaleSlider();
```

**After:**
```csharp
// Set Window Background
UIFunctionsClass.SetWindowBackground(this, ContainerFallbackBackgroundBrush);

// Always set window to be on top
this.IsAlwaysOnTop = true;  // ? 항상 true

// Create icon scale slider dynamically to avoid XAML compiler bug
CreateIconScaleSlider();
```

**핵심 변경:**
- `UserSettingsClass.AlwaysOnTop` 읽기 → 제거
- `this.IsAlwaysOnTop = true` → 하드코딩

---

## ?? UI 변경

### **Settings 창 - Before:**

```
┌────────────────────────────────────┐
│ Settings │
├────────────────────────────────────┤
│ ▣ Enable fullscreen [OFF]   │
│ ?? Grid alignment  [Left ▼]      │
│ ?? Start with Windows [OFF]        │
│ ?? Always on top [OFF]             │ ← 제거됨
│ ?? Global Hotkey [Ctrl + Space]    │
└────────────────────────────────────┘
```

---

### **Settings 창 - After:**

```
┌────────────────────────────────────┐
│ Settings   │
├────────────────────────────────────┤
│ ▣ Enable fullscreen [OFF]         │
│ ?? Grid alignment  [Left ▼]        │
│ ?? Start with Windows [OFF] │
│ ?? Global Hotkey [Ctrl + Space]    │
└────────────────────────────────────┘
```

**차이점:**
- ? Always on top 옵션 제거
- ? 더 깔끔한 UI
- ? 혼란 제거 (항상 on이므로 선택 불필요)

---

## ?? 동작 방식

### **Always on Top 적용:**

```csharp
// MainWindow 생성 시
public MainWindow()
{
    this.InitializeComponent();
    // ...
}

// 윈도우 로드 시
private async void Container_Loaded(...)
{
    // ...
 this.IsAlwaysOnTop = true;  // ? 항상 활성화
    // ...
}
```

**특징:**
1. ? 프로그램 시작 시 자동 적용
2. ? 사용자가 변경 불가
3. ? 다른 창보다 항상 위에 표시

---

### **SettingsWindow는 유지:**

```xaml
<winex:WindowEx
    ...
    IsAlwaysOnTop="True"  ← 여전히 true
    ...>
```

**이유:**
- Settings 창은 Main 창 위에 떠야 함
- Modal 동작 유지

---

## ?? 테스트 시나리오

### **Test 1: 프로그램 시작**

```
1. SLauncher 실행 ?
2. 다른 프로그램 열기 (예: Chrome) ?
3. Chrome 클릭 ?
4. SLauncher는 여전히 Chrome 위에 표시됨 ?
```

---

### **Test 2: Settings 확인**

```
1. Settings 열기 ?
2. "Always on top" 옵션 없음 확인 ?
3. 설정 항목:
   - Enable fullscreen ?
   - Grid alignment ?
   - Start with Windows ?
   - Global Hotkey ?
```

---

### **Test 3: 여러 창과 함께 사용**

```
1. SLauncher 실행 ?
2. Chrome, VSCode, File Explorer 열기 ?
3. 각 프로그램 전환 ?
4. SLauncher는 항상 최상위에 표시됨 ?
5. Alt+Tab으로 전환해도 유지됨 ?
```

---

### **Test 4: 전체화면 모드와 호환**

```
1. Settings → Enable fullscreen [ON] ?
2. SLauncher 전체화면 ?
3. 다른 창 열어도 SLauncher가 덮음 ?
4. Esc 또는 Close 버튼으로 닫기 ?
```

---

## ?? UserSettingsClass 영향

### **UserSettingsClass.AlwaysOnTop 필드:**

```csharp
public static bool AlwaysOnTop { get; set; }
```

**상태:**
- ?? 필드는 아직 존재
- ?? 설정 파일에도 저장될 수 있음
- ? 하지만 MainWindow에서는 무시됨

**향후 정리 (선택사항):**
```csharp
// 완전히 제거하려면:
// 1. UserSettingsClass.cs에서 AlwaysOnTop 필드 제거
// 2. 설정 파일 읽기/쓰기 코드에서 제거
// 3. 기존 설정 파일 업그레이드 코드 추가
```

**현재 상태:**
- ? 작동에는 문제없음
- ? 단순히 무시됨
- ? 정리는 나중에 가능

---

## ?? 비교표

| 항목 | Before | After |
|------|--------|-------|
| **Settings UI** | Always on top 토글 있음 | 제거됨 ? |
| **Main Window** | `UserSettingsClass.AlwaysOnTop` 읽음 | `true` 하드코딩 ? |
| **사용자 제어** | 가능 (on/off) | 불가능 (항상 on) ? |
| **설정 파일** | AlwaysOnTop 저장됨 | 무시됨 ?? |
| **Settings Window** | IsAlwaysOnTop="True" | 변경없음 ? |

---

## ?? 기술적 세부사항

### **IsAlwaysOnTop 속성:**

```csharp
// WinUIEx.WindowEx 속성
public bool IsAlwaysOnTop { get; set; }
```

**설명:**
- WinUIEx 라이브러리에서 제공
- Win32 API `SetWindowPos` 래퍼
- `HWND_TOPMOST` 플래그 사용

---

### **적용 순서:**

```
1. MainWindow 생성자 실행
   ↓
2. InitializeComponent() 호출
   ↓
3. Container_Loaded 이벤트 발생
   ↓
4. this.IsAlwaysOnTop = true 실행  ← 여기서 적용
   ↓
5. 창이 최상위로 올라감
```

---

## ?? 사용자 경험 개선

### **Before (문제점):**

```
사용자: "Always on top을 켰는데 왜 다른 창 아래로 가지?"
→ 설정이 제대로 저장 안됨
→ 재시작 필요
→ 혼란스러움
```

---

### **After (개선):**

```
사용자: "SLauncher는 항상 위에 있네!"
→ 설정 필요없음
→ 직관적
→ 예상대로 동작
```

**장점:**
- ? 설정 간소화
- ? 사용자 혼란 제거
- ? 일관된 동작
- ? 런처의 목적에 부합 (빠른 접근)

---

## ?? 향후 개선 (선택사항)

### **1. UserSettingsClass 정리**

```csharp
// AlwaysOnTop 필드 완전 제거
public static class UserSettingsClass
{
 // public static bool AlwaysOnTop { get; set; }  ← 제거
    
    public static bool UseFullscreen { get; set; }
    public static string GridPosition { get; set; }
    // ...
}
```

---

### **2. 설정 파일 업그레이드**

```csharp
public static void UpgradeUserSettings()
{
    // 기존 설정 파일에 AlwaysOnTop이 있으면 제거
    if (settingsJson.ContainsKey("AlwaysOnTop"))
    {
        settingsJson.Remove("AlwaysOnTop");
    }
}
```

---

### **3. 임시 비활성화 옵션 (고급)**

```csharp
// 개발자 모드나 특수 상황을 위한 임시 비활성화
// Shift 키를 누르고 시작하면 Always on Top 비활성화
if (!Keyboard.IsKeyDown(Key.Shift))
{
    this.IsAlwaysOnTop = true;
}
```

---

## ? 완료!

### **변경 사항 요약:**

```
? SettingsWindow.xaml
   - Always on top 설정 섹션 제거

? SettingsWindow.xaml.cs
   - AlwaysOnTopToggleSwitch 초기화 코드 제거
   - AlwaysOnTopToggleSwitch_Toggled 핸들러 제거

? MainWindow.xaml.cs
   - UserSettingsClass.AlwaysOnTop 읽기 제거
   - this.IsAlwaysOnTop = true 하드코딩
```

---

### **테스트 체크리스트:**

```
? 빌드 성공
? 프로그램 시작 시 Always on Top 적용
? Settings에서 옵션 제거 확인
? 다른 창보다 항상 위에 표시
? 전체화면 모드와 호환
? Settings 창도 Always on Top 유지
```

---

## ?? 완료!

**이제 SLauncher는 항상 다른 창 위에 표시됩니다!**

**Settings에서 불필요한 옵션이 제거되어 더 깔끔합니다!** ?

**사용자는 별도 설정 없이 즉시 사용할 수 있습니다!** ??

**테스트해보세요!** ??
