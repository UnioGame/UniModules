using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.UnityTools.Physics
{
    public interface ISensorObject
    {
        IReadOnlyDictionary<Transform, Collision> CollisionData { get; }
        IReadOnlyDictionary<Transform, Collider> TriggersData { get; }
        
        LayerMask CollisionMask { get; }
        LayerMask RaycastMask { get; }
        
        Collider Collider { get; }
        Collider LastTriggerObject { get;  }
        Collision LastCollisionObject { get;}
        
        Vector3 Position { get; }
        Transform Transform { get; }
        
        void SetCollisionMask(int mask);
        void SetCollisionMask(string[] mask);

        void Reset();
    }
}