using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Build;
using Plavalaguna.Joy.Modules.UnityBuild;
using UnityEditor;
using UnityEngine;

namespace GetOverIt.Content.Programmers
{
    [CreateAssetMenu(menuName = "UnityBuild/Commands/Apply Scripting Define Symbols", fileName = "ApplyScriptingDefineSymbols")]
    public class UnityBuildApplyScriptingDefineSymbolsCommand : UnityPreBuildCommand
    {
        [SerializeField]
        private string definesKey = "-defineValues";

        [SerializeField]
        private List<string> defaultDefines = new List<string>();
        
        private const string DefinesSeparotor = ";";
        
        public override void Execute(BuildTarget target, IArgumentsProvider arguments, IBuildParameters buildParameters)
        {

            if (!arguments.GetStringValue(definesKey, out var defineValues))
            {
                return;
            }

            var activeBuildGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var symbolsValue = PlayerSettings.GetScriptingDefineSymbolsForGroup(activeBuildGroup);
            
            var symbols = symbolsValue.Split(new []{DefinesSeparotor},StringSplitOptions.None);
            var buildDefines = defineValues.Split(new []{DefinesSeparotor},StringSplitOptions.None);

            var defines = new List<string>(symbols.Length + buildDefines.Length + defaultDefines.Count);
            defines.AddRange(symbols);
            defines.AddRange(buildDefines);
            defines.AddRange(defaultDefines);
            
            if (defines.Count == 0)
                return;

            var definesBuilder = new StringBuilder(300);
            
            foreach (var define in defines.Distinct())
            {
                definesBuilder.Append(define);
                definesBuilder.Append(DefinesSeparotor);
            }
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(activeBuildGroup,definesBuilder.ToString());

        }
        
        
    }
}
