using System;
using R3;

namespace RottenNoble.Cores.UI
{
    [Serializable]
    public abstract class ModelBase
    {
        [NonSerialized] public readonly Subject<string> OnPropertyChanged = new Subject<string>();

        protected void NotifyChanged(string propertyName)
            => OnPropertyChanged.OnNext(propertyName);

        public static implicit operator bool(ModelBase m) => m != null;
    }
}
