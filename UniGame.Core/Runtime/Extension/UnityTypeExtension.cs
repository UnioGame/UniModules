using System;
using UnityEngine;

namespace UniGreenModules.UniGame.Core.Runtime.Extension
{
    using Object = UnityEngine.Object;

    public static class UnityTypeExtension
    {
        public static readonly Type componentType  = typeof(Component);
        public static readonly Type gameObjectType = typeof(GameObject);
        public static readonly Type assetType      = typeof(Object);
        public static readonly Type scriptableType = typeof(ScriptableObject);
        public static readonly Type stringType     = typeof(string);

        public static bool IsRegularType(this Type type)
        {
            var result = (type.IsPrimitive || stringType == type);
            return result;
        }

        public static bool IsComponent(this Type type)
        {
            return type != null && componentType.IsAssignableFrom(type);
        }

        public static bool IsGameObject(this Type type)
        {
            return type != null && gameObjectType.IsAssignableFrom(type);
        }

        public static bool IsScriptableObject(this Type type)
        {
            return type != null && scriptableType.IsAssignableFrom(type);
        }

        public static bool IsAsset(this Type type)
        {
            return type != null && assetType.IsAssignableFrom(type);
        }
    }
}