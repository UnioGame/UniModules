namespace UniGame.Shared.Editor.Hierarchy
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public static class HierarchyHeader
    {
        private const string HeaderPrefix = "--";
        
        private static GUIStyle headerStyle;
        private static char[] trimChars;
        
        private static readonly Color HeaderColor = new Color(0.38f, 0.38f, 0.38f, 1.0f);

        private static GUIStyle HeaderStyle
        {
            get
            {
                if (headerStyle == null)
                {
                    headerStyle = new GUIStyle
                    {
                        normal =
                        {
                            background = CreateSinglePixelTexture(HeaderColor), 
                            textColor = Color.white
                        }, 
                        alignment = TextAnchor.MiddleCenter, 
                        fontStyle = FontStyle.Bold
                    };
                }
                else if (headerStyle.normal.background == null)
                {
                    headerStyle.normal.background = CreateSinglePixelTexture(HeaderColor);
                }

                return headerStyle;
            }
        }

        private static char[] TrimChars => trimChars ??= " -=".ToCharArray();

        static HierarchyHeader()
        {
            HierarchyItemDrawer.Register("HierarchyHeader", OnHierarchyItem, -10000);
        }

        [MenuItem("GameObject/Create Header", priority = 1)]
        public static GameObject Create()
        {
            var header = new GameObject($"{HeaderPrefix}Header")
            {
                tag = "EditorOnly"
            };
            
            var active = Selection.activeGameObject;
            if (active != null)
            {
                header.transform.SetParent(active.transform.parent);
                header.transform.SetSiblingIndex(active.transform.GetSiblingIndex());
            }
            
            Undo.RegisterCreatedObjectUndo(header, header.name);
            Selection.activeGameObject = header;
            
            return header;
        }

        private static void OnHierarchyItem(HierarchyItem item)
        {
            var go = item.GameObject;
            if (go == null) 
                return;

            var name = go.name;
            var prefix = HeaderPrefix;

            if (name.Length < prefix.Length) 
                return;
            
            if (prefix.Where((t, i) => name[i] != t).Any())
                return;

            if (Event.current.type == EventType.Repaint)
            {
                var rect = item.Rect;
                var r = new Rect(32, rect.y, rect.xMax - 16, rect.height);

                var start = prefix.Length;
                var end = name.Length;

                for (var i = start; i < name.Length; i++)
                {
                    var c = name[i];
                    int j;
                    for (j = 0; j < TrimChars.Length; j++)
                    {
                        if (TrimChars[j] != c)
                            continue;

                        start++;
                        break;
                    }
                    if (j == TrimChars.Length) break;
                }

                for (var i = end - 1; i > start; i--)
                {
                    var c = name[i];
                    int j;
                    for (j = 0; j < TrimChars.Length; j++)
                    {
                        if (TrimChars[j] != c)
                            continue;

                        end--;
                        break;
                    }
                    if (j == TrimChars.Length) break;
                }

                name = name.Substring(start, end - start);

                var content = new GUIContent
                {
                    text = name,
                    tooltip = null
                };
                HeaderStyle.Draw(r, content, 0, false, false);
            }

            HierarchyItemDrawer.StopCurrentRowGUI();
        }

        private static Texture2D CreateSinglePixelTexture(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            
            return texture;
        }
    }
}