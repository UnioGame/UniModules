using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomDefineManagement
{
    public partial class CustomDefineManager : EditorWindow
    {
        /*private static CustomDefineManagerData _dataFile = null;
        private static CustomDefineManagerData dataFile
        {
            get
            {
                if (_dataFile == null) _dataFile = Resources.Load<CustomDefineManagerData>("CustomDefineManagerData");

                return _dataFile;
            }
        }*/
        
        public List<Directive> LoadDirectives()
        {
            List<Directive> directives = new List<Directive>();

            foreach (cdmBuildTargetGroup platform in Enum.GetValues(typeof(cdmBuildTargetGroup)))
            {
                var platformSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform.ToBuildTargetGroup());

                if (!String.IsNullOrEmpty(platformSymbols))
                {
                    foreach (var symbol in platformSymbols.Split(';'))
                    {
                        var directive = directives.FirstOrDefault(d => d.name == symbol);

                        if (directive == null)
                        {
                            directive = new Directive { name = symbol };
                            directives.Add(directive);
                        }

                        directive.targets |= platform;
                    }
                }
            }

            var dataFileDirectives = GetDirectivesFromXmlFile();

            if (directives.Any())
            {
                if (!dataFileDirectives.Any())
                {
                    SaveDirectives(directives);
                }                                
            }

            // Add any directives from the data file which weren't located in the configuration file
            directives.AddRange(dataFileDirectives.Where(df => !directives.Any(d => d.name == df.name)));

            foreach (var dataFileDirective in dataFileDirectives)
            {
                var directive = directives.First(d => d.name == dataFileDirective.name);

                directive.enabled = dataFileDirective.enabled;
                directive.sortOrder = dataFileDirective.sortOrder;
            }
            
            return directives.OrderBy(d => d.sortOrder).ToList();
        }

        public static void SaveDirectives(List<Directive> directives)
        {
            Dictionary<cdmBuildTargetGroup, List<Directive>> targetGroups = new Dictionary<cdmBuildTargetGroup, List<Directive>>();

            foreach (var directive in directives)
            {
                foreach (cdmBuildTargetGroup targetGroup in Enum.GetValues(typeof(cdmBuildTargetGroup)))
                {
                    if (String.IsNullOrEmpty(directive.name) || !directive.enabled) continue;

                    if (directive.targets.HasFlag(targetGroup))
                    {
                        if (!targetGroups.ContainsKey(targetGroup)) targetGroups.Add(targetGroup, new List<Directive>());

                        targetGroups[targetGroup].Add(directive);
                    }
                }
            }

            foreach (cdmBuildTargetGroup targetGroup in Enum.GetValues(typeof(cdmBuildTargetGroup)))
            {
                string symbols = "";

                if (targetGroups.ContainsKey(targetGroup))
                {
                    symbols = String.Join(";", targetGroups[targetGroup].Select(d => d.name).ToArray());
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup.ToBuildTargetGroup(), symbols);
            }

            SaveDirectivesToDataFile(directives);
        }

        private static void SaveDirectivesToDataFile(List<Directive> directives)
        {
            int x = 0;
            directives.ForEach(d => d.sortOrder = x++);

            SaveDataToXmlFile(directives);
        }

        public static Directive GetDirective(string directiveName)
        {            
            return GetDirectivesFromXmlFile().FirstOrDefault(d => d.name.Equals(directiveName, StringComparison.OrdinalIgnoreCase));
        }

        public static void EnableDirective(string directiveName)
        {
            var directives = GetDirectivesFromXmlFile();
            var directive = directives.FirstOrDefault(d => d.name.Equals(directiveName, StringComparison.OrdinalIgnoreCase));

            if (directive == null)
            {
                Debug.LogErrorFormat("Directive '{0}' not found!", directiveName);
                return;
            }

            directive.enabled = true;            
            CustomDefineManager.SaveDirectives(directives);

            // also update the editor window            
            var window = Resources.FindObjectsOfTypeAll<CustomDefineManager>().LastOrDefault();
            if (window != null)
            {
                var windowDirective = window.directives.FirstOrDefault(d => d.name == directive.name);
                if (windowDirective != null)
                {
                    windowDirective.enabled = true;
                    window.Repaint();
                }
            }            
        }

        public static void DisableDirective(string directiveName)
        {
            var directives = GetDirectivesFromXmlFile();
            var directive = directives.FirstOrDefault(d => d.name.Equals(directiveName, StringComparison.OrdinalIgnoreCase));

            if (directive == null)
            {
                Debug.LogErrorFormat("Directive '{0}' not found!", directiveName);
                return;
            }

            directive.enabled = false;
            CustomDefineManager.SaveDirectives(directives);

            // also update the editor window            
            var window = Resources.FindObjectsOfTypeAll<CustomDefineManager>().LastOrDefault();
            if (window != null)
            {
                var windowDirective = window.directives.FirstOrDefault(d => d.name == directive.name);
                if (windowDirective != null)
                {
                    windowDirective.enabled = false;
                    window.Repaint();
                }
            }            
        }
    }
}