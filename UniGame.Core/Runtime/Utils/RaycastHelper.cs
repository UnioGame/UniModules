namespace Helpers
{
    using UnityEngine;

    public static class RaycastHelper
    {
        public static T Raycast<T>(Camera cameraFromRaycast, LayerMask mask, float distance, Vector3 mousePosition) where T : class
        {
            if (cameraFromRaycast == null)
                return default;
            
            var ray = cameraFromRaycast.ScreenPointToRay(mousePosition);
            return Raycast<T>(ray, distance, mask);
        }

        private static T Raycast<T>(Ray ray, float distance, LayerMask mask) where T : class
        {
            var hitResult = Physics2D.GetRayIntersection(ray, distance, mask);

            if (hitResult.transform != null) {
                return hitResult.transform.GetComponent<T>();
            }

            return null;
        }
    }
}
