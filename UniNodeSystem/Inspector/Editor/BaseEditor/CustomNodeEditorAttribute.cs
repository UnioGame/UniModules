namespace UniGreenModules.UniNodeSystem.Inspector.Editor.BaseEditor
{
    using System;
    using Interfaces;

    [AttributeUsage(AttributeTargets.Class)]
    public class CustomNodeEditorAttribute : Attribute, INodeEditorAttribute
    {
        
        private Type inspectedType;

        /// <summary> Tells a NodeEditor which Node type it is an editor for </summary>
        /// <param name="inspectedType">Type that this editor can edit</param>
        /// <param name="contextMenuName">Path to the node</param>
        public CustomNodeEditorAttribute(Type inspectedType)
        {
            this.inspectedType = inspectedType;
        }

        public Type GetInspectedType()
        {
            return inspectedType;
        }
        
    }
}