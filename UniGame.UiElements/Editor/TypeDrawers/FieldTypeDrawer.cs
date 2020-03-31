namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using Runtime.Attributes.FieldTypeDrawer;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    public abstract class FieldDrawer<TValue,TView> : UiElementsTypeDrawer
        where TView : VisualElement , INotifyValueChanged<TValue>
    {
        public override bool IsTypeSupported(Type type)
        {
            return typeof(TValue).IsAssignableFrom(type);
        }

        public override VisualElement Draw(
            object source, 
            Type type, 
            string label = "", 
            Action<object> onValueChanged = null)
        {
            var view = CreateView(source, type, label);
            view.RegisterValueChangedCallback(x => onValueChanged?.Invoke(x.newValue));
            return view;
        }

        protected abstract TView CreateView(
            object source,
            Type type, string label = "");
    }
    
    public class BaseValueFieldDrawer<TValue,TView> : FieldDrawer<TValue,TView> 
        where TView : BaseField<TValue>, new()
    {
        protected override TView CreateView(object source, Type type, string label = "")
        {
            var view = new TView() {
                value = (TValue) source,
                label = label
            };
            return view;
        }
    }
        
    public class ObjectFieldViewDrawer<TValue,TView> : BaseValueFieldDrawer<Object, TView>
        where TValue : Object
        where TView : ObjectField, new()
    {
        public override bool IsTypeSupported(Type type) => typeof(TValue).IsAssignableFrom(type);
        
        protected override TView CreateView(object source, Type type, string label = "")
        {
            var view = new TView() {
                objectType = typeof(TValue),
                value      = source as TValue,
                label      = label,
            };
            return view;
        }
    }
    
    public class ObjectFieldDrawer<TValue> : ObjectFieldViewDrawer<TValue, ObjectField>
        where TValue : Object{}

    [UiElementsDrawer] public class IntFieldDrawer : BaseValueFieldDrawer<int,IntegerField>{}
    [UiElementsDrawer] public class LongFieldDrawer : BaseValueFieldDrawer<long,LongField>{}
    [UiElementsDrawer] public class FloatFieldDrawer : BaseValueFieldDrawer<float,FloatField>{}
    [UiElementsDrawer] public class DoubleFieldDrawer : BaseValueFieldDrawer<double,DoubleField>{}
    [UiElementsDrawer] public class BoolFieldDrawer : BaseValueFieldDrawer<bool,Toggle>{}
    [UiElementsDrawer] public class TextFieldDrawer : BaseValueFieldDrawer<string,TextField>{}
    [UiElementsDrawer] public class BoundsFieldDrawer : BaseValueFieldDrawer<Bounds,BoundsField>{}
    [UiElementsDrawer] public class ColorFieldDrawer : BaseValueFieldDrawer<Color,ColorField>{}
    [UiElementsDrawer] public class Vector2FieldDrawer : BaseValueFieldDrawer<Vector2,Vector2Field>{}
    [UiElementsDrawer] public class Vector2IntFieldDrawer : BaseValueFieldDrawer<Vector2Int,Vector2IntField>{}
    [UiElementsDrawer] public class Vector3FieldDrawer : BaseValueFieldDrawer<Vector3,Vector3Field>{}
    [UiElementsDrawer] public class Vector3IntFieldDrawer : BaseValueFieldDrawer<Vector3Int,Vector3IntField>{}
    [UiElementsDrawer] public class Vector4FieldDrawer : BaseValueFieldDrawer<Vector4,Vector4Field>{}
    [UiElementsDrawer] public class AnimationCurveFieldDrawer : BaseValueFieldDrawer<AnimationCurve,CurveField>{}
    [UiElementsDrawer] public class GradientFieldDrawer : BaseValueFieldDrawer<Gradient,GradientField>{}
    [UiElementsDrawer] public class RectFieldDrawer : BaseValueFieldDrawer<Rect,RectField>{}

    [UiElementsDrawer(-10)] public class ObjectFieldDrawer : ObjectFieldDrawer<Object>{}
    
    [UiElementsDrawer] public class GameObjectFieldDrawer : ObjectFieldDrawer<GameObject>{}
   

    [UiElementsDrawer(1)]
    public class EnumFieldDrawer : BaseValueFieldDrawer<Enum, EnumField>
    {
        public override bool IsTypeSupported(Type type) => type.IsEnum;
    }
}