using System.Threading;
using Cysharp.Threading.Tasks;

namespace RottenNoble.Cores.CQRS
{
    /// <summary>
    /// 상태를 바꾸는 쪽. 하나의 Command는 하나의 사용자 의도를 실행한다.
    ///
    /// 규칙:
    /// - 입력은 <b>Request</b>(readonly struct)로만 받는다. View나 ViewModel을 인자로 받지 않는다.
    /// - 값을 돌려주지 않는다. 결과가 필요하면 상태를 바꾸고, 읽기는 <see cref="IQuery{TSource,TSummary}"/>가 한다.
    /// - Command 안에서 다른 Command를 부르지 않는다 — 실행 순서가 코드 전체에 흩어진다.
    /// </summary>
    public interface ICommand<in TRequest>
    {
        UniTask ExecuteAsync(TRequest request, CancellationToken ct = default);
    }
}
