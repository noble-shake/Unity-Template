using RottenNoble.Cores.Enum;
using RottenNoble.Cores.Resource;
using RottenNoble.ScriptableObjects;

namespace RottenNoble.Cores.Manager
{
    /// <summary>
    /// 표(Table) 조회의 단일 진입점. 각 소비자가 <see cref="DataManager"/>의 SO를 직접 파고들지 않게 한다.
    ///
    /// 여기서 하는 일은 "SO의 원시 값을 쓰기 좋은 타입으로 감싸는 것"뿐이다. 업무 판단은 하지 않는다.
    /// 프로젝트는 아래 <c>SceneTable</c>·<c>ResourcePathTable</c>처럼 표 클래스를 늘려 간다.
    /// </summary>
    public class TableDataManager
    {
        public AppSettingSO AppSetting { get; }
        public ResourceManager Resource { get; }
        public ResourcePathTable ResourcePath { get; }
        public SceneTable Scene { get; }

        public TableDataManager(AppSettingSO appSetting, ResourceManager resource, DataManager dataManager)
        {
            AppSetting = appSetting;
            Resource = resource;
            ResourcePath = new ResourcePathTable(dataManager);
            Scene = new SceneTable(dataManager);
        }
    }

    // ─── ResourcePath ─────────────────────────────────────────────────────────

    /// <summary>
    /// 리소스 주소를 <see cref="ResourceAddress"/>(타입 + 경로)로 묶어 돌려준다.
    /// 호출부가 "이게 Local인지 Addressables인지"를 매번 기억하지 않게 하는 것이 목적이다.
    /// </summary>
    public class ResourcePathTable
    {
        private readonly DataManager dataManager;

        public ResourcePathTable(DataManager dataManager) => this.dataManager = dataManager;

        public PopupTable Popup => new(dataManager.ResourcePath.Local.Popup);

        public class PopupTable
        {
            private readonly LocalData.LocalPopup popup;
            public PopupTable(LocalData.LocalPopup popup) => this.popup = popup;

            public ResourceAddress BusyDark  => new(ResourceEnum.Local, popup.BusyDark);
            public ResourceAddress BusyLight => new(ResourceEnum.Local, popup.BusyLight);
        }
    }

    // ─── Scene ────────────────────────────────────────────────────────────────

    public class SceneTable
    {
        private readonly DataManager dataManager;

        public SceneTable(DataManager dataManager) => this.dataManager = dataManager;

        public string Main => dataManager.ScenePath.Main;
    }
}
