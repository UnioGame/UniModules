namespace UniGreenModules.UniNodeSystem.Inspector.Editor.BaseEditor
{
    using UnityEditor;
    using UnityEngine;

    public static partial class NodeEditorResources
    {
        // Textures
        public static Texture2D dot
        {
            get { return _dot != null ? _dot : _dot = Resources.Load<Texture2D>("node_dot"); }
        }

        private static Texture2D _dot;

        public static Texture2D dotOuter
        {
            get
            {
                return _dotOuter != null ? _dotOuter : _dotOuter = Resources.Load<Texture2D>("node_dot_outer");
            }
        }

        private static Texture2D _dotOuter;

        public static Texture2D nodeBody
        {
            get { return _nodeBody != null ? _nodeBody : _nodeBody = Resources.Load<Texture2D>("node_node"); }
        }

        private static Texture2D _nodeBody;

        public static Texture2D nodeHighlight
        {
            get
            {
                return _nodeHighlight != null
                    ? _nodeHighlight
                    : _nodeHighlight = Resources.Load<Texture2D>("node_node_highlight");
            }
        }

        private static Texture2D _nodeHighlight;

        // Styles
        public static NodeEditorStyles styles
        {
            get { return _styles != null ? _styles : _styles = new NodeEditorStyles(); }
        }

        public static NodeEditorStyles _styles = null;

        public static GUIStyle OutputPort
        {
            get { return new GUIStyle(EditorStyles.label) {alignment = TextAnchor.UpperRight}; }
        }

        public static Texture2D GenerateGridTexture(Color line, Color bg)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = bg;
                    if (y % 16 == 0 || x % 16 == 0) col = Color.Lerp(line, bg, 0.65f);
                    if (y == 63 || x == 63) col = Color.Lerp(line, bg, 0.35f);
                    cols[(y * 64) + x] = col;
                }
            }

            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }

        public static Texture2D GenerateCrossTexture(Color line)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = line;
                    if (y != 31 && x != 31) col.a = 0;
                    cols[(y * 64) + x] = col;
                }
            }

            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }
    }
}