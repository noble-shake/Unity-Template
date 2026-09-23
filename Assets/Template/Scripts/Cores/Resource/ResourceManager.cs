using VContainer.Unity;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

using RottenNoble.Cores.Enum;

namespace RottenNoble.Cores.Resource
{
    public class ResourceManager : IInitializable, IDisposable
    {
        private readonly Dictionary<ResourceEnum, IResourceManager> resourceProvider = new();

        public void Initialize()
        {
            var addressableManager = new ResourceAddressableManager();
            resourceProvider.Add(ResourceEnum.Local, addressableManager);
            resourceProvider.Add(ResourceEnum.Addressables, addressableManager);
            resourceProvider.Add(ResourceEnum.Resource, new ResourceLocalManager());
        }

        public async UniTask<T> LoadAsync<T>(ResourceEnum type, string path, Action<string, float> onProgress = null) where T : UnityEngine.Object
            => await resourceProvider[type].LoadAsync<T>(path, onProgress);

        public async UniTask<List<T>> LoadsAsync<T>(ResourceEnum type, List<string> paths, Action<string, float> onProgress = null) where T : UnityEngine.Object
            => await resourceProvider[type].LoadsAsync<T>(paths, onProgress);

        public async UniTask<List<T>> LoadByLabelAsync<T>(ResourceEnum type, string label, Action<string, float> onProgress = null) where T : UnityEngine.Object
            => await resourceProvider[type].LoadByLabelAsync<T>(label, onProgress);

        public async UniTask<T> CreateInstanceAsync<T>(ResourceEnum type, string path, Transform parent) where T : UnityEngine.Object
            => await resourceProvider[type].CreateInstanceAsync<T>(path, parent);

        public async UniTask<List<T>> CreateInstancesAsync<T>(ResourceEnum type, List<string> paths, Transform parent) where T : UnityEngine.Object
            => await resourceProvider[type].CreateInstancesAsync<T>(paths, parent);

        public void DeleteInstance(ResourceEnum type, UnityEngine.Object instanceObject)
            => resourceProvider[type].DeleteInstance(instanceObject);

        public void Dispose()
        {
            foreach (var pair in resourceProvider)
                pair.Value.Dispose();
        }
    }
}
