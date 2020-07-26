using System;
using UnityEngine;

namespace UniGreenModules.UniGame.Core.Runtime.Extension
{
    using Object = UnityEngine.Object;

    public static class UnityTypeExtension
    {
        public static Type componentType = typeof(Component);
        public static Type gameObjectType = typeof(GameObject);
        public static Type assetType = typeof(Object);
        public static Type scriptableType = typeof(ScriptableObject);

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
