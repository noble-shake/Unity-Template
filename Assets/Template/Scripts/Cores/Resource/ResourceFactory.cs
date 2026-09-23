using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

using RottenNoble.Cores.Enum;

namespace RottenNoble.Cores.Resource
{
    public class ResourceFactory
    {
        private readonly IObjectResolver resolver;
        private readonly ResourceManager resourceManager;

        public ResourceFactory(IObjectResolver resolver, ResourceManager resourceManager)
        {
            this.resolver = resolver;
            this.resourceManager = resourceManager;
        }

        public async UniTask<T> CreateAsync<T>(
            ResourceEnum type,
            string path,
            Transform parent = null,
            IObjectResolver injectionResolver = null) where T : UnityEngine.Object
        {
            var instance = await resourceManager.CreateInstanceAsync<T>(type, path, parent);
            InjectInternal(instance, injectionResolver);
            return instance;
        }

        public async UniTask<List<T>> CreateInstancesAsync<T>(ResourceEnum type, List<string> paths, Transform parent = null) where T : UnityEngine.Object
        {
            var instances = await resourceManager.CreateInstancesAsync<T>(type, paths, parent);
            foreach (var inst in instances) InjectInternal(inst);
            return instances;
        }

        public async UniTask<T> LoadAsync<T>(ResourceEnum type, string path, Action<string, float> onProgress = null) where T : UnityEngine.Object
        {
            var asset = await resourceManager.LoadAsync<T>(type, path, onProgress);
            InjectInternal(asset);
            return asset;
        }

        public async UniTask<List<T>> LoadByLabelAsync<T>(ResourceEnum type, string label, Action<string, float> onProgress = null) where T : UnityEngine.Object
        {
            var assets = await resourceManager.LoadByLabelAsync<T>(type, label, onProgress);
            foreach (var asset in assets) InjectInternal(asset);
            return assets;
        }

        public void DeleteInstance(ResourceEnum type, UnityEngine.Object instance)
            => resourceManager.DeleteInstance(type, instance);

        private void InjectInternal(UnityEngine.Object obj, IObjectResolver injectionResolver = null)
        {
            if (obj == null) return;
            var resolverToUse = injectionResolver ?? resolver;
            if (obj is GameObject go) resolverToUse.InjectGameObject(go);
            else if (obj is Component comp) resolverToUse.InjectGameObject(comp.gameObject);
        }
    }
}
