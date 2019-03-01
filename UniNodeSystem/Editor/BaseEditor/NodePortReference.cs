using UnityEngine;

namespace UniNodeSystemEditor
{
    public partial class NodeEditorWindow
    {
        [System.Serializable]
        public class NodePortReference
        {
            [SerializeField] private UniNodeSystem.UniBaseNode _node;
            [SerializeField] private string _name;

            public NodePortReference(UniNodeSystem.NodePort nodePort)
            {
                _node = nodePort.node;
                _name = nodePort.fieldName;
            }

            public UniNodeSystem.NodePort GetNodePort()
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