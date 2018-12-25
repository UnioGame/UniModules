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
        protected List<Directive> directives = new List<Directive>();

        Color guiColor;
        Color guiBackgroundColor;
        private Vector2 scroll;

        void OnEnable()
        {
            Reload();
        }

        void OnGUI()
        {            
            guiColor = GUI.color;
            guiBackgroundColor = GUI.backgroundColor;

            var directiveLineStyle = new GUIStyle(EditorStyles.toolbar);
            directiveLineStyle.fixedHeight = 0;
            directiveLineStyle.padding = new RectOffset(8, 8, 0, 0);

            var headerStyle = new GUIStyle(EditorStyles.largeLabel);
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;

            GUI.color = new Color(0.5f, 0.5f, 0.5f);

            EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(20));
            {
                GUI.color = guiColor;
                EditorGUILayout.LabelField("", GUILayout.Width(31));
                EditorGUILayout.LabelField("Custom Define Manager", headerStyle, GUILayout.Height(20));
            }
            EditorGUILayout.EndHorizontal();

            List<Directive> directivesToRemove = new List<Directive>();

            RenderTableHeader();

            var textFieldStyle = new GUIStyle(EditorStyles.toolbarTextField);
            textFieldStyle.alignment = TextAnchor.MiddleLeft;
            textFieldStyle.fixedHeight = 0;
            textFieldStyle.padding = new RectOffset(4, 4, 4, 4);
            textFieldStyle.fontSize = 12;
            textFieldStyle.margin = new RectOffset(0, 0, 1, 1);

            var platformsStyles = new GUIStyle(directiveLineStyle);
            platformsStyles.padding = new RectOffset(4, 4, 4, 4);

            var removeButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            removeButtonStyle.normal.textColor = Color.red;
            removeButtonStyle.fixedHeight = 0;
            removeButtonStyle.margin = new RectOffset(0, 0, 1, 1);

            var toggleStyle = new GUIStyle(EditorStyles.toggle);
            toggleStyle.alignment = TextAnchor.MiddleCenter;
            toggleStyle.fixedWidth = 0;

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (var directive in directives)
            {
                EditorStyles.helpBox.alignment = TextAnchor.MiddleLeft;

                GUI.color = new Color(0.65f, 0.65f, 0.65f);

                EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(24), GUILayout.ExpandWidth(true));
                {
                    GUI.color = guiColor;

                    if (GUILayout.Button(new GUIContent("X", "Remove this directive"), removeButtonStyle, GUILayout.Width(32), GUILayout.Height(24)))
                    {
                        directivesToRemove.Add(directive);
                    }

                    GUILayout.Space(4);

                    directive.name = EditorGUILayout.TextField(directive.name, textFieldStyle, GUILayout.Width(250), GUILayout.Height(24));

                    GUILayout.Space(7);

                    EditorGUILayout.BeginHorizontal(platformsStyles, GUILayout.Height(24), GUILayout.Width(370));
                    {
                        foreach (cdmBuildTargetGroup targetGroup in Enum.GetValues(typeof(cdmBuildTargetGroup)))
                        {
                            var platformButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
                            platformButtonStyle.fontStyle = FontStyle.Bold;

                            var hasFlag = directive.targets.HasFlag(targetGroup);

                            if (!hasFlag)
                            {
                                GUI.backgroundColor = new Color(1, 1, 1, 0.25f);
                                GUI.color = new Color(1, 1, 1, 0.25f);
                            }
                            else
                            {
                                GUI.backgroundColor = new Color(0.5f, 1f, 0.5f, 1f);
                            }

                            GUIContent buttonContent = null;

                            var icon = EditorGUIUtility.IconContent("BuildSettings." + targetGroup.ToIconName());
                            if (icon != null)
                            {
                                buttonContent = new GUIContent(icon.image, targetGroup.ToString());
                            }
                            else
                            {
                                buttonContent = new GUIContent(targetGroup.ToString()[0].ToString(), targetGroup.ToString());
                            }

                            if (GUILayout.Button(buttonContent, platformButtonStyle, GUILayout.Width(24), GUILayout.Height(18)))
                            {
                                if (hasFlag)
                                {
                                    directive.targets &= ~targetGroup;
                                }
                                else
                                {
                                    directive.targets |= targetGroup;
                                }
                            }

                            GUI.backgroundColor = guiBackgroundColor;
                            GUI.color = guiColor;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(7);

                    
                    if (!directive.enabled)
                    {
                        GUI.backgroundColor = new Color(1, 1, 1, 0.5f);
                    }

                    EditorGUILayout.BeginHorizontal(platformsStyles, GUILayout.Height(24), GUILayout.Width(80));
                    {
                        GUI.backgroundColor = guiBackgroundColor;

                        GUILayout.Space(25);
                        directive.enabled = GUILayout.Toggle(directive.enabled, new GUIContent(), toggleStyle);
                    }                    
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();
            }

            RenderNewDirectiveLine();

            GUI.color = new Color(0.75f, 0.75f, 0.75f);
            EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(24), GUILayout.ExpandWidth(true));
            {
                GUI.color = guiColor;

                GUILayout.Label("", GUILayout.Width(31));

                if (GUILayout.Button("Apply", GUILayout.Width(250))) SaveDirectives(directives);

                GUILayout.Space(2);

                if (GUILayout.Button("Revert", GUILayout.Width(370))) Reload();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();

            if (directivesToRemove.Any())
            {
                foreach (var directiveToRemove in directivesToRemove)
                {
                    directives.Remove(directiveToRemove);
                }
            }
        }

        void RenderTableHeader()
        {
            var style = new GUIStyle(EditorStyles.toolbar);
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 12;
            style.alignment = TextAnchor.MiddleCenter;
            style.fixedHeight = 0;

            GUI.color = new Color(0.5f, 0.5f, 0.5f);

            EditorGUILayout.BeginHorizontal(style, GUILayout.Height(20));

            GUI.color = guiColor;

            EditorGUILayout.LabelField("", GUILayout.Width(31));

            GUILayout.Space(4);

            EditorGUILayout.LabelField("Directive", style, GUILayout.Width(248), GUILayout.Height(20));

            GUILayout.Space(4);

            EditorGUILayout.LabelField("Platforms", style, GUILayout.Width(370), GUILayout.Height(20));

            GUILayout.Space(4);

            EditorGUILayout.LabelField("Enabled", style, GUILayout.Width(80), GUILayout.Height(20));

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        void RenderNewDirectiveLine()
        {
            var directiveLineStyle = new GUIStyle(EditorStyles.toolbar);
            directiveLineStyle.fixedHeight = 0;
            directiveLineStyle.padding = new RectOffset(8, 8, 0, 0);

            var addButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            addButtonStyle.fixedHeight = 0;
            addButtonStyle.margin = new RectOffset(0, 0, 1, 1);

            GUI.color = new Color(0.75f, 0.75f, 0.75f);

            EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(24));

            GUI.color = guiColor;

            if (GUILayout.Button(new GUIContent("+", "Add new Directive"), addButtonStyle, GUILayout.Width(32), GUILayout.Height(24)))
            {
                var lastDirective = directives.LastOrDefault();
                var newDirective = new Directive();

                if (lastDirective != null)
                {
                    newDirective.targets = lastDirective.targets;
                }
                directives.Add(newDirective);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        void Reload()
        {
            directives = LoadDirectives();
        }
    }
}