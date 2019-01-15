using System.Collections.Generic;
using Assets.UI.Windows.Tools.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XNode;

namespace XNodeEditor 
{

    [InitializeOnLoad]
    public partial class NodeEditorWindow : EditorWindow
    {
        public static List<XNode.NodeGraph> NodeGraphs;
        public static NodeEditorWindow current;

        private Dictionary<ulong, XNode.NodePort> _portsIds = new Dictionary<ulong, XNode.NodePort>();
        private Dictionary<XNode.NodePort, Rect> _portConnectionPoints = new Dictionary<XNode.NodePort, Rect>();

        /// <summary> Stores node positions for all nodePorts. </summary>
        public Dictionary<XNode.NodePort, Rect> portConnectionPoints
        {
            get { return _portConnectionPoints; }
        }

        [SerializeField] private NodePortReference[] _references = new NodePortReference[0];
        [SerializeField] private Rect[] _rects = new Rect[0];

        [System.Serializable]
        private class NodePortReference
        {
            [SerializeField] private XNode.Node _node;
            [SerializeField] private string _name;

            public NodePortReference(XNode.NodePort nodePort)
            {
                _node = nodePort.node;
                _name = nodePort.fieldName;
            }

            public XNode.NodePort GetNodePort()
            {
                if (_node == null)
                {
                    return null;
                }

                return _node.GetPort(_name);
            }
        }

        private void OnDisable()
        {
            // Cache portConnectionPoints before serialization starts
            var count = portConnectionPoints.Count;
            _references = new NodePortReference[count];
            _rects = new Rect[count];
            var index = 0;
            foreach (var portConnectionPoint in portConnectionPoints)
            {
                _references[index] = new NodePortReference(portConnectionPoint.Key);
                _rects[index] = portConnectionPoint.Value;
                index++;
            }
        }

        private void OnEnable()
        {

            // Reload portConnectionPoints if there are any
            var length = _references.Length;
            if (length == _rects.Length)
            {
                for (var i = 0; i < length; i++)
                {
                    var nodePort = _references[i].GetNodePort();
                    if (nodePort != null)
                    {
                        _portsIds[nodePort.Id] = nodePort;
                        _portConnectionPoints.Add(nodePort, _rects[i]);
                    }
                }
            }

            graphEditor?.OnEnable();

        }

        public Dictionary<XNode.Node, Vector2> nodeSizes
        {
            get { return _nodeSizes; }
        }

        private Dictionary<XNode.Node, Vector2> _nodeSizes = new Dictionary<XNode.Node, Vector2>();
        public XNode.NodeGraph graph;

        public Vector2 panOffset
        {
            get { return _panOffset; }
            set
            {
                _panOffset = value;
                Repaint();
            }
        }

        private Vector2 _panOffset;

        public float zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = Mathf.Clamp(value, 1f, 5f);
                Repaint();
            }
        }

        private float _zoom = 1;

        void OnFocus()
        {
            current = this;
            graphEditor = NodeGraphEditor.GetEditor(graph);
            var settings = NodeEditorPreferences.GetSettings();

            if (graphEditor != null && settings.autoSave)
            {
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary> Create editor window </summary>
        public static NodeEditorWindow Init()
        {
            var w = CreateInstance<NodeEditorWindow>();
            w.titleContent = new GUIContent("NodeGraph");
            w.wantsMouseMove = true;
            w.Show();
            return w;
        }

        public void Save()
        {
            if (AssetDatabase.Contains(graph))
            {
                EditorUtility.SetDirty(graph);
                if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
            }
            else SaveAs();
        }

        public void OnInspectorUpdate()
        {
            if (!Application.isPlaying)
                return;
            Repaint();
        }

        public void SaveAs()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save NodeGraph", "NewNodeGraph", "asset", "");
            if (string.IsNullOrEmpty(path)) return;
            else
            {
                var existingGraph = AssetDatabase.LoadAssetAtPath<XNode.NodeGraph>(path);
                if (existingGraph != null) AssetDatabase.DeleteAsset(path);
                AssetDatabase.CreateAsset(graph, path);
                EditorUtility.SetDirty(graph);
                if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
            }
        }

        private void DraggableWindow(int windowID)
        {
            GUI.DragWindow();
        }

        public Vector2 WindowToGridPosition(Vector2 windowPosition)
        {
            return (windowPosition - (position.size * 0.5f) - (panOffset / zoom)) * zoom;
        }

        public Vector2 GridToWindowPosition(Vector2 gridPosition)
        {
            return (position.size * 0.5f) + (panOffset / zoom) + (gridPosition / zoom);
        }

        public Rect GridToWindowRectNoClipped(Rect gridRect)
        {
            gridRect.position = GridToWindowPositionNoClipped(gridRect.position);
            return gridRect;
        }

        public Rect GridToWindowRect(Rect gridRect)
        {
            gridRect.position = GridToWindowPosition(gridRect.position);
            gridRect.size /= zoom;
            return gridRect;
        }

        public Vector2 GridToWindowPositionNoClipped(Vector2 gridPosition)
        {
            var center = position.size * 0.5f;
            var xOffset = (center.x * zoom + (panOffset.x + gridPosition.x));
            var yOffset = (center.y * zoom + (panOffset.y + gridPosition.y));
            return new Vector2(xOffset, yOffset);
        }

        public void SelectNode(XNode.Node node, bool add)
        {
            if (add)
            {
                var selection = new List<Object>(Selection.objects);
                selection.Add(node);
                Selection.objects = selection.ToArray();
            }
            else Selection.objects = new Object[] {node};
        }

        public void DeselectNode(XNode.Node node)
        {
            var selection = new List<Object>(Selection.objects);
            selection.Remove(node);
            Selection.objects = selection.ToArray();
        }

        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {

            var nodeGraph = EditorUtility.InstanceIDToObject(instanceID) as XNode.NodeGraph;
            return nodeGraph != null && Open(nodeGraph);
        }

        public static bool Open(NodeGraph nodeGraph)
        {
            var w = GetWindow(typeof(NodeEditorWindow), false, "xNode", true) as NodeEditorWindow;

            var nodeEditor = w as NodeEditorWindow;
            nodeEditor?.portConnectionPoints.Clear();

            w.wantsMouseMove = true;
            w.graph = nodeGraph;
            
            return true;
        }

        public static void UpdateEditorNodeGraphs()
        {
            NodeGraphs = AssetEditorTools.GetAssets<NodeGraph>();
        }

        /// <summary> Repaint all open NodeEditorWindows. </summary>
        public static void RepaintAll() 
        {
            var windows = Resources.FindObjectsOfTypeAll<NodeEditorWindow>();
            for (var i = 0; i < windows.Length; i++) {
                windows[i].Repaint();
            }
        }
    }
}