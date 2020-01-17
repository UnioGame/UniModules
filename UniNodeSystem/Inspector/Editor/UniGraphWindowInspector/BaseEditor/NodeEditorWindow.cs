namespace UniGreenModules.UniNodeSystem.Inspector.Editor.BaseEditor
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Core;
    using UniCore.EditorTools.Editor.AssetOperations;
    using UniCore.EditorTools.Editor.PrefabTools;
    using UniNodeSystem.Nodes;
    using UniResourceSystem.Editor;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEngine;

    [InitializeOnLoad]
    public partial class NodeEditorWindow : EditorWindow
    {
        public const string ActiveGraphPath = "ActiveGraphPath";
        public const string UniNodesWindowTitle = "UniNodes";

        public static NodeEditorWindow Current;
        
        public static NodeGraph LastEditorGraph;
        
        private Dictionary<ulong, NodePort> _portsIds = new Dictionary<ulong, NodePort>();
        private Dictionary<NodePort, Rect> _portConnectionPoints = new Dictionary<NodePort, Rect>();
        private Dictionary<UniBaseNode, Vector2> _nodeSizes = new Dictionary<UniBaseNode, Vector2>();

        private float _zoom = 1;
        private Vector2 _panOffset;

        [SerializeField] private NodePortReference[] _references = new NodePortReference[0];
        [SerializeField] private Rect[] _rects = new Rect[0];
        
        public static List<UniGraphAsset> NodeGraphs { get; protected set; } = new List<UniGraphAsset>();
        public static HashSet<NodeEditorWindow> ActiveWindows { get; protected set; } = new HashSet<NodeEditorWindow>();
        
        
        public NodeGraph ActiveGraph;

        /// <summary> Stores node positions for all nodePorts. </summary>
        public Dictionary<NodePort, Rect> PortConnectionPoints => _portConnectionPoints;

        public Dictionary<UniBaseNode, Vector2> NodeSizes => _nodeSizes;

        public string Title { get; protected set; }

        public Vector2 PanOffset
        {
            get => _panOffset;
            set
            {
                _panOffset = value;
                Repaint();
            }
        }

        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = Mathf.Clamp(value, 1f, 5f);
                Repaint();
            }
        }

        #region public static methods
        
        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
            var nodeGraph = EditorUtility.InstanceIDToObject(instanceID) as NodeGraph;
            return nodeGraph != null && Open(nodeGraph);
        }

        public static NodeEditorWindow Open(NodeGraph nodeGraph,bool replaceActive = false)
        {
            if (Current != null) 
                Current.Save();
            
            if (!nodeGraph) return null;

            var window = SelectWindow(nodeGraph,replaceActive);
            
            window.Initialize(nodeGraph);
            window.Show();
            return window;
        }

        private static NodeEditorWindow SelectWindow(NodeGraph nodeGraph,bool replaceActive)
        {
            var window = ActiveWindows.FirstOrDefault(x => x.ActiveGraph == nodeGraph);
            if (window) return window;
            
            window = ActiveWindows.FirstOrDefault(x => !x.ActiveGraph);
            if (window) return window;
            
            window = replaceActive && Current!=null ? Current :
                CreateInstance<NodeEditorWindow>();
            
            return window;
        }

        public static void UpdateEditorNodeGraphs()
        {
            NodeGraphs = AssetEditorTools.GetAssets<UniGraphAsset>();
        }

        /// <summary> Repaint all open NodeEditorWindows. </summary>
        public static void RepaintAll()
        {
            var windows = Resources.FindObjectsOfTypeAll<NodeEditorWindow>();
            for (var i = 0; i < windows.Length; i++)
            {
                windows[i].Repaint();
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
        
        #endregion

        public void Initialize(NodeGraph nodeGraph)
        {
            PortConnectionPoints.Clear();
            titleContent   = new GUIContent(nodeGraph.name);
            Title = nodeGraph.name;
            wantsMouseMove = true;
            ActiveGraph          = nodeGraph;
        }
        
        public void Save()
        {
            Save(ActiveGraph);
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
            var existingGraph = AssetDatabase.LoadAssetAtPath<NodeGraph>(path);
            if (existingGraph != null) AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(ActiveGraph, path);
            EditorUtility.SetDirty(ActiveGraph);
            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        }

        public Vector2 WindowToGridPosition(Vector2 windowPosition)
        {
            return (windowPosition - (position.size * 0.5f) - (PanOffset / Zoom)) * Zoom;
        }

        public Vector2 GridToWindowPosition(Vector2 gridPosition)
        {
            return (position.size * 0.5f) + (PanOffset / Zoom) + (gridPosition / Zoom);
        }

        public Rect GridToWindowRectNoClipped(Rect gridRect)
        {
            gridRect.position = GridToWindowPositionNoClipped(gridRect.position);
            return gridRect;
        }

        public Rect GridToWindowRect(Rect gridRect)
        {
            gridRect.position = GridToWindowPosition(gridRect.position);
            gridRect.size /= Zoom;
            return gridRect;
        }

        public Vector2 GridToWindowPositionNoClipped(Vector2 gridPosition)
        {
            var center = position.size * 0.5f;
            var xOffset = (center.x * Zoom + (PanOffset.x + gridPosition.x));
            var yOffset = (center.y * Zoom + (PanOffset.y + gridPosition.y));
            return new Vector2(xOffset, yOffset);
        }

        public void SelectNode(UniBaseNode node, bool add)
        {
            if (add)
            {
                var selection = new List<Object>(Selection.objects);
                selection.Add(node);
                Selection.objects = selection.ToArray();
            }
            else Selection.objects = new Object[] {node};
        }

        public void DeselectNode(UniBaseNode node)
        {
            var selection = new List<Object>(Selection.objects);
            selection.Remove(node);
            Selection.objects = selection.ToArray();
        }

        private static NodeGraph GetGraphItem(string assetPath)
        {
            //var loadedGraphObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            var loadedGraphObject = PrefabUtility.LoadPrefabContents(assetPath);
            var targetGraph = loadedGraphObject.GetComponent<NodeGraph>();
            return targetGraph;
        }

        private void DraggableWindow(int windowID)
        {
            GUI.DragWindow();
        }

        private void OnFocus()
        {
            Current = this;
            graphEditor = NodeGraphEditor.GetEditor(ActiveGraph);
            var settings = NodeEditorPreferences.GetSettings();

            if (graphEditor != null && settings.autoSave)
            {
                return;
                AssetDatabase.SaveAssets();
            }
        }
        
        private void OnDisable()
        {
            ActiveWindows.Remove(this);
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            
            // Cache portConnectionPoints before serialization starts
            var count = PortConnectionPoints.Count;
            _references = new NodePortReference[count];
            _rects = new Rect[count];
            var index = 0;
            
            foreach (var portConnectionPoint in PortConnectionPoints)
            {
                _references[index] = new NodePortReference(portConnectionPoint.Key);
                _rects[index] = portConnectionPoint.Value;
                index++;
            }
        }
        
        private List<Object> selectionAssets = new List<Object>();
        
        private void OnSelectionChange()
        {
            selectionAssets.Clear();
            selectionAssets.AddRange(Selection.objects);
            selectionAssets.AddRange(Selection.gameObjects);
            
            var selections = selectionAssets.OfType<GameObject>();
            
            NodeGraph target = null;
            foreach (var selection in selections) {
                target = selection.GetComponent<NodeGraph>();
                if(target != null) break;
            }
            
            if(target == null) return;
            
            Open(target);
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            ActiveWindows.Add(this);
            
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

        private void OnPlayModeChanged(PlayModeStateChange modeStateChange)
        {
            switch (modeStateChange)
            {
                case PlayModeStateChange.EnteredEditMode:
                    if (LastEditorGraph)
                    {
                        Open(LastEditorGraph);
                    }
                    LastEditorGraph = null;
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    LastEditorGraph = ActiveGraph;
                    ActiveGraph = null;
                    var activeGraph = NodeGraph.ActiveGraphs.FirstOrDefault();
                    Open(activeGraph);
                    break;
            }
        }
        
        
        private NodeGraph Save(NodeGraph nodeGraph)
        {
            if (Application.isPlaying || !nodeGraph)
                return nodeGraph;

            var prefabResource = nodeGraph.GetPrefabDefinition();
            prefabResource.SavePrefab();

            return nodeGraph;
        }

    }
}