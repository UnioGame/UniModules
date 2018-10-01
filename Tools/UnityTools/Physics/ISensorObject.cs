using System.Collections.Generic;
using UnityEngine;

public interface ISensorObject
{
    IReadOnlyDictionary<Transform, Collision> CollisionData { get; }
    IReadOnlyDictionary<Transform, Collider> TriggersData { get; }
    void SetCollisionMask(int mask);
    void SetCollisionMask(string[] mask);

    void Reset();
}