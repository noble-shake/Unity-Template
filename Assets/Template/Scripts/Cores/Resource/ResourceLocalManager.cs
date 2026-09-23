using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RottenNoble.Cores.Resource
{
    public class ResourceLocalManager : IResourceManager
    {
        private readonly Dictionary<string, UnityEngine.Object> refCacheContainer = new();
        private readonly Dictionary<int, UnityEngine.Object> createdInstancesById = new();

        public async UniTask<T> LoadAsync<T>(string path, Action<string, float> onProgress = null) where T : UnityEngine.Object
        {
            if (refCacheContainer.TryGetValue(path, out var cached))
            {
                onProgress?.Invoke(path, 1f);
                return cached as T;
            }

            var request = Resources.LoadAsync<T>(path);
            while (!request.isDone)
            {
                onProgress?.Invoke(path, request.progress);
                await UniTask.Yield();
            }

            refCacheContainer[path] = request.asset;
            return request.asset as T;
        }

        public async UniTask<List<T>> LoadsAsync<T>(List<string> paths, Action<string, float> onProgress = null) where T : UnityEngine.Object
        {
            var results = new List<T>();
            foreach (var p in paths) results.Add(await LoadAsync<T>(p, onProgress));
            return results;
        }

        public async UniTask<List<T>> LoadByLabelAsync<T>(string label, Action<string, float> onProgress = null) where T : UnityEngine.Object
        {
            var assets = Resources.LoadAll<T>(label);
            onProgress?.Invoke(label, 1f);
            await UniTask.CompletedTask;
            return assets.ToList();
        }

        public async UniTask<T> CreateInstanceAsync<T>(string path, Transform parent) where T : UnityEngine.Object
        {
            var asset = await LoadAsync<T>(path);
            var instance = UnityEngine.Object.Instantiate(asset, parent);
            createdInstancesById[instance.GetInstanceID()] = instance;
            return instance;
        }

        public async UniTask<List<T>> CreateInstancesAsync<T>(List<string> paths, Transform parent) where T : UnityEngine.Object
        {
            var results = new List<T>();
            foreach (var p in paths) results.Add(await CreateInstanceAsync<T>(p, parent));
            return results;
        }

        public void DeleteInstance(UnityEngine.Object instanceObject)
        {
            if (instanceObject == null) return;
            int id = instanceObject.GetInstanceID();
            if (createdInstancesById.TryGetValue(id, out var obj))
            {
                UnityEngine.Object.Destroy(obj);
                createdInstancesById.Remove(id);
            }
        }

        public void Dispose()
        {
            foreach (var obj in createdInstancesById.Values)
                UnityEngine.Object.Destroy(obj);
            createdInstancesById.Clear();
            refCacheContainer.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}
