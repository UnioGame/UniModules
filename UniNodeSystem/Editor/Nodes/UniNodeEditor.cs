using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Drawers;
using Modules.UniTools.UniNodeSystem.Editor.BaseEditor;
using UniEditorTools;
using UniStateMachine;
using UniStateMachine.NodeEditor;
using UnityEngine;
using UniNodeSystem;
using UniNodeSystemEditor;

namespace SubModules.Scripts.UniStateMachine.NodeEditor
{
    [CustomNodeEditor(typeof(UniGraphNode))]
    public class UniNodeEditor : UniNodeSystemEditor.NodeEditor, IUniNodeEditor
    {

        #region static data
        
        private static GUIStyle SelectedHeaderStyle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
        };

        #endregion

        protected List<INodeEditorDrawer> _portsDrawer = new List<INodeEditorDrawer>();
        
        public override bool IsSelected()
        {
            var node = target as UniGraphNode;
            if (!node)
                return false;
            return node.IsAnyActive;
        }


        public override void OnHeaderGUI()
        {
            if (IsSelected())
            {
                EditorDrawerUtils.DrawWithContentColor(Color.red, base.OnHeaderGUI);
                return;
            }
            base.OnHeaderGUI();
        }

        public override void OnBodyGUI()
        {
            var node = target as UniGraphNode;

            node.Initialize();

            UpdateData(node);

            base.OnBodyGUI();

            DrawPorts(node);

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public virtual void UpdateData(UniGraphNode node)
        {
            node.UpdatePortsCache();
        }

        public void DrawPorts(UniGraphNode node)
        {
            Draw(_portsDrawer);
        }

        protected override void OnEditorEnabled()
        {
            base.OnEditorEnabled();
            _portsDrawer = InitializePortDrawers();
        }

        protected virtual List<INodeEditorDrawer> InitializePortDrawers()
        {
            _portsDrawer.Add(new UniNodeBasePortsDrawer());
            return _portsDrawer;
        }


    }
}