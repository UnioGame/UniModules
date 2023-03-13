using System;
using System.Threading;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UniGame.Core.Runtime.SerializableType;
using UniGame.Core.Runtime.SerializableType.Attributes;
using UniGame.LeoEcs.Converter.Runtime;
using UniGame.LeoEcs.Shared.Extensions;
using UniGame.LeoEcs.ViewSystem.Components;
using UniGame.LeoEcs.ViewSystem.Extensions;
using UniModules.UniGame.UiSystem.Runtime;
using UnityEngine;

namespace UniGame.LeoEcs.ViewSystem.Converters
{
    [Serializable]
    public class CreateEcsViewConverter : LeoEcsConverter
    {
        
#if ODIN_INSPECTOR
        [DrawWithUnity]
#endif
        [STypeFilter(typeof(IEcsView))]
        public SType viewType;

        public ViewType layoutType = ViewType.Screen;

        public SkinId skinTag;
        
        public override void Apply(GameObject target, 
            EcsWorld world, int entity, 
            CancellationToken cancellationToken = default)
        {
            world.MakeViewRequest(viewType, layoutType,null,skinTag);
        }
    }
}
