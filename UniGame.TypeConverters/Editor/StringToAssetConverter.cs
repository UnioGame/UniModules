namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.TypeConverters
{
    using System;
    using System.Linq;
    using Abstract;
    using Core.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UnityEngine;

    [Serializable]
    [CreateAssetMenu(menuName = "UniGame/TypeConverters/StringToAssetConverter",fileName = nameof(StringToAssetConverter))]
    public class StringToAssetConverter : BaseTypeConverter
    {
        private Type _stringType = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            return fromType == _stringType && toType.IsAsset();
        }

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null || !CanConvert(source.GetType(), target)) {
                return (false, source);
            }

            var asssetFilter = source as string;
            var assets = AssetEditorTools.GetAssets(asssetFilter,target);
            return (assets.Count > 0, assets.FirstOrDefault());
        }
    }
}
