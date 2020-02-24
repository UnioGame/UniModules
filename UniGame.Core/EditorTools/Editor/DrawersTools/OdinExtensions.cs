using System.Diagnostics;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Odin Inspector extensions methods
/// </summary>
public static class OdinExtensions 
{
    
    
    [Conditional("ODIN_INSPECTOR")]
    public static void DrawOdinPropertyInspector(this object asset)
    {
        if (asset == null) return;

        using (var drawer = Sirenix.OdinInspector.Editor.PropertyTree.Create(asset)) {
            drawer.Draw(false);
        }

    }
    
    public static bool DrawOdinPropertyWithFoldout(this SerializedProperty property, 
        Rect position, bool foldOut)
    {
        var asset = property.objectReferenceValue;
        if (!asset) return false;
        
        foldOut = EditorGUI.Foldout(position,foldOut,string.Empty);
        if (foldOut) {
            asset.DrawOdinPropertyInspector();
        }

        return foldOut;
    }

    
}
