using System.Collections.Generic;
using UnityEngine;

namespace UniModule.UnityTools.Utils
{
    public static class VectorUtils  {


        public static Vector3 NearestPosition(this IEnumerable<GameObject> objects, Vector3 position,Vector3 defaultPosition)
        {
            if(objects == null) return Vector3.zero;

            var selectedPosition =Vector3.zero;
            var minDistance = 0f; 

            var first = true;
            
            foreach (var item in objects) {

                if (first) {
                    first = false;
                    selectedPosition = item.transform.position;
                    minDistance = Vector3.Distance(position, selectedPosition);
                    continue;
                }
                
                var objectPosition = item.transform.position;
                var distance = Vector3.Distance(position, objectPosition);
                if (distance < minDistance) {
                    selectedPosition = objectPosition;
                    minDistance = distance;
                }
            }

            return first ? defaultPosition : selectedPosition;
        }

        public static float Distance(this Component source, Component target)
        {
            return Vector3.Distance(source.transform.position, target.transform.position);
        }
        
        public static float Distance(this Component source, Vector3 target)
        {
            return Vector3.Distance(source.transform.position, target);
        }
    }
}
