namespace UniGreenModules.UniGame.SerializableContext.Examples.ResourcesTypeContexts
{
    using Runtime;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class DemoResourcesContextIntSetter : MonoBehaviour
    {
        #region inspector

        public string resourcePath;

        public Button button;

        #endregion
        
        private IntContextValue intValue;
        
        // Start is called before the first frame update
        private void Start()
        {
            button = GetComponent<Button>();
            intValue = Resources.Load<IntContextValue>(resourcePath);
            
            button.onClick.
                AsObservable().
                Subscribe(x => intValue.SetValue(intValue.Value+1)).
                AddTo(this);
        }

    }
}
