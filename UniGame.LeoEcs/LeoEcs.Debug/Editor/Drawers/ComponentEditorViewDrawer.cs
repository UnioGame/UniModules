// namespace UniGame.LeoEcs.Debug.Editor
// {
//     using Sirenix.OdinInspector.Editor;
//     using Sirenix.Utilities.Editor;
//     using UniModules.UniGame.Editor.DrawersTools;
//     using UnityEditor;
//     using UnityEngine;
//
//     public class ComponentEditorViewDrawer : OdinValueDrawer<ComponentEditorView>
//     {
//
//         protected override void DrawPropertyLayout(GUIContent label)
//         {
//             var componentView = ValueEntry.SmartValue;
//             var component = componentView.value;
//             
//             EditorGUILayout.LabelField(component.GetType().Name);
//
//             foreach (var child in Property.Children)
//             {
//                 
//             }    
//             
//             component.DrawOdinPropertyInspector();
//         }
//         
//     }
// }