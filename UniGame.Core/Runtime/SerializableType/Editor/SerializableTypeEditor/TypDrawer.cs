namespace UniGreenModules.UniGame.Core.Runtime.SerializableType.Editor.SerializableTypeEditor
{
    using System;
    using System.Collections.Generic;
    using UniCore.Runtime.ReflectionUtils;
    using UniCore.Runtime.Utils;
    using UnityEditor;
    using UnityEngine;

    public static class TypDrawer
    {

        private static Func<Type, List<Type>> getAssagnableTypes = MemorizeTool.Create<Type, List<Type>>(ReflectionTools.GetAssignableTypes);

        private const string noneValue = "none";
        
        private static List<string> popupValues = new List<string>();
        
        public static Type DrawTypePopup(Rect position,GUIContent label, Type baseType, Type selectedType)
        {
            //all assignable types
            var types = getAssagnableTypes(baseType);

            var selectedIndex = 0;
            
            popupValues.Clear();
            popupValues.Add(noneValue);
            for (var i = 0; i < types.Count; i++) {
                var item      = types[i];
                var itemIndex = i + 1;
                
                popupValues.Add(item.Name);
                selectedIndex = item == selectedType ? 
                    itemIndex : selectedIndex;
            }
            
            var newSelection = EditorGUI.Popup(position, label.text, selectedIndex, popupValues.ToArray());

            return newSelection == 0 ? null : types[newSelection - 1];
        }
        
        
        
    }
}