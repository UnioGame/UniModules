namespace UniModules.UniGame.UiElements.Editor.TypeDrawers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::UniGame.Core.EditorTools.Editor.UiElements;
    using global::UniGame.Core.Runtime.Attributes.FieldTypeDrawer;
    using global::UniGame.UiElements.Runtime.Attributes;
    using UniModules.UniCore.Runtime.ReflectionUtils;
    using UniModules.UniCore.Runtime.Utils;
    using UniModules.UniGame.Core.Runtime.Attributes.FieldTypeDrawer;
    using UniRx;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    [InitializeOnLoad]
    public static class UiElementFactory 
    {
        private static List<Attribute>              _attributes = new List<Attribute>();
        private static BoolReactiveProperty         ready       = new BoolReactiveProperty();
        private static List<IUiElementsTypeDrawer>  drawers;
        private static List<IUiElementsFieldDrawer> fieldDrawers;
        
        private static IUiElementsTypeDrawer defaultDrawer = new ClassUiElementsDrawer();
        
        private static Dictionary<object,VisualElement> visualElementsCache = new Dictionary<object, VisualElement>(16);

        public static IReadOnlyList<IUiElementsTypeDrawer> Drawers => drawers;
        public static IReadOnlyList<IUiElementsFieldDrawer> FieldDrawers => fieldDrawers;
        public static IReadOnlyReactiveProperty<bool> Ready => ready;
        
        private static Func<Type, IUiElementsTypeDrawer> GetDrawer =
            MemorizeTool.Create<Type,IUiElementsTypeDrawer>((type => {
                foreach (var drawer in drawers) {
                    if (drawer.IsTypeSupported(type)) 
                        return drawer;
                }
                return null;
            }));
        
        private static Func<Type, IUiElementsFieldDrawer> GetFieldDrawer =
            MemorizeTool.Create<Type,IUiElementsFieldDrawer>((type => {
                foreach (var drawer in fieldDrawers) {
                    if (drawer.IsTypeSupported(type)) 
                        return drawer;
                }
                return null;
            }));
        
        public static Type GameObjectType = typeof(GameObject);
        public static Type ComponentType = typeof(Component);
        public static Type ScriptableType = typeof(ScriptableObject);
        public static Type UnityObjectType = typeof(Object);
        
        public static Texture DefaultTexture { get; } = new Texture2D(8, 8);

        static UiElementFactory()
        {
            drawers = new List<IUiElementsTypeDrawer>();
            fieldDrawers = new List<IUiElementsFieldDrawer>();

            ReloadDrawers();
            
            //reload drawers on code changes
            AssemblyReloadEvents.afterAssemblyReload += ReloadDrawers;
        }

        public static void ReloadDrawers()
        {
            LoadDrawers();
            LoadFieldInfoDrawers();
            ready.SetValueAndForceNotify(true);
        }
        
        public static VisualElement CachedDrawer(
            object data, 
            Type type,
            Action<object> valueChanged = null,
            string label = "")
        {
            visualElementsCache.Clear();
            var result = Create(data, type, valueChanged, label);
            visualElementsCache.Clear();
            return result;
        }

        public static VisualElement CreateField(
            object data,FieldInfo info,
            Action<object> valueChanged = null,
            string label ="")
        {
            VisualElement result = null;
            
            try {
                result = CreateVisualElementInner(
                    data,info.FieldType,
                    null,valueChanged,label);
            }
            catch (Exception e) {
                Debug.LogException(e);
            }

            return result;
        }
        
        public static VisualElement Create(object data)
        {
            return Create(data, data?.GetType());
        }

        public static VisualElement Create(
            object data,
            Type targetType, 
            Action<object> valueChanged = null,
            string label ="")
        {
            VisualElement result = null;
            try {
                result = CreateVisualElementInner(data,targetType,null,valueChanged,label);
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
            
            _attributes.Clear();
            _attributes.AddRange(info.GetCustomAttributes());

            foreach (var attribute in _attributes) {
                switch (attribute) {
                    case HideInInspector hideInInspector:
                    case HideDrawerAttribute drawerAttribute:
                    case HideNodeInspectorAttribute nodeInspector:
                        return false;
                }
            }

            foreach (var attribute in _attributes) {
                if (attribute is SerializeField || attribute is SerializeReference)
                    return true;
            }

            _attributes.Clear();
            
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

        public static ObjectField CreateObjectField(
            object data,
            Type targetType, 
            Action<object> valueChanged = null,
            bool allowSceneObjects = true,
            string label ="")
        {
            var objectField = new ObjectField(label) {
                value = data as Object,
                objectType = targetType,
                allowSceneObjects = allowSceneObjects,
            };
            objectField.RegisterValueChangedCallback<Object>(x => valueChanged?.Invoke(x.newValue));
            return objectField;
        }
        
        private static VisualElement CreateVisualElementInner(
            object data,
            Type type,
            FieldInfo fieldInfo = null,
            Action<object> onValueChanged = null, 
            string label  = "")
        {
            if (data != null && visualElementsCache.TryGetValue(data, out var visualElement))
                return visualElement;

            visualElement = fieldInfo == null ? 
                GetDrawer(type)?.Draw(data, type, label, onValueChanged) : 
                GetFieldDrawer(type)?.Draw(data,fieldInfo, type, label, onValueChanged);
            
            return visualElement ?? new VisualElement();
        }

        private static void LoadDrawers() => LoadDrawers<UiElementsDrawerAttribute,IUiElementsTypeDrawer>(drawers);

        private static void LoadFieldInfoDrawers() => LoadDrawers<UiElementsFieldDrawerAttribute,IUiElementsFieldDrawer>(fieldDrawers);

        private static void LoadDrawers<TSource,TApi>(List<TApi> container)
            where TSource : Attribute,IPriorityValue
        {
            container.Clear();
            
            //find all active ui elements drawers
            var allDrawers = typeof(TApi).
                GetAssignableTypes().
                Where(x => x.IsAbstract == false).
                Select(x => (attribute : x.GetCustomAttribute<TSource>(),type : x)).
                Where(x => x.attribute != null).
                OrderByDescending(x => x.attribute.Priority).
                Select(x => Activator.CreateInstance(x.type)).
                OfType<TApi>().
                ToList();
            
            container.AddRange(allDrawers);
        }
    }
}
