using System.Collections.Generic;
using Assets.Tools.UnityTools.ProfilerTools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Tools.UnityTools.ObjectPool.Scripts
{
    // This component allows you to pool Unity objects for fast instantiation and destruction
    [AddComponentMenu("Utils/ObjectPool/Pool")]
    public class ObjectPool : MonoBehaviour
    {
        private static GameObject _poolsRoot;

        // All the currently active pools in the scene
        public static List<ObjectPool> AllPools = new List<ObjectPool>();
        // The reference between a spawned GameObject and its pool
        public static Dictionary<GameObject, ObjectPool> AllLinks = new Dictionary<GameObject, ObjectPool>();
        // The reference between a spawned GameObject and its pool
        public static Dictionary<GameObject, ObjectPool> AllReleasedLinks = new Dictionary<GameObject, ObjectPool>();
        //The reference between a spawned source GameObject and its pool
        public static Dictionary<Object, ObjectPool> AllSourceLinks = new Dictionary<Object, ObjectPool>();

        [Tooltip("The prefab the clones will be based on")]
        public GameObject Prefab;

        [Tooltip("Should this pool preload some clones?")]
        public int Preload;

        [Tooltip("Should this pool have a maximum amount of spawnable clones?")]
        public int Capacity;

        // All the currently cached prefab instances
        private Stack<GameObject> cache = new Stack<GameObject>();

        // The total amount of created prefabs
        private int total;

        // These methods allows you to spawn prefabs via Component with varying levels of transform data
        public static T Spawn<T>(T prefab)
            where T : Component
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null,false);
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

        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false)
            where T : Component
        {
            // Clone this prefabs's GameObject
            var gameObject = prefab != null ? prefab.gameObject : null;
            var clone = Spawn(gameObject, position, rotation, parent, stayWorld, 0);

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }

        // These methods allows you to spawn prefabs via GameObject with varying levels of transform data
        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null,false, 0);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation,bool stayWorld = false)
        {
            return Spawn(prefab, position, rotation, null, stayWorld, 0);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld)
        {
            return Spawn(prefab, position, rotation, parent, stayWorld, 0);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld, int preload)
        {
            if (prefab == null)
            {
                Debug.LogError("Attempting to spawn a null prefab");
                return null;
            }

            GameProfiler.BeginSample("Tools ObjectPool.Spawn");

            var pool = CreatePool(prefab, preload);
            // Spawn a clone from this pool
            var clone = pool.FastSpawn(position, rotation, parent);

            GameProfiler.EndSample();

            // Was a clone created?
            // NOTE: This will be null if the pool's capacity has been reached
            if (clone == null) return null;
            // Associate this clone with this pool
            AllLinks.Add(clone, pool);
            AllReleasedLinks.Remove(clone);
            // Return the clone
            return clone.gameObject;
        }


        public static ObjectPool CreatePool(Object targetPrefab, int preloads = 0)
        {
            var component = targetPrefab as Component;
            var prefab = component ? component.gameObject : targetPrefab as GameObject;

            if (!prefab) return null;

            ObjectPool pool;
            // Find the pool that handles this prefab
            if (AllSourceLinks.TryGetValue(prefab, out pool))
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
            pool = new GameObject(prefab.name).AddComponent<ObjectPool>();
            pool.Prefab = prefab;
            pool.transform.SetParent(_poolsRoot.transform,false);
            AllSourceLinks.Add(prefab, pool);
            if (preloads <= 0) return pool;
            pool.Preload = preloads;
            pool.UpdatePreload();
            return pool;
        }


        // This allows you to despawn a clone via Component, with optional delay
        public static void Despawn(Component clone, float delay = 0.0f)
        {
            if (clone) Despawn(clone.gameObject);
        }

        // This allows you to despawn a clone via GameObject, with optional delay
        public static void Despawn(GameObject clone)
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
        public GameObject FastSpawn(Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false)
        {
            if (!Prefab)
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

                GameProfiler.BeginSample("LeanPool.FastSpawn");
                // Execute transform of clone
                var cloneTransform = clone.transform;
                cloneTransform.localPosition = position;
                cloneTransform.localRotation = rotation;
                if(parent && cloneTransform!= parent)
                    cloneTransform.SetParent(parent, stayWorld);
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
        public void FastDespawn(GameObject clone)
        {
            if (!clone) return;
            // Add it to the cache
            cache.Push(clone);

            // Hide it
            clone.SetActive(false);

            // Move it under this GO
            clone.transform.SetParent(transform, false);
        }

        // This allows you to make another clone and add it to the cache
        public void FastPreload()
        {
            if (!Prefab) return;
            // Create clone
            var clone = FastClone(Vector3.zero, Quaternion.identity, null);

            // Add it to the cache
            cache.Push(clone);

            // Hide it
            clone.SetActive(false);

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
            AllSourceLinks.Remove(Prefab);
        }

        private void OnDestroy()
        {
            AllLinks.Clear();
        }

        // Makes sure the right amount of prefabs have been preloaded
        public void UpdatePreload()
        {
            if (Prefab == null) return;
            for (var i = total; i < Preload; i++)
            {
                FastPreload();
            }
        }


        // Returns a clone of the prefab and increments the total
        // NOTE: Prefab is assumed to exist
        private GameObject FastClone(Vector3 position, Quaternion rotation, Transform parent,bool stayWorldPosition = false)
        {
            GameProfiler.BeginSample("LeanPool.FastClone");
            var clone = (GameObject)Instantiate(Prefab, position, rotation);

            total += 1;

            clone.name = Prefab.name;

            if(clone.transform != parent)
                clone.transform.SetParent(parent, stayWorldPosition);

            GameProfiler.EndSample();

            return clone;
        }

        public static string GetPrefabName(GameObject effect)
        {
            ObjectPool pool;
            if (AllLinks.TryGetValue(effect, out pool))
                return pool.Prefab.name;

            return null;
        }
    }
}