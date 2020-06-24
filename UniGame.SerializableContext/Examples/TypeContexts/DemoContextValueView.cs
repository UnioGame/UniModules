using UnityEngine;

namespace UniGreenModules.UniGame.SerializableContext.Examples.TypeContexts
{
    using Runtime;
    using TMPro;
    using UniModules.UniGame.SerializableContext.Runtime.AssetTypes;
    using UniRx;

    public class DemoContextValueView : MonoBehaviour
    {
        public IntContextValue intContextValue;

        public TextMeshProUGUI intText;
        
        private void Start()
        {
            intContextValue.
                Subscribe(x => intText.text = x.ToString()).
                AddTo(this);
        }
    }
}
