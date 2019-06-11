using Node = UnityEditor.Graphs.Node;

namespace Modules.UniTools.UniNodeSystem.Editor.UnityGraph
{
    using UniGreenModules.UniNodeSystem.Runtime.Runtime;

    public class UnityGraphUniNode : Node
    {
    
        #region factory methods

        public static Node Create(UniBaseNode graphNode)
        {
            var node = CreateInstance<UnityGraphUniNode>();
            node.Initialize(graphNode);
            return node;
        }
        
        #endregion

        private UniBaseNode _node;
        
        public void Initialize(UniBaseNode graphNode)
        {
            _node = graphNode;
        }
        
    }
}
