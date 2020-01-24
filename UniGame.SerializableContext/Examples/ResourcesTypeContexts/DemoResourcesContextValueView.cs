namespace UniGreenModules.UniGame.SerializableContext.Examples.ResourcesTypeContexts
{
    using Runtime;
    using TMPro;
    using UniRx;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DemoResourcesContextValueView : MonoBehaviour
    {
#region inspector

        public string resourcePath;

        public TextMeshProUGUI intText;

#endregion
        
        private IntContextValue intContextValue;

        private void Start()
        {
            intText = GetComponent<TextMeshProUGUI>();
            intContextValue = Resources.Load<IntContextValue>(resourcePath);
            intContextValue.
                Subscribe(x => intText.text = x.ToString()).
                AddTo(this);
        }
    }
}
