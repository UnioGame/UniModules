namespace UniGreenModules.UniCore.EditorTools.Editor.Controls
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using Utility;

    public class TabPanelControl
    {
        const float DarkGray = 0.4f;
        const float LightGray = 0.9f;
        const float StartSpace = 10;

        private string[] _options;
        private Action[] _drawActions;
        
        private Color _highlightCol = new Color(LightGray, LightGray, LightGray);
        private Color _bgCol = new Color(DarkGray, DarkGray, DarkGray);
        private GUIStyle _buttonStyle;

        public int Selected;

        #region public methods

        public int Show(string[] options, int selected,Action[] drawActions)
        {
            _options = options;
            if (_options == null) return 0;
            Selected = selected;
            _drawActions = drawActions;
            EditorDrawerUtils.DrawAndRevertColor(Draw);
            return Selected;
        }

        #endregion

        #region private methods

        private void Draw()
        {
            if(_buttonStyle==null)
                _buttonStyle = new GUIStyle(GUI.skin.button);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(StartSpace);  
            OnDrawPanel();
            EditorGUILayout.EndVertical();
        }

        private void OnDrawPanel()
        {
            EditorDrawerUtils.DrawHorizontalLayout(DrawTabs);
            GUILayout.Space(StartSpace);
            ApplyDrawAction(Selected);
        }

        private void DrawTabs()
        {
            for (int i = 0; i < _options.Length; ++i)
            {
                var color = GUI.backgroundColor;
                GUI.backgroundColor = i == Selected ? _highlightCol : _bgCol;
                if (GUILayout.Button(_options[i], _buttonStyle))
                {
                    Selected = i;
                }
                GUI.backgroundColor = color;
            }
        }

        private void ApplyDrawAction(int index)
        {
            if (_drawActions != null && _drawActions.Length > index && _drawActions[index]!=null)
            {
                _drawActions[index]();
            }
        }

        #endregion
    }
}
