using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.Drawers;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Editor.Nodes
{
    public class UniAssetNodeEditor<TTarget> : UniDrawersNodeEditor
        where TTarget : Object
    {

        public override List<INodeEditorDrawer> GetDrawers()
        {
            return new List<INodeEditorDrawer>()
            {
                new AssetNodeDrawer<GameObject>(),
            };
        }

        
        
    }
    
}
