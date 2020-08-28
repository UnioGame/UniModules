namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetToStringConverter : BaseTypeConverter
    {
        #region inspector

        public bool addTypeFilter = true;
        
        #endregion
        
        private static Type stringType = typeof(string);
        
        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            return toType == stringType && fromType.IsAsset();
        }

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null || !CanConvert(source.GetType(), target)) {
                return (false, source);
            }

            if (source is Object asset) {
                var value = addTypeFilter ? 
                    $"t:{asset.GetType().Name} {asset.name}" : 
                    asset.name;
                return (true, value);
            }

            return (false, string.Empty);
        }
    }
}
