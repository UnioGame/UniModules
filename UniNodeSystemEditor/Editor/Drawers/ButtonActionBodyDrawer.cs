using System;
using System.Collections.Generic;
using UniEditorTools;
using UniNodeSystem;
using UniNodeSystemEditor;
using UnityEditor;

namespace Modules.UniTools.UniNodeSystem.Drawers
{
    public class ButtonActionBodyDrawer : INodeEditorDrawer
    {
        private readonly string _label;
        private readonly Action _action;

        public ButtonActionBodyDrawer(string label,Action action)
        {
            _label = label;
            _action = action;
        }
    
        public bool Draw(INodeEditor editor, UniBaseNode node)
        {
           
            EditorDrawerUtils.DrawButton(_label,_action);

            return true;
        }
    
    }
}
