namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using Core.EditorTools.Editor.AssetOperations;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class AssetReferenceToStringConverter : BaseTypeConverter
    {
        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.LabelWidth(120)]
#endif
        public bool addTypeFilter = true;
        
        #endregion
        
        private static Type assetReferenceType = typeof(AssetReference);
        private static Type stringType = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            return toType == stringType && assetReferenceType.IsAssignableFrom(fromType);
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
