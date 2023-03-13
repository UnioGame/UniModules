using System;
using Leopotam.EcsLite;

namespace UniGame.LeoEcs.Shared.Components
{
    /// <summary>
    /// link to owner entity, but don't destroy on owner destroy
    /// </summary>
    [Serializable]
    public struct OwnerLinkComponent
    {
        public EcsPackedEntity Entity;
    }
    
    [Serializable]
    public struct LinkComponent<T>
    {
        public EcsPackedEntity Entity;
    }
}