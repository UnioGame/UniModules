namespace UniModules.UniGame.Context.Editor.ContextEditorWindow
{
    using System.Collections.Generic;
    using UiElements.Editor.TypeDrawers;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ContextContentWindow : EditorWindow
    {
        private const string stylePath      = "";
        private const string uiViewPath     = "";
        private const string uiIconViewPath = "";
        private const string refreshLabel   = "refresh";

        private Color                    defaultBackgroundColor = new Color(0.7f, 0.7f, 0.7f);
        private List<ContextDescription> values;
        private List<VisualElement>      views = new List<VisualElement>();
        private string                   portName;

        private ScrollView scrollView;
        private Button     refreshButton;

        public static void Open(ContextDescription source)
        {
            Open(new List<ContextDescription>() {
                source
            });
        }

        public static void Open(List<ContextDescription> sources)
        {
            var window = GetWindow<ContextContentWindow>();
            window.Initialize(sources);
            window.minSize      = new Vector2(400, 200);
            window.titleContent = new GUIContent("Context Data");
            window.Show();
        }

        public void Initialize(List<ContextDescription> sources)
        {
            this.values = sources;
            Refresh();
        }

        public void Refresh()
        {
            CreateContent(scrollView, values);
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
                    marginTop       = 20,
                    backgroundColor = new StyleColor(defaultBackgroundColor),
                },
                showVertical = true,
            };
            rootVisualElement.Add(scrollView);
        }


        public void CreateContent(VisualElement container, List<ContextDescription> source)
        {
            container.Clear();
            views.Clear();

            if (source == null) return;

            for (var i = 0; i < source.Count; i++) {
                var item = source[i];
                if (item.Data == null)
                    continue;

                var data  = item.Data;
                var title = string.IsNullOrEmpty(item.Label) ? $"Item {1}" : item.Label;

                var foldout = new Foldout() {
                    text = $"[{i}] {title} : {data.GetType()}",
                    style = {
                        borderBottomColor = new StyleColor(Color.black),
                        borderBottomWidth = 4,
                        marginBottom      = 4
                    }
                };

                var element = UiElementFactory.Create(data);
                //is empty value or value already shown
                if (element == null)
                    continue;

                foldout.Add(element);
                container.Add(foldout);
            }
        }
    }
}