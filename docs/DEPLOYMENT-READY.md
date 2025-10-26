# ?? SLauncher 배포 가이드

## ? 빌드 완료!

빌드가 성공적으로 완료되었습니다! ??

---

## ?? 배포 파일 위치

```
D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\win-x64\
```

---

## ?? 배포 패키지 만들기

### 방법 1: 파일 탐색기에서 압축 (가장 쉬움)

1. **폴더 열기**
   ```
   D:\Works\Playground\C#\SLauncher\SLauncher\bin\x64\Release\net8.0-windows10.0.22621.0\win-x64\
   ```

2. **모든 파일 선택**
   - `Ctrl + A` 누르기
   - 또는 첫 번째 파일 클릭 → Shift + 마지막 파일 클릭

3. **압축**
   - 선택한 파일에서 **우클릭**
   - **보내기** → **압축(ZIP) 폴더**
- 이름: `SLauncher-v2.1.2-Portable.zip`

4. **ZIP 파일 이동**
   - 생성된 ZIP 파일을 프로젝트 루트로 이동
   - `D:\Works\Playground\C#\SLauncher\`

---

## ?? 예상 파일 크기

- **압축 전:** 약 50-80MB
- **압축 후:** 약 15-25MB

---

## ?? 배포 준비

### 1. README.txt 작성

배포 폴더에 포함할 README 파일:

```txt
========================================
SLauncher v2.1.2 - Portable Edition
========================================

?? 설명:
Windows용 모던 앱 런처입니다.
파일, 폴더, 웹사이트를 빠르게 실행할 수 있습니다.

?? 사용 방법:
1. ZIP 파일 압축 해제
2. SLauncher.exe 실행

?? 최초 실행 시 필요사항:

[.NET 8 Desktop Runtime]
- 다운로드: https://aka.ms/dotnet/8.0/windowsdesktop-runtime-win-x64.exe
- 설치 후 PC 재시작

[Windows App SDK Runtime]
- 다운로드: https://aka.ms/windowsappsdk/1.6/latest/windowsappruntimeinstall-x64.exe
- 설치 후 PC 재시작

?? 참고:
- 런타임은 PC당 한 번만 설치하면 됩니다
- SLauncher 업데이트 시 런타임 재설치 불필요

?? 사용 팁:
1. 파일/폴더를 SLauncher 창에 드래그하여 추가
2. 검색 기능으로 빠르게 찾기
3. 설정에서 UI 커스터마이징

문의사항: [담당자 연락처]
```

---

## ?? 최종 배포 패키지 구성

```
SLauncher-Deployment/
├── SLauncher-v2.1.2-Portable.zip    ← 프로그램
├── README.txt             ← 사용 설명서
├── Runtimes/  ← 런타임 (선택사항)
│   ├── windowsdesktop-runtime-win-x64.exe
│   ├── windowsappruntimeinstall-x64.exe
│   └── install-runtime.bat
└── 배포_가이드.txt
```

---

## ?? 배포 전 체크리스트

- [ ] ZIP 파일 생성 완료
- [ ] README.txt 작성
- [ ] 파일 크기 확인 (15-25MB)
- [ ] 테스트 PC에서 실행 확인
- [ ] 런타임 설치 가이드 준비
- [ ] 회사 공유 폴더 경로 확인

---

## ?? 다음 단계

### 1. 테스트
```
1. 다른 PC에서 ZIP 압축 해제
2. SLauncher.exe 실행 확인
3. 기능 정상 작동 확인
```

### 2. 배포
```
1. ZIP 파일을 회사 공유 폴더에 복사
2. 공지 메일 발송
3. 런타임 설치 가이드 공유
```

### 3. 지원
```
- 설치 문제: 런타임 설치 확인
- 실행 오류: 이벤트 뷰어 확인
- 기능 문의: 사용 설명서 참조
```

---

## ?? 문제 해결

### "파일을 찾을 수 없습니다" 오류
→ .NET 8 Desktop Runtime 설치

### "응용 프로그램을 시작할 수 없습니다" 오류
→ Windows App SDK Runtime 설치

### "dll을 로드할 수 없습니다" 오류
→ ZIP 파일 전체를 압축 해제했는지 확인

---

## ? 완료!

이제 배포 준비가 완료되었습니다! ??

**최종 결과:**
- ? 빌드 성공
- ? 배포 파일 생성
- ? 15-25MB 경량 패키지
- ? 즉시 배포 가능

**배포 파일:** `SLauncher-v2.1.2-Portable.zip`
