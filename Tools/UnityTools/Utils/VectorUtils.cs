using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.UnityTools.Utils
{
    public static class VectorUtils  {


        public static Vector3 NearestPosition(this List<GameObject> objects, Vector3 position)
        {
            if(objects == null || objects.Count == 0)
                return Vector3.zero;

            var selectedPosition = objects[0].transform.position;
            var minDistance = Vector3.Distance(position, selectedPosition);

            for (int i = 1; i < objects.Count; i++)
            {
                var objectPosition = objects[i].transform.position;
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
