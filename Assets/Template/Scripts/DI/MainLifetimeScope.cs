using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

using RottenNoble.Cores.Input;
using RottenNoble.Cores.Manager;
using RottenNoble.Cores.Resource;
using RottenNoble.Cores.Scene;
using RottenNoble.Cores.Sound;
using RottenNoble.Cores.UI;
using RottenNoble.Cores.UI.Navigation;
using RottenNoble.Cores.UI.Popup;
using RottenNoble.ScriptableObjects;

namespace RottenNoble.Extension.VContainer
{
    /// <summary>
    /// 앱 전역 DI 루트. 씬이 바뀌어도 살아 있어야 하는 것만 여기서 등록한다.
    /// 화면(피처) 단위 등록은 그 화면의 자식 LifetimeScope에서 한다 — 여기에 쌓으면
    /// 안 쓰는 화면의 의존까지 부팅 시점에 만들어진다.
    ///
    /// 프로젝트가 추가하는 전역 서비스는 아래 구획 중 맞는 자리에 넣는다.
    /// </summary>
    public class MainLifetimeScope : LifetimeScope
    {
        [field: SerializeField] private UIManager uiManager;
        [field: SerializeField] private InputManager inputManager;
        [field: SerializeField] private SoundManager soundManager;

        [Header("App Config")]
        [field: SerializeField] private AppSettingSO appSettingSO;

        protected override void Configure(IContainerBuilder builder)
        {
            // MessagePipe (Additive Scene 간 통신)
            builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(container => GlobalMessagePipe.SetProvider(container.AsServiceProvider()));

            // Scene / Resource
            builder.Register<SceneManager>(Lifetime.Singleton);
            builder.Register<ResourceManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ResourceFactory>(Lifetime.Singleton);

            // Data
            builder.RegisterInstance(appSettingSO);
            builder.Register<DataManager>(Lifetime.Singleton);
            builder.Register<TableDataManager>(Lifetime.Singleton);

            // Provider — 플랫폼·네트워크 환경별 구현을 여기서 등록한다.
            // 기본은 Offline이다. Online 구현은 서버가 준비된 프로젝트에서만 추가하고,
            // Offline 구현은 항상 남겨 둔다 (ProviderSelector의 마지막 보루).
            //
            //   builder.Register<IAuthProvider, OfflineAuthProvider>(Lifetime.Singleton);
            // #if UNITY_ANDROID
            //   builder.Register<IAuthProvider, GooglePlayAuthProvider>(Lifetime.Singleton);
            // #endif
            //   builder.Register<ProviderSelector<IAuthProvider>>(Lifetime.Singleton);

            // UI
            builder.Register<UINavigationManager>(Lifetime.Singleton);
            builder.Register<PopupManager>(Lifetime.Singleton);

            // Managers (MonoBehaviour)
            builder.RegisterComponentInNewPrefab<UIManager>(uiManager, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponentInNewPrefab<InputManager>(inputManager, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponentInNewPrefab<SoundManager>(soundManager, Lifetime.Singleton).DontDestroyOnLoad();
        }
    }
}
