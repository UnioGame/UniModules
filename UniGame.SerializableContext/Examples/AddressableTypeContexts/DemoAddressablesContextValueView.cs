namespace UniGreenModules.UniGame.SerializableContext.Examples.AddressableTypeContexts
{
    using Runtime;
    using TMPro;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DemoAddressablesContextValueView : MonoBehaviour
    {
#region inspector

        public TextMeshProUGUI intText;

        public AssetReference intContextResource;
        
#endregion
        
        private IntContextValue intContextValue;

        private async void Start()
        {
            intText = GetComponent<TextMeshProUGUI>();
            intContextValue = await intContextResource.LoadAssetAsync<IntContextValue>().Task;
            intContextValue.
                Subscribe(x => intText.text = x.ToString()).
                AddTo(this);
        }
    }
}
