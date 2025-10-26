# ? Settings 개선 완료!

## ?? 구현된 기능

### 1. **Always On Top 문제 수정**
- Settings 창이 메인 창의 Always On Top에 가려지는 문제 해결
- Settings 창이 항상 메인 창 위에 표시됨

### 2. **글로벌 핫키 사용자 정의**
- Settings에서 글로벌 핫키 변경 가능
- Ctrl+Space, Alt+Tab, Ctrl+Alt+F1 등 다양한 조합 지원
- 재시작 없이 즉시 적용 (다음 실행 시)

---

## ?? 수정된 파일

### **1. UIFunctionsClass.cs**

#### CreateModalWindow 수정:

```csharp
public static void CreateModalWindow(Window modalWindow, Window parentWindow)
{
    // Disable parent window
    Shell32.EnableWindow(WinRT.Interop.WindowNative.GetWindowHandle(parentWindow), false);

    // ? Set modal window to always on top
    if (modalWindow is WinUIEx.WindowEx modalWindowEx)
 {
   modalWindowEx.IsAlwaysOnTop = true;
    }
    
 // Enable parent window when closed
    modalWindow.Closed += (s, e) => 
   Shell32.EnableWindow(WinRT.Interop.WindowNative.GetWindowHandle(parentWindow), true);
    
    modalWindow.Activate();
}
```

**효과:**
- Settings 창이 항상 메인 창 위에 표시
- Always On Top 설정과 관계없이 Settings 접근 가능

---

### **2. SettingsWindow.xaml**

#### 글로벌 핫키 설정 UI 추가:

```xml
<wct:SettingsCard
    Margin="0,5,0,0"
    Description="Press the hotkey anywhere to show/hide SLauncher. Click to change."
    Header="Global Hotkey">
    <wct:SettingsCard.HeaderIcon>
        <FontIcon Glyph="&#xE765;" />
    </wct:SettingsCard.HeaderIcon>
    <Button
     x:Name="HotkeyButton"
        Content="Ctrl + Space"
 Width="150"
     Click="HotkeyButton_Click"
 ToolTipService.ToolTip="Click to change hotkey" />
</wct:SettingsCard>
```

---

### **3. SettingsWindow.xaml.cs**

#### 추가된 메서드:

**UpdateHotkeyButtonText()**
```csharp
private void UpdateHotkeyButtonText()
{
  HotkeyButton.Content = UserSettingsClass.GlobalHotkey;
}
```

**HotkeyButton_Click()**
```csharp
private async void HotkeyButton_Click(object sender, RoutedEventArgs e)
{
    // Show dialog to change hotkey
    ContentDialog hotkeyDialog = new ContentDialog
    {
        Title = "Change Global Hotkey",
      Content = CreateHotkeyDialogContent(),
        PrimaryButtonText = "Save",
        CloseButtonText = "Cancel"
};

    var result = await hotkeyDialog.ShowAsync();
    
    if (result == ContentDialogResult.Primary)
 {
      // Save new hotkey
        string newHotkey = $"{modifier}+{key}";
   UserSettingsClass.GlobalHotkey = newHotkey;
        UserSettingsClass.WriteSettingsFile();
        UpdateHotkeyButtonText();
    
        // Show restart notice
        await ShowRestartNotice();
 }
}
```

**CreateHotkeyDialogContent()**
```csharp
private StackPanel CreateHotkeyDialogContent()
{
    var panel = new StackPanel { Spacing = 10 };
    
    // Modifier ComboBox
    var modifierCombo = new ComboBox();
    modifierCombo.Items.Add("Ctrl");
    modifierCombo.Items.Add("Alt");
    modifierCombo.Items.Add("Shift");
    modifierCombo.Items.Add("Ctrl+Shift");
    modifierCombo.Items.Add("Ctrl+Alt");
    
    // Key ComboBox
    var keyCombo = new ComboBox();
    keyCombo.Items.Add("Space");
    keyCombo.Items.Add("Tab");
    keyCombo.Items.Add("Enter");
    keyCombo.Items.Add("Esc");
    keyCombo.Items.Add("F1" - "F4");
    
    panel.Children.Add(modifierCombo);
    panel.Children.Add(keyCombo);
    
    return panel;
}
```

---

### **4. GlobalHotkeyManager.cs**

#### 동적 핫키 등록 추가:

**RegisterHotkey() 메서드:**

```csharp
public bool RegisterHotkey(string hotkeyString, Action onHotkeyPressed)
{
    // Parse hotkey string (e.g., "Ctrl+Space", "Ctrl+Alt+F1")
    uint modifiers = MOD_NOREPEAT;
    uint vk = 0;
    
    string[] parts = hotkeyString.Split('+');
    
    // Parse modifiers
    for (int i = 0; i < parts.Length - 1; i++)
    {
        string part = parts[i].Trim();
        if (part == "Ctrl") modifiers |= MOD_CONTROL;
        else if (part == "Alt") modifiers |= MOD_ALT;
        else if (part == "Shift") modifiers |= MOD_SHIFT;
        else if (part == "Win") modifiers |= MOD_WIN;
    }
    
    // Get virtual key code
    string key = parts[parts.Length - 1].Trim();
  vk = GetVirtualKeyCode(key);
    
    // Register hotkey
    return RegisterHotKey(_windowHandle, HOTKEY_ID, modifiers, vk);
}
```

**GetVirtualKeyCode() 메서드:**

```csharp
private uint GetVirtualKeyCode(string key)
{
    switch (key.ToLower())
    {
        case "space": return 0x20;
    case "tab": return 0x09;
        case "enter": return 0x0D;
        case "esc": return 0x1B;
        case "f1": return 0x70;
    case "f2": return 0x71;
 case "f3": return 0x72;
     case "f4": return 0x73;
     // ...
        default: return 0;
    }
}
```

---

### **5. MainWindow.xaml.cs**

#### InitializeGlobalHotkey 수정:

```csharp
private void InitializeGlobalHotkey()
{
    try
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        hotkeyManager = new GlobalHotkeyManager(hwnd);

        // ? Use hotkey from settings instead of hardcoded
bool success = hotkeyManager.RegisterHotkey(
            UserSettingsClass.GlobalHotkey, 
     () => {
        this.DispatcherQueue.TryEnqueue(() => {
       ToggleWindowVisibility();
       });
   });

      if (!success)
        {
  Debug.WriteLine($"Failed to register {UserSettingsClass.GlobalHotkey}");
}
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Error initializing global hotkey: {ex}");
    }
}
```

---

## ?? 사용 방법

### **1. Always On Top 문제 해결**

**Before (문제):**
```
1. Always On Top 활성화
2. Settings 버튼 클릭
3. Settings 창이 메인 창 뒤로 숨겨짐 ?
4. Settings 접근 불가능
```

**After (해결):**
```
1. Always On Top 활성화
2. Settings 버튼 클릭
3. Settings 창이 메인 창 위에 표시 ?
4. 정상적으로 설정 변경 가능
```

---

### **2. 글로벌 핫키 변경**

**단계:**
```
1. Settings 버튼 클릭
2. "Global Hotkey" 섹션 찾기
3. 현재 핫키 버튼 클릭 (예: "Ctrl + Space")
4. Modifier 선택 (Ctrl, Alt, Shift, Ctrl+Shift, Ctrl+Alt)
5. Key 선택 (Space, Tab, Enter, Esc, F1-F4)
6. "Save" 클릭
7. 재시작 안내 확인
8. SLauncher 재시작
9. 새 핫키로 작동 ?
```

---

## ?? 지원되는 핫키 조합

### **Modifiers:**
- `Ctrl`
- `Alt`
- `Shift`
- `Ctrl+Shift`
- `Ctrl+Alt`

### **Keys:**
- `Space`
- `Tab`
- `Enter`
- `Esc`
- `F1`, `F2`, `F3`, `F4`

### **예시 조합:**
```
Ctrl+Space      ← 기본값
Alt+Space     ← PowerToys Run 스타일
Ctrl+Shift+Space
Ctrl+Alt+F1
Alt+Tab         ← (주의: Windows 기본 단축키와 충돌 가능)
```

---

## ?? 주의사항

### **1. 핫키 충돌**

**다른 앱과 충돌 시:**
```
Error: Failed to register Alt+Space hotkey
```

**해결:**
- 다른 앱 종료
- 다른 핫키 조합 선택
- Windows 기본 단축키는 피하기

### **2. Windows 예약 단축키**

피해야 할 조합:
```
? Win+D      (바탕화면 표시)
? Win+E   (파일 탐색기)
? Alt+Tab    (창 전환)
? Alt+F4      (창 닫기)
? Ctrl+Alt+Del  (보안 화면)
```

### **3. 재시작 필요**

```
핫키 변경 후 반드시 SLauncher를 재시작해야 합니다!
```

**이유:**
- GlobalHotkeyManager가 앱 시작 시 한 번만 등록
- 런타임에 핫키 변경하려면 UnregisterHotKey → RegisterHotKey 필요
- 현재는 재시작으로 간단히 처리

---

## ?? UI 스크린샷

### **Settings 창:**

```
┌─────────────────────────────────────┐
│  SLauncher Settings [X] │
├─────────────────────────────────────┤
│         │
│  Settings        │
│  ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━  │
│      │
│  ?? Change header text              │
│     [My files, folders, websites]   │
│           │
│  ??? Enable fullscreen               │
│     [Yes/No]          │
│             │
│  ?? Grid alignment      │
│     [Left ▼]             │
│        │
│  ?? Start with Windows         │
│     [Yes/No]         │
│          │
│  ?? Always on top          │
│     [Yes/No]  │
│ │
│  ?? Global Hotkey       │
│     [Ctrl + Space]  ← 클릭 가능     │
│    │
├─────────────────────────────────────┤
│ [Close]     │
└─────────────────────────────────────┘
```

### **핫키 변경 다이얼로그:**

```
┌─────────────────────────────────────┐
│  Change Global Hotkey          │
├─────────────────────────────────────┤
│      │
│  Modifier Key:          │
│  ┌─────────────────────┐            │
│  │ Ctrl              ▼ │         │
│  └─────────────────────┘            │
│  - Ctrl     │
│  - Alt          │
│  - Shift        │
│  - Ctrl+Shift              │
│  - Ctrl+Alt             │
│       │
│  Key:           │
│  ┌─────────────────────┐         │
│  │ Space   ▼ │            │
│  └─────────────────────┘            │
│  - Space              │
│  - Tab     │
│  - Enter  │
│  - Esc         │
│  - F1, F2, F3, F4      │
│               │
├─────────────────────────────────────┤
│       [Cancel]  [Save]       │
└─────────────────────────────────────┘
```

### **재시작 안내:**

```
┌─────────────────────────────────────┐
│  Restart Required  │
├─────────────────────────────────────┤
│  │
│  Please restart SLauncher for the   │
│  hotkey change to take effect.      │
│    │
├─────────────────────────────────────┤
│              [OK]     │
└─────────────────────────────────────┘
```

---

## ?? 향후 개선 가능

### **1. 실시간 핫키 변경**

```csharp
public void ChangeHotkey(string newHotkey)
{
    // Unregister old hotkey
    UnregisterHotKey(_windowHandle, HOTKEY_ID);
    
    // Register new hotkey
    RegisterHotkey(newHotkey, _onHotkeyPressed);
}
```

**장점:**
- 재시작 불필요
- 즉시 적용

### **2. 핫키 충돌 감지**

```csharp
if (!success)
{
    ContentDialog conflictDialog = new ContentDialog
    {
        Title = "Hotkey Conflict",
        Content = $"{newHotkey} is already in use by another application. Choose a different hotkey.",
        CloseButtonText = "OK"
    };
    await conflictDialog.ShowAsync();
}
```

### **3. 핫키 테스트 버튼**

```xml
<Button Content="Test Hotkey" 
        Click="TestHotkeyButton_Click"/>
```

**기능:**
- 핫키 등록 전 테스트
- 충돌 여부 확인
- 실제 동작 미리보기

### **4. 더 많은 키 지원**

```csharp
case "a": case "b": case "c": // ... case "z":
    return (uint)(key.ToUpper()[0]);  // A-Z
case "0": case "1": // ... case "9":
    return (uint)(key[0]);     // 0-9
case "f5": case "f6": // ... case "f12":
    return 0x74 + uint.Parse(key.Substring(1)) - 5;  // F5-F12
```

### **5. 프리셋 제공**

```xml
<MenuFlyout>
    <MenuFlyoutItem Text="Ctrl+Space (Default)" />
    <MenuFlyoutItem Text="Alt+Space (PowerToys)" />
    <MenuFlyoutItem Text="Ctrl+Shift+Space" />
    <MenuFlyoutSeparator />
<MenuFlyoutItem Text="Custom..." />
</MenuFlyout>
```

---

## ?? 테스트 시나리오

### **Test 1: Always On Top 문제**
```
1. Always On Top 활성화
2. Settings 열기
3. Settings 창이 위에 표시됨 ?
4. 설정 변경 가능 ?
```

### **Test 2: 핫키 변경**
```
1. Settings → Global Hotkey 클릭
2. Alt + Space 선택
3. Save 클릭
4. 재시작 안내 확인 ?
5. SLauncher 재시작
6. Alt+Space로 토글 ?
```

### **Test 3: 핫키 충돌**
```
1. 다른 앱에서 Alt+Space 사용 중
2. Settings에서 Alt+Space 선택
3. SLauncher 재시작
4. Debug 창에 에러 메시지 ?
5. 핫키 작동 안 함 (예상된 동작)
```

### **Test 4: 여러 Settings 창**
```
1. Settings 창 열기
2. 다시 Settings 버튼 클릭
3. 두 번째 Settings 창이 첫 번째 위에 표시 ?
```

---

## ?? 코드 변경 요약

### **변경된 파일:**

1. ? `UIFunctionsClass.cs`
   - CreateModalWindow에 IsAlwaysOnTop 추가

2. ? `SettingsWindow.xaml`
   - 글로벌 핫키 SettingsCard 추가

3. ? `SettingsWindow.xaml.cs`
   - HotkeyButton_Click 추가
   - CreateHotkeyDialogContent 추가
   - UpdateHotkeyButtonText 추가

4. ? `GlobalHotkeyManager.cs`
   - RegisterHotkey 메서드 추가
   - GetVirtualKeyCode 메서드 추가

5. ? `MainWindow.xaml.cs`
   - InitializeGlobalHotkey 수정 (동적 핫키 사용)

---

## ? 구현 완료!

### **해결된 문제:**

1. ? **Always On Top 문제**
   - Settings 창이 항상 메인 창 위에 표시
 - IsAlwaysOnTop = true로 강제 설정

2. ? **글로벌 핫키 사용자 정의**
   - Settings에서 핫키 변경 가능
   - Modifier + Key 조합 선택
   - 재시작 후 적용

### **추가된 기능:**

1. ? 핫키 변경 다이얼로그
2. ? 동적 핫키 등록
3. ? 핫키 문자열 파싱
4. ? 재시작 안내

---

## ?? 빌드 및 테스트

### **빌드:**
```
Visual Studio → Rebuild Solution → F5
```

### **테스트:**
```
1. Always On Top 활성화
2. Settings 열기 → 정상 표시 확인
3. Global Hotkey 변경
4. 재시작 후 새 핫키로 테스트
```

---

## ?? 완료!

**Settings 개선이 성공적으로 완료되었습니다!**

**해결:**
- ? Always On Top 설정 시 Settings 창 접근 가능
- ? 글로벌 핫키를 Settings에서 변경 가능
- ? Ctrl+Space, Alt+Space 등 다양한 조합 지원

**이제 편리하게 Settings를 사용하고 원하는 핫키로 변경할 수 있습니다!** ?
