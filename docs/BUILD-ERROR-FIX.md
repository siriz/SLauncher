# SLauncher 빌드 오류 해결 방법

## 문제 원인
.NET 9 SDK와 Windows App SDK 1.6 간의 호환성 문제로 `MrtCore.PriGen.targets` 오류가 발생합니다.

## ? 해결 방법

### 방법 1: Visual Studio에서 빌드 (권장 ?)

1. Visual Studio 2022에서 솔루션 열기
2. 상단 메뉴: **빌드 > 구성 관리자**
3. 설정:
   - **구성**: Release
   - **플랫폼**: x64
4. **빌드 > 솔루션 빌드** (Ctrl+Shift+B)
5. 배포 폴더: `SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\`

### 방법 2: 배치 파일 사용

```cmd
publish-vs.bat
```

이 스크립트는 Visual Studio의 MSBuild를 자동으로 찾아 빌드합니다.

### 방법 3: .NET 8 SDK 사용 (고급)

프로젝트 루트에 `global.json` 파일 생성:

```json
{
  "sdk": {
    "version": "8.0.100",
    "rollForward": "latestFeature"
  }
}
```

그 후 `dotnet build` 실행.

## ?? 빌드 후 배포

빌드가 완료되면:

1. **배포 폴더 위치:**
   ```
 SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\
   ```

2. **폴더 전체를 ZIP으로 압축**
   - 폴더 이름 예시: `SLauncher-v2.1.2-Portable.zip`

3. **배포**
   - ZIP 파일을 공유 드라이브에 복사
   - 사용자들이 압축 해제 후 `SLauncher.exe` 실행

## ?? 대상 PC 요구사항

사용자 PC에 다음이 설치되어 있어야 합니다:

### .NET 8 Desktop Runtime
- 다운로드: https://dotnet.microsoft.com/download/dotnet/8.0
- 직접 링크: https://aka.ms/dotnet/8.0/windowsdesktop-runtime-win-x64.exe

### Windows App SDK Runtime
- 다운로드: https://aka.ms/windowsappsdk/1.6/latest/windowsappruntimeinstall-x64.exe

## ?? 문제 해결

### "시스템에서 지정한 파일을 찾을 수 없습니다" 오류
→ .NET 8 Desktop Runtime 설치

### "앱을 시작할 수 없습니다" 오류
→ Windows App SDK Runtime 설치

### 실행 파일이 바이러스로 감지됨
→ 바이러스 백신 예외 설정에 추가

## ?? 팁

### 일괄 설치 스크립트 제공
사용자들을 위한 런타임 설치 스크립트를 함께 배포하면 편리합니다:

```batch
@echo off
echo .NET 8 Desktop Runtime 설치 중...
start /wait windowsdesktop-runtime-win-x64.exe /install /quiet /norestart

echo Windows App SDK Runtime 설치 중...
start /wait windowsappruntimeinstall-x64.exe

echo 설치 완료! SLauncher를 실행하세요.
pause
```

### 배포 패키지 구성
```
SLauncher-Portable/
├── SLauncher.exe
├── (기타 DLL 파일들)
├── Resources/
├── README.txt (사용 방법)
└── Runtimes/
    ├── install-runtime.bat
    ├── windowsdesktop-runtime-win-x64.exe
    └── windowsappruntimeinstall-x64.exe
```
