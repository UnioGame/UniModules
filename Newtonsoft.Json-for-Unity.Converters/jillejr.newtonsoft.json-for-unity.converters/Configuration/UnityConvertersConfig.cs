using System;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Newtonsoft.Json.UnityConverters.Configuration
{

#pragma warning disable CA2235 // Mark all non-serializable fields
    [Serializable]
    [CreateAssetMenu(menuName = "Newtonsoft.Json-for-Unity/Converters Config", fileName = "Newtonsoft-Json-for-Unity-Converters")]
    public sealed class UnityConvertersConfig : ScriptableObject
    {
        internal const string PATH = "Assets/Resources/Newtonsoft-Json-for-Unity-Converters.asset";
        internal const string PATH_FOR_RESOURCES_LOAD = "Newtonsoft-Json-for-Unity-Converters";

        public bool useBakedConverters = true;

        public bool useUnityContractResolver = true;

        public bool useAllOutsideConverters = true;

        public List<ConverterConfig> outsideConverters = new();

        public bool useAllUnityConverters = true;

        public List<ConverterConfig> unityConverters = new();

        public bool useAllJsonNetConverters;

        public List<ConverterConfig> jsonNetConverters = new()
        {
            new ConverterConfig { converterName = typeof(StringEnumConverter).FullName, enabled = true },
            new ConverterConfig { converterName = typeof(VersionConverter).FullName, enabled = true },
        };
        
    }
#pragma warning restore CA2235 // Mark all non-serializable fields
}
