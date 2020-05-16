using System;
using UnityEngine;

namespace UniGreenModules.UniGame.Core.Runtime.Extension
{
    using Object = UnityEngine.Object;

    public static class UnityTypeExtension
    {
        private static Type componentType = typeof(Component);
        private static Type assetType = typeof(Object);

        public static bool IsComponent(this Type type)
        {
            return type != null && componentType.IsAssignableFrom(type);
        }
        
        public static bool IsAsset(this Type type)
        {
            return type != null && assetType.IsAssignableFrom(type);
        }
    
    }
}
