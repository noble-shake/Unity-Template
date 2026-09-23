namespace RottenNoble.Cores.CQRS
{
    /// <summary>
    /// 읽는 쪽. Model을 화면이 그대로 쓸 수 있는 <b>Summary</b>로 변환한다.
    ///
    /// 규칙:
    /// - 부작용이 없다. 상태를 바꾸지 않고, 비동기도 아니다.
    /// - 표시용 가공(포맷·합계·등급 계산)은 여기서 한다. View에서 문자열을 조립하지 않는다.
    /// - Summary는 readonly struct다. 화면이 들고 있는 동안 원본이 바뀌어도 흔들리지 않는다.
    /// </summary>
    public interface IQuery<in TSource, out TSummary>
    {
        TSummary Execute(TSource source);
    }
}
