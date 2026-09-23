using Cysharp.Threading.Tasks;
using R3;

using RottenNoble.Contents.Feature.Models;
using RottenNoble.Contents.Feature.Query;
using RottenNoble.Contents.Feature.Views;
using RottenNoble.Cores.UI;

namespace RottenNoble.Contents.Feature.ViewModels
{
    /// <summary>
    /// View와 Model을 동시에 아는 유일한 자리.
    ///
    /// - Model 변경 구독 → Query로 Summary를 만들어 View에 넘긴다.
    /// - 구독은 <see cref="ViewModelCore.disposableBag"/>에 넣는다. 안 넣으면 화면을 닫아도 살아남는다.
    /// - 상태를 바꿔야 하면 여기서 직접 하지 않고 Command를 부른다.
    /// </summary>
    public class FeatureViewModel : ViewModelBase<FeatureView, FeatureModel>
    {
        private readonly FeatureSummaryQuery summaryQuery = new();

        public override async UniTask Initialize(FeatureView view, FeatureModel model)
        {
            await base.Initialize(view, model);

            model.OnPropertyChanged
                .Subscribe(_ => Refresh())
                .AddTo(ref disposableBag);

            Refresh();
        }

        private void Refresh()
        {
            var summary = summaryQuery.Execute(Model);
            View.Render(in summary);
        }
    }
}
