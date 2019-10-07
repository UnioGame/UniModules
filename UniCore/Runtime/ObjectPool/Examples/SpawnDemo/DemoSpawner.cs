using System.Collections.Generic;
using UnityEngine;

namespace UniGreenModules.UniCore.Runtime.ObjectPool.Examples.SpawnDemo
{
    using Time = UnityEngine.Time;

    public class DemoSpawner : MonoBehaviour
    {
        private float                           timer;
        private bool                            spawnState = true;
        private Dictionary<Object,List<Object>> items      = new Dictionary<Object, List<Object>>();
    
        public List<Object> SpawnItems = new List<Object>();
        public List<Transform> SpawnComponents = new List<Transform>();
        
        public int Count;

        public float Delay;
    
    
        // Start is called before the first frame update
        private void Start()
        {
            foreach (var spawnItem in SpawnItems) {
                ObjectPool.CreatePool(spawnItem, Count);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer < Delay) return;

            timer = 0;

            if (spawnState) {
                Spawn<Object>(SpawnItems);
                Spawn<Transform>(SpawnComponents);
            }
            else {

                foreach (var item in items) {
                    foreach (var spawnedItem in item.Value) {
                        spawnedItem.Despawn();
                    }
                    item.Value.Clear();
                }
                items.Clear();
            
            }
        
            spawnState = !spawnState;

        }

        private void Spawn<T>(List<T> targetItems)
            where T : Object
        {
            foreach (var item in targetItems) {

                if (!items.TryGetValue(item, out var spawned)) {
                    spawned     = new List<Object>();
                    items[item] = spawned;
                }

                for (int i = 0; i < Count; i++) {
                    var spawnedItem = item.Spawn<T>();
                    if(spawnedItem is Component component)
                        component.gameObject.SetActive(true);
                    if(spawnedItem is GameObject assetObject)
                        assetObject.SetActive(true);
                        
                    spawned.Add(spawnedItem);
                }
            }
        }
    }
}
