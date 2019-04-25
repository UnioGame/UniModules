using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniTools.BuildTools {

    [CreateAssetMenu(menuName = "Build/PlayerBuild Configuration")]
    public class PlayerBuildConfiguration : ScriptableObject {

        public List<BuildOptions> defaultBuildOptions = new List<BuildOptions>();
        public List<PlayerBuildOption> platformConfiguration;

        public BuildOptions GetBuildOptions(BuildTargetGroup platform) {

            var buildOptions = BuildOptions.None;
            var configFound = false;

            var optionItems = platformConfiguration.Where(x => x.runtimePlatforms.Contains(platform));

            foreach (var configuration in optionItems) {

                configFound = true;
                buildOptions |= CreateOptions(configuration.buildOptions);

            }
            if (configFound == false) {

                buildOptions = CreateOptions(defaultBuildOptions);

            }

            return buildOptions;

        }

        private BuildOptions CreateOptions(List<BuildOptions> options) {

            var buildOptions = BuildOptions.None;
            foreach (var option in options) {

                Debug.LogFormat("~~~Add BuildOption [{0}]", option);
                buildOptions |= option;

            }

            return buildOptions;

        }

    }

    [Serializable]
    public class PlayerBuildOption {

        public List<BuildTargetGroup> runtimePlatforms = new List<BuildTargetGroup>();
        public List<BuildOptions> buildOptions = new List<BuildOptions>();

    }

}
