# ?? UserCache 복사 및 공유 가이드

## ? UserCache 폴더는 100% 포터블입니다!

UserCache 폴더를 다른 사용자/PC에 복사하면 **모든 설정과 데이터가 그대로** 작동합니다.

---

## ?? UserCache 구조

```
SLauncher\
└── UserCache\
    ├── userSettings.json   ← 사용자 설정
    ├── Files\  ← 추가한 아이템
    │   ├── 0.json       (파일 정보)
    │ ├── 1.json      (폴더 정보)
    │   ├── 2.json    (웹사이트 정보)
  │   └── 3\  (그룹)
    │  ├── props.json
│       └── 0.json
  └── FaviconCache\    ← 웹사이트 아이콘
        ├── github.com.png
        ├── google.com.png
      └── intranet.company.com.png
```

---

## ?? 복사 시나리오

### 1?? 개인 백업/복원

**백업:**
```
C:\Tools\SLauncher\UserCache\
↓ 복사
D:\Backup\SLauncher-UserCache-2025-01-25\
```

**복원:**
```
D:\Backup\SLauncher-UserCache-2025-01-25\
↓ 복사
C:\Tools\SLauncher\UserCache\
```

**결과:** ? 모든 설정과 아이템 복원됨

---

### 2?? 여러 PC에서 동일하게 사용

```
데스크톱: C:\SLauncher\UserCache\
↓ 복사
노트북: D:\Apps\SLauncher\UserCache\
↓ 복사
USB 드라이브: E:\Portable\SLauncher\UserCache\
```

**결과:** ? 모든 PC에서 동일한 환경

---

### 3?? 팀원들에게 표준 설정 배포

**준비 (관리자):**
```
1. SLauncher 설치 및 설정
2. 회사 웹사이트 추가:
 - 인트라넷
   - 업무 포털
   - 공용 폴더
3. 설정 조정:
   - 헤더 텍스트: "회사 업무 도구"
   - 아이콘 크기: 1.2
4. UserCache 폴더를 ZIP으로 압축
```

**배포 (팀원들):**
```
1. SLauncher-v2.1.2-Portable.zip 압축 해제
2. UserCache-Template.zip 압축 해제하여 UserCache 폴더에 복사
3. SLauncher.exe 실행
4. 즉시 사용 가능! ?
```

---

## ?? 포함되는 내용

### ? userSettings.json
```json
{
  "headerText": "My files, folders, and websites",
  "gridScale": 1.2,
  "useFullscreen": false,
  "gridPosition": "Center"
}
```
**복사 후:** 동일한 UI 설정

---

### ? Files\ 폴더
```
Files\
├── 0.json    → 파일: C:\Work\document.docx
├── 1.json    → 폴더: \\Server\Share\Projects
├── 2.json    → 웹사이트: https://intranet.company.com
└── 3\    → 그룹: "즐겨찾기"
    ├── props.json
├── 0.json
    └── 1.json
```
**복사 후:** 동일한 아이템 목록

---

### ? FaviconCache\ 폴더
```
FaviconCache\
├── github.com.png
├── google.com.png
└── intranet.company.com.png
```
**복사 후:** 아이콘 즉시 표시 (다운로드 불필요)

---

## ?? 주의사항

### 1. 파일 경로 문제

**개인 폴더 경로:**
```json
// ? 문제 발생 가능
{
  "executingPath": "C:\\Users\\UserA\\Documents\\work.docx"
}
```
→ User B에게 복사하면 경로 없음

**권장 해결책:**
```json
// ? 네트워크 경로 사용
{
  "executingPath": "\\\\Server\\Share\\work.docx"
}

// ? 공용 폴더 사용
{
"executingPath": "C:\\Public\\Documents\\work.docx"
}
```

---

### 2. 권한 문제

**시나리오:**
```
User A: 관리자 권한으로 추가한 파일
User B: 일반 사용자 → 접근 불가
```

**해결책:**
- 공용 폴더 사용
- 네트워크 공유 사용
- 권한 확인

---

### 3. 드라이브 문자 차이

**문제:**
```
User A: D:\Projects\...
User B: D: 드라이브 없음
```

**해결책:**
- UNC 경로 사용: `\\Server\Share\...`
- 상대 경로 사용 (가능한 경우)

---

## ?? 배포 전략

### Strategy 1: 최소 배포 (권장)
```
배포 패키지:
└── SLauncher-v2.1.2-Portable.zip

사용자:
- 자신의 파일/폴더 직접 추가
- 개인화된 환경
```

---

### Strategy 2: 표준 설정 배포
```
배포 패키지:
├── SLauncher-v2.1.2-Portable.zip
└── UserCache-Standard\
    ├── userSettings.json        (표준 설정)
 └── Files\
        ├── 0.json              (회사 인트라넷)
     ├── 1.json   (업무 포털)
        └── 2.json     (공용 폴더)

사용자:
1. SLauncher 압축 해제
2. UserCache-Standard 복사 (선택사항)
3. 추가 아이템 개인화
```

---

### Strategy 3: 완전 표준화 배포
```
배포 패키지:
└── SLauncher-Preconfigured.zip
    ├── SLauncher.exe
├── (모든 실행 파일)
    └── UserCache\   (미리 설정됨)
        ├── userSettings.json
     ├── Files\
        └── FaviconCache\

사용자:
1. 압축 해제
2. 실행
3. 끝!
```

---

## ?? 복사 체크리스트

### 개인 백업 시:
- [ ] UserCache 폴더 전체 복사
- [ ] 백업 날짜 기록
- [ ] 주기적 백업 (선택)

### 팀 배포 시:
- [ ] 개인 경로 없는지 확인
- [ ] 네트워크 경로 사용 확인
- [ ] 권한 문제 없는지 테스트
- [ ] 다른 PC에서 테스트
- [ ] README 작성

### 여러 PC 동기화 시:
- [ ] 최신 버전 확인
- [ ] 충돌 없는지 확인
- [ ] 주기적 동기화 계획

---

## ?? 실전 예시

### 예시 1: USB 드라이브로 이동
```
1. 데스크톱에서 SLauncher 설치 및 설정
2. UserCache 폴더를 USB에 복사
3. 노트북에서 SLauncher 설치
4. USB의 UserCache를 노트북에 복사
5. 노트북에서 SLauncher 실행 → 동일한 환경!
```

---

### 예시 2: 회사 표준 환경 배포
```bash
# 관리자 작업
1. 표준 SLauncher 설정:
   - 회사 인트라넷
   - 업무 시스템 링크
- 공용 폴더
   - 표준 설정 (아이콘 크기 등)

2. UserCache 폴더를 공유 폴더에 배치:
   \\Server\Software\SLauncher\UserCache-Standard\

# 직원 작업
1. SLauncher 설치
2. 공유 폴더의 UserCache-Standard를 복사
3. 필요에 따라 개인 아이템 추가
```

---

### 예시 3: 팀 프로젝트 공유
```
프로젝트 팀:
- 공통 프로젝트 폴더
- 공통 문서 링크
- 공통 웹사이트

UserCache 공유:
1. 팀장이 표준 UserCache 생성
2. 팀 공유 폴더에 배치
3. 팀원들이 복사하여 사용
4. 개인 아이템 추가 가능
```

---

## ? 결론

### UserCache는 완전히 포터블합니다!

**복사 가능:**
- ? 다른 사용자에게
- ? 다른 PC에
- ? USB 드라이브에
- ? 네트워크 드라이브에

**작동 보장:**
- ? 설정 동일
- ? 아이템 동일
- ? 아이콘 캐시 동일

**주의사항:**
- ?? 파일 경로 확인 (개인 vs 공용)
- ?? 권한 확인
- ?? 드라이브 문자 차이 고려

---

## ?? Best Practice

```
1. 개인 사용:
   → UserCache를 정기적으로 백업

2. 팀 사용:
   → 표준 UserCache 템플릿 제공
   → 개인화 가능하도록 안내

3. 회사 배포:
   → 네트워크 경로 사용
   → 공용 리소스만 포함
   → 개인 추가는 자유롭게
```

**SLauncher의 포터블 설계 덕분에 UserCache만 있으면 어디서든 동일한 환경을 사용할 수 있습니다!** ??
