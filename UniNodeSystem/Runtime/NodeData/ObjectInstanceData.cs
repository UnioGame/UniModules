using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ObjectInstanceData
{

    public Transform Parent;
    public Vector3 Position;
    public bool StayAtWorld;
    public bool Immortal;

}
