# Unity-Template

새 Unity 프로젝트의 **부트스트랩 원본**. 게임이 아니다.
[`DevelopPrompt`](https://github.com/noble-shake/DevelopPrompt)의 `Unity/01_PROJECT_SETUP.md`가
규정한 상태를 실제로 열리는 프로젝트 형태로 굳혀 둔 것이다.

## 쓰는 법

1. GitHub에서 **Use this template** → 새 저장소를 만든다 (clone/fork가 아니다 — 히스토리를 물려받지 않는다).
2. Unity Hub에서 그 폴더를 연다. 패키지가 복원될 때까지 기다린다.
3. `Assets/Template/`을 제품명으로 바꾼다.
4. `ProjectSettings` → Player에서 `companyName`·`productName`·`applicationIdentifier`를 채운다.
5. `CLAUDE.md`의 `{}` 자리를 실제 경로·등록명으로 채운다.
6. DevelopPrompt에 `TOOLS/new_project.cmd`로 `CurrentProject/_{프로젝트명}/`을 만든다.

## 들어 있는 것

| 항목 | 내용 |
|---|---|
| Unity | **6000.4.0b11** — `ProjectSettings/ProjectVersion.txt`가 유일한 진실 |
| 렌더 | URP 17.4.0. PC·Mobile 2티어 (`PC_RPAsset`/`PC_Renderer`, `Mobile_RPAsset`/`Mobile_Renderer`) |
| DI | VContainer 1.17.0 |
| 비동기 | UniTask |
| 리액티브 | R3 1.3.0 (+ NuGet R3 1.3.1) |
| 이벤트 버스 | MessagePipe (+ VContainer 연동) |
| 에셋 로딩 | Addressables 2.9.1 |
| 입력 | Input System 1.19.0 (`Assets/Template/Settings/InputSchema`) |
| 직렬화 | Newtonsoft.Json 3.2.2 |
| NuGet | NuGetForUnity (`Assets/packages.config`) |

폴더 구조는 `01_PROJECT_SETUP.md` §3을 그대로 따른다.

```
Assets/
├── ArtWorkspace/
├── Template/                  # ← {Product} 자리. 새 프로젝트에서 이름을 바꾼다
│   ├── AddressableResources/
│   ├── Scenes/Develop/
│   ├── Settings/              #   URP Asset · Renderer · Volume · Build Profiles
│   └── Scripts/               #   Commons · Constants · Contents · Cores · DI
│                              #   Editor · Managers · ScriptableObjects · Utils
├── Plugins/
└── StreamingAssets/aa/
```

## 들어 있지 않은 것 — 의도적이다

| 빠진 것 | 이유 |
|---|---|
| DOTween (Pro) | 유료 에셋. public 저장소에 커밋하지 않는다. 임포트 후 `UNITASK_DOTWEEN_SUPPORT` 정의 |
| Unity Localization · Cinemachine · Animation Rigging | `01_PROJECT_SETUP.md` §2의 코어 스택이지만 이 템플릿의 출처 프로젝트에 없었다. 버전을 지어내지 않고 비워 둔다 |
| Protobuf | 소켓을 쓰는 프로젝트에서만 추가한다 |
| Entities(ECS) · Purchasing · Cloud Build | 도메인 선택. 쓰는 프로젝트에서 추가하고 그 프로젝트의 04 문서에 기록한다 |
| 씬 | `EditorBuildSettings`가 비어 있다. 첫 씬은 프로젝트에서 만든다 |

## 버전 정책

`main`이 현재 권장 버전이다. 다른 Unity 버전을 함께 유지해야 하면 폴더가 아니라
**브랜치**(`unity/6000.3` 등)로 나눈다. 같은 골격을 폴더로 복제하면 `.meta` GUID가 중복되고
유지보수가 갈래 수만큼 늘어난다.
