using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
using UnityEngine;

namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    [CreateAssetMenu(menuName = "UniGame/LeoEcs/Systems Update Map",
        fileName = "Systems Update Map")]
    public class LeoEcsUpdateMapAsset : ScriptableObject
    {
        [InlineProperty]
        public List<LeoEcsUpdateQueue> updateQueue = new List<LeoEcsUpdateQueue>();

        [InlineProperty]
        [SerializeReference]
        public List<ILeoEcsSystemsPluginProvider> systemsPlugins = new List<ILeoEcsSystemsPluginProvider>();
        
        [SerializeReference]
        public ILeoEcsUpdateOrderProvider defaultFactory;

    }
}
