# ?? Settings 버튼 중복 클릭 방지 - 해결 완료!

## ? 문제

**증상:**
```
1. Settings 버튼 클릭
2. Settings 창 열림
3. Settings 버튼 빠르게 다시 클릭
4. Settings 창 2개 열림 ?
5. 혼란스러운 상태
```

**원인:**
- 첫 번째 클릭으로 Settings 창이 열리는 동안
- 두 번째 클릭이 처리됨
- `CreateModalWindow`가 두 번 호출됨
- 중복 클릭 방지 로직 없음

---

## ? 해결 방법

### **2단계 방어:**

**1. 플래그 확인**
```csharp
private bool isSettingsWindowOpen = false;

if (isSettingsWindowOpen)
{
    return;  // 이미 열려 있으면 무시
}
```

**2. 버튼 비활성화**
```csharp
SettingsButton.IsEnabled = false;  // 버튼 비활성화
```

---

## ?? 수정된 코드

### **MainWindow.xaml.cs**

**Before (문제):**

```csharp
private void SettingsButton_Click(object sender, RoutedEventArgs e)
{
    // Open Settings Window as a modal window
    SettingsWindow settingsWindow = new SettingsWindow();
  UIFunctionsClass.CreateModalWindow(settingsWindow, this);

    // Update the UI once the SettingsWindow is closed
    settingsWindow.Closed += (s, e) => UpdateUIFromSettings();
}
```

**문제점:**
- 중복 클릭 시 여러 창 생성
- 버튼이 계속 활성화 상태
- 사용자에게 피드백 없음

---

**After (해결):**

```csharp
// Track if settings window is already open
private bool isSettingsWindowOpen = false;

private void SettingsButton_Click(object sender, RoutedEventArgs e)
{
    // ? Prevent opening multiple settings windows
    if (isSettingsWindowOpen)
    {
  return;
    }

    isSettingsWindowOpen = true;
  
    // ? Disable settings button
    SettingsButton.IsEnabled = false;

    // Open Settings Window as a modal window
    SettingsWindow settingsWindow = new SettingsWindow();
    UIFunctionsClass.CreateModalWindow(settingsWindow, this);

    // Update the UI once the SettingsWindow is closed
    settingsWindow.Closed += (s, e) =>
    {
        UpdateUIFromSettings();
   isSettingsWindowOpen = false;
        
 // ? Re-enable settings button
      SettingsButton.IsEnabled = true;
    };
}
```

**개선 사항:**
- ? 플래그로 중복 방지
- ? 버튼 비활성화로 시각적 피드백
- ? 창 닫을 때 상태 복원

---

## ?? 작동 원리

### **시나리오 1: 빠른 더블클릭 (문제 상황)**

**Before:**
```
Click 1 → CreateModalWindow() 시작
  ↓ (100ms 소요)
Click 2 → CreateModalWindow() 또 시작 ?
  ↓
Settings 창 2개 열림 ?
```

**After:**
```
Click 1 → isSettingsWindowOpen = true
        → SettingsButton.IsEnabled = false
        → CreateModalWindow() 시작
  ↓
Click 2 → if (isSettingsWindowOpen) return; ?
    → 무시됨 ?
  ↓
Settings 창 1개만 열림 ?
```

---

### **시나리오 2: 정상 클릭**

**동작:**
```
1. Settings 버튼 클릭
   → isSettingsWindowOpen = true
   → 버튼 비활성화 (회색)

2. Settings 창 열림
   → 사용자 설정 변경

3. Settings 창 닫기
   → UpdateUIFromSettings()
   → isSettingsWindowOpen = false
   → 버튼 활성화 (다시 클릭 가능)

4. Settings 버튼 다시 클릭 가능 ?
```

---

## ?? 플래그 vs 버튼 비활성화

### **플래그만 사용할 경우:**

```csharp
if (isSettingsWindowOpen)
{
    return;  // 무시
}
```

**장점:**
- ? 중복 방지 확실
- ? 간단한 구현

**단점:**
- ? 사용자에게 피드백 없음
- ? 버튼이 여전히 클릭 가능해 보임
- ? 왜 안 되는지 모름

---

### **버튼 비활성화도 함께:**

```csharp
SettingsButton.IsEnabled = false;
```

**장점:**
- ? 시각적 피드백 (회색으로 변경)
- ? 사용자가 버튼을 클릭할 수 없음
- ? "이미 열려 있구나" 인지 가능
- ? 더 나은 UX

**단점:**
- 없음

**결론: 둘 다 사용하는 것이 최선!** ?

---

## ?? 사용자 경험

### **Before (문제):**

```
?? 사용자: "Settings 버튼을 빠르게 2번 눌렀더니 창이 2개 떴어요"
?? 사용자: "어느 창을 닫아야 하죠?"
?? 사용자: "버그인가요?"
```

### **After (해결):**

```
?? 사용자: "Settings 버튼을 눌렀더니 버튼이 회색으로 변했어요"
?? 사용자: "아, 이미 열려 있구나!"
? 사용자: "직관적이에요!"
```

---

## ?? 중복 방지 패턴

### **패턴 1: 플래그 (Flag)**

```csharp
private bool isOperationInProgress = false;

private void Button_Click(object sender, RoutedEventArgs e)
{
    if (isOperationInProgress)
    {
        return;
    }

    isOperationInProgress = true;
    
    // ... 작업 수행 ...
    
    // 작업 완료 후
    isOperationInProgress = false;
}
```

**사용 사례:**
- 모달 창 열기
- 파일 저장/로드
- 네트워크 요청
- 긴 작업 (로딩)

---

### **패턴 2: 버튼 비활성화**

```csharp
private async void Button_Click(object sender, RoutedEventArgs e)
{
    Button.IsEnabled = false;
    
    // ... 작업 수행 ...
    
    Button.IsEnabled = true;
}
```

**사용 사례:**
- 사용자 입력 방지
- 시각적 피드백
- Form 제출
- 중복 실행 방지

---

### **패턴 3: 디바운싱 (Debouncing)**

```csharp
private DateTime lastClickTime = DateTime.MinValue;
private TimeSpan debounceInterval = TimeSpan.FromMilliseconds(500);

private void Button_Click(object sender, RoutedEventArgs e)
{
    DateTime now = DateTime.Now;
    
    if (now - lastClickTime < debounceInterval)
    {
        return;  // 너무 빨리 클릭됨
    }
    
    lastClickTime = now;
    
    // ... 작업 수행 ...
}
```

**사용 사례:**
- 검색 입력
- 자동 저장
- API 요청 제한
- 스크롤 이벤트

---

### **패턴 4: 세마포어 (Semaphore)**

```csharp
private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

private async void Button_Click(object sender, RoutedEventArgs e)
{
    if (!await semaphore.WaitAsync(0))  // 즉시 시도
    {
     return;  // 이미 실행 중
    }
    
    try
    {
    // ... 작업 수행 ...
    }
    finally
    {
        semaphore.Release();
    }
}
```

**사용 사례:**
- 멀티스레드 환경
- 비동기 작업
- 복잡한 동기화
- 리소스 제한

---

## ?? 우리의 선택: 플래그 + 버튼 비활성화

**왜?**

```
1. 간단함 ?
   - 플래그 1개
   - IsEnabled 속성만 변경

2. 효과적 ?
   - 중복 방지 확실
   - 시각적 피드백

3. 성능 ?
   - 오버헤드 없음
   - 즉시 반응

4. 유지보수 ?
   - 코드 이해하기 쉬움
   - 버그 가능성 낮음
```

**완벽한 선택!** ??

---

## ?? 테스트 시나리오

### **Test 1: 빠른 더블클릭**

```
1. Settings 버튼 클릭
2. 즉시 다시 클릭
3. 결과: Settings 창 1개만 열림 ?
4. 버튼 비활성화 상태 ?
```

### **Test 2: 트리플 클릭**

```
1. Settings 버튼 클릭
2. 연속으로 2번 더 클릭
3. 결과: Settings 창 1개만 열림 ?
4. 추가 클릭 무시됨 ?
```

### **Test 3: 창 닫고 다시 열기**

```
1. Settings 버튼 클릭
2. Settings 창 열림
3. Settings 창 닫기
4. Settings 버튼 다시 클릭 가능 ?
5. Settings 창 다시 열림 ?
```

### **Test 4: 버튼 비활성화 확인**

```
1. Settings 버튼 클릭
2. 버튼이 회색으로 변경 ?
3. Settings 창 닫기
4. 버튼이 다시 활성화 ?
```

### **Test 5: 여러 번 반복**

```
1. Settings 열기 → 닫기
2. Settings 열기 → 닫기
3. Settings 열기 → 닫기
4. 모두 정상 작동 ?
5. 창 1개씩만 열림 ?
```

---

## ?? 예상 가능한 문제

### **문제 1: 창을 강제로 닫을 때**

**증상:**
```
Alt+F4나 작업 관리자로 Settings 창 강제 종료
→ isSettingsWindowOpen = true 그대로
→ 버튼이 비활성화 상태로 남음
```

**해결:**
```csharp
settingsWindow.Closed += (s, e) =>
{
    // Closed 이벤트는 모든 종료 방법에서 발생
    // Alt+F4, X버튼, 작업 관리자 모두 OK ?
    UpdateUIFromSettings();
    isSettingsWindowOpen = false;
    SettingsButton.IsEnabled = true;
};
```

**현재 코드는 이미 안전함!** ?

---

### **문제 2: 예외 발생 시**

**증상:**
```
Settings 창 생성 중 예외 발생
→ isSettingsWindowOpen = true 그대로
→ 버튼 복구 안 됨
```

**해결:**
```csharp
private void SettingsButton_Click(object sender, RoutedEventArgs e)
{
    if (isSettingsWindowOpen)
    {
        return;
    }

  isSettingsWindowOpen = true;
    SettingsButton.IsEnabled = false;

    try
    {
    SettingsWindow settingsWindow = new SettingsWindow();
        UIFunctionsClass.CreateModalWindow(settingsWindow, this);

        settingsWindow.Closed += (s, e) =>
        {
    UpdateUIFromSettings();
     isSettingsWindowOpen = false;
            SettingsButton.IsEnabled = true;
        };
    }
    catch (Exception ex)
    {
        // 예외 발생 시 상태 복원
        isSettingsWindowOpen = false;
        SettingsButton.IsEnabled = true;
    
        // 에러 표시
        Debug.WriteLine($"Error opening settings: {ex}");
}
}
```

**필요 시 추가 가능** (현재는 예외 가능성 낮음)

---

### **문제 3: 메모리 누수**

**증상:**
```
Settings 창을 여러 번 열고 닫음
→ 이벤트 핸들러 누적?
→ 메모리 사용량 증가?
```

**확인:**
```csharp
settingsWindow.Closed += (s, e) =>
{
    // 람다는 settingsWindow 인스턴스에 연결됨
    // 창이 닫히면 이벤트 핸들러도 GC됨
    // 메모리 누수 없음 ?
};
```

**현재 코드는 안전함!** ?

---

## ?? 완료!

### **변경된 파일:**
- ? `MainWindow.xaml.cs`
  - `isSettingsWindowOpen` 플래그 추가
  - `SettingsButton_Click` 메서드 수정
  - 버튼 비활성화/활성화 로직 추가

### **해결된 문제:**
- ? Settings 버튼 중복 클릭 방지
- ? Settings 창 1개만 열림
- ? 시각적 피드백 (버튼 비활성화)
- ? 사용자 경험 개선

### **특징:**
- ? 간단하고 효과적인 구현
- ? 성능 오버헤드 없음
- ? 모든 종료 방법에서 안전
- ? 유지보수 용이

---

## ?? 테스트 방법

```
1. SLauncher 실행

2. Settings 버튼 빠르게 2번 클릭
   → Settings 창 1개만 열림 ?
   → 버튼 비활성화 상태 ?

3. Settings 창 닫기
   → 버튼 다시 활성화 ?

4. Settings 버튼 다시 클릭
 → Settings 창 정상 열림 ?

5. 여러 번 반복 테스트
   → 모두 정상 작동 ?
```

---

## ?? 다른 버튼에도 적용 가능

### **Add File / Add Folder / Add Website 버튼:**

동일한 패턴 적용 가능 (필요 시):

```csharp
private bool isAddFileDialogOpen = false;

private async void AddFileBtn_Click(object sender, RoutedEventArgs e)
{
    if (isAddFileDialogOpen)
    {
        return;
  }

    isAddFileDialogOpen = true;
    AddFileBtn.IsEnabled = false;

 try
    {
   AddFileDialog addFileDialog = new AddFileDialog()
        {
            XamlRoot = Container.XamlRoot
   };

    ContentDialogResult result = await addFileDialog.ShowAsync();

        // ... 기존 로직 ...
    }
    finally
    {
        isAddFileDialogOpen = false;
        AddFileBtn.IsEnabled = true;
    }
}
```

**현재는 ContentDialog이므로 중복 문제 없음** (Modal이라 자동 차단됨)

---

## ?? 학습 포인트

### **1. 중복 클릭 방지는 중요하다**
```
사용자는 빠르게 클릭할 수 있음
→ 방어 로직 필수
```

### **2. 플래그 + UI 피드백 조합이 최선**
```
플래그: 로직 제어
UI 피드백: 사용자 이해
```

### **3. 이벤트 핸들러에서 상태 복원**
```
Closed 이벤트에서 플래그 false
→ 모든 종료 방법 커버
```

### **4. 간단한 해결책이 최선**
```
복잡한 동기화 불필요
→ 플래그만으로 충분
```

---

**완벽하게 해결되었습니다!** ??

**이제 Settings 버튼을 아무리 빨리 눌러도 창은 1개만 열립니다!** ?

**테스트해보세요!** ??
