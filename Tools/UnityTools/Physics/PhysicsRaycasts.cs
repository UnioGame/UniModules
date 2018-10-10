using UnityEngine;

namespace Modules.UnityToolsModule.Tools.UnityTools.Physics
{
    public struct PointRayData
    {
        public RaycastHit Hit;
        public bool Valid;
        public Vector3 Position;
    }
    
    public static class PhysicsRaycasts 
    {
        
        public static PointRayData MakePointRaycast(Vector3 position,Vector3 direction,float distance,int layerMask)
        {
		
            if (!UnityEngine.Physics.Raycast(position, direction, out var hit, distance ,layerMask))
                return new PointRayData(){ Valid =  false};
		
            return new PointRayData()
            {
                Valid = true,
                Position = position,
                Hit = hit,
            };
            
        }

        public static PointRayData RaycastAtPoint(this Vector3 position, Vector3 direction, float distance, int layerMask)
        {
            return MakePointRaycast(position, direction, distance, layerMask);
        }

    }
}
