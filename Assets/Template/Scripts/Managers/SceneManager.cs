using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

using RottenNoble.Cores.UI;

namespace RottenNoble.Cores.Scene
{
    using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

    public class SceneManager
    {
        private readonly string Tag = $"[{nameof(SceneManager)}]";

        public List<string> CurrentLoadedSceneNames { get; private set; } = new();

        private readonly UIManager uiManager;

        public SceneManager(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        public void SetActiveScene(string sceneName)
        {
            var scene = UnitySceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded)
                UnitySceneManager.SetActiveScene(scene);
        }

        public async UniTask LoadAdditiveScenesAsync(List<string> sceneNames, Action<string, float> onProgress = null)
        {
            var tasks = new List<UniTask>();
            foreach (var name in sceneNames)
                tasks.Add(LoadSceneAsync(name, LoadSceneMode.Additive, onProgress));

            await UniTask.WhenAll(tasks);

            CurrentLoadedSceneNames.Clear();
            CurrentLoadedSceneNames = sceneNames.ToList();
        }

        public async UniTask LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, Action<string, float> onProgress = null)
        {
            if (loadSceneMode == LoadSceneMode.Single)
                uiManager?.ClearAllCanvas();

            uiManager?.ShowBackground();

            var operation = UnitySceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                await UniTask.Yield();
                onProgress?.Invoke(sceneName, operation.progress);

                if (operation.progress >= 0.9f)
                {
                    onProgress?.Invoke(sceneName, 1f);
                    operation.allowSceneActivation = true;
                }
            }

            if (loadSceneMode == LoadSceneMode.Single)
                CurrentLoadedSceneNames.Clear();

            CurrentLoadedSceneNames.Add(sceneName);
        }

    }
}
