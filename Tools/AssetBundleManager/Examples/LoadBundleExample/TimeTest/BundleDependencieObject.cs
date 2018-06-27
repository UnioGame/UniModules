using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR

using UnityEditor;

#endif

public class BundleDependencieObject : MonoBehaviour {

	public List<GameObject> DependenciesList = new List<GameObject>();

#if UNITY_EDITOR
    [ContextMenu("Update")]
    public void UpdateItem() {
        var assetsPaths = AssetDatabase.FindAssets("t:GameObject", new[] {"Assets/Data"});
        var assets = assetsPaths.Select(x => {
            var path = AssetDatabase.GUIDToAssetPath(x);
            return AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }).ToList();
        DependenciesList = assets;
        EditorUtility.SetDirty(gameObject);
    }
#endif

}
