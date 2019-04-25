using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
