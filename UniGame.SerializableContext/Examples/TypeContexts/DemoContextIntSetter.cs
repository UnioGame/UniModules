using UnityEngine;

namespace UniGreenModules.UniGame.SerializableContext.Examples.TypeContexts
{
    using Runtime;
    using UniModules.UniGame.SerializableContext.Runtime.AssetTypes;
    using UniRx;
    using UnityEngine.UI;

    public class DemoContextIntSetter : MonoBehaviour
    {

        public Button button;
        
        public IntContextValue intValue;
        
        // Start is called before the first frame update
        private void Start()
        {
            button.onClick.
                AsObservable().
                Subscribe(x => intValue.SetValue(intValue.Value+1)).
                AddTo(this);
        }

    }
}
