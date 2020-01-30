using System;
using System.Collections.Generic;
using UnityEngine;

namespace PostprocessCollection
{
    
    [Serializable]
    public class iOSFrameworkDescription
    {
        [SerializeField] public string Name;
        [SerializeField] public bool IsWeak;
    }

    [Serializable]
    public class BuildProperties
    {
        [SerializeField] public string Name;
        [SerializeField] public string Value;
    }

    [Serializable]
    public class PlistStringKey
    {
        [SerializeField] public string Name;
        [SerializeField] public string Value;
    }

    [Serializable]
    public class PlistBoolKey
    {
        [SerializeField] public string Name;
        [SerializeField] public bool Value;  
    }
    
    [Serializable]
    public class PlistIntKey
    {
        [SerializeField] public string Name;
        [SerializeField] public int Value;  
    }
    
    [Serializable]
    public class PlistFloatKey
    {
        [SerializeField] public string Name;
        [SerializeField] public float Value;  
    }

    [Serializable]
    public class PlistKeys
    {
        [SerializeField] public List<PlistStringKey> StringKeys;
        [SerializeField] public List<PlistIntKey> IntKeys;
        [SerializeField] public List<PlistBoolKey> BoolKeys;
        [SerializeField] public List<PlistFloatKey> FloatKeys;
    }
    
}