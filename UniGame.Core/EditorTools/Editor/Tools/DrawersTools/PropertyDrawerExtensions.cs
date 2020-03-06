namespace UniGreenModules.UniCore.EditorTools.Editor.PropertiesDrawers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Runtime.Utils;
    using UnityEditor;

    public static class PropertyDrawerExtensions
    {
        private const string AssembliesFilter      = "ScriptAttributeUtility";
        private const string GetEditorDrawerMethod = "GetDrawerTypeForType";
        private const string FieldInfoName         = "m_FieldInfo";

        private static BindingFlags staticFlags    = BindingFlags.NonPublic | BindingFlags.Static;
        private static BindingFlags nonStaticFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        public static Func<Type,PropertyDrawer> PropertyDrawers = 
            MemorizeTool.Create((Type type) => (PropertyDrawer)Activator.CreateInstance(PropertyDrawerFactory(type)));
        
        public static Func<Type, Type> PropertyDrawerFactory = 
            MemorizeTool.Create((Type type) => CreateDrawerType(type));
        
        public static PropertyDrawer GetDrawer<T>(bool cached = false)
        {
            return GetDrawer(typeof(T),cached);
        }

        public static PropertyDrawer GetDrawer(Type targetType,bool cached = false)
        {
            var temp = cached ? 
                PropertyDrawers(targetType) :
                (PropertyDrawer) Activator.CreateInstance(PropertyDrawerFactory(targetType));
            return temp;
        }

        public static Type CreateDrawerType(Type targetType)
        {
            var typeInfo = AppDomain.CurrentDomain.GetAssemblies().
                Select(a => a.GetTypes().
                    FirstOrDefault(t => t.Name.Contains(AssembliesFilter))).
                FirstOrDefault(t => t != null);

            var methodInfo = typeInfo.GetMethod(GetEditorDrawerMethod, staticFlags);
            var getDrawer     = (Func<Type, Type>) Delegate.CreateDelegate(typeof(Func<Type, Type>), null, methodInfo);
            return getDrawer(targetType);
        }

        public static PropertyDrawer GetTypeDrawer<T>(this T target,bool cached = false) => GetDrawer<T>(cached);

        //get drawer by field info and initialize default data
        public static PropertyDrawer GetDrawer(this FieldInfo fieldInfo, Type target, bool cached = false)
        {
            var drawer = GetDrawer(target,cached);
            var field  = drawer.GetType().
                GetField(FieldInfoName, BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(drawer, fieldInfo);
            return drawer;
        }

        public static PropertyDrawer GetDrawer(this object fieldInfo, Type target,bool cached = false) => GetDrawer(target,cached);
    }
}