namespace UniGreenModules.UniCore.Runtime.Physics
{
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    public interface ISensorObject
    {
        IReadOnlyCollection<Collision> CollisionData { get; }
        IReadOnlyCollection<UnityEngine.Collider> TriggersData { get; }
        
        LayerMask CollisionMask { get; }
        
        IReadOnlyReactiveProperty<bool> TriggerConnectionChanged { get;  }
        IReadOnlyReactiveProperty<bool> CollisionConnectionChanged { get;  }
        
        UnityEngine.Collider Collider { get; }
        UnityEngine.Collider LastTriggerObject { get;  }
        Collision LastCollisionObject { get;}
        
        Vector3 Position { get; }
        Transform Transform { get; }
        
        void SetCollisionMask(int mask);
        void SetCollisionMask(string[] mask);

        void Reset();
    }
}