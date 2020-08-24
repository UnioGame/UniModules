namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using Core.EditorTools.Editor.AssetOperations;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    [CreateAssetMenu(menuName = "UniGame/TypeConverters/AssetReferenceToStringConverter",fileName = nameof(AssetReferenceToStringConverter))]
    public class AssetReferenceToStringConverter : BaseTypeConverter
    {
        #region inspector

        public bool addTypeFilter = true;
        
        #endregion
        
        private Type _assetReferenceType = typeof(AssetReference);
        private Type _stringType = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            return toType == _stringType && _assetReferenceType.IsAssignableFrom(fromType);
        }

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null)
                return (false, source);
            var sourceType = source.GetType();

            if(!CanConvert(sourceType, target))
                return (false, source);
 
            var reference = source as AssetReference;
            var asset     = reference?.editorAsset;
            
            var assetName = asset == null ? 
                string.Empty : 
                addTypeFilter ? 
                    $"t:{asset.GetType().Name} {asset.name}" :
                    asset.name;
            
            return (true, assetName);
        }
        
    }
}
