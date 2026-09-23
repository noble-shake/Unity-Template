using System.Threading;
using Cysharp.Threading.Tasks;

namespace RottenNoble.Cores.Provider
{
    /// <summary>
    /// 플랫폼·네트워크 환경별 Provider의 <b>작성 예시</b>다. 인증이 필요 없는 프로젝트는 지운다.
    ///
    /// 같은 모양으로 결제·푸시·랭킹 Provider를 만든다 —
    /// 계약 하나 + 플랫폼별 구현 + <see cref="OfflineAuthProvider"/> 같은 Offline 대체.
    /// </summary>
    public interface IAuthProvider : IEnvironmentProvider
    {
        /// <summary>유저 인터랙션 없이 자동 로그인. 실패하면 빈 문자열.</summary>
        UniTask<string> TrySilentLoginAsync(CancellationToken ct = default);

        /// <summary>유저가 버튼을 눌렀을 때의 인터랙티브 로그인.</summary>
        UniTask<string> GetTokenAsync(CancellationToken ct = default);
    }
}
