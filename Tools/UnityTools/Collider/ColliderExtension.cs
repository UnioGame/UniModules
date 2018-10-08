using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderExtension
{

    public static (Vector3 sourcePoint, Vector3 targetPoint) GetCollisionPoints(this Collider source, Collider target, Vector3 point)
    {
        var colliderPoint = target.ClosestPointOnBounds(point);
        var positionPoint = source.ClosestPointOnBounds(colliderPoint);
        return (positionPoint,colliderPoint);
    }
	
}
