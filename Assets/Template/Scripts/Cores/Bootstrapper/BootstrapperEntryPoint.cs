using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

using RottenNoble.Cores.Manager;
using RottenNoble.Cores.Scene;

namespace RottenNoble.Bootstrapper
{
    /// <summary>
    /// 앱의 첫 실행 지점. Bootstrap 씬의 LifetimeScope가 Singleton으로 이것 하나만 등록한다.
    ///
    /// 여기서는 <b>다음 씬으로 넘어가기 위해 반드시 끝나 있어야 하는 것</b>만 한다.
    /// 로그인·패치·공지처럼 실패할 수 있는 단계는 각자의 씬에서 처리한다 — 여기서 하면
    /// 실패했을 때 보여 줄 화면이 아직 없다.
    ///
    /// 기본 흐름은 <b>Offline</b>이다. 원격 카탈로그·서버 통신은 프로젝트가 Online을 켤 때 추가한다.
    /// </summary>
    public class BootstrapperEntryPoint : IAsyncStartable
    {
        private readonly string Tag = $"[{nameof(BootstrapperEntryPoint)}]";

        private readonly TableDataManager tableDataManager;
        private readonly DataManager dataManager;
        private readonly SceneManager sceneManager;

        public BootstrapperEntryPoint(
            TableDataManager tableDataManager,
            DataManager dataManager,
            SceneManager sceneManager)
        {
            this.tableDataManager = tableDataManager;
            this.dataManager = dataManager;
            this.sceneManager = sceneManager;
        }

        public async UniTask StartAsync(CancellationToken ct = default)
        {
            tableDataManager.AppSetting.ApplyAppSettings();

            await dataManager.InitializeAsync(ct);

            // Online 프로젝트는 여기서 원격 카탈로그를 받는다.
            // 실패해도 로컬 리소스로 계속 진행할 수 있어야 한다 — 받지 못했다고 앱이 멈추면 안 된다.
            //
            //   if (tableDataManager.AppSetting.NetworkMode == NetworkMode.Online)
            //       await ApplyRemoteCatalogAsync(ct);

            RNDebug.Log(Tag + " Bootstrap Complete");

            var next = tableDataManager.Scene.Main;
            if (string.IsNullOrWhiteSpace(next))
            {
                // 씬 주소가 비어 있는 채로 넘기면 Unity 안쪽에서 터져서 원인을 알 수 없다.
                RNDebug.LogWarning(Tag + " ScenePathSO의 Main이 비어 있어 씬을 전환하지 않는다.");
                return;
            }

            await sceneManager.LoadSceneAsync(next);
        }
    }
}
