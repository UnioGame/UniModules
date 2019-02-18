using UniStateMachine.Nodes;
using UnityEngine;

namespace UniTools.UniNodeSystem
{
    [CreateAssetMenu(menuName = "UniGraph/GraphAsset", fileName = "GraphAsset")]
    public class UniGraphAsset : ScriptableObject
    {

        public UniNodesGraph Graph;

    }
}
