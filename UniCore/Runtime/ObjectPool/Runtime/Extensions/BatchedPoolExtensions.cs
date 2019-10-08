namespace ZR.Runtime.Utils.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniGreenModules.UniCore.Runtime.Extension;
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UniRx;
    using UnityEngine;

    public struct TimedCounter
    {
        public float Time;
        public int Count;
    }
    
    public static class BatchedPoolExtensions
    {
        private static IDisposable batchDisposable;
        private static IDisposable spawnBatchDisposable;
        
        private static Queue<GameObject> items = new Queue<GameObject>();
        private static Dictionary<GameObject,int> spawnAmountAssets = new Dictionary<GameObject, int>();
        private static Dictionary<GameObject,TimedCounter> timedSpawnAssets = new Dictionary<GameObject, TimedCounter>();
        
        public static int BatchSize = 40;
        public static bool Enabled = true;

        public static GameObject SpawnTimed(this GameObject target,int batchAmount = 20, float     time = 0.1f)
        {
            return SpawnTimed(target, Vector3.zero, Quaternion.identity, null, false, batchAmount, time);
        }

        public static GameObject SpawnTimed(this GameObject target, Vector3 position, 
            Quaternion rotation, Transform parent, bool stayWorld, 
            int batchAmount = 20, float time = 0.1f)
        {
            if (!target) return null;

            Initialize();

            var isFound = timedSpawnAssets.TryGetValue(target, out var spawnData);
            var resetTime   = spawnData.Time + time;
            var currentTime = Time.realtimeSinceStartup;
            var counter = spawnData.Count;
            
            if (resetTime < currentTime) {
                spawnData.Count          = 0;
                spawnData.Time           = currentTime + time;
                timedSpawnAssets[target] = spawnData;
            }

            if (counter > batchAmount) {
                return null;
            }
            
            spawnData.Count = counter + 1;
            timedSpawnAssets[target] = spawnData;
            
            var asset = target.Spawn(position, rotation, parent, stayWorld);
            return asset;
            
        }

        
        public static GameObject SpawnBatched(this GameObject target, Vector3 position, Quaternion rotation, Transform parent, bool stayWorld, int batchAmount = 20)
        {
            if (!target) return null;

            if (spawnAmountAssets.TryGetValue(target, out var amount)) {
                if (amount > batchAmount) return null;
            }
            
            Initialize();

            var asset = target.Spawn(position, rotation, parent, stayWorld);
            spawnAmountAssets[target] = amount + 1;

            return asset;
            
        }

        public static GameObject SpawnBatched(this GameObject target, Transform parent = null, bool stayWorldPosition = false, int batchAmount = 20)
        {
            return SpawnBatched(target, Vector3.zero, Quaternion.identity, parent, stayWorldPosition, batchAmount);
        }

        public static GameObject SpawnBatched(this GameObject target, int batchAmount = 20)
        {
            return SpawnBatched(target, Vector3.zero, Quaternion.identity, null, false, batchAmount);
        }
        
        public static void DespawnBatched(this GameObject target, bool disableImmediately = false)
        {
            Initialize();
            if (!target) return;
            if(disableImmediately)
                target.SetActive(false);
            
            items.Enqueue(target);
        }
        
        public static void DespawnBatched(this Component target)
        {
            if (!target) return;
            items.Enqueue(target.gameObject);
        }

        public static void CleanUp()
        {
            while (items.Count > 0) {
                DespawnItem();
            }
            
            batchDisposable.Cancel();
            batchDisposable = null;
            spawnBatchDisposable.Cancel();
            spawnBatchDisposable = null;
            
            timedSpawnAssets.Clear();
        }

        private static void Initialize()
        {
            if (batchDisposable == null)
            {
                batchDisposable = Observable.FromMicroCoroutine(OnUpdate).Subscribe();
            }
            if (spawnBatchDisposable == null) {
                spawnBatchDisposable = Observable.
                    EveryLateUpdate().
                    Subscribe(x => spawnAmountAssets.Clear());
            }
        }

        private static IEnumerator OnUpdate()
        {
            while (Enabled) {
                yield return null;

                for (var i = 0; i < BatchSize && items.Count > 0; i++) {
                    DespawnItem();
                }
                
            }

            CleanUp();
            
        }

        private static void DespawnItem()
        {
            var item = items.Dequeue();
            if(!item) return;
            item.Despawn();
        }
        
        
    }
}
