using System;

namespace XNodeEditor
{
    public partial class NodeEditor
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class CustomNodeEditorAttribute : Attribute,
            XNodeEditor.Internal.NodeEditorBase<NodeEditor, NodeEditor.CustomNodeEditorAttribute, XNode.Node>.INodeEditorAttrib {
            private Type inspectedType;
            /// <summary> Tells a NodeEditor which Node type it is an editor for </summary>
            /// <param name="inspectedType">Type that this editor can edit</param>
            /// <param name="contextMenuName">Path to the node</param>
            public CustomNodeEditorAttribute(Type inspectedType) {
                this.inspectedType = inspectedType;
            }

            public Type GetInspectedType() {
                return inspectedType;
            }
        }
    }
}