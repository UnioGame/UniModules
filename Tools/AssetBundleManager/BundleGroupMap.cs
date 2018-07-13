using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BundleGroup {

    Gameplay,
    Campaign1,
    Campaign2,
    Campaign3,
    Menu,

}

[Serializable]
public class BundleGroupItem {

    public BundleGroup Group;
    public List<string> Bundles = new List<string>();

}

[CreateAssetMenu(fileName = "BundleGroupMap",menuName = "Asset Bundles/BundleGroupMap")]
public class BundleGroupMap : ScriptableObject {

    public List<BundleGroupItem> BundleGroups = new List<BundleGroupItem>();

}
