using UnityEngine;

namespace Modules.UnityToolsModule.Tools.UnityTools.Physics
{

    public static class PhysicsRaycasts 
    {

        public static RaycastHit RaycastAtPoint(this Vector3 position, Vector3 direction, float distance, int layerMask)
        {
            UnityEngine.Physics.Raycast(position, direction, out var hit, distance, layerMask);
            return hit;
        }

    }
}
