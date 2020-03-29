namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Runtime.Attributes.FieldTypeDrawer;
    using UniGreenModules.UniCore.Runtime.ReflectionUtils;
    using UniGreenModules.UniGame.Core.Runtime.Attributes.FieldTypeDrawer;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    public static class UiElementFactory 
    {

        private static List<IUiElementsTypeDrawer> drawers;
        private static IUiElementsTypeDrawer defaultDrawer = new ClassUiElementsDrawer();

        public static Type GameObjectType = typeof(GameObject);
        public static Type ComponentType = typeof(Component);
        public static Type ScriptableType = typeof(ScriptableObject);
        public static Type UnityObjectType = typeof(Object);
        
        public static Texture DefaultTexture { get; } = new Texture2D(8, 8);

        static UiElementFactory()
        {
            drawers = new List<IUiElementsTypeDrawer>();

            LoadDrawers();
            //reload drawers on code changes
            AssemblyReloadEvents.afterAssemblyReload += LoadDrawers;
        }

        public static VisualElement Create(object data)
        {
            return Create(data, data?.GetType());
        }

        public static VisualElement Create(object data,Type targetType, Action<object> valueChanged = null,string label ="")
        {
            VisualElement result = null;
            
            try {
                result = CreateVisualElement(data,targetType,valueChanged,label);
            }
            catch (Exception e) {
                Debug.LogException(e);
            }

            return result;
        }

        public static bool IsVisibleField(FieldInfo info)
        {
            if (info.IsNotSerialized)
                return false;
            
            var attributes = info.GetCustomAttributes();

            foreach (var attribute in attributes) {
                switch (attribute) {
                    case HideInInspector hideInInspector:
                    case HideDrawerAttribute drawerAttribute:
                    case HideNodeInspectorAttribute nodeInspector:
                        return false;
                }
            }

            foreach (var attribute in attributes) {
                if (attribute is SerializeField || attribute is SerializeReference)
                    return true;
            }

            return info.IsPublic;
        }

        public static Foldout CreateFoldout(string label, int marginLeft = 0)
        {
            var foldout = new Foldout() {
                text = label,
                style = {
                    marginLeft = marginLeft,
                }
            };
            return foldout;
        }

        public static ObjectField CreateObjectField(object data,Type targetType, 
            Action<object> valueChanged = null,bool allowSceneObjects = true,string label ="")
        {
            var objectField = new ObjectField(label) {
                value = data as Object,
                objectType = targetType,
                allowSceneObjects = allowSceneObjects,
            };
            objectField.RegisterValueChangedCallback<Object>(x => valueChanged?.Invoke(x.newValue));
            return objectField;
        }
        
        public static VisualElement CreateVisualElement(object data,Type type, Action<object> onValueChanged = null,string label  = "")
        {
            var result = CreateVisualElementInner(data, type, onValueChanged, label);
            
            if(result == null)
                return new VisualElement();

            return result;
            
        }

        private static VisualElement CreateVisualElement(object data,  Action<object> onValueChanged = null, string label = "")
        {
            return CreateVisualElement(data, data?.GetType(), onValueChanged, label);
        }

        private static VisualElement CreateVisualElementInner(object data,Type type, Action<object> onValueChanged = null,string label  = "")
        {
            VisualElement result = null;

            foreach (var drawer in drawers) {
                if (!drawer.IsTypeSupported(type)) 
                    continue;
                result = drawer.Draw(data, type, label, onValueChanged);
                break;
            }

            result = result ?? defaultDrawer.Draw(data, type, label, onValueChanged);
            return result;
        }

        private static void LoadDrawers()
        {
            drawers.Clear();
            
            //find all active ui elements drawers
            var allDrawers = typeof(IUiElementsTypeDrawer).
                GetAssignableTypes().
                Where(x => x.IsAbstract == false).
                Select(x => (attribute : x.GetCustomAttribute<UiElementsDrawerAttribute>(),type : x)).
                Where(x => x.attribute != null && x.attribute.IsActive).
                OrderByDescending(x => x.attribute.Priority).
                Select(x => Activator.CreateInstance(x.type)).
                OfType<IUiElementsTypeDrawer>().
                ToList();
            
            drawers.AddRange(allDrawers);
        }
    }
}
