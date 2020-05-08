namespace UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;

    [Serializable]
    public class UniBuildConfigurationData
    {
        public List<BuildTarget> BuildTargets = new List<BuildTarget>();
        
        public List<BuildTargetGroup> BuildTargetGroups = new List<BuildTargetGroup>();

        public bool CloudBuild = false;
    }
}