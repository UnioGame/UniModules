using System;
using UnityEngine;

namespace UniGreenModules.UniGame.Core.Runtime.Extension
{
    public static class ComponentExtension
    {
        private static Type componentType = typeof(Component);

        public static bool IsComponent(this Type type)
        {
            return type != null && componentType.IsAssignableFrom(type);
        }
    
    }
}
