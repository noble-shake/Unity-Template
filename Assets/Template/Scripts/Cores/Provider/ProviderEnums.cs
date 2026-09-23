namespace RottenNoble.Cores.Enum
{
    /// <summary>실행 플랫폼. Provider 선택의 첫 번째 축.</summary>
    public enum PlatformKind
    {
        Editor,
        Standalone,
        Android,
        iOS,
    }

    /// <summary>
    /// 네트워크 환경. Provider 선택의 두 번째 축.
    /// <b>기본값은 Offline이다</b> — 서버 없이도 실행되는 것이 이 템플릿의 기본 상태다.
    /// </summary>
    public enum NetworkMode
    {
        Offline = 0,
        Online  = 1,
    }
}
