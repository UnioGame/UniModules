using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Editor.UnityGraph
{
    using UniGreenModules.UniNodeSystem.Runtime;

    public class UnityGraphWindow : EditorWindow
    {
        private const float _barHeight = 17;
        
        private UniGraphEditor _graphEditor;
        private GraphGUI _graphGuiEditor;
        
        public static void Show(UniGraph graph)
        {
            var window = GetWindow<UnityGraphWindow>(graph.name);
            window.Initialize(graph);
            window.Show();
        }

        public void Initialize(UniGraph graph)
        {
            _graphEditor = UniGraphEditor.Create(graph);
            _graphGuiEditor = _graphEditor.GetEditor();
        }
        
        #region private methods

        private void OnGUI()
        {
            if (_graphGuiEditor == null)
                return;
            
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
