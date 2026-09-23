# _Feature — 화면 하나의 세로 단면 (골격)

새 화면을 만들 때 이 폴더를 복사해 이름만 바꾼다. 폴더 이름의 `_`는 "실제 피처가 아닌 본보기"라는 뜻이다.
프로젝트에 첫 화면이 생기면 이 폴더는 지워도 된다.

```
_Feature/
├── Models/       Model      상태. 화면이 모르는 원본 데이터
├── Request/      Request    Command의 입력. readonly struct
├── Command/      Command    상태를 바꾸는 쪽 (쓰기)
├── Query/        Query      Model → Summary 변환 (읽기)
├── ViewModels/   ViewModel  View와 Model을 잇는다. 여기서만 둘을 동시에 안다
├── Views/        View       MonoBehaviour. 그리기만 한다
└── FeatureLifetimeScope.cs  이 화면에서만 사는 등록
```

읽기와 쓰기를 나누는 이유: 화면이 늘어날수록 "이 값을 누가 바꾸나"를 찾는 비용이 커진다.
Command만 상태를 바꾸므로 그 답이 항상 `Command/` 안에 있다.
