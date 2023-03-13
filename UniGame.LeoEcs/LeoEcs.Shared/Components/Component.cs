using System;
using Leopotam.EcsLite;

namespace UniGame.LeoEcs.Shared.Components
{
    /// <summary>
    /// request component that will be deleted after "counter" cycles
    /// </summary>
    [Serializable]
    public struct Component<T> : IEcsAutoReset<Component<T>>
    {
        public int counter;
        
        public void AutoReset(ref Component<T> c)
        {
            c.counter = 0;
        }
    }
}