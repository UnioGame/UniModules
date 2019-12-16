namespace UniGreenModules.UniNodeSystem.Inspector.Editor.BaseEditor
{
    using Runtime.Core;
    using UnityEngine;

    public partial class NodeEditorWindow
    {
        [System.Serializable]
        public class NodePortReference
        {
            [SerializeField] private UniBaseNode _node;
            [SerializeField] private string _name;

            public NodePortReference(NodePort nodePort)
            {
                _node = nodePort.node;
                _name = nodePort.fieldName;
            }

            public NodePort GetNodePort()
            {
                if (_node == null)
                {
                    return null;
                }

                return _node.GetPort(_name);
            }
        }
    }
}