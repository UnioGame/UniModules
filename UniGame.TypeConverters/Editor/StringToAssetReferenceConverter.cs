namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using Core.EditorTools.Editor.AssetOperations;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class StringToAssetReferenceConverter : BaseTypeConverter
    {
        private static Type assetReferenceType = typeof(AssetReference);
        private static Type stringType         = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            if (!assetReferenceType.IsAssignableFrom(toType))
                return false;
            if (!assetReferenceType.IsAssignableFrom(fromType) && fromType != stringType)
                return false;

            return true;
        }

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null)
                return (false, source);
            var sourceType = source.GetType();
            var canConvert = CanConvert(sourceType, target);
            if(!canConvert)
                return (false, source);
            
            if(assetReferenceType.IsAssignableFrom(sourceType))
                return (true, source);

            var filter = source as string;
            var asset = AssetEditorTools.GetAsset(filter);
            var guid = asset.GetGUID();
            if (string.IsNullOrEmpty(guid)) {
                return (false, source);
            }

            var args = new object[]{guid};
            var reference = Activator.CreateInstance(target, args);
            return (true, reference);
        }
        
    }
}
