using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UniGreenModules.UniResourceSystem.Runtime
{
    using Object = UnityEngine.Object;

    [Serializable]
    public class AsyncResourceItem
    {

        [SerializeField]
        private AssetReference assetReference;

        private Object cachedAsset;
        
        public async Task<TResource> LoadAsync<TResource>()
            where TResource : Object
        {
            return null;
        }

        private TResource LoadFromCache<TResource>()
            where TResource : Object
        {
            return null;
        }
        
        
    }
}
