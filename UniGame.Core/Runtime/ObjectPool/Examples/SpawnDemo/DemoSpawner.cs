using System.Collections.Generic;
using UnityEngine;

namespace UniGreenModules.UniCore.Runtime.ObjectPool.Examples.SpawnDemo
{
    using System;
    using Runtime;
    using Runtime.Extensions;
    using Object = UnityEngine.Object;
    using Time = UnityEngine.Time;

    public class DemoSpawner : MonoBehaviour
    {
        private float                           timer;
        private bool                            spawnState = true;
        private Dictionary<Object,List<Object>> items      = new Dictionary<Object, List<Object>>();
    
        public List<Object> SpawnItems = new List<Object>();
        public List<Transform> SpawnComponents = new List<Transform>();
        
        public List<SpriteRenderer> SpawnSpriteRenderer;
        public Sprite Sprite;
        
        public Transform childItem;
        
        public int Count;

        public float Delay;
    
    
        // Start is called before the first frame update
        private void Start()
        {
            foreach (var spawnItem in SpawnItems) {
                ObjectPool.CreatePool(spawnItem, Count);
            }
            
            var item = childItem.Spawn(Vector3.one, Quaternion.identity, transform);
            item.gameObject.SetActive(true);
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
                Spawn<SpriteRenderer>(SpawnSpriteRenderer, x => x.sprite = Sprite.Spawn());
            }
            else {

                foreach (var item in items) {
                    foreach (var spawnedItem in item.Value) {
                        switch (spawnedItem) {
                            case SpriteRenderer spriteRenderer:
                                spriteRenderer.sprite.Despawn();
                                spriteRenderer.sprite = null;
                                break;
                        }
                        spawnedItem.Despawn();
                    }
                    item.Value.Clear();
                }
                items.Clear();
            
            }
        
            spawnState = !spawnState;

        }

        private void Spawn<T>(List<T> targetItems, Action<T> onSpawn = null)
            where T : Object
        {
            var counterItem = 0;
            
            foreach (var item in targetItems) {

                if (!items.TryGetValue(item, out var spawned)) {
                    spawned     = new List<Object>();
                    items[item] = spawned;
                }

                for (var i = 0; i < Count; i++) {
                    var spawnedItem = item.Spawn<T>(new Vector3(i*2,counterItem*2,0),Quaternion.identity);
                    
                    switch (spawnedItem) {
                        case Component component:
                            component.gameObject.SetActive(true);
                            break;
                        case GameObject assetObject:
                            assetObject.SetActive(true);
                            break;
                    }

                    onSpawn?.Invoke(spawnedItem);
                    
                    spawned.Add(spawnedItem);
                }

                counterItem++;
            }
        }
    }
}
