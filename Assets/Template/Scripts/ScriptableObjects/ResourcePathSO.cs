using System;
using UnityEngine;
using Newtonsoft.Json;

namespace RottenNoble.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ResourcePathSO", menuName = "RottenNoble/ScriptableObjects/Cores - Resource Path")]
    public class ResourcePathSO : ScriptableObject
    {
        [SerializeField] private LocalData local = new();
        public LocalData Local => local;

        [SerializeField] private RemoteData remote = new();
        public RemoteData Remote => remote;
    }

    // ─── Local ────────────────────────────────────────────────────────────────

    [Serializable]
    public class LocalData
    {
        [JsonProperty("popup")]
        [SerializeField] private LocalPopup popup;
        public LocalPopup Popup => popup;

        [Serializable]
        public class LocalPopup
        {
            [JsonProperty("busyDark")]
            [SerializeField] private string busyDark;
            public string BusyDark => busyDark;

            [JsonProperty("busyLight")]
            [SerializeField] private string busyLight;
            public string BusyLight => busyLight;
        }
    }

    // ─── Remote ───────────────────────────────────────────────────────────────

    [Serializable]
    public class RemoteData
    {
        // 프로젝트의 원격 주소를 여기에 추가한다. JsonProperty를 달면 원격 JSON으로
        // 덮어쓸 수 있다 — 번들을 다시 빌드하지 않고 주소만 교체하기 위한 것이다.
        //
        // 액터·스테이지처럼 종류가 늘어나는 주소는 이 전역 표에 두지 않는다.
        // 그것을 쓰는 데이터(SO)가 직접 들고 있게 한다 — 여기에 필드를 늘리면
        // 구성별로 다른 세트를 만들 수 없다.
    }
}
