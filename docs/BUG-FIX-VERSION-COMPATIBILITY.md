# ?? System.Drawing.Common 버전 호환성 문제 해결

## ? 추가 문제 발견

### 증상:
```
? System.Drawing.Common 9.0.10 설치 후에도 앱 실행 안 됨
? 에러: 0xc000027b (STATUS_DLL_NOT_FOUND)
? 버전 충돌 경고 발생
```

---

## ?? 근본 원인

### 문제 1: .NET 버전 불일치
```
프로젝트: .NET 8.0 (net8.0-windows10.0.22621.0)
System.Drawing.Common: 9.0.10 (.NET 9 전용)
→ 호환되지 않음!
```

### 문제 2: 종속성 충돌
```
System.Collections.Immutable:
- .NET 8: Version 8.0.0.0
- System.Drawing.Common 9.0: Version 9.0.0.0
→ 버전 충돌!

System.Reflection.Metadata:
- .NET 8: Version 8.0.0.0
- System.Drawing.Common 9.0: Version 9.0.0.0
→ 버전 충돌!
```

### 빌드 경고 로그:
```
warning MSB3277: 
해결할 수 없는 "System.Collections.Immutable"의 다른 버전 간 충돌이 발견되었습니다.
"System.Collections.Immutable, Version=8.0.0.0"과(와) "Version=9.0.0.0" 사이에 충돌이 발생했습니다.
```

---

## ? 해결 방법

### 1. System.Drawing.Common 다운그레이드

**잘못된 버전 (9.0.10):**
```bash
dotnet add package System.Drawing.Common
# → 최신 버전 9.0.10 설치 (NET 9 전용)
```

**올바른 버전 (8.0.11):**
```bash
# 기존 패키지 제거
dotnet remove package System.Drawing.Common

# .NET 8 호환 버전 설치
dotnet add package System.Drawing.Common --version 8.0.11
```

---

## ?? 최종 패키지 버전

### SLauncher.csproj
```xml
<ItemGroup>
  <!-- WinUI 3 Core -->
  <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
  <PackageReference Include="WinUIEx" Version="2.5.0" />
  
  <!-- UI Components -->
  <PackageReference Include="CommunityToolkit.WinUI.Controls.SettingsControls" Version="8.1.240916" />
  <PackageReference Include="CommunityToolkit.WinUI.Extensions" Version="8.1.240916" />
  
  <!-- System Libraries -->
  <PackageReference Include="System.Drawing.Common" Version="8.0.11" />  ← 8.0.11 필수!
  <PackageReference Include="System.Text.Json" Version="9.0.0" />
  
  <!-- Build Tools -->
  <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.26100.1742" />
</ItemGroup>
```

### 버전 호환성 테이블

| 패키지 | .NET 8 버전 | .NET 9 버전 | SLauncher 사용 |
|--------|------------|------------|---------------|
| System.Drawing.Common | ? **8.0.11** | 9.0.10 | **8.0.11** |
| System.Text.Json | 8.0.x | ? **9.0.0** | 9.0.0 (호환됨) |
| Microsoft.Win32.SystemEvents | ? **8.0.0** | 9.0.0 | 8.0.0 (자동) |

---

## ?? 버전 확인 방법

### 1. 설치된 패키지 확인
```bash
cd "D:\Works\Playground\C#\SLauncher\SLauncher"
dotnet list package
```

**출력 예시:**
```
최상위 패키지
   [net8.0-windows10.0.22621.0]: 
   패키지   요청한 항목    해결됨  
   -----------------------------------------------------------  --------  -------
   CommunityToolkit.WinUI.Controls.SettingsControls          8.1.240916  8.1.240916
   CommunityToolkit.WinUI.Extensions   8.1.240916  8.1.240916
   Microsoft.Windows.SDK.BuildTools              10.0.26100  10.0.26100.1742
   Microsoft.WindowsAppSDK    1.6.241114  1.6.241114003
   System.Drawing.Common           8.0.11      8.0.11  ← 확인!
   System.Text.Json           9.0.09.0.0
   WinUIEx              2.5.0   2.5.0
```

### 2. 종속성 트리 확인
```bash
dotnet list package --include-transitive | findstr "System.Drawing"
```

**출력:**
```
System.Drawing.Common 8.0.11 8.0.11
Microsoft.Win32.SystemEvents           (생략)      8.0.0
```

---

## ?? .NET 버전별 주의사항

### .NET 8 프로젝트
```xml
<TargetFramework>net8.0-windows10.0.22621.0</TargetFramework>
```
**사용 가능 패키지:**
- ? System.Drawing.Common **8.0.x**
- ? System.Drawing.Common 9.0.x (호환 안 됨)

### .NET 9 프로젝트
```xml
<TargetFramework>net9.0-windows10.0.22621.0</TargetFramework>
```
**사용 가능 패키지:**
- ? System.Drawing.Common **9.0.x**
- ? System.Drawing.Common 8.0.x (하위 호환)

---

## ?? 디버깅 팁

### 1. 예외 설정 활성화
```
Visual Studio:
1. Debug → Windows → Exception Settings (Ctrl+Alt+E)
2. "Common Language Runtime Exceptions" 체크
3. 앱 실행 → 정확한 예외 위치 확인
```

### 2. Fusion Log 활성화
```
레지스트리:
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Fusion
- ForceLog = 1 (DWORD)
- LogFailures = 1 (DWORD)
- LogPath = "C:\FusionLogs"
```

### 3. DLL 종속성 확인
```powershell
# Dependencies.exe 다운로드
# https://github.com/lucasg/Dependencies
Dependencies.exe "D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\SLauncher.exe"
```

---

## ?? 재발 방지

### 1. 프로젝트 파일에 명시적 버전 지정
```xml
<ItemGroup>
  <!-- .NET 8 호환 버전만 사용 -->
  <PackageReference Include="System.Drawing.Common" Version="8.0.11" />
  
  <!-- 범위 지정으로 안정적인 버전 유지 -->
  <!-- <PackageReference Include="System.Drawing.Common" Version="[8.0.11,9.0.0)" /> -->
</ItemGroup>
```

### 2. Global.json 사용
```json
{
  "sdk": {
    "version": "8.0.404",
    "rollForward": "latestMinor"
  },
  "msbuild-sdks": {
    "Microsoft.Build.NoTargets": "3.7.0"
  }
}
```

### 3. Directory.Build.props 사용
```xml
<!-- Directory.Build.props -->
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
</Project>

<!-- Directory.Packages.props -->
<Project>
  <ItemGroup>
  <PackageVersion Include="System.Drawing.Common" Version="8.0.11" />
  </ItemGroup>
</Project>
```

---

## ?? 체크리스트

### 설치 전 확인
```
? .NET SDK 버전 확인 (dotnet --version)
? 프로젝트 TargetFramework 확인
? 패키지 버전 호환성 확인
```

### 설치 후 확인
```
? dotnet list package 실행
? 빌드 경고 확인
? 앱 실행 테스트
```

### 문제 발생 시
```
? 패키지 캐시 클리어 (dotnet nuget locals all --clear)
? obj/bin 폴더 삭제
? dotnet restore 재실행
? Visual Studio 재시작
```

---

## ? 최종 확인

### Before:
```
? System.Drawing.Common 9.0.10 (NET 9)
? .NET 8 프로젝트와 호환 안 됨
? 버전 충돌 경고
? 앱 실행 실패
```

### After:
```
? System.Drawing.Common 8.0.11 (NET 8)
? .NET 8 프로젝트와 완벽 호환
? 버전 충돌 없음
? 빌드 성공
? 앱 정상 실행 예상
```

---

## ?? 핵심 교훈

### 1. **패키지 버전 관리의 중요성**
```
최신 버전 ≠ 최선의 선택
프로젝트 .NET 버전과 호환되는 패키지 버전 선택 필수
```

### 2. **명시적 버전 지정**
```bash
# 나쁜 예
dotnet add package System.Drawing.Common

# 좋은 예
dotnet add package System.Drawing.Common --version 8.0.11
```

### 3. **빌드 경고 무시 금지**
```
경고는 잠재적 문제의 신호
특히 버전 충돌 경고는 반드시 해결
```

---

## ?? 다음 단계

### 1. 앱 실행 테스트
```
1. Visual Studio에서 F5 (디버그 실행)
2. 아이콘이 정상적으로 로드되는지 확인
3. 파일/폴더 추가 기능 테스트
```

### 2. Release 빌드 테스트
```bash
dotnet build --configuration Release
cd bin\x64\Release\net8.0-windows10.0.22621.0\win-x64
SLauncher.exe
```

### 3. 배포 준비
```
? System.Drawing.Common 8.0.11 포함 확인
? Microsoft.Win32.SystemEvents 8.0.0 포함 확인
? 모든 종속성 DLL 복사 확인
```

---

## ?? 참고 자료

### Microsoft 문서
- [System.Drawing.Common 버전 호환성](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only)
- [NuGet 패키지 버전 관리](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning)
- [.NET 버전 호환성](https://learn.microsoft.com/en-us/dotnet/standard/frameworks)

### NuGet 패키지
- [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common/)
- [Microsoft.Win32.SystemEvents](https://www.nuget.org/packages/Microsoft.Win32.SystemEvents/)

---

**이제 System.Drawing.Common 8.0.11이 설치되어 .NET 8과 완벽하게 호환됩니다!** ?

**앱을 실행해보시고, 문제가 있다면 디버그 출력 로그나 예외 메시지를 공유해주세요!** ??
