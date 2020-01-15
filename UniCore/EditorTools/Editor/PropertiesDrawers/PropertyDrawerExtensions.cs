namespace UniGreenModules.UniCore.EditorTools.Editor.PropertiesDrawers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;

    public static class PropertyDrawerExtensions
    {
        private const  string       AssembliesFilter      = "ScriptAttributeUtility";
        private const  string       GetEditorDrawerMethod = "GetDrawerTypeForType";
        private static BindingFlags staticFlags           = BindingFlags.NonPublic | BindingFlags.Static;
        private static BindingFlags nonStaticFlags        = BindingFlags.NonPublic | BindingFlags.Instance;
        
        private static Dictionary<Type, Func<Type, Type>> drawers = new Dictionary<Type, Func<Type, Type>>();
        
        public static PropertyDrawer GetDrawer<T>()
        {
            return GetDrawer(typeof(T));
        }
        
        public static PropertyDrawer GetDrawer(Type targetType)
        {
            var type = targetType;
            if (!drawers.TryGetValue(type,out var getDrawer)) {
                var typeInfo = AppDomain.CurrentDomain.GetAssemblies().
                    Select(a => a.GetTypes().
                    FirstOrDefault(t => t.Name.Contains(AssembliesFilter))).
                    FirstOrDefault(t => t != null);
                
                var methodInfo = typeInfo.GetMethod(GetEditorDrawerMethod, staticFlags);
                getDrawer     = (Func<Type, Type>) Delegate.CreateDelegate(typeof(Func<Type, Type>), null, methodInfo);
                drawers[type] = getDrawer;
            }

            if (getDrawer == null)
                return null;
            
            var temp = (PropertyDrawer) Activator.CreateInstance(getDrawer(type));
            return temp;
        }

        public static PropertyDrawer GetMyDrawer<T>(this T target) => GetDrawer<T>();
        
        public static PropertyDrawer GetDrawer(this object source, Type target) => GetDrawer(target);
    }
}
