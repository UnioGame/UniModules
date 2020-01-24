using UnityEditor;
using UnityEngine;

namespace UniGreenModules.UniCore.EditorTools.Editor.Utility
{
    public class UniPrefabUtility 
    {

        public static GameObject LoadPrefabContents(GameObject target)
        {

            var path = AssetDatabase.GetAssetPath(target);
        
            if (string.IsNullOrEmpty(path))
            {
                return target;
            }

            return PrefabUtility.LoadPrefabContents(path);

        }
    
    }
}
