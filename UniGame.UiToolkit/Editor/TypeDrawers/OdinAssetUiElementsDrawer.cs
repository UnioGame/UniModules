namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using Runtime.Attributes.FieldTypeDrawer;
    using UniModules.UniGame.Core.EditorTools.Editor.DrawersTools;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;


    [UiElementsDrawer]
    public class ScriptableObjectFieldDrawer : OdinAssetUiElementsDrawer<ScriptableObject>{}
    
    [UiElementsDrawer]
    public class ComponentObjectFieldDrawer : OdinAssetUiElementsDrawer<Component>{}
    

    public class OdinAssetUiElementsDrawer<TAsset> :  ObjectFieldDrawer<TAsset>
        where TAsset : Object
    {
        public override VisualElement Draw(object source, Type type, string label = "", Action<object> onValueChanged = null)
        {

            var control = new OdinValueView() {
                Label       = label,
                Value       = source as Object,
                IsOpen      = false,
                AssetAction = onValueChanged,
            };

            var odinView = control.View;
            
            var view = base.Draw(source, type, label, x => {
                control.Value = x as Object;
                onValueChanged?.Invoke(x);
            });
            
            var row = new VisualElement();
            
            var container = new VisualElement() {
                style = {
                    flexDirection = new StyleEnum<FlexDirection>(){
                        value = FlexDirection.Row,
                    },
                }
            };
            
            var foldout = new Foldout();
            {
                
            }
            container.Add(foldout);
            container.Add(view);
            
            row.Add(container);
            row.Add(control.View);

            foldout.RegisterValueChangedCallback(x => odinView.visible = x.newValue);
            
            return container;
        }
    }
}