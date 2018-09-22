using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomDefineManagement
{
    [Serializable]
    public class Directive
    {
        [SerializeField]
        public string name;
        
        [SerializeField]
        public CustomDefineManager.cdmBuildTargetGroup targets;

        [SerializeField]
        public bool enabled = true;
        
        [SerializeField]
        public int sortOrder = 0;

        public override string ToString()
        {
            return string.Format("{0} : {1}", name, targets.ToString());
        }
    }
}