# SLauncher 런타임 설치 가이드

## 회사 PC에 런타임 설치 (관리자 권한 필요)

포터블 버전(경량)을 사용하려면 각 PC에 런타임을 한 번만 설치하면 됩니다.

### 1. .NET 8 Desktop Runtime 설치

**다운로드:**
- 직접 링크: https://dotnet.microsoft.com/download/dotnet/8.0
- 또는 직접 다운로드: https://aka.ms/dotnet/8.0/windowsdesktop-runtime-win-x64.exe

**설치 방법:**
```cmd
windowsdesktop-runtime-win-x64.exe /install /quiet /norestart
```

### 2. Windows App SDK Runtime 설치

**다운로드:**
- 직접 다운로드: https://aka.ms/windowsappsdk/1.6/latest/windowsappruntimeinstall-x64.exe

**설치 방법:**
```cmd
windowsappruntimeinstall-x64.exe
```

---

## 배포 옵션 비교

### 옵션 1: 포터블 버전 (권장)
- **파일:** `publish-portable.bat` 실행
- **용량:** 약 10-30MB
- **장점:** 매우 가벼움, 업데이트 빠름
- **단점:** 런타임 사전 설치 필요
- **용도:** 회사 내부 배포 권장

### 옵션 2: Self-Contained 버전
- **파일:** `publish-selfcontained.bat` 실행
- **용량:** 약 100-150MB
- **장점:** 설치 없이 바로 실행
- **단점:** 용량이 큼
- **용도:** 외부 배포 또는 임시 사용

---

## 일괄 설치 스크립트 (관리자용)

회사 PC에 런타임을 일괄 설치하려면:

```batch
@echo off
echo .NET 8 Desktop Runtime 설치 중...
windowsdesktop-runtime-win-x64.exe /install /quiet /norestart

echo Windows App SDK Runtime 설치 중...
windowsappruntimeinstall-x64.exe

echo 설치 완료!
pause
```

---

## 배포 시 체크리스트

- [ ] 대상 PC: Windows 11 64비트
- [ ] 런타임 설치 완료 (포터블 버전 사용 시)
- [ ] publish 폴더 전체 복사
- [ ] 실행 권한 확인
- [ ] 방화벽 예외 설정 (필요 시)

---

## 문제 해결

### "런타임을 찾을 수 없습니다" 오류
→ .NET 8 Desktop Runtime 설치

### "Windows App SDK를 찾을 수 없습니다" 오류
→ Windows App SDK Runtime 설치

### 실행이 안 됨
→ Windows 11 64비트 확인
→ 바이러스 백신 예외 설정 확인
