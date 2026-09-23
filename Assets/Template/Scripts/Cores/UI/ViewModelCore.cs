using R3;
using UnityEngine;
using VContainer;

using RottenNoble.Cores.Enum;
using RottenNoble.Cores.Manager;
using RottenNoble.Cores.Resource;
using RottenNoble.Cores.Scene;
using RottenNoble.Cores.Sound;
using RottenNoble.Cores.UI.Popup;

namespace RottenNoble.Cores.UI
{
    public class ViewModelCore : MonoBehaviour
    {
        protected IObjectResolver objectResolver;
        protected ResourceFactory resourceFactory;
        protected SceneManager sceneManager;
        protected DataManager dataManager;
        protected TableDataManager tableDataManager;
        protected SoundManager soundManager;
        protected PopupManager popupManager;

        public DisposableBag disposableBag = new DisposableBag();

        [Inject]
        private void InjectCores(
            IObjectResolver objectResolver,
            ResourceFactory resourceFactory,
            SceneManager sceneManager,
            DataManager dataManager,
            TableDataManager tableDataManager,
            SoundManager soundManager,
            PopupManager popupManager)
        {
            this.objectResolver = objectResolver;
            this.resourceFactory = resourceFactory;
            this.sceneManager = sceneManager;
            this.dataManager = dataManager;
            this.tableDataManager = tableDataManager;
            this.soundManager = soundManager;
            this.popupManager = popupManager;
        }

        private void OnDestroy()
        {
            disposableBag.Clear();
            resourceFactory?.DeleteInstance(ResourceEnum.Resource, gameObject);
            resourceFactory?.DeleteInstance(ResourceEnum.Local, gameObject);
            resourceFactory?.DeleteInstance(ResourceEnum.Addressables, gameObject);
        }
    }
}
