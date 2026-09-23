using TMPro;
using UnityEngine;

using RottenNoble.Contents.Feature.Query;
using RottenNoble.Cores.UI;

namespace RottenNoble.Contents.Feature.Views
{
    /// <summary>
    /// 그리기만 한다. Model을 모르고 <see cref="FeatureSummary"/>만 받는다 —
    /// View가 Model을 알면 표시용 가공이 여기로 새어 들어온다.
    ///
    /// Show/Hide 연출이 필요하면 <see cref="ViewBase"/>의 OnShowAsync/OnHideAsync를 덮어쓴다.
    /// </summary>
    public class FeatureView : ViewBase
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text countText;

        public void Render(in FeatureSummary summary)
        {
            if (titleText) titleText.text = summary.Title;
            if (countText) countText.text = summary.CountText;
        }
    }
}
