using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RottenNoble.Cores.UI
{
    public class ViewModelBase<TView, TModel> : ViewModelCore
        where TView  : ViewBase
        where TModel : ModelBase
    {
        protected TView View { get; private set; }
        protected TModel Model { get; private set; }

        public Func<UniTask> OnComplete { get; set; }

        public virtual UniTask Initialize(TView view, TModel model)
        {
            View  = view;
            Model = model;
            return UniTask.CompletedTask;
        }

        public void UpdateModel(TModel model)
        {
            Model = model;
        }

        protected UniTask HideViewAsync(bool immediate = false)
        {
            if (immediate)
            {
                View.HideImmediate();
                return UniTask.CompletedTask;
            }
            return View.HideAsync();
        }

        public static T Get<T>(string objectName) where T : ViewModelBase<TView, TModel>
        {
            var all = FindObjectsByType<ViewModelBase<TView, TModel>>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            return all.FirstOrDefault(o => o.name == objectName) as T;
        }
    }
}
