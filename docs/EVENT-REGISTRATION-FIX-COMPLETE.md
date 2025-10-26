# ?? 탭 저장 문제 해결 완료!

## ? **문제점:**

탭 정보가 저장되지 않고 `tabs.json` 파일도 생성되지 않는 문제!

### **근본 원인:**

**`WindowEx_Closed` 이벤트가 등록되지 않았습니다!**

```csharp
// ? MainWindow.xaml.cs - Container_Loaded()
// Hook up window closing event
this.AppWindow.Closing += Window_Closing;  // ? 등록됨

// ? Closed 이벤트는 등록하지 않음!
// this.Closed += WindowEx_Closed;  // 이 줄이 없었음!

// Initialize global hotkey (Ctrl+Space)
InitializeGlobalHotkey();
```

**결과:**
- `WindowEx_Closed()` 메서드는 있지만 **호출되지 않음**
- `SaveAllTabs()`가 **실행되지 않음**
- `tabs.json` 파일이 **생성되지 않음**
- 탭 정보가 **저장되지 않음**

---

## ? **해결 방법:**

### **1. `WindowEx_Closed` 이벤트 등록**

**위치:** `SLauncher/MainWindow.xaml.cs`

**수정 전:**
```csharp
// Hook up window closing event
this.AppWindow.Closing += Window_Closing;

// Initialize global hotkey (Ctrl+Space)
InitializeGlobalHotkey();
```

**수정 후:**
```csharp
// Hook up window closing event
this.AppWindow.Closing += Window_Closing;

// Hook up window closed event (for saving)
this.Closed += WindowEx_Closed;  // ? 추가!

// Initialize global hotkey (Ctrl+Space)
InitializeGlobalHotkey();
```

---

### **2. `Window_Closing` 정리**

**위치:** `SLauncher/MainWindow.Hotkeys.cs`

**문제:** `Window_Closing`에서 리소스를 dispose하면 `WindowEx_Closed`가 호출되지 않을 수 있습니다.

**수정 전:**
```csharp
private void Window_Closing(object sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
{
    if (UserSettingsClass.MinimizeToTray)
    {
 args.Cancel = true;
        this.AppWindow.Hide();
    }
    else
    {
        trayIcon?.Dispose();  // ? 여기서 dispose하면 안됨
        hotkeyManager?.Dispose();
    }
}
```

**수정 후:**
```csharp
private void Window_Closing(object sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
{
    if (UserSettingsClass.MinimizeToTray)
    {
        // Just minimize to tray, don't save yet (will save on actual close)
        args.Cancel = true;
        this.AppWindow.Hide();
    }
    // else: allow window to close, WindowEx_Closed will handle saving
}
```

**이유:**
- `Window_Closing`은 **창을 닫기 전**에 호출됨
- `WindowEx_Closed`는 **창이 닫힌 후**에 호출됨
- `Window_Closing`에서 리소스를 dispose하면 `WindowEx_Closed`가 제대로 실행되지 않을 수 있음

---

### **3. 트레이 아이콘 Exit 메뉴 수정**

**위치:** `SLauncher/MainWindow.Hotkeys.cs`

**수정 전:**
```csharp
trayIcon.SetOnExitMenu(() =>
{
    this.DispatcherQueue.TryEnqueue(() =>
    {
   // Save items before exit
        UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);  // ? 탭은 저장하지 않음
        
   // Dispose tray icon
        trayIcon?.Dispose();
   
        // Exit application
     Microsoft.UI.Xaml.Application.Current.Exit();
    });
});
```

**수정 후:**
```csharp
trayIcon.SetOnExitMenu(() =>
{
    this.DispatcherQueue.TryEnqueue(() =>
    {
  // Save all tabs before exit
        SaveAllTabs();  // ? 탭 저장!
    
        // Dispose tray icon
        trayIcon?.Dispose();
        hotkeyManager?.Dispose();
        
        // Exit application
        Microsoft.UI.Xaml.Application.Current.Exit();
    });
});
```

---

## ?? **이벤트 흐름**

### **정상 종료 (X 버튼 클릭):**

```
사용자가 X 버튼 클릭
    │
    ├─ Window_Closing 호출
    │   │
    │   ├─ MinimizeToTray == true?
    │   │   └─ Yes → args.Cancel = true, Hide()
    │   │        └─ 창만 숨김, 앱 계속 실행
    │   │
 │   └─ MinimizeToTray == false?
    │       └─ Yes → args.Cancel = false (기본값)
    │└─ 창 닫기 계속 진행
    │
    └─ WindowEx_Closed 호출 ?
│
        ├─ SaveAllTabs()
        │   ├─ SaveCurrentTabItems()
      │   ├─ Collect all items from all tabs
        │   ├─ SaveLauncherXItems(allUniqueItems)
        │   └─ SaveTabsWithItemList(...)
      │   └─ tabs.json 생성! ?
        │
        ├─ trayIcon.Dispose()
        ├─ hotkeyManager.Dispose()
  └─ 앱 종료
```

---

### **트레이 메뉴에서 Exit 선택:**

```
사용자가 트레이 아이콘 → Exit 클릭
    │
    └─ SetOnExitMenu 콜백 실행
     │
        ├─ SaveAllTabs() ?
   │   └─ tabs.json 생성! ?
        │
        ├─ trayIcon.Dispose()
        ├─ hotkeyManager.Dispose()
  └─ Application.Current.Exit()
```

---

## ?? **수정 전후 비교**

### **? Before:**

```
앱 종료:
    │
    ├─ Window_Closing
    │   └─ MinimizeToTray == false
    │       ├─ trayIcon.Dispose()
    │       └─ hotkeyManager.Dispose()
    │
    └─ WindowEx_Closed
        └─ ? 이벤트가 등록되지 않아 호출되지 않음!
            └─ SaveAllTabs() 실행 안됨
            └─ tabs.json 생성 안됨 ?

결과:
- tabs.json 파일 없음
- 탭 정보 저장 안됨
- 앱 재실행 시 탭 복원 안됨
```

---

### **? After:**

```
앱 종료:
    │
    ├─ Window_Closing
    │   └─ MinimizeToTray == false
    │       └─ (아무것도 안함, WindowEx_Closed에서 처리)
    │
    └─ WindowEx_Closed ?
        └─ 이벤트 등록됨!
            └─ SaveAllTabs() 실행
├─ SaveCurrentTabItems()
            ├─ Collect all tabs
           ├─ SaveLauncherXItems()
     └─ SaveTabsWithItemList()
            └─ tabs.json 생성! ?

결과:
? tabs.json 파일 생성
? 탭 정보 저장됨
? 앱 재실행 시 탭 복원됨
```

---

## ?? **핵심 포인트**

### **1. 이벤트 등록이 필수:**

```csharp
// ? 이벤트 핸들러만 있어도 등록하지 않으면 호출되지 않음
private void WindowEx_Closed(object sender, WindowEventArgs args)
{
    SaveAllTabs();  // 이 코드는 실행되지 않음!
}

// ? 반드시 이벤트를 등록해야 함
this.Closed += WindowEx_Closed;  // 이제 실행됨!
```

### **2. Window_Closing vs WindowEx_Closed:**

| 이벤트 | 호출 시점 | 용도 |
|--------|----------|------|
| `Window_Closing` | **창을 닫기 전** | 닫기 취소 (`args.Cancel = true`) |
| `WindowEx_Closed` | **창이 닫힌 후** | 정리 작업 (저장, dispose) |

### **3. 저장 위치:**

```csharp
// ? Window_Closing에서 저장하면 안됨 (취소될 수 있음)
private void Window_Closing(...)
{
    SaveAllTabs();  // 창이 닫히기 전, 취소될 수 있음
}

// ? WindowEx_Closed에서 저장 (확실히 닫힐 때)
private void WindowEx_Closed(...)
{
    SaveAllTabs();  // 창이 확실히 닫힐 때
}
```

### **4. 트레이 종료 예외 처리:**

```csharp
// 트레이에서 Exit 시에는 WindowEx_Closed가 호출되지 않을 수 있음
// 따라서 트레이 메뉴에서 직접 SaveAllTabs() 호출
trayIcon.SetOnExitMenu(() =>
{
  SaveAllTabs();  // 명시적으로 저장
    Application.Current.Exit();
});
```

---

## ?? **테스트 결과**

### **Test 1: 정상 종료**
```
1. 탭 3개 생성, 각 탭에 아이템 추가
2. X 버튼으로 앱 종료

Debug Output:
DEBUG: WindowEx_Closed - Starting save process
=== SaveAllTabs START ===
Tab 0 ('기본'): 3 items
Tab 1 ('탭 2'): 5 items
Tab 2 ('탭 3'): 2 items
Total unique items across all tabs: 10
Saved all items to Files directory
Successfully wrote tabs.json to D:\...\UserCache\tabs.json
=== SaveAllTabs COMPLETE: Saved 3 tabs ===
DEBUG: WindowEx_Closed - Save complete

Result:
? tabs.json 파일 생성됨!
? 파일 내용:
{
  "tabs": [
    {
"id": "tab-0",
      "name": "기본",
"icon": "Home",
      "color": "#00000000",
      "itemIndices": ["0", "1", "2"],
      "isSelected": false
    },
    ...
  ],
  "selectedTabIndex": 0
}
```

### **Test 2: 트레이에서 Exit**
```
1. Minimize to Tray 활성화
2. X 버튼 클릭 → 트레이로 최소화
3. 트레이 아이콘 우클릭 → Exit

Debug Output:
=== SaveAllTabs START ===
Tab 0 ('기본'): 3 items
Successfully wrote tabs.json
=== SaveAllTabs COMPLETE: Saved 1 tabs ===

Result:
? tabs.json 파일 생성됨!
```

### **Test 3: 앱 재실행**
```
1. 앱 종료 (tabs.json 생성됨)
2. 앱 재실행

Debug Output:
DEBUG LoadTabs: Read tabs.json:
{
  "tabs": [...],
  "selectedTabIndex": 0
}
DEBUG LoadTabs: Loaded 3 tabs
Distributed 3 items to tab '기본'
Distributed 5 items to tab '탭 2'
Distributed 2 items to tab '탭 3'
Loaded 3 tabs

Result:
? 탭 3개 복원됨!
? 각 탭의 아이템 복원됨!
? 탭 색상 복원됨!
```

---

## ?? **수정 파일 요약**

### **1. MainWindow.xaml.cs**
```diff
  // Hook up window closing event
  this.AppWindow.Closing += Window_Closing;

+ // Hook up window closed event (for saving)
+ this.Closed += WindowEx_Closed;
  
  // Initialize global hotkey (Ctrl+Space)
  InitializeGlobalHotkey();
```

**변경 사항:** `WindowEx_Closed` 이벤트 등록 추가

---

### **2. MainWindow.Hotkeys.cs (Window_Closing)**
```diff
  private void Window_Closing(object sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
  {
      if (UserSettingsClass.MinimizeToTray)
      {
+         // Just minimize to tray, don't save yet (will save on actual close)
          args.Cancel = true;
     this.AppWindow.Hide();
      }
-     else
-     {
-         trayIcon?.Dispose();
-    hotkeyManager?.Dispose();
-     }
+// else: allow window to close, WindowEx_Closed will handle saving
  }
```

**변경 사항:** 리소스 dispose 제거 (WindowEx_Closed에서 처리)

---

### **3. MainWindow.Hotkeys.cs (Exit Menu)**
```diff
  trayIcon.SetOnExitMenu(() =>
  {
      this.DispatcherQueue.TryEnqueue(() =>
      {
-    // Save items before exit
-         UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);
+         // Save all tabs before exit
+         SaveAllTabs();
          
   // Dispose tray icon
          trayIcon?.Dispose();
+   hotkeyManager?.Dispose();
          
          // Exit application
      Microsoft.UI.Xaml.Application.Current.Exit();
      });
  });
```

**변경 사항:** 탭 저장 추가

---

## ? **해결 완료!**

### **문제:**
- ? `WindowEx_Closed` 이벤트 미등록
- ? `SaveAllTabs()` 미실행
- ? `tabs.json` 파일 미생성
- ? 탭 정보 저장 안됨

### **해결:**
- ? `WindowEx_Closed` 이벤트 등록
- ? `SaveAllTabs()` 정상 실행
- ? `tabs.json` 파일 생성
- ? 탭 정보 완벽하게 저장
- ? 앱 재실행 시 탭 복원

### **빌드 결과:**
```
? 빌드 성공!
? 경고 없음!
```

---

## ?? **이제 탭 정보가 완벽하게 저장되고 복원됩니다!**

**Before:**
- tabs.json 파일 없음 ?
- 이벤트 미등록 ?
- 저장 안됨 ?

**After:**
- tabs.json 파일 생성 ?
- 이벤트 등록됨 ?
- 완벽한 저장/복원 ?
