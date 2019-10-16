using System.Collections.Generic;
using UniGreenModules.UniCore.Runtime.ProfilerTools;
using UnityEngine;

namespace UniGreenModules.UniCore.Runtime.ObjectPool.Runtime
{
    using System;
    using Object = Object;

    public class AssetsPool : MonoBehaviour
    {
        private GameObject gameObjectAsset;
        private Component componentAsset;
        
        private Func<Vector3, Quaternion, Transform, bool, Object> factoryMethod;
        
        [Tooltip("The prefab the clones will be based on")]
        public Object Asset;

        [Tooltip("Should this pool preload some clones?")]
        public int Preload;

        [Tooltip("Should this pool have a maximum amount of spawnable clones?")]
        public int Capacity;

        // The total amount of created prefabs
        public int total;

        // All the currently cached prefab instances
        public Stack<Object> cache = new Stack<Object>();

        public void Initialize(Object asset, int preloadCount = 0)
        {
            Asset = asset;
            Preload = preloadCount;
            
            switch (asset) {
                case GameObject gameObjectTarget:
                    factoryMethod = CreateGameObject;
                    gameObjectAsset = gameObjectTarget;
                    break;
                case Component componentTarget:
                    componentAsset = componentTarget;
                    gameObjectAsset = componentTarget.gameObject;
                    factoryMethod = CreateGameObject;
                    break;
                default:
                    factoryMethod = CreateAsset;
                    break;
            }

            UpdatePreload();
        }

        // This will return a clone from the cache, or create a new instance
        public Object FastSpawn(Vector3 position, Quaternion rotation, Transform parent = null, bool stayWorld = false)
        {
            if (!Asset) {
                Debug.LogError("Attempting to spawn null");
                return null;
            }

            // Attempt to spawn from the cache
            while (cache.Count > 0) {
            
                var clone = cache.Pop();
                if (!clone) {
                    GameLog.LogError("The " + name + " pool contained a null cache entry");
                    continue;
                }

                GameProfiler.BeginSample("ObjectPool.FastSpawn");

                ApplyGameObjectProperties(clone, position, rotation, parent, stayWorld);

                GameProfiler.EndSample();

                return clone;
            }

            // Make a new clone?
            if (Capacity <= 0 || total < Capacity) {
                var clone = FastClone(position, rotation, parent, stayWorld);
                return clone;
            }

            return null;
        }

        // This will despawn a clone and add it to the cache
        public void FastDespawn(Object clone)
        {
            if (!clone) return;
            // Add it to the cache
            cache.Push(clone);

            if (clone is GameObject targetObject) {
                // Hide it
                targetObject.SetActive(false);
                // Move it under this GO
                if (targetObject.transform.parent != null) targetObject.transform.SetParent(transform, false);
            }
        }

        // Makes sure the right amount of prefabs have been preloaded
        public void UpdatePreload()
        {
            if (Asset == null) return;
            for (var i = total; i < Preload; i++) {
                FastPreload();
            }
        }

        private Object FastClone(Vector3 position, Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            if (!Asset) return null;

            GameProfiler.BeginSample("UniPool.FastClone");

            var clone = factoryMethod(position, rotation, parent, stayWorldPosition);

            total += 1;

            GameProfiler.EndSample();

            return clone;
        }

        public void ApplyGameObjectProperties(
            Object target, Vector3 position,
            Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            switch (target) {
                case Component componentTarget:
                    ApplyGameObjectProperties(componentTarget.gameObject, position, rotation, parent, stayWorldPosition);
                    break;
                case GameObject gameObjectTarget:
                    ApplyGameObjectProperties(gameObjectTarget, position, rotation, parent, stayWorldPosition);
                    break;
            }
        }

        public void ApplyGameObjectProperties(GameObject target, Vector3 position,
            Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            var transform = target.transform;
            transform.localPosition = position;
            transform.localRotation = rotation;

            if (transform.parent != parent)
                transform.SetParent(parent, stayWorldPosition);

            // Hide it
            target.SetActive(false);
        }

        // This allows you to make another clone and add it to the cache
        public void FastPreload()
        {
            if (!Asset) return;
            // Create clone
            var clone = FastClone(Vector3.zero, Quaternion.identity, null);
            // Add it to the cache
            cache.Push(clone);
        }

        // Execute preloaded count
        protected virtual void Awake()
        {
            UpdatePreload();
        }

        // Adds pool to list
        protected virtual void OnEnable()
        {
            ObjectPool.AllPools.Add(this);
        }

        // Remove pool from list
        protected virtual void OnDisable()
        {
            ObjectPool.AllPools.Remove(this);
            ObjectPool.AllSourceLinks.Remove(Asset);
        }

        private void OnDestroy()
        {
            ObjectPool.AllLinks.Clear();
        }

        private Object CreateGameObject(Vector3 position,
            Quaternion rotation, Transform parent = null, bool stayWorldPosition = false)
        {
            if (!Asset) return null;
            var result = Instantiate(gameObjectAsset, position, rotation);
            var resultTransform = result.transform;
            if (resultTransform.parent != parent)
                resultTransform.SetParent(parent, stayWorldPosition);
            return result;
        }
        
        
        private Object CreateAsset(Vector3 position,
            Quaternion rotation, Transform parent = null, bool stayWorldPosition = false)
        {
            return !Asset ? null : Instantiate(Asset);
        }
    }
}