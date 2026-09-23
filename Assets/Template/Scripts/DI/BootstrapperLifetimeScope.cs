using VContainer;
using VContainer.Unity;

using RottenNoble.Bootstrapper;

namespace RottenNoble.Extension.VContainer
{
    public class BootstrapperLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // 엔트리포인트는 Singleton이 기본이자 정답이다. Scoped면 이 스코프를 부모로 갖는
            // 자식 스코프가 생길 때마다 자식이 이걸 새로 만들어 Start를 다시 돌린다
            // (InGame에서 스테이지 무한 로드로 터졌다 — InGameLifetimeScope 주석 참고).
            builder.RegisterEntryPoint<BootstrapperEntryPoint>(Lifetime.Singleton);
        }
    }
}
