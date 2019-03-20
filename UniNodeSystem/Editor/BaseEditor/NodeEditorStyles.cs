using UnityEngine;

namespace UniNodeSystemEditor
{
    
    public class NodeEditorStyles
    {
        public GUIStyle inputPort;
        public GUIStyle nodeHeader;
        public GUIStyle nodeBody;
        public GUIStyle tooltip;
        public GUIStyle nodeHighlight;

        public NodeEditorStyles()
        {
            var baseStyle = new GUIStyle("Label");
            baseStyle.fixedHeight = 18;

            inputPort = new GUIStyle(baseStyle);
            inputPort.alignment = TextAnchor.UpperLeft;
            inputPort.padding.left = 10;

            nodeHeader = new GUIStyle();
            nodeHeader.alignment = TextAnchor.MiddleCenter;
            nodeHeader.fontStyle = FontStyle.Bold;
            nodeHeader.normal.textColor = Color.white;

            nodeBody = new GUIStyle();
            nodeBody.normal.background = NodeEditorResources.nodeBody;
                
            nodeBody.border = new RectOffset(32, 32, 32, 32);
            nodeBody.padding = new RectOffset(16, 16, 4, 16);

            nodeHighlight = new GUIStyle();
            nodeHighlight.normal.background = NodeEditorResources.nodeHighlight;
            nodeHighlight.border = new RectOffset(32, 32, 32, 32);

            tooltip = new GUIStyle("helpBox");
            tooltip.alignment = TextAnchor.MiddleCenter;
        }
    }
}