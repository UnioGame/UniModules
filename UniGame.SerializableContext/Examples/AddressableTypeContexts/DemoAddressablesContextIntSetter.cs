namespace UniGreenModules.UniGame.SerializableContext.Examples.AddressableTypeContexts
{
    using Runtime;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class DemoAddressablesContextIntSetter : MonoBehaviour
    {
        #region inspector

        public Button button;

        public AssetReference intContextResource;

        
        #endregion
        
        private IntContextValue intValue;
        
        // Start is called before the first frame update
        private async void Start()
        {
            button = GetComponent<Button>();
            intValue = await intContextResource.LoadAssetAsync<IntContextValue>().Task;
            
            button.onClick.
                AsObservable().
                Subscribe(x => intValue.SetValue(intValue.Value+1)).
                AddTo(this);
        }

    }
}
