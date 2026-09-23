using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using RottenNoble.Cores.Enum;

namespace RottenNoble.Cores.Provider
{
    /// <summary>
    /// 등록된 Provider 중 현재 플랫폼·네트워크 환경에 맞는 것을 고른다.
    ///
    /// 선택 순서: (1) 현재 플랫폼 + 요청 모드 → (2) 현재 플랫폼 + Offline → (3) 없음.
    /// <b>Offline 대체가 항상 마지막 보루다.</b> 온라인 구현이 없거나 못 쓰는 상황에서 앱이 죽지 않게 한다.
    /// </summary>
    public class ProviderSelector<T> where T : class, IEnvironmentProvider
    {
        private readonly IReadOnlyList<T> providers;

        public ProviderSelector(IEnumerable<T> providers)
        {
            this.providers = providers?.ToList() ?? new List<T>();
        }

        public static PlatformKind CurrentPlatform
        {
            get
            {
#if UNITY_EDITOR
                return PlatformKind.Editor;
#elif UNITY_ANDROID
                return PlatformKind.Android;
#elif UNITY_IOS
                return PlatformKind.iOS;
#else
                return PlatformKind.Standalone;
#endif
            }
        }

        /// <summary>요청 모드에 맞는 구현을 고른다. 없으면 Offline 구현으로 내려간다. 그것도 없으면 null.</summary>
        public T Resolve(NetworkMode requested)
        {
            var platform = CurrentPlatform;

            var exact = Find(platform, requested);
            if (exact != null) return exact;

            if (requested != NetworkMode.Offline)
            {
                var fallback = Find(platform, NetworkMode.Offline);
                if (fallback != null)
                {
                    RNDebug.LogWarning(
                        $"[ProviderSelector] {typeof(T).Name}: {platform}/{requested} 구현이 없어 Offline으로 내려간다.");
                    return fallback;
                }
            }

            RNDebug.LogWarning($"[ProviderSelector] {typeof(T).Name}: {platform}/{requested}에 쓸 구현이 없다.");
            return null;
        }

        public IReadOnlyList<T> ResolveAll(NetworkMode requested)
            => providers.Where(p => p.IsAvailable && p.Platform == CurrentPlatform && p.Mode == requested).ToList();

        private T Find(PlatformKind platform, NetworkMode mode)
            => providers.FirstOrDefault(p => p.IsAvailable && p.Platform == platform && p.Mode == mode);
    }
}
