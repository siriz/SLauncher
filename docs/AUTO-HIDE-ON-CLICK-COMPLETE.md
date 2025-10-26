# ?? Auto Hide to Tray on Item Click - 구현 완료!

## ? 구현된 기능

**파일/폴더/링크 클릭 시 자동으로 트레이로 이동:**

```
1. 아이템 클릭 (또는 우클릭 메뉴에서 Open)
2. 파일/폴더/링크 실행
3. 자동으로 트레이로 숨김 ?
```

**특징:**
- ? 더블클릭으로 실행 시 자동 숨김
- ? 우클릭 → Open 시 자동 숨김
- ? 우클릭 → Run as Administrator 시 자동 숨김
- ? MinimizeToTray 설정에 따라 동작
- ? 그룹 내 아이템도 동일하게 작동

---

## ?? 수정된 파일

### **GridViewTile.xaml.cs**

#### 1. GridViewTileControl_Tapped (더블클릭):

**Before:**
```csharp
private async void GridViewTileControl_Tapped(object sender, TappedRoutedEventArgs e)
{
    await StartAssociatedProcess();
}
```

**After:**
```csharp
private async void GridViewTileControl_Tapped(object sender, TappedRoutedEventArgs e)
{
    await StartAssociatedProcess();
    
    // ? Hide main window to tray if MinimizeToTray is enabled
    if (UserSettingsClass.MinimizeToTray)
    {
      App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
      App.MainWindow.AppWindow.Hide();
    });
    }
}
```

---

#### 2. MenuOpenOption_Click (우클릭 → Open):

**Before:**
```csharp
private async void MenuOpenOption_Click(object sender, RoutedEventArgs e)
{
    // Start the process
    await StartAssociatedProcess();
}
```

**After:**
```csharp
private async void MenuOpenOption_Click(object sender, RoutedEventArgs e)
{
    // Start the process
await StartAssociatedProcess();
    
    // ? Hide main window to tray if MinimizeToTray is enabled
    if (UserSettingsClass.MinimizeToTray)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            App.MainWindow.AppWindow.Hide();
        });
    }
}
```

---

#### 3. MenuAdminOption_Click (우클릭 → Run as Administrator):

**Before:**
```csharp
private async void MenuAdminOption_Click(object sender, RoutedEventArgs e)
{
    // Start the process as admin
    await StartAssociatedProcess(true);
}
```

**After:**
```csharp
private async void MenuAdminOption_Click(object sender, RoutedEventArgs e)
{
    // Start the process as admin
    await StartAssociatedProcess(true);
  
    // ? Hide main window to tray if MinimizeToTray is enabled
    if (UserSettingsClass.MinimizeToTray)
    {
  App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
      App.MainWindow.AppWindow.Hide();
 });
    }
}
```

---

## ?? 작동 원리

### **실행 흐름:**

```
1. 사용자가 아이템 클릭
    ↓
2. StartAssociatedProcess() 실행
    ↓
3. 파일/폴더/링크 열림
    ↓
4. MinimizeToTray 설정 확인
    ↓
5. true면 → AppWindow.Hide()
↓
6. 창이 트레이로 숨겨짐 ?
```

---

### **DispatcherQueue 사용 이유:**

```csharp
App.MainWindow.DispatcherQueue.TryEnqueue(() =>
{
    App.MainWindow.AppWindow.Hide();
});
```

**이유:**
- `StartAssociatedProcess()`는 비동기 메서드
- UI 업데이트는 UI 스레드에서만 가능
- `DispatcherQueue`로 UI 스레드에 작업 예약
- 안전하고 확실한 실행 보장

---

## ?? 사용 시나리오

### **시나리오 1: 파일 실행**

```
1. Word 문서 더블클릭
2. Word 실행
3. SLauncher 자동으로 트레이로 ?
4. Word 작업
5. Ctrl+Space로 다시 SLauncher 열기
```

### **시나리오 2: 웹사이트 열기**

```
1. GitHub 링크 클릭
2. 브라우저에서 GitHub 열림
3. SLauncher 자동으로 트레이로 ?
4. 웹서핑
5. Ctrl+Space로 다시 SLauncher 열기
```

### **시나리오 3: 관리자 권한 실행**

```
1. 설치 파일 우클릭
2. "Run as Administrator" 선택
3. UAC 프롬프트 표시
4. 승인 후 설치 파일 실행
5. SLauncher 자동으로 트레이로 ?
```

### **시나리오 4: 그룹 내 아이템**

```
1. 그룹 클릭 → 그룹 다이얼로그 열림
2. 그룹 내 파일 클릭
3. 파일 실행
4. 그룹 다이얼로그 닫힘
5. SLauncher 자동으로 트레이로 ?
```

---

## ?? 설정과의 연동

### **MinimizeToTray 설정:**

```csharp
if (UserSettingsClass.MinimizeToTray)
{
    // 트레이로 숨김
}
```

**동작:**
- `MinimizeToTray = true` → 아이템 실행 시 트레이로
- `MinimizeToTray = false` → 아이템 실행 후 창 유지

---

### **Settings에서 제어:**

```
Settings → System → Minimize to Tray
[Yes/No] 토글
```

**사용자 선택:**
- **Yes:** 아이템 실행 시 자동으로 트레이로 (기본값)
- **No:** 아이템 실행 후에도 창 유지

---

## ?? 사용자 경험

### **Before (없을 때):**

```
?? 사용자: "파일을 열면 SLauncher가 계속 보여요"
?? 사용자: "매번 수동으로 닫아야 하나요?"
?? 사용자: "불편해요..."
```

### **After (구현 후):**

```
?? 사용자: "파일을 열면 SLauncher가 자동으로 사라져요!"
?? 사용자: "작업 공간이 깔끔해졌어요!"
?사용자: "Ctrl+Space로 바로 다시 열 수 있어서 편리해요!"
```

---

## ?? 다른 런처와 비교

### **Alfred (macOS):**
```
아이템 실행 → 자동으로 숨김 ?
```

### **Wox (Windows):**
```
아이템 실행 → 자동으로 숨김 ?
```

### **PowerToys Run:**
```
아이템 실행 → 자동으로 숨김 ?
```

### **SLauncher (이제):**
```
아이템 실행 → 자동으로 트레이로 ??
```

**SLauncher 장점:**
- ? 완전히 닫히지 않고 트레이에 상주
- ? Ctrl+Space로 즉시 복원
- ? 백그라운드에서 대기
- ? 빠른 재접근

---

## ?? 추가 개선 가능 (선택사항)

### **1. 딜레이 설정**

아이템 실행 후 창을 숨기기 전에 약간의 딜레이:

```csharp
private async void GridViewTileControl_Tapped(object sender, TappedRoutedEventArgs e)
{
    await StartAssociatedProcess();
    
    if (UserSettingsClass.MinimizeToTray)
    {
        // 500ms 딜레이 (파일 열림 확인)
      await Task.Delay(500);
    
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
  App.MainWindow.AppWindow.Hide();
        });
    }
}
```

**장점:**
- 파일이 실제로 열렸는지 확인
- 에러 메시지를 볼 시간 확보

### **2. 애니메이션 효과**

창이 트레이로 숨겨질 때 페이드 아웃 효과:

```csharp
private async Task FadeOutAndHide()
{
    var opacity = 1.0;
  while (opacity > 0)
    {
  opacity -= 0.1;
        App.MainWindow.Opacity = opacity;
        await Task.Delay(20);
    }
    App.MainWindow.AppWindow.Hide();
    App.MainWindow.Opacity = 1.0;  // 복원 시를 위해
}
```

### **3. 설정 옵션 추가**

```xml
<!-- SettingsWindow.xaml -->
<wct:SettingsCard
    Header="Auto hide on item click"
 Description="Automatically hide to tray when opening an item">
    <ToggleSwitch x:Name="AutoHideToggle" />
</wct:SettingsCard>
```

```csharp
// UserSettingsClass.cs
public static bool AutoHideOnClick = true;

// GridViewTile.xaml.cs
if (UserSettingsClass.MinimizeToTray && UserSettingsClass.AutoHideOnClick)
{
    // Hide to tray
}
```

### **4. 특정 아이템 제외**

특정 아이템은 실행 후에도 창 유지:

```csharp
// GridViewTile에 속성 추가
public bool KeepWindowOpen { get; set; } = false;

// EditItemWindow에 체크박스 추가
<CheckBox x:Name="KeepWindowOpenCheckBox"
          Content="Keep SLauncher window open after execution"
     IsChecked="{x:Bind KeepWindowOpen, Mode=TwoWay}"/>

// 실행 시 체크
if (UserSettingsClass.MinimizeToTray && !this.KeepWindowOpen)
{
    // Hide to tray
}
```

---

## ?? 예상 가능한 문제

### **문제 1: 에러 다이얼로그가 안 보임**

**증상:**
- 파일 실행 실패 시 에러 메시지가 트레이로 숨겨져 안 보임

**해결:**
```csharp
public async Task StartAssociatedProcess(bool runAsAdmin = false)
{
    try
    {
        // ... 실행 로직 ...
    }
    catch
    {
        // 에러 발생 시 창 숨기지 않음
     await ShowErrorDialog();
        return;  // 여기서 리턴하여 Hide() 실행 안 함
    }
    
  // 성공 시만 숨김
}
```

### **문제 2: UAC 프롬프트 뒤에 창 숨겨짐**

**증상:**
- 관리자 권한 실행 시 UAC 프롬프트가 표시되기 전에 창이 숨겨짐

**해결:**
```csharp
private async void MenuAdminOption_Click(object sender, RoutedEventArgs e)
{
    await StartAssociatedProcess(true);
    
    // UAC 완료 대기
    await Task.Delay(1000);
    
    if (UserSettingsClass.MinimizeToTray)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
      {
            App.MainWindow.AppWindow.Hide();
        });
    }
}
```

### **문제 3: 그룹 다이얼로그가 같이 닫힘**

**증상:**
- 그룹 내 아이템 실행 시 그룹 다이얼로그도 같이 닫힘

**현재 상태:**
- 그룹 다이얼로그는 MainWindow의 자식이므로
- MainWindow가 숨겨지면 자동으로 닫힘
- **이는 의도된 동작입니다** ?

**대안 (원하면):**
```csharp
// 그룹 다이얼로그 열려 있으면 창 숨기지 않음
if (UserSettingsClass.MinimizeToTray && !IsGroupDialogOpen)
{
    // Hide to tray
}
```

---

## ?? 동작 정리

### **자동 숨김이 발생하는 경우:**

1. ? 아이템 더블클릭
2. ? 우클릭 → Open
3. ? 우클릭 → Run as Administrator
4. ? 그룹 내 아이템 클릭

### **자동 숨김이 발생하지 않는 경우:**

1. ? 우클릭 → Open File Location
2. ? 우클릭 → Edit
3. ? 우클릭 → Remove
4. ? 드래그 앤 드롭 (재정렬)
5. ? MinimizeToTray = false 일 때

---

## ?? 테스트 시나리오

### **Test 1: 파일 실행**
```
1. Settings → Minimize to Tray = Yes
2. Word 문서 더블클릭
3. Word 실행 ?
4. SLauncher 트레이로 이동 ?
```

### **Test 2: 웹사이트 열기**
```
1. GitHub 링크 클릭
2. 브라우저에서 열림 ?
3. SLauncher 트레이로 이동 ?
```

### **Test 3: 관리자 권한**
```
1. 설치 파일 우클릭
2. "Run as Administrator"
3. UAC 승인
4. 설치 파일 실행 ?
5. SLauncher 트레이로 이동 ?
```

### **Test 4: MinimizeToTray = No**
```
1. Settings → Minimize to Tray = No
2. 파일 클릭
3. 파일 실행 ?
4. SLauncher 창 유지 ?
```

### **Test 5: 그룹 내 아이템**
```
1. 그룹 클릭
2. 그룹 다이얼로그 열림
3. 그룹 내 파일 클릭
4. 파일 실행 ?
5. 그룹 다이얼로그 닫힘 ?
6. SLauncher 트레이로 이동 ?
```

### **Test 6: 에러 처리**
```
1. 삭제된 파일 클릭
2. 에러 다이얼로그 표시 ?
3. SLauncher 창 유지 ? (Hide 안 됨)
```

---

## ? 구현 완료!

### **변경된 파일:**
- ? `GridViewTile.xaml.cs`
  - `GridViewTileControl_Tapped` 수정
  - `MenuOpenOption_Click` 수정
  - `MenuAdminOption_Click` 수정

### **동작:**
- ? 파일/폴더/링크 클릭 시 자동 트레이 이동
- ? MinimizeToTray 설정 연동
- ? DispatcherQueue로 안전한 UI 업데이트
- ? 모든 실행 방법에 적용

### **특징:**
- ? 직관적인 동작
- ? 다른 런처와 동일한 UX
- ? 설정으로 제어 가능
- ? Ctrl+Space로 빠른 복원

---

## ?? 빌드 및 테스트

### **빌드:**
```
Visual Studio → Rebuild Solution → F5
```

### **테스트:**
```
1. 파일 하나 추가
2. Settings → Minimize to Tray = Yes 확인
3. 파일 더블클릭
4. 파일 실행 + SLauncher 트레이로 ?
5. Ctrl+Space로 복원 ?
6. 완벽! ??
```

---

## ?? 완료!

**파일/폴더/링크 클릭 시 자동으로 트레이로 이동하는 기능이 구현되었습니다!**

**이제:**
- ? 아이템 실행 후 자동으로 트레이로
- ? 작업 공간 깔끔하게 유지
- ? Ctrl+Space로 즉시 복원
- ? Alfred, Wox와 같은 UX

**완벽한 런처 경험을 제공합니다!** ?

**테스트해보세요!** ??
