namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
    using System.Collections.Generic;
    using Interfaces;
    using ProfilerTools;
    using UnityEngine;

    // This component allows you to pool Unity objects for fast instantiation and destruction
    [AddComponentMenu("Utils/ObjectPool/Pool")]
    public class ObjectPool : MonoBehaviour
    {
        private static GameObject _poolsRoot;

        // All the currently active pools in the scene
        public static List<ObjectPool> AllPools = new List<ObjectPool>();
        // The reference between a spawned GameObject and its pool
        public static Dictionary<Object, ObjectPool> AllLinks = new Dictionary<Object, ObjectPool>();
        // The reference between a spawned GameObject and its pool
        public static Dictionary<Object, ObjectPool> AllReleasedLinks = new Dictionary<Object, ObjectPool>();
        //The reference between a spawned source GameObject and its pool
        public static Dictionary<Object, ObjectPool> AllSourceLinks = new Dictionary<Object, ObjectPool>();

        [Tooltip("The prefab the clones will be based on")]
        public Object Asset;

        [Tooltip("Should this pool preload some clones?")]
        public int Preload;

        [Tooltip("Should this pool have a maximum amount of spawnable clones?")]
        public int Capacity;

        // All the currently cached prefab instances
        private Stack<Object> cache = new Stack<Object>();

        // The total amount of created prefabs
        private int total;

        // These methods allows you to spawn prefabs via Component with varying levels of transform data
        public static T Spawn<T>(Object asset)
            where T : Object
        {
            return Spawn<T>(asset, Vector3.zero, Quaternion.identity, null,false) as T;
        }

        public static T Spawn<T>(GameObject prefab)
        {
            var item = Spawn(prefab, Vector3.zero, Quaternion.identity, null, false);
            var result = item.GetComponent<T>();
            if (result == null)
            {
                Despawn(item);
            }
            return result;
        }
        

        public static T Spawn<T>(Object target, Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false)
            where T : Object
        {
            var isComponent = target is Component;
            // Clone this prefabs's GameObject
            var asset = target ? isComponent ? 
                    ((Component)target).gameObject : target : target;
            
            var clone = Spawn(asset, position, rotation, parent, stayWorld, 0);

            if (isComponent && clone is GameObject gameAsset) {
                return gameAsset.GetComponent<T>();
            }
            
            // Return the same component from the clone
            return clone as T;
        }

        // These methods allows you to spawn prefabs via GameObject with varying levels of transform data
        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null,false, 0) as GameObject;
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation,bool stayWorld = false)
        {
            return Spawn(prefab, position, rotation, null, stayWorld, 0) as GameObject;
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld)
        {
            return Spawn(prefab, position, rotation, parent, stayWorld, 0) as GameObject;
        }

        public static Object Spawn(Object prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld, int preload)
        {
            if (!prefab)
            {
                Debug.LogError("Attempting to spawn a null prefab");
                return null;
            }

            GameProfiler.BeginSample("Tools ObjectPool.Spawn");

            var pool = CreatePool(prefab, preload);
            // Spawn a clone from this pool
            var clone = pool.FastSpawn(position, rotation, parent,stayWorld);

            GameProfiler.EndSample();

            // Was a clone created?
            // NOTE: This will be null if the pool's capacity has been reached
            if (clone == null) return null;
            // Associate this clone with this pool
            AllLinks.Add(clone, pool);
            AllReleasedLinks.Remove(clone);
            // Return the clone
            return clone;
        }


        public static ObjectPool CreatePool(Object targetPrefab, int preloads = 0)
        {
            if (!targetPrefab) return null;

            ObjectPool pool;
            // Find the pool that handles this prefab
            if (AllSourceLinks.TryGetValue(targetPrefab, out pool))
            {
                var preloadCount = preloads - pool.Cached;
                for (int i = 0; i < preloadCount; i++)
                {
                    pool.FastPreload();
                }
                return pool;
            }
            //create root
            if (!_poolsRoot)
            {
                _poolsRoot = new GameObject("PoolsRootObject");
            }
            // Create a new pool for this prefab?
            pool = new GameObject(targetPrefab.name).AddComponent<ObjectPool>();
            pool.Asset = targetPrefab;
            pool.transform.SetParent(_poolsRoot.transform,false);
            AllSourceLinks.Add(targetPrefab, pool);
            if (preloads <= 0) return pool;
            pool.Preload = preloads;
            pool.UpdatePreload();
            return pool;
            
        }


        // This allows you to despawn a clone via Component, with optional delay
        public static void Despawn(Component clone, float delay = 0.0f)
        {
            if (clone is IPoolable poolable)
            {
                poolable.Release();
            }
            if (clone) Despawn(clone.gameObject);
        }

        // This allows you to despawn a clone via GameObject, with optional delay
        public static void Despawn(Object clone)
        {
            if (clone)
            {
                var pool = default(ObjectPool);

                // Try and find the pool associated with this clone
                if (AllLinks.TryGetValue(clone, out pool))
                {
                    // Remove the association
                    AllLinks.Remove(clone);
                    AllReleasedLinks.Add(clone, pool);
                    // Despawn it
                    pool.FastDespawn(clone);
                }
                if (AllReleasedLinks.ContainsKey(clone))
                {
                    return;
                }
                // Fall back to normal destroying
                Destroy(clone);
            }
        }

        // Returns the total amount of spawned clones
        public int Total
        {
            get
            {
                return total;
            }
        }

        // Returns the amount of cached clones
        public int Cached
        {
            get
            {
                return cache.Count;
            }
        }

        public static void RemoveFromPool(GameObject target)
        {
            ObjectPool pool;
            if (!AllLinks.TryGetValue(target, out pool)) return;
            // Remove the association
            AllLinks.Remove(target);
            AllReleasedLinks.Remove(target);
        }

        // This will return a clone from the cache, or create a new instance
        public Object FastSpawn(Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false)
        {
            if (!Asset)
            {

                Debug.LogError("Attempting to spawn null");
                return null;
            }

            // Attempt to spawn from the cache
            while (cache.Count > 0)
            {
                var clone = cache.Pop();
                if (!clone)
                {
                    GameLog.LogError("The " + name + " pool contained a null cache entry");
                    continue;
                }

                GameProfiler.BeginSample("ObjectPool.FastSpawn");

                ApplyGameObjectProperties(clone, position, rotation, parent, stayWorld);

                GameProfiler.EndSample();

                return clone;
            }

            // Make a new clone?
            if (Capacity <= 0 || total < Capacity)
            {
                var clone = FastClone(position, rotation, parent);
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
                if(targetObject.transform.parent != null) targetObject.transform.SetParent(transform, false);
            }
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
            AllPools.Add(this);
        }

        // Remove pool from list
        protected virtual void OnDisable()
        {
            AllPools.Remove(this);
            AllSourceLinks.Remove(Asset);
        }

        private void OnDestroy()
        {
            AllLinks.Clear();
        }

        // Makes sure the right amount of prefabs have been preloaded
        public void UpdatePreload()
        {
            if (Asset == null) return;
            for (var i = total; i < Preload; i++)
            {
                FastPreload();
            }
        }

        private Object FastClone(Vector3 position, Quaternion rotation, Transform parent,bool stayWorldPosition = false)
        {
            GameProfiler.BeginSample("LeanPool.FastClone");
            if (!Asset) return null;
            
            var clone = Instantiate(Asset);
            total += 1;

            GameProfiler.EndSample();

            return clone;
        }

        public static void ApplyGameObjectProperties(
            Object target,   Vector3   position,
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

        public static void ApplyGameObjectProperties(GameObject target,Vector3 position,
            Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            var transform = target.transform;
            transform.position = position;
            transform.rotation = rotation;
            
            if(transform.parent != parent)
                transform.SetParent(parent, stayWorldPosition);
            
            // Hide it
            target.SetActive(false);

        }
        
        public static string GetPrefabName(GameObject effect)
        {
            ObjectPool pool;
            if (AllLinks.TryGetValue(effect, out pool))
                return pool.Asset.name;

            return null;
        }
    }
}