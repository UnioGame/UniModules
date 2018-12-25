using System;
using UnityEngine;

namespace Assets.Tools.UnityTools.Animator
{
    [Serializable]
    public class AnimatorParameter
    {

        public AnimatorControllerParameterType ParameterType;
        public string Name;

        public float FloatValue;
        public int IntValue;
        public bool BoolValue;

  
    }
}

