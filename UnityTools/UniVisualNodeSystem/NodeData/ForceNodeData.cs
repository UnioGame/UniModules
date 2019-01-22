using System;
using UnityEngine;

namespace UnityTools.UniVisualNodeSystem.NodeData
{
    [Serializable]
    public class ForceNodeData
    {
        public ForceMode ForceMode;
    
        public Vector3 Direction;
        
        public float MaxForce;

        public float MinForce;

        public float LerpTime = 1f;

    }
}
