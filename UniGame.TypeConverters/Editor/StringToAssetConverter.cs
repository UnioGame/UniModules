namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UnityEngine;

    [Serializable]
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
            var assets = AssetEditorTools.GetAssets(target,asssetFilter);
            return (assets.Count > 0, assets.FirstOrDefault());
        }
    }
}
