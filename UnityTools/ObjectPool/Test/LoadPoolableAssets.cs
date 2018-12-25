using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.UnityTools.ObjectPool.Test
{
    [Serializable]
    public class LoadPoolsAsset
    {
        public int count;
        public GameObject asset;
    }

    public class LoadPoolableAssets : MonoBehaviour {

        public List<LoadPoolsAsset> PoolsAssets = new List<LoadPoolsAsset>();

        // Use this for initialization
        private IEnumerator Start () {

            yield return new WaitForSeconds(1);

            for (int i = 0; i < PoolsAssets.Count; i++)
            {
                var item = PoolsAssets[i];
                for (int j = 0; j < item.count; j++)
                {
                    Scripts.ObjectPool.Spawn(item.asset);
                }
            }

        }
	
    }
}