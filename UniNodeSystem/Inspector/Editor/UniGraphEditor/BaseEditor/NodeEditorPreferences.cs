namespace UniGreenModules.UniNodeSystem.Inspector.Editor.BaseEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public static class NodeEditorPreferences
    {
        private static Type NodeEditorType = typeof(NodeGraphEditor.CustomNodeGraphEditorAttribute);
        
        /// <summary> The last editor we checked. This should be the one we modify </summary>
        private static NodeGraphEditor lastEditor;

        /// <summary> The last key we checked. This should be the one we modify </summary>
        private static string lastKey = "UniNodeSystem.Settings";

        private static Dictionary<string, Color> typeColors = new Dictionary<string, Color>();
        
        private static Dictionary<string, NodeEditorSettings> settings = new Dictionary<string, NodeEditorSettings>();

        /// <summary> Get settings of current active editor </summary>
        public static NodeEditorSettings GetSettings()
        {
            var currentEditor = NodeEditorWindow.Current;
            var graphEditor   = currentEditor.graphEditor;
            
            if (graphEditor!=null && lastEditor != NodeEditorWindow.Current.graphEditor)
            {
                var attribs = graphEditor.GetType()
                    .GetCustomAttributes(NodeEditorType, true);
                
                if (attribs.Length == 1)
                {
                    var attrib =
                        attribs[0] as NodeGraphEditor.CustomNodeGraphEditorAttribute;
                    lastEditor = NodeEditorWindow.Current.graphEditor;
                    lastKey = attrib.editorPrefsKey;
                }
                else return null;
            }

            if (!settings.ContainsKey(lastKey)) VerifyLoaded();
            return settings[lastKey];
        }

        [PreferenceItem("Node Editor")]
        private static void PreferencesGUI()
        {
            VerifyLoaded();
            var settings = NodeEditorPreferences.settings[lastKey];

            NodeSettingsGUI(lastKey, settings);
            GridSettingsGUI(lastKey, settings);
            SystemSettingsGUI(lastKey, settings);
            TypeColorsGUI(lastKey, settings);
            if (GUILayout.Button(new GUIContent("Set Default", "Reset all values to default"), GUILayout.Width(120)))
            {
                ResetPrefs();
            }
        }

        private static void GridSettingsGUI(string key, NodeEditorSettings settings)
        {
            //Label
            EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);
            settings.gridSnap = EditorGUILayout.Toggle(new GUIContent("Snap", "Hold CTRL in editor to invert"),
                settings.gridSnap);

            settings.gridLineColor = EditorGUILayout.ColorField("Color", settings.gridLineColor);
            settings.gridBgColor = EditorGUILayout.ColorField(" ", settings.gridBgColor);
            if (GUI.changed)
            {
                SavePrefs(key, settings);

                NodeEditorWindow.RepaintAll();
            }

            EditorGUILayout.Space();
        }

        private static void SystemSettingsGUI(string key, NodeEditorSettings settings)
        {
            //Label
            EditorGUILayout.LabelField("System", EditorStyles.boldLabel);
            settings.autoSave =
                EditorGUILayout.Toggle(new GUIContent("Autosave", "Disable for better editor performance"),
                    settings.autoSave);
            if (GUI.changed) SavePrefs(key, settings);
            EditorGUILayout.Space();
        }

        private static void NodeSettingsGUI(string key, NodeEditorSettings settings)
        {
            //Label
            EditorGUILayout.LabelField("Node", EditorStyles.boldLabel);
            settings.highlightColor = EditorGUILayout.ColorField("Selection", settings.highlightColor);
            settings.noodleType = (NodeEditorNoodleType) EditorGUILayout.EnumPopup("Noodle type", settings.noodleType);
            if (GUI.changed)
            {
                SavePrefs(key, settings);
                NodeEditorWindow.RepaintAll();
            }

            EditorGUILayout.Space();
        }

        private static void TypeColorsGUI(string key, NodeEditorSettings settings)
        {
            //Label
            EditorGUILayout.LabelField("Types", EditorStyles.boldLabel);

            //Display type colors. Save them if they are edited by the user
            var typeColorKeys = new List<string>(typeColors.Keys);
            foreach (var typeColorKey in typeColorKeys)
            {
                var col = typeColors[typeColorKey];
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                col = EditorGUILayout.ColorField(typeColorKey, col);
                EditorGUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck())
                {
                    typeColors[typeColorKey] = col;
                    if (settings.typeColors.ContainsKey(typeColorKey)) settings.typeColors[typeColorKey] = col;
                    else settings.typeColors.Add(typeColorKey, col);
                    SavePrefs(typeColorKey, settings);
                    NodeEditorWindow.RepaintAll();
                }
            }
        }

        /// <summary> Load prefs if they exist. Create if they don't </summary>
        private static NodeEditorSettings LoadPrefs()
        {
            // Create settings if it doesn't exist
            if (!EditorPrefs.HasKey(lastKey))
            {
                if (lastEditor != null)
                    EditorPrefs.SetString(lastKey, JsonUtility.ToJson(lastEditor.GetDefaultPreferences()));
                else EditorPrefs.SetString(lastKey, JsonUtility.ToJson(new NodeEditorSettings()));
            }

            return JsonUtility.FromJson<NodeEditorSettings>(EditorPrefs.GetString(lastKey));
        }

        /// <summary> Delete all prefs </summary>
        public static void ResetPrefs()
        {
            if (EditorPrefs.HasKey(lastKey)) EditorPrefs.DeleteKey(lastKey);
            if (settings.ContainsKey(lastKey)) settings.Remove(lastKey);
            typeColors = new Dictionary<string, Color>();
            VerifyLoaded();
            NodeEditorWindow.RepaintAll();
        }

        /// <summary> Save preferences in EditorPrefs </summary>
        private static void SavePrefs(string key, NodeEditorSettings settings)
        {
            EditorPrefs.SetString(key, JsonUtility.ToJson(settings));
        }

        /// <summary> Check if we have loaded settings for given key. If not, load them </summary>
        private static void VerifyLoaded()
        {
            if (!settings.ContainsKey(lastKey)) settings.Add(lastKey, LoadPrefs());
        }

        /// <summary> Return color based on type </summary>
        public static Color GetTypeColor(Type type)
        {
            VerifyLoaded();
            if (type == null) return Color.gray;
            var typeName = type.PrettyName();
            if (!typeColors.ContainsKey(typeName))
            {
                if (settings[lastKey].typeColors.ContainsKey(typeName))
                    typeColors.Add(typeName, settings[lastKey].typeColors[typeName]);
                else
                {
#if UNITY_5_4_OR_NEWER
                    Random.InitState(typeName.GetHashCode());
#else
                    UnityEngine.Random.seed = typeName.GetHashCode();
#endif
                    typeColors.Add(typeName, new Color(Random.value, Random.value, Random.value));
                }
            }

            return typeColors[typeName];
        }
    }
}