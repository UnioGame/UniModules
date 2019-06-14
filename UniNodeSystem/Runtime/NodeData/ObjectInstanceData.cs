using System;
using UnityEngine;

namespace UniGreenModules.UniNodeSystem.Runtime.NodeData
{
    [Serializable]
    public struct ObjectInstanceData
    {
        public Transform Parent;
        public Vector3   Position;
        public bool      StayAtWorld;
        public bool      Immortal;
        public bool      SharedInstance;
    }
}