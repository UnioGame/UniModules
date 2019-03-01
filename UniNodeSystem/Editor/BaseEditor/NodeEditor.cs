using System;
using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.Drawers;
using UniNodeSystem;
using UnityEditor;
using UnityEngine;

namespace UniNodeSystemEditor
{
    /// <summary> Base class to derive custom Node editors from. Use this to create your own custom inspectors and editors for your nodes. </summary>
    [CustomNodeEditor(typeof(UniBaseNode))]
    public class NodeEditor : NodeEditorBase<NodeEditor, CustomNodeEditorAttribute, UniBaseNode>, INodeEditor
    {

        public static Dictionary<NodePort, Vector2> PortPositions = new Dictionary<NodePort, Vector2>();

        /// <summary> Fires every whenever a node was modified through the editor </summary>
        public static Action<UniBaseNode> OnUpdateNode;
        public static int Renaming;


        protected List<INodeEditorDrawer> _bodyDrawers = new List<INodeEditorDrawer>();
        
        protected List<INodeEditorDrawer> _headerDrawers = new List<INodeEditorDrawer>();

        public UniBaseNode Target => target;

        public SerializedObject SerializedObject => serializedObject;

        public sealed override void OnEnable()
        {
            _bodyDrawers = new List<INodeEditorDrawer>();
            _headerDrawers = new List<INodeEditorDrawer>();
            
            InitializedBodyDrawers();
            InitializeHeaderDrawers();
            
            OnEditorEnabled();
            
        }

        public virtual bool IsSelected()
        {
            return Selection.Contains(target);
        }
        
        public virtual void OnHeaderGUI()
        {
            Draw(_headerDrawers);
        }

        /// <summary> Draws standard field editors for all public fields </summary>
        public virtual void OnBodyGUI()
        {
            PortPositions = new Dictionary<NodePort, Vector2>();
            
            serializedObject.Update();
            
            Draw(_bodyDrawers);

            serializedObject.ApplyModifiedProperties();
        }

        public virtual int GetWidth()
        {
            var type = target.GetType();
            int width;
            if (NodeEditorWindow.nodeWidth.TryGetValue(type, out width)) return width;
            return 208;
        }

        public virtual Color GetTint()
        {
            var type = target.GetType();
            return NodeEditorWindow.nodeTint.TryGetValue(type, out var color) ? 
                color : Color.white;
        }

        public virtual GUIStyle GetBodyStyle()
        {
            return NodeEditorResources.styles.nodeBody;
        }

        public void InitiateRename()
        {
            Renaming = 1;
        }

        public void Rename(string newName)
        {
            target.name = newName;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(target));
        }
        
        #region private methods

        protected virtual void InitializeHeaderDrawers()
        {
            _headerDrawers.Add(new BaseHeaderDrawer());
        }

        protected virtual void InitializedBodyDrawers()
        {
            _bodyDrawers.Add(new BaseBodyDrawer());
        }

        protected virtual void OnEditorEnabled(){}

        private void Draw(List<INodeEditorDrawer> drawers)
        {
            for (var i = 0; i < drawers.Count; i++)
            {
                var drawer = drawers[i];
                drawer.Draw(this, target);
            }
        }
        
        #endregion
        
    }
}