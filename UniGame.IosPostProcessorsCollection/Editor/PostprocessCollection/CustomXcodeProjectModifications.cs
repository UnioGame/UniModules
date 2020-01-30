using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PostprocessCollection
{

    [CreateAssetMenu(fileName = "New_XCode_Modification", menuName = "XCode Modification", order = 51)]
    public class CustomXcodeProjectModifications : ScriptableObject
    {
        [SerializeField] public List<iOSFrameworkDescription> Frameworks;
        [SerializeField] public List<BuildProperties> Flags;
        [SerializeField] public PlistKeys PlistKeys;
        [SerializeField] public DefaultAsset CopyFilesDirectory;
        [SerializeField] public DefaultAsset EntitlementsFile;
        [SerializeField] public DefaultAsset NewDelegateFile;
    }
    
}