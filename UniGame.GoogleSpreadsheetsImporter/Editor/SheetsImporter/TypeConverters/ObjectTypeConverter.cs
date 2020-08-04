namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor;
    using Core.EditorTools.Editor.Tools;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
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
                    typeConverters.Add(new StringToPrimitiveTypeConverter());
                    typeConverters.Add(new PrimitiveTypeConverter());
                    _typeConverters.SaveAsset(nameof(ObjectTypeConverter), DefaultConverterPath);
                }

                return _typeConverters;

            }
        }
        
        #endregion
        
        [SerializeReference]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.ListDrawerSettings(Expanded = true)]
#endif
        public List<ITypeConverter> converters = new List<ITypeConverter>();
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return converters.Any(x => x.CanConvert(fromType, toType));
        }

        public object ConvertValue(object source, Type target)
        {
            if (source == null)
                return null;

            var targetType = source.GetType();
            if (target.IsAssignableFrom(targetType)) {
                return source;
            }

            foreach (var converter in converters) {
                if (converter.CanConvert(source.GetType(), target)) {
                    return converter.ConvertValue(source, target);
                }
            }

            return null;
        }
    }
}