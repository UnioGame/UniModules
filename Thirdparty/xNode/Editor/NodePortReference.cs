using UnityEngine;

namespace XNodeEditor
{
    public partial class NodeEditorWindow
    {
        [System.Serializable]
        public class NodePortReference
        {
            [SerializeField] private XNode.Node _node;
            [SerializeField] private string _name;

            public NodePortReference(XNode.NodePort nodePort)
            {
                _node = nodePort.node;
                _name = nodePort.fieldName;
            }

            public XNode.NodePort GetNodePort()
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