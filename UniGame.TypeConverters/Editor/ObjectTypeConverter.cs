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

    [CreateAssetMenu(menuName = "UniGame/Google/SpreadsheetTypeConverters", fileName = nameof(ObjectTypeConverter))]
    public class ObjectTypeConverter : ScriptableObject, ITypeConverter
    {
        #region static data

        private static string DefaultConverterPath =
                EditorFileUtils.Combine(EditorPathConstants.GeneratedContentPath,"TypeConverters/Editor/");

        private static ObjectTypeConverter _typeConverters;
        public static ObjectTypeConverter TypeConverters {
            get {

                _typeConverters = AssetEditorTools.GetAsset<ObjectTypeConverter>();
                if (!_typeConverters) {
                    _typeConverters = ScriptableObject.CreateInstance<ObjectTypeConverter>();
                    var typeConverters = _typeConverters.converters;
                    
                    typeConverters.Add(ScriptableObject.CreateInstance<StringToAssetConverter>().
                        SaveAsset(nameof(StringToAssetConverter),DefaultConverterPath));
                    typeConverters.Add(ScriptableObject.CreateInstance<StringToAssetReferenceConverter>().
                        SaveAsset(nameof(StringToAssetReferenceConverter),DefaultConverterPath));
                    typeConverters.Add(ScriptableObject.CreateInstance<StringToPrimitiveTypeConverter>().
                        SaveAsset(nameof(StringToPrimitiveTypeConverter),DefaultConverterPath));
                    typeConverters.Add(ScriptableObject.CreateInstance<PrimitiveTypeConverter>().
                        SaveAsset(nameof(PrimitiveTypeConverter),DefaultConverterPath));
                    
                    _typeConverters.SaveAsset(nameof(ObjectTypeConverter), DefaultConverterPath);
                }

                return _typeConverters;

            }
        }
        
        #endregion
        
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor(Expanded = true)]
        [Sirenix.OdinInspector.ListDrawerSettings(Expanded = true)]
#endif
        [SerializeReference]
        public List<BaseTypeConverter> converters = new List<BaseTypeConverter>();
        
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