namespace UniGame.UniNodes.NodeSystem.Inspector.Editor.ContentContextWindow
{
    using System.Linq;
    using Core.EditorTools.Editor.UiElements;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces.Rx;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ContextContentWindow : EditorWindow
    {
        private const string stylePath      = "";
        private const string uiViewPath     = "";
        private const string uiIconViewPath = "";
        private const string refreshLabel   = "refresh";

        private ITypeData            containerValue;
        private string               portName;


        private ScrollView scrollView;
        private Button     refreshButton;

        public static void Open(ITypeData data)
        {
            var window = GetWindow<ContextContentWindow>();
            window.Initialize(data);
            window.minSize      = new Vector2(400, 200);
            window.titleContent = new GUIContent("Context Data");
            window.Show();
        }

        public void Initialize(ITypeData contextData)
        {
            this.containerValue = contextData;
            Refresh();
        }

        public void Refresh()
        {
            CreateContent(scrollView, containerValue);
        }

        public void OnEnable()
        {
            rootVisualElement.style.flexDirection = FlexDirection.Column;

            refreshButton = new Button(Refresh) {
                text = refreshLabel
            };
            rootVisualElement.Add(refreshButton);

            scrollView = new ScrollView() {
                style = {
                    marginTop = 20,
                    backgroundColor = new StyleColor(new Color(0.5f,0.5f,0.5f)),
                },
                showVertical = true,
            };
            rootVisualElement.Add(scrollView);
        }


        public void CreateContent(VisualElement container, ITypeData data)
        {
            container.Clear();

            if (data == null) return;

            foreach (var pair in data.EditorValues) {
                var valueContainer = pair.Value;
                var objectValue    = valueContainer as IObjectValue;
                var type           = pair.Key;

                if (valueContainer.HasValue == false || objectValue == null)
                    continue;

                var value     = objectValue.GetValue();
                var valueType = value?.GetType();

                var valueTypeLabel = valueType?.Name;
                var genericTypes = valueType?.GenericTypeArguments.Select(x => x.Name).ToArray();
                var genericLabel = genericTypes == null || genericTypes.Length == 0 ? string.Empty : string.Join(",", genericTypes);
                var foldout = new Foldout() {
                    text = $"[Registered Type :{type.Name}] : [Value Type :{valueTypeLabel} {genericLabel}]",
                };

                foldout.style.backgroundColor = new StyleColor(new Color(0.8f, 0.8f, 0.8f));
                foldout.style.marginBottom   = 4;
                foldout.style.borderTopColor = new StyleColor(Color.black);
                foldout.style.borderTopWidth = 1;
                
                container.Add(foldout);

                var element = UiElementFactory.Create(value);
                //is empty value or value already shown
                if (element == null)
                    continue;

                foldout.Add(element);
            }
        }
    }
}