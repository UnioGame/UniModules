namespace UniModules.UniGame.AddressableTools.Runtime.Components
{
    using Core.Runtime.DataFlow.Extensions;
    using Cysharp.Threading.Tasks;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class InstantiateAddressables : MonoBehaviour
    {

        public AssetReference reference;
        public bool           createOnStart = true;
        public Transform      parent;
        public bool           useParent = true;
    
        // Start is called before the first frame update
        public async void Start()
        {
            if (createOnStart)
                await Create();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public async UniTask Create()
        {
            if (!reference.RuntimeKeyIsValid())
                return;
            
            var targetParent = useParent ? parent == null ? transform : parent : null;
            var asset        = await reference.LoadAssetTaskAsync<Object>(this.GetLifeTime());

            if (asset is GameObject gameObjectAsset) {
                GameObject.Instantiate(gameObjectAsset, Vector3.zero, Quaternion.identity, targetParent);
            }
            else {
                Object.Instantiate(asset);
            }
        }

    }
}
