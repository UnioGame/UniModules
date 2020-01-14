using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniGreenModules.UniNodeSystem.Inspector.Editor.NodesSelectorWindow
{
    public class UniGraphNodesWindow : EditorWindow
    {
        private const string windowClassName = "unigame-nodes-window";
        
        [MenuItem("UniGame/UniGraph/NodesWindow")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<UniGraphNodesWindow>();
            wnd.titleContent = new GUIContent("NodesWindow");
        }

        public void OnEnable()
        {
            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = Resources.Load<StyleSheet>("UniGraphNodesWindowStyle");
            
            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;

            root.AddToClassList(windowClassName);
            root.styleSheets.Add(styleSheet);
            
            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);

            // Import UXML
            var           visualTree    = Resources.Load<VisualTreeAsset>("UniGraphNodesWindow");
            VisualElement labelFromUXML = visualTree.CloneTree();
            root.Add(labelFromUXML);

            VisualElement labelWithStyle = new Label("Hello World! With Style");
            labelWithStyle.AddToClassList("unigame-nodes-label");
            labelWithStyle.styleSheets.Add(styleSheet);
            root.Add(labelWithStyle);
        }
    }
}