using RottenNoble.Cores.UI;

namespace RottenNoble.Contents.Feature.Models
{
    /// <summary>
    /// 화면이 다루는 상태. 표시용 가공은 하지 않는다 — 그건 Query가 한다.
    /// 값이 바뀌면 <see cref="ModelBase.NotifyChanged"/>로 알린다.
    /// </summary>
    public class FeatureModel : ModelBase
    {
        private string title;
        public string Title
        {
            get => title;
            set { title = value; NotifyChanged(nameof(Title)); }
        }

        private int count;
        public int Count
        {
            get => count;
            set { count = value; NotifyChanged(nameof(Count)); }
        }
    }
}
