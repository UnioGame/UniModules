using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsBallisticsMotion {

    [Serializable]
    public struct BallisticsMotionData {
        
        public Vector3 InitialPosition;
        public Vector3 Force;
        public ForceMode ForceMode;
        public float Time;
        
    }

}



