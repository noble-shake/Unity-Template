using Cysharp.Threading.Tasks;
using UnityEngine;

using VisibleStateEnum = RottenNoble.Cores.Enum.VisibleState;

namespace RottenNoble.Cores.UI.Popup
{
    public class PopupBusyView : ViewBase
    {
        [SerializeField] private Animator loadingAnimator;

        private static readonly int PlayHash = Animator.StringToHash("Play");
        private static readonly int StopHash = Animator.StringToHash("Stop");

        protected override UniTask OnShowAsync()
        {
            gameObject.SetActive(true);
            loadingAnimator.SetTrigger(PlayHash);
            return UniTask.CompletedTask;
        }

        protected override void OnShowImmediate()
        {
            gameObject.SetActive(true);
            loadingAnimator.SetTrigger(PlayHash);
        }

        protected override UniTask OnHideAsync()
        {
            loadingAnimator.SetTrigger(StopHash);
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        protected override void OnHideImmediate()
        {
            loadingAnimator.SetTrigger(StopHash);
            gameObject.SetActive(false);
        }
    }
}
