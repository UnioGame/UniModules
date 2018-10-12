using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CustomDefineManagement
{
    public partial class CustomDefineManager
    {
        [MenuItem("Window/Custom Define Manager")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            CustomDefineManager window = (CustomDefineManager)EditorWindow.GetWindow<CustomDefineManager>("Custom Define Manager", true, typeof(SceneView));
            window.Show();                                                            
        }
    }
}
