using System.Threading;
using Cysharp.Threading.Tasks;

using RottenNoble.Cores.Enum;

namespace RottenNoble.Cores.Provider
{
    /// <summary>
    /// 기본 구현. <b>서버가 없어도 앱이 끝까지 돈다</b>는 것을 보장하는 자리다.
    ///
    /// 로컬에서 만든 식별자를 토큰처럼 돌려준다. 서버 검증이 없으므로 이걸로 과금·랭킹을 다루지 않는다.
    /// Online 구현이 생겨도 이 구현은 지우지 않는다 — 네트워크가 끊긴 상태의 대체 경로다.
    /// </summary>
    public class OfflineAuthProvider : IAuthProvider
    {
        private const string DeviceIdKey = "offline.device.id";

        public PlatformKind Platform => ProviderSelector<IAuthProvider>.CurrentPlatform;
        public NetworkMode Mode => NetworkMode.Offline;
        public bool IsAvailable => true;

        public UniTask<string> TrySilentLoginAsync(CancellationToken ct = default)
            => UniTask.FromResult(GetOrCreateDeviceId());

        public UniTask<string> GetTokenAsync(CancellationToken ct = default)
            => UniTask.FromResult(GetOrCreateDeviceId());

        private static string GetOrCreateDeviceId()
        {
            var id = UnityEngine.PlayerPrefs.GetString(DeviceIdKey, string.Empty);
            if (!string.IsNullOrEmpty(id)) return id;

            id = System.Guid.NewGuid().ToString("N");
            UnityEngine.PlayerPrefs.SetString(DeviceIdKey, id);
            UnityEngine.PlayerPrefs.Save();
            return id;
        }
    }
}
