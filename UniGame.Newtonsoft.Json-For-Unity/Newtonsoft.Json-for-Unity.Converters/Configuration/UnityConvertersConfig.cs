using System;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Newtonsoft.Json.UnityConverters.Configuration
{
    using System.Linq;

#if UNITY_EDITOR
    using UnityEditor;
#endif

#pragma warning disable CA2235 // Mark all non-serializable fields
    [Serializable]
    public sealed class UnityConvertersConfig : ScriptableObject
    {
        internal const string PATH = "Assets/Resources/Newtonsoft.Json-for-Unity.Converters.asset";
        internal const string PATH_FOR_RESOURCES_LOAD = "Newtonsoft.Json-for-Unity.Converters";

        public bool useBakedConverters = true;
        
        public bool useUnityContractResolver = true;

        public bool useAllOutsideConverters = true;

        public List<ConverterConfig> outsideConverters = new List<ConverterConfig>();

        public bool useAllUnityConverters = true;

        public List<ConverterConfig> unityConverters = new List<ConverterConfig>();

        public bool useAllJsonNetConverters;

        public List<ConverterConfig> jsonNetConverters = new List<ConverterConfig> {
            new ConverterConfig { converterName = typeof(StringEnumConverter).FullName, enabled = true },
            new ConverterConfig { converterName = typeof(VersionConverter).FullName, enabled = true },
        };

        public Type[] unityConvertersTypes = Array.Empty<Type>();
        public Type[] customConvertersTypes = Array.Empty<Type>();
        public Type[] jsonNetConvertersTypes = Array.Empty<Type>();

        public void BakeTypes()
        {
            UnityConverterInitializer.BakeConverters(this);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
#endif
        }
        
        public IEnumerable<Type> GetCustomConvertersTypes()
        {
            customConvertersTypes = customConvertersTypes == null ||
                                    customConvertersTypes.Length == 0
                ? ConvertTypes(outsideConverters,useAllOutsideConverters).ToArray()
                : customConvertersTypes;
                
            foreach (var type in customConvertersTypes)
                yield return type;
        }
        
        public IEnumerable<Type> GetUnityConvertersTypes()
        {
            unityConvertersTypes = unityConvertersTypes == null ||
                                   unityConvertersTypes.Length == 0
                ? ConvertTypes(unityConverters,useAllUnityConverters).ToArray()
                : unityConvertersTypes;
                
            foreach (var type in unityConvertersTypes)
                yield return type;
        }
        
        public IEnumerable<Type> GetJsonNetConvertersTypes()
        {
            jsonNetConvertersTypes = jsonNetConvertersTypes == null ||
                                     jsonNetConvertersTypes.Length == 0
                ? ConvertTypes(jsonNetConverters,useAllJsonNetConverters).ToArray()
                : jsonNetConvertersTypes;
                
            foreach (var type in jsonNetConvertersTypes)
                yield return type;
        }
        
        private IEnumerable<Type> ConvertTypes(IEnumerable<ConverterConfig> items,bool useAll)
        {
            foreach (var item in items)
            {
                if(!useAll && !item.enabled) continue;
                var type = Type.GetType(item.converterType, false, true);
                yield return type;
            }
        }

    }
#pragma warning restore CA2235 // Mark all non-serializable fields
}
