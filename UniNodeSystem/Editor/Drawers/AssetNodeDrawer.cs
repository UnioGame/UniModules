using UniStateMachine;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Editor.Drawers
{
    public class AssetNodeDrawer<TValue> : NodeEditorDrawer
        where TValue : Object
    {
        
        public override UniNode DrawNodeHeader(UniNode target)
        {
            var node = target as AssetNode<TValue>;
            if (node == null)
                return target;
            
            var targetAsset = node.Target;
            if (node.Target)
            {
                target.name = targetAsset.name;
            }
            return base.DrawNodeHeader(node);
            
        }
    }
}
