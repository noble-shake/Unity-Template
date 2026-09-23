using VContainer;
using VContainer.Unity;

using RottenNoble.Contents.Feature.Command;
using RottenNoble.Contents.Feature.Query;

namespace RottenNoble.Contents.Feature
{
    /// <summary>
    /// 이 화면에서만 사는 등록. 씬에 두고 MainLifetimeScope를 부모로 갖는다.
    ///
    /// 전역 서비스는 여기서 다시 등록하지 않는다 — 부모 것을 그대로 주입받는다.
    /// 여기에 등록한 것은 씬이 내려가면 함께 사라진다.
    /// </summary>
    public class FeatureLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<FeatureSummaryQuery>(Lifetime.Scoped);
            builder.Register<OpenFeatureCommand>(Lifetime.Scoped);
        }
    }
}
