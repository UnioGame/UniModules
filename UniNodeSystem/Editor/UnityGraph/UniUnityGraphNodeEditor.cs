using UnityEditor.Graphs;

namespace Modules.UniTools.UniNodeSystem.Editor.UnityGraph
{
    public class UniUnityGraphNodeEditor : Node
    {
    
        #region factory methods

        public static Node Create(XNode.Node graphNode)
        {
            var node = CreateInstance<UniUnityGraphNodeEditor>();
            node.Initialize(graphNode);
            return node;
        }
        
        #endregion

        private XNode.Node _node;
        
        public void Initialize(XNode.Node graphNode)
        {
            _node = graphNode;
        }
        
    }
}
