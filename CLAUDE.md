# CLAUDE.md

이 저장소는 [`DevelopPrompt`](https://github.com/noble-shake/DevelopPrompt.git)의 개인 규약을 참조한다.
규칙 전문을 이 파일에 복제하지 않는다 — 어디를 읽어야 하는지만 둔다.

> 이 파일은 **템플릿에서 복제된 상태**다. 새 프로젝트로 쓰기 시작했다면 아래 `{}` 자리를
> 실제 값으로 채운다. 채우기 전까지 에이전트는 추측으로 진행하지 않는다.

## 0. 원본 연결

1. `{DevelopPrompt 원본 경로}`가 이 머신에 있는지 확인한다.
   - 있으면 그 체크아웃을 그대로 원본으로 쓴다.
   - 없으면 `git clone https://github.com/noble-shake/DevelopPrompt.git {DevelopPrompt 원본 경로}`로
     클론한다. 클론 위치를 정하지 못했으면 임의 폴더에 만들지 않고 사용자에게 확인한다.
2. 이번 요청이 이 프로젝트의 실제 작업이면, **그 DevelopPrompt 체크아웃 자체에 대해**
   `POLICY/BRANCHING.md`의 "세션 시작 시 원본 동기화"를 수행한다 (이 저장소가 아니다).
3. 원본의 `README.md` → `POLICY/TEAM_RULES_PRIORITY.md` → `POLICY/SESSION_START.md`를 읽는다.
4. 이 저장소에 팀 문서가 있으면 그것이 위 전부보다 우선한다. 팀 문서는 읽고 따르기만 하고
   개인 규약에 맞추려고 수정하지 않는다.

## 1. 문서 위치

| 별칭 | 실제 위치 |
|---|---|
| `Docs/` | `{DevelopPrompt 원본 경로}/Unity/` |
| `Policy/` | `{DevelopPrompt 원본 경로}/POLICY/` |
| `Project/` | `{DevelopPrompt 원본 경로}/CurrentProject/_{등록명}/` |

`Project/README.md`가 이 프로젝트의 문서 지도다. 목표·현재 상태·설치 패키지·대상 플랫폼은
`Project/04_CURRENT_PROJECT.md`에서 확인한다.

## 2. 작업별 읽기

| 작업 | 추가 문서 |
|---|---|
| 코드 작성 | `Docs/02_CODING_CONVENTION.md`의 관련 절 |
| 기능·구조 변경 | `Docs/03_ARCHITECTURE.md`, `Docs/06_SCOPE_PLAYBOOK.md` |
| 구현 예시가 필요함 | `Docs/11_CODE_EXAMPLES.md`의 해당 예시 |
| 리뷰 | `Docs/05_CODE_REVIEW_CHECKLIST.md` |
| 버그 조사 | `Docs/08_PITFALLS.md`의 관련 증상 |
| 세팅·패키지·빌드 | `Docs/01_PROJECT_SETUP.md` |
| 검증·완료 | `Docs/13_VERIFICATION.md`, `Docs/14_DEFINITION_OF_DONE.md` |
| 결정 기록·인계 | `Policy/CODE_MEMO.md`, `Policy/SESSION_HANDOFF.md` |
| ECS/DOTS | `Docs/ECS/CONVENTIONS.md`, `Docs/ECS/EXAMPLES.md` (설치돼 있을 때만) |

## 3. 이 저장소의 전제

- 렌더 파이프라인은 **URP**다. Built-in·HDRP 셰이더 코드를 제안하지 않는다.
- 같은 역할의 라이브러리를 둘 넣지 않는다 (`Docs/01_PROJECT_SETUP.md` §2).
  DI=VContainer · 비동기=UniTask · 리액티브=R3 · 이벤트=MessagePipe · 로딩=Addressables.
- `Assets/Template/`은 템플릿의 `{Product}` 자리다. 새 프로젝트에서 제품명으로 바꾼다.
- `Editor/` 밖에서 `UnityEditor` 네임스페이스를 쓰면 빌드가 깨진다.

## 4. 검증

컴파일·테스트·에디터 제어가 이 환경에서 실제로 가능한지 먼저 확인한다.
`Project/04_CURRENT_PROJECT.md` §6의 검증 기록을 읽고 명령·버전이 아직 유효한지 확인한다.
확인하지 않은 것을 가능/불가능으로 단정하지 않는다. 실행한 검사·결과·미수행 항목과 이유를
완료 보고에 남긴다.

## 5. 기록

계약·불변식은 코드에, 배경·측정·실패한 대안은 `Project/CODE_MEMO.md`에 남긴다.
커밋·병합·푸시는 요청받았을 때 한다.
