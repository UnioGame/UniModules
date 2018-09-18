using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension  {

    public static bool IsLeft(this Vector3 a, Vector3 b)
    {

        return -a.x * b.y + a.y * b.x < 0;

    }

    public static Vector3 Reflect(this Vector3 point, Vector3 source, Vector3 reflectionLine) {

        var dot = Vector3.Dot(point, reflectionLine);
        var projection = reflectionLine * dot;
        var projectionPoint = projection - point;
        return point + 2 * projectionPoint;

    }
}
