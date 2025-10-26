# ?? Visual Studio 빌드 가이드

## ?? 빌드 방법

### 1단계: Visual Studio 열기
```
LauncherX-master.sln 더블클릭
```

### 2단계: 빌드 구성 설정
1. 상단 툴바에서:
   - **구성**: `Release` 선택
   - **플랫폼**: `x64` 선택

   ```
   Debug ▼  →  Release
   Any CPU ▼  →  x64
   ```

### 3단계: 빌드 실행
**방법 A: 메뉴**
```
빌드 > 솔루션 빌드 (Ctrl+Shift+B)
```

**방법 B: 솔루션 탐색기**
```
솔루션 우클릭 > 빌드
```

### 4단계: 빌드 완료 확인
출력 창에 다음 메시지가 표시되어야 합니다:
```
========== 빌드: 성공 2, 실패 0, 최신 0, 건너뛰기 0 ==========
```

---

## ?? 배포 파일 위치

빌드 완료 후:
```
D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\
```

### 파일 구조:
```
net8.0-windows10.0.22621.0/
├── SLauncher.exe      ← 실행 파일
├── SLauncher.dll
├── Resources/              ← 리소스 폴더
│   ├── icon.ico
│   ├── icon.png
│   ├── LinkedFolderIcon.png
│   └── websitePlaceholder.png
├── Microsoft.*.dll         ← Windows App SDK
└── (기타 DLL 파일들)
```

**예상 크기:** 10-30MB

---

## ?? 배포 준비

### 1. 폴더 압축
```
1. net8.0-windows10.0.22621.0 폴더 선택
2. 우클릭 > 보내기 > 압축(ZIP) 폴더
3. 이름 변경: SLauncher-v2.1.2-Portable.zip
```

### 2. 배포
```
1. ZIP 파일을 회사 공유 폴더에 복사
2. 사용자들에게 알림
```

---

## ?? 대상 PC 요구사항

사용자 PC에 다음 런타임이 **한 번만** 설치되어야 합니다:

### .NET 8 Desktop Runtime
**다운로드:**
- https://dotnet.microsoft.com/download/dotnet/8.0
- 직접 링크: https://aka.ms/dotnet/8.0/windowsdesktop-runtime-win-x64.exe

**설치 명령 (자동 설치용):**
```cmd
windowsdesktop-runtime-win-x64.exe /install /quiet /norestart
```

### Windows App SDK Runtime
**다운로드:**
- https://aka.ms/windowsappsdk/1.6/latest/windowsappruntimeinstall-x64.exe

**설치 명령:**
```cmd
windowsappruntimeinstall-x64.exe
```

---

## ?? 배포 체크리스트

배포 전 확인사항:

- [ ] Release 모드로 빌드
- [ ] x64 플랫폼으로 빌드
- [ ] 빌드 성공 확인
- [ ] 배포 폴더 확인 (net8.0-windows10.0.22621.0)
- [ ] SLauncher.exe 실행 테스트
- [ ] Resources 폴더 포함 확인
- [ ] ZIP 파일 생성
- [ ] 파일 크기 확인 (10-30MB)
- [ ] 런타임 설치 가이드 준비

---

## ?? 문제 해결

### "빌드 실패" 오류
**해결책:**
```
1. 출력 창 확인 (보기 > 출력)
2. 오류 메시지 확인
3. 솔루션 정리: 빌드 > 솔루션 정리
4. 다시 빌드
```

### ".NET 8 SDK not found" 오류
**해결책:**
```
1. Visual Studio Installer 실행
2. "수정" 클릭
3. ".NET 데스크톱 개발" 워크로드 확인
4. ".NET 8.0 Runtime" 확인
5. 수정 적용
```

### "Windows App SDK" 관련 오류
**해결책:**
```
1. NuGet 패키지 복원:
   솔루션 우클릭 > NuGet 패키지 복원
2. 다시 빌드
```

---

## ?? 배포 팁

### 런타임 일괄 설치 스크립트
사용자들을 위해 런타임 설치 스크립트를 함께 배포:

**install-runtime.bat:**
```batch
@echo off
echo SLauncher 런타임 설치 중...
echo.

echo [1/2] .NET 8 Desktop Runtime 설치...
start /wait windowsdesktop-runtime-win-x64.exe /install /quiet /norestart

echo [2/2] Windows App SDK Runtime 설치...
start /wait windowsappruntimeinstall-x64.exe

echo.
echo ? 설치 완료!
echo SLauncher.exe를 실행하세요.
pause
```

### 배포 패키지 구성
```
SLauncher-Deployment/
├── SLauncher-v2.1.2-Portable.zip    ← 프로그램
├── Runtimes/    ← 런타임 (선택)
│   ├── windowsdesktop-runtime-win-x64.exe
│   ├── windowsappruntimeinstall-x64.exe
│   └── install-runtime.bat
└── README.txt            ← 사용 설명서
```

---

## ?? 완료!

빌드가 성공하면:
1. ? 배포 가능한 실행 파일 생성
2. ? 크기: 10-30MB (Framework-Dependent)
3. ? 즉시 배포 가능

**다음 단계:** ZIP 파일을 만들어 배포하세요! ??
