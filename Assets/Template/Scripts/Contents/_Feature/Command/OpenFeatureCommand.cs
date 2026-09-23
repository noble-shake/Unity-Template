using System.Threading;
using Cysharp.Threading.Tasks;

using RottenNoble.Contents.Feature.Request;
using RottenNoble.Cores.CQRS;
using RottenNoble.Cores.Scene;

namespace RottenNoble.Contents.Feature.Command
{
    /// <summary>
    /// 사용자 의도 하나 = Command 하나. 여기서 다른 Command를 부르지 않는다.
    /// 실제 전환·로딩은 이미 있는 서비스(SceneManager·UINavigationManager)에 위임한다.
    /// </summary>
    public sealed class OpenFeatureCommand : ICommand<FeatureOpenRequest>
    {
        private readonly SceneManager sceneManager;

        public OpenFeatureCommand(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public UniTask ExecuteAsync(FeatureOpenRequest request, CancellationToken ct = default)
            => sceneManager.LoadSceneAsync(request.Address);
    }
}
