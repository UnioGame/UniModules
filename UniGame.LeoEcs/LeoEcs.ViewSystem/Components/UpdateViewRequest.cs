using System;
using Leopotam.EcsLite;

namespace UniGame.LeoEcs.ViewSystem.Components
{
    [Serializable]
    public struct UpdateViewRequest : IEcsAutoReset<UpdateViewRequest>
    {
        public int counter;
        
        public void AutoReset(ref UpdateViewRequest c)
        {
            c.counter = 0;
        }
    }
    
    [Serializable]
    public struct UpdateViewRequest<T> {}
}