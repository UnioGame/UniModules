namespace UniGame.LeoEcs.Debug.Editor.Drawers
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector.Editor;
    using UniModules.UniCore.Runtime.Utils;
    using UnityEditor;
    using UnityEngine;

    public class EntitiesEditorDrawer : OdinValueDrawer<EntityGridEditorView>
    {

        private Color _rectColor = new Color(0.4f, 0.6f, 0.3f);
        private GUILayoutOption[] _options = new[] {GUILayout.ExpandWidth(false)};
        private GUILayoutOption[] _buttonOptions = new[] {GUILayout.ExpandWidth(false),};
        
        //private int rowLimit = 5;
        private int itemsLimit = 500;
        private GUIStyle _buttonStyle = new GUIStyle(GUI.skin.button);
        private GUIContent _buttonContent = new GUIContent();
        private Vector2 _scroll;

        protected override void Initialize()
        {
            base.Initialize();

            _buttonStyle.wordWrap = true;
            _buttonStyle.stretchWidth = false;
            _buttonStyle.fixedHeight = _buttonStyle.lineHeight * 2;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var options = GUILayout.ExpandWidth(false);
            var value = ValueEntry.SmartValue;
            var items = value.items;
            
            Draw(items);
        }

        public string GetKey(IEntityEditorView item)
        {
            var key = $"{item.Name} : {item.Id}";
            return key;
        }

        public void Draw(List<IEntityEditorView> values)
        {
            var width = 0f;

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            EditorGUILayout.LabelField($"Limit to Show: {itemsLimit}",GUILayout.Width(130));
            EditorGUILayout.LabelField($"Total Entities: {values.Count}");
            EditorGUILayout.EndHorizontal();

            var contentWidth = EditorGUIUtility.currentViewWidth;
            
            var itemsMount = Mathf.Min(values.Count, itemsLimit);
            
            GUILayout.BeginVertical(_options);
            _scroll = GUILayout.BeginScrollView(_scroll);
            GUILayout.BeginHorizontal(_options);

            for (var i = 0; i < itemsMount; i++)
            {
                var item = values[i];
                var key = GetKey(item);
                var keyWidth = key.Length * 8f;

                _buttonContent.text = key;

                width += keyWidth;
                
                if (width > 0 && (width > contentWidth && contentWidth > 10))
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(_options);
                    width = 0;
                }

                if(GUILayout.Button(_buttonContent, _buttonStyle,_buttonOptions))
                    item.Show();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}