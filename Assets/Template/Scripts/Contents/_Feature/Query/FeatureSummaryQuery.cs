using RottenNoble.Contents.Feature.Models;
using RottenNoble.Cores.CQRS;

namespace RottenNoble.Contents.Feature.Query
{
    /// <summary>화면이 그대로 출력할 수 있는 형태. 문자열 조립을 View에서 하지 않기 위한 것이다.</summary>
    public readonly struct FeatureSummary
    {
        public string Title { get; }
        public string CountText { get; }

        public FeatureSummary(FeatureModel model)
        {
            Title = model.Title;
            CountText = $"{model.Count:N0}";
        }
    }

    public sealed class FeatureSummaryQuery : IQuery<FeatureModel, FeatureSummary>
    {
        public FeatureSummary Execute(FeatureModel source) => new(source);
    }
}
