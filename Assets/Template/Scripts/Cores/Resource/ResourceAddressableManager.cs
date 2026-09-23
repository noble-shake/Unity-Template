using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RottenNoble.Cores.Resource
{
    public class ResourceAddressableManager : IResourceManager
    {
        private readonly Dictionary<string, UnityEngine.Object> assetCache = new();
        private readonly Dictionary<string, AsyncOperationHandle> assetHandles = new();
        private readonly Dictionary<int, (UnityEngine.Object instance, Action<UnityEngine.Object> release)> instancesById = new();

        public async UniTask<T> LoadAsync<T>(string path, Action<string, float> onProgress = null) where T : UnityEngine.Object
        {
            if (assetCache.TryGetValue(path, out var cached))
                return cached as T;

            var handle = Addressables.LoadAssetAsync<T>(path);
            while (!handle.IsDone)
            {
                onProgress?.Invoke(path, handle.PercentComplete);
                await UniTask.Yield();
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Addressables Load failed: {path}");

            assetCache[path] = handle.Result;
            assetHandles[path] = handle;
            return handle.Result;
        }

        public async UniTask<List<T>> LoadsAsync<T>(List<string> paths, Action<string, float> onProgress = null) where T : UnityEngine.Object
        {
            var results = new List<T>(paths.Count);
            foreach (var path in paths)
                results.Add(await LoadAsync<T>(path, onProgress));
            return results;
        }

        public async UniTask<List<T>> LoadByLabelAsync<T>(string label, Action<string, float> onProgress = null) where T : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetsAsync<T>((object)label, null);
            if (!assetHandles.ContainsKey(label))
                assetHandles[label] = handle;

            while (!handle.IsDone)
            {
                onProgress?.Invoke(label, handle.PercentComplete);
                await UniTask.Yield();
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Addressables.Release(handle);
                assetHandles.Remove(label);
                throw new Exception($"Addressables Label Load failed: {label}");
            }

            return new List<T>(handle.Result);
        }

        public async UniTask<T> CreateInstanceAsync<T>(string path, Transform parent) where T : UnityEngine.Object
        {
            if (typeof(T) == typeof(GameObject))
            {
                var h = Addressables.InstantiateAsync(path, parent);
                while (!h.IsDone) await UniTask.Yield();

                if (h.Status != AsyncOperationStatus.Succeeded)
                    throw new Exception($"Addressables Instantiate failed: {path}");

                var go = h.Result;
                go.name = go.name.Replace("(Clone)", "").Trim();
                RegisterInstance(go, obj => Addressables.ReleaseInstance((GameObject)obj));
                return go as T;
            }

            var asset = await LoadAsync<T>(path);
            var inst = UnityEngine.Object.Instantiate(asset, parent);
            inst.name = inst.name.Replace("(Clone)", "").Trim();
            RegisterInstance(inst, obj => UnityEngine.Object.Destroy(obj));
            return inst;
        }

        public async UniTask<List<T>> CreateInstancesAsync<T>(List<string> paths, Transform parent) where T : UnityEngine.Object
        {
            var results = new List<T>(paths.Count);
            foreach (var path in paths)
                results.Add(await CreateInstanceAsync<T>(path, parent));
            return results;
        }

        public void DeleteInstance(UnityEngine.Object instanceObject)
        {
            if (instanceObject == null) return;
            int id = instanceObject.GetInstanceID();
            if (!instancesById.TryGetValue(id, out var entry)) return;
            entry.release?.Invoke(entry.instance);
            instancesById.Remove(id);
        }

        public void Dispose()
        {
            foreach (var entry in instancesById.Values)
                entry.release?.Invoke(entry.instance);
            instancesById.Clear();

            foreach (var handle in assetHandles.Values)
                if (handle.IsValid()) Addressables.Release(handle);

            assetHandles.Clear();
            assetCache.Clear();
        }

        private void RegisterInstance(UnityEngine.Object obj, Action<UnityEngine.Object> release)
        {
            int id = obj.GetInstanceID();
            if (!instancesById.ContainsKey(id))
                instancesById[id] = (obj, release);
        }
    }
}
