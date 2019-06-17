namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Nodes
{
    using System.Collections.Generic;
    using BaseEditor;
    using Drawers;
    using Runtime;
    using UniEditorTools;
    using UnityEngine;

    [CustomNodeEditor(typeof(UniNode))]
    public class UniNodeEditor : NodeEditor, IUniNodeEditor
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
            var node = target as UniNode;
            if (!node)
                return false;
            return node.IsActive;
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
            var node = target as UniNode;

            node.Initialize();

            UpdateData(node);

            base.OnBodyGUI();

            DrawPorts(node);

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public virtual void UpdateData(UniNode node)
        {
            node.UpdatePortsCache();
        }

        public void DrawPorts(UniNode node)
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