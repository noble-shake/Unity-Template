using RottenNoble.Cores.Enum;

namespace RottenNoble.Cores.Provider
{
    /// <summary>
    /// 플랫폼·네트워크 환경별로 구현이 갈리는 서비스의 공통 계약.
    /// 인증·결제·푸시·랭킹처럼 "PC에선 되고 모바일에선 다르고 오프라인에선 대체가 필요한" 것들이 여기에 해당한다.
    ///
    /// 구현체는 자기가 언제 쓰일 수 있는지만 말하고, 고르는 일은 <see cref="ProviderSelector{T}"/>가 한다.
    /// 구현체 안에서 <c>#if UNITY_ANDROID</c>로 분기하지 않는다 — 분기가 파일마다 흩어지면 추적할 수 없다.
    /// </summary>
    public interface IEnvironmentProvider
    {
        /// <summary>이 구현이 지원하는 플랫폼. 어디서나 쓸 수 있으면 null 대신 현재 플랫폼을 담아 등록한다.</summary>
        PlatformKind Platform { get; }

        /// <summary>이 구현이 요구하는 네트워크 환경.</summary>
        NetworkMode Mode { get; }

        /// <summary>
        /// 런타임에 실제로 사용 가능한가. 플랫폼·모드가 맞아도 SDK 초기화 실패 등으로 false가 될 수 있다.
        /// 선택기는 이 값이 false인 구현을 건너뛴다.
        /// </summary>
        bool IsAvailable { get; }
    }
}
