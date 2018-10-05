using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.UnityTools.Utils
{
    public static class VectorUtils  {


        public static Vector3 NearestPosition(this IEnumerable<GameObject> objects, Vector3 position)
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

            return selectedPosition;
        }
    
    }
}
