using UniStateMachine.Nodes;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Editor.UnityGraph
{
    public class UnityGraphWindow : EditorWindow
    {
        private const float _barHeight = 17;
        
        private UniGraphEditor _graphEditor;
        private GraphGUI _graphGuiEditor;
        
        public static void Show(UniNodesGraph graph)
        {
            var window = EditorWindow.GetWindow<UnityGraphWindow>("UniGraph");
            window.Initialize(graph);
            window.Show();
        }


        public void Initialize(UniNodesGraph graph)
        {
            _graphEditor = UniGraphEditor.Create(graph);
            _graphGuiEditor = _graphEditor.GetEditor();
        }
        
        #region private methods

        private void OnGUI()
        {
            var width = position.width;
            var height = position.height;
            // Main graph area
            _graphGuiEditor.BeginGraphGUI(this, new Rect(0, 0, width, height - _barHeight));
            _graphGuiEditor.OnGraphGUI();
            _graphGuiEditor.EndGraphGUI();
            
        }

        #endregion
    
    }
}
