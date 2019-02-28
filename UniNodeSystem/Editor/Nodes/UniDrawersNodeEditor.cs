using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.Drawers;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniStateMachine;

namespace Modules.UniTools.UniNodeSystem.Editor.Nodes
{
    public class UniDrawersNodeEditor : UniNodeEditor
    {
        protected List<INodeEditorDrawer> _drawers = new List<INodeEditorDrawer>();


        public override void OnEnable()
        {
            base.OnEnable();
            _drawers = GetDrawers();
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            for (int i = 0; i < _drawers.Count; i++)
            {
                var drawer = _drawers[i];
                drawer.DrawNodeBody(target as UniNode);
            }
        }

        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
            for (int i = 0; i < _drawers.Count; i++)
            {
                var drawer = _drawers[i];
                drawer.DrawNodeHeader(target as UniNode);
            }
        }

        public virtual List<INodeEditorDrawer> GetDrawers()
        {
            return new List<INodeEditorDrawer>();
        }
        
    }
}
