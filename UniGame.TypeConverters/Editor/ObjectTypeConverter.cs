namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using Core.EditorTools.Editor;
    using Core.EditorTools.Editor.AssetOperations;
    using Core.EditorTools.Editor.Tools;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/ObjectTypeConverter/Create Converter", fileName = nameof(ObjectTypeConverter))]
    public class ObjectTypeConverter : ScriptableObject, ITypeConverter
    {
        #region static data

        private static string _defaultConverterPath;
        
        public static string DefaultConverterPath => _defaultConverterPath = string.IsNullOrEmpty(_defaultConverterPath) ?
                EditorFileUtils.Combine(EditorPathConstants.GeneratedContentPath,"TypeConverters/Editor/") : 
                _defaultConverterPath;

        private static ObjectTypeConverter _typeConverters;
        public static ObjectTypeConverter TypeConverters {
            get {
                if (_typeConverters)
                    return _typeConverters;
                
                _typeConverters = AssetEditorTools.GetAsset<ObjectTypeConverter>();
                if (!_typeConverters) {
                    _typeConverters = ScriptableObject.CreateInstance<ObjectTypeConverter>();
                    _typeConverters.ResetToDefault();
                    _typeConverters.SaveAsset(nameof(ObjectTypeConverter), DefaultConverterPath);
                }

                return _typeConverters;
            }
        }

        #endregion
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ListDrawerSettings(Expanded = true)]
#endif
        [SerializeReference]
        public List<BaseTypeConverter> converters = new List<BaseTypeConverter>();

        [ContextMenu(nameof(ResetToDefault))]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void ResetToDefault()
        {
            this.converters.Clear();
                                
            converters.Add(new AssetReferenceToStringConverter());
            converters.Add(new AssetToStringConverter());
            converters.Add(new StringToAssetConverter());
            converters.Add(new StringToAssetReferenceConverter());
            converters.Add(new JsonSerializableClassConverter());
            converters.Add(new StringToPrimitiveTypeConverter());
            converters.Add(new PrimitiveTypeConverter());
        }
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return converters.Any(x => x.CanConvert(fromType, toType));
        }

        public (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null) {
                return (false, source);
            }

            for (var i = 0; i < converters.Count; i++) {
                var converter     = converters[i];
                var convertResult = converter.TryConvert(source, target);
                if (convertResult.isValid)
                    return convertResult;
            }

            return (false, source);
        }

        public object ConvertValue(object source, Type toType)
        {
            if (source == null)
                return null;
            
            var sourceType = source.GetType();
            if (sourceType == toType || toType.IsAssignableFrom(sourceType)) {
                return source;
            }

            var convertResult = TryConvert(source, toType);

            if (!convertResult.isValid) {
                GameLog.LogWarning($"Convert Failed for {source} to Type = {toType.Name}",this);
            }
            
            return convertResult.result;
        }
    }
}