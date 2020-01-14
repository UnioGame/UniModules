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

            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;

            root.AddToClassList(windowClassName);
            
            var           visualTree    = Resources.Load<VisualTreeAsset>("UniGraphNodesWindow");
            VisualElement labelFromUXML = visualTree.CloneTree();
            root.Add(labelFromUXML);

        }
    }
}