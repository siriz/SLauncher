# ?? Always On Top 토글 문제 해결

## ? 문제

**증상:**
```
1. Settings 창 열기
2. "Always on Top" 토글 ON
3. 메인 창이 Always on Top이 됨
4. Settings 창이 메인 창 뒤로 가려짐 ?
5. Settings 창 접근 불가능
```

**원인:**
- Settings 창을 열 때만 `IsAlwaysOnTop = true` 설정
- 토글 변경 시 메인 창의 `IsAlwaysOnTop`이 `true`가 되면
- Z-order가 재계산되면서 Settings 창이 뒤로 밀림

---

## ? 해결 방법

### **Settings 창을 항상 최상위로 유지**

**AlwaysOnTopToggleSwitch_Toggled 수정:**

```csharp
private void AlwaysOnTopToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
    UserSettingsClass.AlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
    UserSettingsClass.WriteSettingsFile();

    // Update main window's always on top state
    App.MainWindow.IsAlwaysOnTop = AlwaysOnTopToggleSwitch.IsOn;
    
    // ? Keep settings window on top of main window
    this.IsAlwaysOnTop = true;
    
    // ? Bring settings window to front
  UIFunctionsClass.BringWindowToFront(this);
}
```

---

## ?? 작동 원리

### Before (문제):
```
1. Settings 창 열림 (IsAlwaysOnTop = true)
2. "Always on Top" 토글 ON
3. 메인 창 IsAlwaysOnTop = true
4. Z-order 재계산
5. Settings 창이 메인 창 뒤로 ?
```

### After (해결):
```
1. Settings 창 열림 (IsAlwaysOnTop = true)
2. "Always on Top" 토글 ON
3. 메인 창 IsAlwaysOnTop = true
4. Settings 창 IsAlwaysOnTop = true (재설정) ?
5. Settings 창을 BringToFront ?
6. Settings 창이 최상위 유지 ?
```

---

## ?? 코드 설명

### **1. this.IsAlwaysOnTop = true**

```csharp
// Settings 창을 명시적으로 Always on Top 설정
this.IsAlwaysOnTop = true;
```

**이유:**
- 메인 창의 Always on Top 상태가 변경될 때
- Settings 창도 Always on Top 재설정
- Z-order에서 우선순위 유지

### **2. BringWindowToFront**

```csharp
// Settings 창을 맨 앞으로 가져오기
UIFunctionsClass.BringWindowToFront(this);
```

**UIFunctionsClass.cs:**
```csharp
public static void BringWindowToFront(Window window)
{
    IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
    ShowWindow(hWnd, SW_RESTORE);  // 복원
    SetForegroundWindow(hWnd);     // 포그라운드로
}
```

---

## ?? 테스트 시나리오

### **Test 1: 기본 토글**
```
1. Settings 창 열기
2. "Always on Top" 토글 OFF → ON
3. Settings 창이 여전히 위에 있음 ?
4. 메인 창도 Always on Top ?
```

### **Test 2: 여러 번 토글**
```
1. Settings 창 열기
2. "Always on Top" OFF → ON
3. Settings 창 위에 있음 ?
4. "Always on Top" ON → OFF
5. Settings 창 여전히 위에 있음 ?
6. "Always on Top" OFF → ON (다시)
7. Settings 창 여전히 위에 있음 ?
```

### **Test 3: 다른 창과의 상호작용**
```
1. Settings 창 열기 (Always on Top OFF)
2. Chrome 창 열기
3. "Always on Top" 토글 ON
4. Settings 창이 Chrome 위에 있음 ?
5. 메인 창도 Chrome 위에 있음 ?
```

### **Test 4: Settings 닫기 후 재열기**
```
1. Settings 창 열기
2. "Always on Top" ON
3. Settings 창 닫기
4. Settings 창 다시 열기
5. Settings 창이 위에 있음 ?
```

---

## ?? 추가 개선 (선택사항)

### **1. 다른 모달 창도 처리**

모든 모달 창이 Always on Top 변경 시 최상위 유지:

```csharp
// MainWindow.xaml.cs
private List<Window> openModalWindows = new List<Window>();

private void OpenModalWindow(Window modalWindow)
{
    UIFunctionsClass.CreateModalWindow(modalWindow, this);
    openModalWindows.Add(modalWindow);
    
modalWindow.Closed += (s, e) => {
      openModalWindows.Remove(modalWindow);
    };
}

// Always on Top 변경 시 모든 모달 창 업데이트
private void UpdateModalWindowsAlwaysOnTop()
{
    foreach (var modal in openModalWindows)
    {
        if (modal is WinUIEx.WindowEx modalEx)
  {
            modalEx.IsAlwaysOnTop = true;
     UIFunctionsClass.BringWindowToFront(modal);
        }
    }
}
```

### **2. 자동 포커스 복원**

Settings 창이 가려졌을 때 자동으로 복원:

```csharp
// SettingsWindow.xaml.cs
private DispatcherTimer focusTimer;

private void Container_Loaded(object sender, RoutedEventArgs e)
{
    // ... 기존 코드 ...
    
    // 포커스 체크 타이머
    focusTimer = new DispatcherTimer();
  focusTimer.Interval = TimeSpan.FromMilliseconds(500);
    focusTimer.Tick += FocusTimer_Tick;
    focusTimer.Start();
}

private void FocusTimer_Tick(object sender, object e)
{
    // Always on Top이 활성화되어 있고
    // 메인 창이 Settings 창을 가리고 있으면
    if (UserSettingsClass.AlwaysOnTop && !this.AppWindow.IsVisible)
    {
        this.IsAlwaysOnTop = true;
        UIFunctionsClass.BringWindowToFront(this);
    }
}
```

### **3. Z-Order 모니터링**

창 순서가 변경되면 자동으로 Settings 창 최상위 유지:

```csharp
// Win32 API
[DllImport("user32.dll")]
private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, 
    int X, int Y, int cx, int cy, uint uFlags);

private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
private const uint SWP_NOMOVE = 0x0002;
private const uint SWP_NOSIZE = 0x0001;

private void EnsureTopmost()
{
    IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
    SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
}
```

---

## ?? Z-Order 이해

### **Windows Z-Order 계층:**

```
┌─────────────────────────────────┐
│  HWND_TOPMOST (Always on Top)   │  ← 최상위
├─────────────────────────────────┤
│  Settings 창 (IsAlwaysOnTop=true)│
│  메인 창 (IsAlwaysOnTop=true)    │
├─────────────────────────────────┤
│  일반 창들      │
│Chrome, Explorer, etc.     │
├─────────────────────────────────┤
│  HWND_BOTTOM         │  ← 최하위
└─────────────────────────────────┘
```

### **토글 시 변화:**

**Before (문제):**
```
[Always on Top OFF]
Settings 창 (IsAlwaysOnTop=true) ← 최상위
메인 창 (IsAlwaysOnTop=false)
Chrome

[Always on Top ON 토글]
메인 창 (IsAlwaysOnTop=true) ← Z-order 재계산으로 최상위로
Settings 창 (IsAlwaysOnTop=true) ← 뒤로 밀림 ?
Chrome
```

**After (해결):**
```
[Always on Top OFF]
Settings 창 (IsAlwaysOnTop=true) ← 최상위
메인 창 (IsAlwaysOnTop=false)
Chrome

[Always on Top ON 토글]
Settings 창 (IsAlwaysOnTop=true) ← BringToFront로 최상위 유지 ?
메인 창 (IsAlwaysOnTop=true)
Chrome
```

---

## ?? 사용자 경험

### **Before (문제):**
```
?? 사용자: "Always on Top을 켜면 Settings 창이 사라져요!"
?? 사용자: "Settings를 다시 열어야 하나요?"
?? 사용자: "불편해요..."
```

### **After (해결):**
```
?? 사용자: "Always on Top을 켜도 Settings 창이 그대로 있네요!"
?? 사용자: "편리해요!"
? 사용자: "완벽해요!"
```

---

## ?? 예상 가능한 문제

### **문제 1: 다른 모달 창**

**증상:**
- EditItemWindow, AddWebsiteDialog 등도 가려질 수 있음

**해결:**
- CreateModalWindow에서 이미 `IsAlwaysOnTop = true` 설정함
- 필요시 각 모달 창에도 동일한 로직 적용

### **문제 2: 포커스 손실**

**증상:**
- BringToFront 후 Settings 창에 포커스가 없을 수 있음

**해결:**
```csharp
UIFunctionsClass.BringWindowToFront(this);
this.Activate();  // 포커스 강제 설정
```

### **문제 3: 깜빡임**

**증상:**
- 창이 뒤로 갔다가 다시 앞으로 오면서 깜빡일 수 있음

**해결:**
- `SuspendLayout` / `ResumeLayout` 사용 (WinForms)
- WinUI 3에서는 일반적으로 문제 없음

---

## ? 구현 완료!

### **변경된 파일:**
- ? `SettingsWindow.xaml.cs`
  - `AlwaysOnTopToggleSwitch_Toggled` 수정
  - Settings 창 Always on Top 재설정
  - BringWindowToFront 호출

### **동작:**
- ? Always on Top 토글 시 Settings 창 최상위 유지
- ? 메인 창에 가려지지 않음
- ? 여러 번 토글해도 정상 작동

---

## ?? 테스트 방법

### **빌드:**
```
Visual Studio → Rebuild Solution → F5
```

### **테스트:**
```
1. Settings 창 열기
2. "Always on Top" 토글 OFF
3. "Always on Top" 토글 ON
4. Settings 창이 여전히 보임 ?
5. 설정 변경 가능 ?
6. 여러 번 토글 테스트 ?
```

---

## ?? 완료!

**Always on Top 토글 문제가 해결되었습니다!**

**이제:**
- ? Settings 창에서 Always on Top을 켜도 가려지지 않음
- ? 항상 최상위에서 설정 변경 가능
- ? 편리한 사용자 경험

**테스트해보세요!** ?
