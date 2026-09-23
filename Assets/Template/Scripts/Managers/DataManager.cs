using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

using RottenNoble.Cores.Enum;
using RottenNoble.Cores.Resource;
using RottenNoble.ScriptableObjects;

namespace RottenNoble.Cores.Manager
{
    /// <summary>
    /// 부팅 시 필요한 표(SO)를 로드해 보관한다. 여기서 로드한 것만이 <see cref="TableDataManager"/>의 원본이다.
    /// 게임 데이터(밸런스·스테이지 등)는 여기에 쌓지 않는다 — 그건 프로젝트의 전용 매니저가 갖는다.
    /// </summary>
    public class DataManager
    {
        private readonly string Tag = $"[{nameof(DataManager)}]";

        public ScenePathSO ScenePath { get; private set; }
        public ResourcePathSO ResourcePath { get; private set; }

        private readonly ResourceFactory resourceFactory;

        public DataManager(ResourceFactory resourceFactory)
        {
            this.resourceFactory = resourceFactory;
        }

        public async UniTask InitializeAsync(CancellationToken cancellation = default)
        {
            // Local 라벨은 앱에 내장된(StreamingAssets) 그룹이다. 원격이 없어도 여기까지는 항상 성공한다.
            await resourceFactory.LoadByLabelAsync<Object>(ResourceEnum.Local, "Local");

            ScenePath    = await resourceFactory.LoadAsync<ScenePathSO>(ResourceEnum.Local, nameof(ScenePathSO));
            ResourcePath = await resourceFactory.LoadAsync<ResourcePathSO>(ResourceEnum.Local, nameof(ResourcePathSO));

            RNDebug.Log(Tag + " Initialize Done");
        }

        /// <summary>
        /// 원격 JSON으로 리소스 주소를 덮어쓴다. Online 환경에서만 호출된다 —
        /// Offline이면 SO에 들어 있는 로컬 주소를 그대로 쓴다.
        /// </summary>
        public void UpdateRemoteResourcePath(string updateSourceJson)
        {
            if (string.IsNullOrEmpty(updateSourceJson) == false)
                JsonConvert.PopulateObject(updateSourceJson, ResourcePath);
        }
    }
}
