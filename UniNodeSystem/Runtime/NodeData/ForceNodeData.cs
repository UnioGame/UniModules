namespace UniGreenModules.UniNodeSystem.Runtime.NodeData
{
    using System;
    using UnityEngine;

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
