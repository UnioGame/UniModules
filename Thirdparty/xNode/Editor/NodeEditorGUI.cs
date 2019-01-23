using System;
using System.Collections.Generic;
using System.Linq;
using UniEditorTools;
using UniStateMachine.Nodes;
using UnityEditor;
using UnityEngine;

namespace XNodeEditor {
    /// <summary> Contains GUI methods </summary>
    public partial class NodeEditorWindow {
        public NodeGraphEditor graphEditor;
        private List<UnityEngine.Object> selectionCache;
        private List<XNode.Node> culledNodes;
        private int topPadding { get { return isDocked() ? 19 : 22; } }

        private void OnGUI() {
            var e = Event.current;
            var m = GUI.matrix;
            if (graph == null) return;
            graphEditor = NodeGraphEditor.GetEditor(graph);
            graphEditor.position = position;

            Controls();

            DrawGrid(position, zoom, panOffset);
            DrawConnections();
            DrawDraggedConnection();
            DrawNodes();
            DrawSelectionBox();
            DrawTooltip();
            DrawGraphsButtons();
            
            graphEditor.OnGUI();

            GUI.matrix = m;
        }

        public static void BeginZoomed(Rect rect, float zoom, float topPadding) {
            GUI.EndClip();

            GUIUtility.ScaleAroundPivot(Vector2.one / zoom, rect.size * 0.5f);
            var padding = new Vector4(0, topPadding, 0, 0);
            padding *= zoom;
            GUI.BeginClip(new Rect(-((rect.width * zoom) - rect.width) * 0.5f, -(((rect.height * zoom) - rect.height) * 0.5f) + (topPadding * zoom),
                rect.width * zoom,
                rect.height * zoom));
        }

        public static void EndZoomed(Rect rect, float zoom, float topPadding) {
            GUIUtility.ScaleAroundPivot(Vector2.one * zoom, rect.size * 0.5f);
            var offset = new Vector3(
                (((rect.width * zoom) - rect.width) * 0.5f),
                (((rect.height * zoom) - rect.height) * 0.5f) + (-topPadding * zoom) + topPadding,
                0);
            GUI.matrix = Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);
        }

        public void DrawGrid(Rect rect, float zoom, Vector2 panOffset) {

            rect.position = Vector2.zero;

            var center = rect.size / 2f;
            var gridTex = graphEditor.GetGridTexture();
            var crossTex = graphEditor.GetSecondaryGridTexture();

            // Offset from origin in tile units
            var xOffset = -(center.x * zoom + panOffset.x) / gridTex.width;
            var yOffset = ((center.y - rect.size.y) * zoom + panOffset.y) / gridTex.height;

            var tileOffset = new Vector2(xOffset, yOffset);

            // Amount of tiles
            var tileAmountX = Mathf.Round(rect.size.x * zoom) / gridTex.width;
            var tileAmountY = Mathf.Round(rect.size.y * zoom) / gridTex.height;

            var tileAmount = new Vector2(tileAmountX, tileAmountY);

            // Draw tiled background
            GUI.DrawTextureWithTexCoords(rect, gridTex, new Rect(tileOffset, tileAmount));
            GUI.DrawTextureWithTexCoords(rect, crossTex, new Rect(tileOffset + new Vector2(0.5f, 0.5f), tileAmount));
        }

        public void DrawSelectionBox() {
            if (currentActivity == NodeActivity.DragGrid) {
                var curPos = WindowToGridPosition(Event.current.mousePosition);
                var size = curPos - dragBoxStart;
                var r = new Rect(dragBoxStart, size);
                r.position = GridToWindowPosition(r.position);
                r.size /= zoom;
                Handles.DrawSolidRectangleWithOutline(r, new Color(0, 0, 0, 0.1f), new Color(1, 1, 1, 0.6f));
            }
        }

        public static bool DropdownButton(string name, float width) {
            return GUILayout.Button(name, EditorStyles.toolbarDropDown, GUILayout.Width(width));
        }

        /// <summary> Show right-click context menu for hovered reroute </summary>
        void ShowRerouteContextMenu(RerouteReference reroute) {
            var contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Remove"), false, () => reroute.RemovePoint());
            contextMenu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        }

        /// <summary> Show right-click context menu for hovered port </summary>
        void ShowPortContextMenu(XNode.NodePort hoveredPort) {
            var contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Clear Connections"), false, () => hoveredPort.ClearConnections());
            contextMenu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        }

        /// <summary> Show right-click context menu for selected nodes </summary>
        public void ShowNodeContextMenu() {
            var contextMenu = new GenericMenu();
            // If only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node) {
                var node = Selection.activeObject as XNode.Node;
                contextMenu.AddItem(new GUIContent("Move To Top"), false, () => MoveNodeToTop(node));
                contextMenu.AddItem(new GUIContent("Rename"), false, RenameSelectedNode);
            }

            contextMenu.AddItem(new GUIContent("Duplicate"), false, DublicateSelectedNodes);
            contextMenu.AddItem(new GUIContent("Remove"), false, RemoveSelectedNodes);

            // If only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node) {
                var node = Selection.activeObject as XNode.Node;
                AddCustomContextMenuItems(contextMenu, node);
            }

            contextMenu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
        }

        /// <summary> Show right-click context menu for current graph </summary>
        void ShowGraphContextMenu() {
            var contextMenu = new GenericMenu();
            var pos = WindowToGridPosition(Event.current.mousePosition);
            for (var i = 0; i < nodeTypes.Length; i++) {
                var type = nodeTypes[i];

                //Get node context menu path
                var path = graphEditor.GetNodeMenuName(type);
                if (string.IsNullOrEmpty(path)) continue;

                contextMenu.AddItem(new GUIContent(path), false, () => {
                    CreateNode(type, pos);
                });
            }
            contextMenu.AddSeparator("");
            contextMenu.AddItem(new GUIContent("Preferences"), false, () => OpenPreferences());
            AddCustomContextMenuItems(contextMenu, graph);
            contextMenu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
        }

        void AddCustomContextMenuItems(GenericMenu contextMenu, object obj) {
            var items = GetContextMenuMethods(obj);
            if (items.Length != 0) {
                contextMenu.AddSeparator("");
                for (var i = 0; i < items.Length; i++) {
                    var kvp = items[i];
                    contextMenu.AddItem(new GUIContent(kvp.Key.menuItem), false, () => kvp.Value.Invoke(obj, null));
                }
            }
        }

        /// <summary> Draw a bezier from startpoint to endpoint, both in grid coordinates </summary>
        public void DrawConnection(Vector2 startPoint, Vector2 endPoint, Color col) {
            startPoint = GridToWindowPosition(startPoint);
            endPoint = GridToWindowPosition(endPoint);

            switch (NodeEditorPreferences.GetSettings().noodleType) {
                case NodeEditorPreferences.NoodleType.Curve:
                    var startTangent = startPoint;
                    if (startPoint.x < endPoint.x) startTangent.x = Mathf.LerpUnclamped(startPoint.x, endPoint.x, 0.7f);
                    else startTangent.x = Mathf.LerpUnclamped(startPoint.x, endPoint.x, -0.7f);

                    var endTangent = endPoint;
                    if (startPoint.x > endPoint.x) endTangent.x = Mathf.LerpUnclamped(endPoint.x, startPoint.x, -0.7f);
                    else endTangent.x = Mathf.LerpUnclamped(endPoint.x, startPoint.x, 0.7f);
                    Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, col, null, 4);
                    break;
                case NodeEditorPreferences.NoodleType.Line:
                    Handles.color = col;
                    Handles.DrawAAPolyLine(5, startPoint, endPoint);
                    break;
                case NodeEditorPreferences.NoodleType.Angled:
                    Handles.color = col;
                    if (startPoint.x <= endPoint.x - (50 / zoom)) {
                        var midpoint = (startPoint.x + endPoint.x) * 0.5f;
                        var start_1 = startPoint;
                        var end_1 = endPoint;
                        start_1.x = midpoint;
                        end_1.x = midpoint;
                        Handles.DrawAAPolyLine(5, startPoint, start_1);
                        Handles.DrawAAPolyLine(5, start_1, end_1);
                        Handles.DrawAAPolyLine(5, end_1, endPoint);
                    } else {
                        var midpoint = (startPoint.y + endPoint.y) * 0.5f;
                        var start_1 = startPoint;
                        var end_1 = endPoint;
                        start_1.x += 25 / zoom;
                        end_1.x -= 25 / zoom;
                        var start_2 = start_1;
                        var end_2 = end_1;
                        start_2.y = midpoint;
                        end_2.y = midpoint;
                        Handles.DrawAAPolyLine(5, startPoint, start_1);
                        Handles.DrawAAPolyLine(5, start_1, start_2);
                        Handles.DrawAAPolyLine(5, start_2, end_2);
                        Handles.DrawAAPolyLine(5, end_2, end_1);
                        Handles.DrawAAPolyLine(5, end_1, endPoint);
                    }
                    break;
            }
        }

        /// <summary> Draws all connections </summary>
        public void DrawConnections() {
            var mousePos = Event.current.mousePosition;
            var selection = preBoxSelectionReroute != null ? new List<RerouteReference>(preBoxSelectionReroute) : new List<RerouteReference>();
            hoveredReroute = new RerouteReference();

            var col = GUI.color;
            foreach (var node in graph.nodes) {
                //If a null node is found, return. This can happen if the nodes associated script is deleted. It is currently not possible in Unity to delete a null asset.
                if (node == null) continue;

                // Draw full connections and output > reroute
                foreach (var output in node.Outputs) {
                    
                    //Needs cleanup. Null checks are ugly
                    var item = _portConnectionPoints.FirstOrDefault(x => x.Key.Id == output.Id);
                    if (item.Key == null) continue;

                    var fromRect = item.Value;    
                    var connectionColor = graphEditor.GetTypeColor(output.ValueType);

                    for (var k = 0; k < output.ConnectionCount; k++) {
                        var input = output.GetConnection(k);

                        // Error handling
                        if (input == null) continue; //If a script has been updated and the port doesn't exist, it is removed and null is returned. If this happens, return.
                        if (!input.IsConnectedTo(output)) input.Connect(output);
                        Rect toRect;
                        if (!_portConnectionPoints.TryGetValue(input, out toRect)) continue;

                        var from = fromRect.center;
                        var to = Vector2.zero;
                        var reroutePoints = output.GetReroutePoints(k);
                        // Loop through reroute points and draw the path
                        for (var i = 0; i < reroutePoints.Count; i++) {
                            to = reroutePoints[i];
                            DrawConnection(from, to, connectionColor);
                            from = to;
                        }
                        to = toRect.center;

                        DrawConnection(from, to, connectionColor);

                        // Loop through reroute points again and draw the points
                        for (var i = 0; i < reroutePoints.Count; i++) {
                            var rerouteRef = new RerouteReference(output, k, i);
                            // Draw reroute point at position
                            var rect = new Rect(reroutePoints[i], new Vector2(12, 12));
                            rect.position = new Vector2(rect.position.x - 6, rect.position.y - 6);
                            rect = GridToWindowRect(rect);

                            // Draw selected reroute points with an outline
                            if (selectedReroutes.Contains(rerouteRef)) {
                                GUI.color = NodeEditorPreferences.GetSettings().highlightColor;
                                GUI.DrawTexture(rect, NodeEditorResources.dotOuter);
                            }

                            GUI.color = connectionColor;
                            GUI.DrawTexture(rect, NodeEditorResources.dot);
                            if (rect.Overlaps(selectionBox)) selection.Add(rerouteRef);
                            if (rect.Contains(mousePos)) hoveredReroute = rerouteRef;

                        }
                    }
                }
            }
            GUI.color = col;
            if (Event.current.type != EventType.Layout && currentActivity == NodeActivity.DragGrid) selectedReroutes = selection;
        }

        private Vector2 _nodeGraphScroll;
        private bool _showGraphsList = false;
        
        private void DrawGraphsButtons()
        {
           
            if (NodeGraphs == null)
            {
                UpdateEditorNodeGraphs();
            }
            
            EditorDrawerUtils.DrawVertialLayout(() =>
            {
                EditorGUILayout.Separator();
                EditorGUILayout.Space();
                
                EditorDrawerUtils.DrawButton(_showGraphsList ? "hide graphs" : "show graphs",
                    () => _showGraphsList = !_showGraphsList);
            
                if (!_showGraphsList)
                    return;
            
                EditorDrawerUtils.DrawButton("Update Graphs", UpdateEditorNodeGraphs, GUILayout.Width(100));
            
                EditorGUILayout.Separator();
                EditorGUILayout.Space();
            
                EditorDrawerUtils.DrawScroll(_nodeGraphScroll, () =>
                {
                    NodeGraphs.RemoveAll(x => !x);
                    foreach (var nodeGraph in NodeGraphs)
                    {
                        EditorDrawerUtils.DrawButton(nodeGraph.name,() => Open(nodeGraph), GUILayout.Width(100));
                    }
                    
                }, GUILayout.ExpandWidth(true));

                
            },GUILayout.Width(100));

            EditorDrawerUtils.DrawVertialLayout(() =>
            {
                EditorDrawerUtils.DrawButton("Stop All", () =>
                {
                    foreach (var graph in NodeGraphs)
                    {
                        graph.Dispose();
                    }
                });
            },GUILayout.Width(100));
            
        }
        
        private void DrawNodes() {
            var e = Event.current;
            if (e.type == EventType.Layout) {
                selectionCache = new List<UnityEngine.Object>(Selection.objects);
            }

            //Active node is hashed before and after node GUI to detect changes
            var nodeHash = 0;
            System.Reflection.MethodInfo onValidate = null;
            if (Selection.activeObject != null && Selection.activeObject is XNode.Node) {
                onValidate = Selection.activeObject.GetType().GetMethod("OnValidate");
                if (onValidate != null) nodeHash = Selection.activeObject.GetHashCode();
            }

            BeginZoomed(position, zoom, topPadding);

            var mousePos = Event.current.mousePosition;

            if (e.type != EventType.Layout) {
                hoveredNode = null;
                hoveredPort = null;
            }

            var preSelection = preBoxSelection != null ? new List<UnityEngine.Object>(preBoxSelection) : new List<UnityEngine.Object>();

            // Selection box stuff
            var boxStartPos = GridToWindowPositionNoClipped(dragBoxStart);
            var boxSize = mousePos - boxStartPos;
            if (boxSize.x < 0) { boxStartPos.x += boxSize.x; boxSize.x = Mathf.Abs(boxSize.x); }
            if (boxSize.y < 0) { boxStartPos.y += boxSize.y; boxSize.y = Mathf.Abs(boxSize.y); }
            var selectionBox = new Rect(boxStartPos, boxSize);

            //Save guiColor so we can revert it
            var guiColor = GUI.color;

            if (e.type == EventType.Layout) culledNodes = new List<XNode.Node>();
            for (var n = 0; n < graph.nodes.Count; n++) {
                // Skip null nodes. The user could be in the process of renaming scripts, so removing them at this point is not advisable.
                if (graph.nodes[n] == null) continue;
                if (n >= graph.nodes.Count) return;
                var node = graph.nodes[n];

                // Culling
                if (e.type == EventType.Layout) {
                    // Cull unselected nodes outside view
                    if (!Selection.Contains(node) && ShouldBeCulled(node)) {
                        culledNodes.Add(node);
                        continue;
                    }
                } else if (culledNodes.Contains(node)) continue;

                if (e.type == EventType.Repaint) {
                    _portConnectionPoints = _portConnectionPoints.Where(x => x.Key.node != node).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }

                var nodeEditor = NodeEditor.GetEditor(node);

                NodeEditor.portPositions = new Dictionary<XNode.NodePort, Vector2>();

                //Get node position
                var nodePos = GridToWindowPositionNoClipped(node.position);

                GUILayout.BeginArea(new Rect(nodePos, new Vector2(nodeEditor.GetWidth(), 4000)));

                var selected = selectionCache.Contains(graph.nodes[n]);

                if (selected) {
                    var style = new GUIStyle(nodeEditor.GetBodyStyle());
                    var highlightStyle = new GUIStyle(NodeEditorResources.styles.nodeHighlight);
                    highlightStyle.padding = style.padding;
                    style.padding = new RectOffset();
                    GUI.color = nodeEditor.GetTint();
                    GUILayout.BeginVertical(style);
                    GUI.color = NodeEditorPreferences.GetSettings().highlightColor;
                    GUILayout.BeginVertical(new GUIStyle(highlightStyle));
                } else {
                    var style = new GUIStyle(nodeEditor.GetBodyStyle());
                    GUI.color = nodeEditor.GetTint();
                    GUILayout.BeginVertical(style);
                }

                GUI.color = guiColor;
                EditorGUI.BeginChangeCheck();

                //Draw node contents
                nodeEditor.OnHeaderGUI();
                nodeEditor.OnBodyGUI();

                //If user changed a value, notify other scripts through onUpdateNode
                if (EditorGUI.EndChangeCheck()) {
                    if (NodeEditor.onUpdateNode != null) NodeEditor.onUpdateNode(node);
                    EditorUtility.SetDirty(node);
                    nodeEditor.serializedObject.ApplyModifiedProperties();
                }

                GUILayout.EndVertical();

                //Cache data about the node for next frame
                if (e.type == EventType.Repaint) {
                    var size = GUILayoutUtility.GetLastRect().size;
                    if (nodeSizes.ContainsKey(node)) nodeSizes[node] = size;
                    else nodeSizes.Add(node, size);

                    foreach (var kvp in NodeEditor.portPositions) {
                        var portHandlePos = kvp.Value;
                        portHandlePos += node.position;
                        var rect = new Rect(portHandlePos.x - 8, portHandlePos.y - 8, 16, 16);
                        if (portConnectionPoints.ContainsKey(kvp.Key)) portConnectionPoints[kvp.Key] = rect;
                        else portConnectionPoints.Add(kvp.Key, rect);
                    }
                }

                if (selected) GUILayout.EndVertical();

                if (e.type != EventType.Layout) {
                    //Check if we are hovering this node
                    var nodeSize = GUILayoutUtility.GetLastRect().size;
                    var windowRect = new Rect(nodePos, nodeSize);
                    if (windowRect.Contains(mousePos)) hoveredNode = node;

                    //If dragging a selection box, add nodes inside to selection
                    if (currentActivity == NodeActivity.DragGrid) {
                        if (windowRect.Overlaps(selectionBox)) preSelection.Add(node);
                    }

                    //Check if we are hovering any of this nodes ports
                    //Check input ports
                    foreach (var input in node.Inputs) {
                        //Check if port rect is available
                        if (!portConnectionPoints.ContainsKey(input)) continue;
                        var r = GridToWindowRectNoClipped(portConnectionPoints[input]);
                        if (r.Contains(mousePos)) hoveredPort = input;
                    }
                    //Check all output ports
                    foreach (var output in node.Outputs) {
                        //Check if port rect is available
                        if (!portConnectionPoints.ContainsKey(output)) continue;
                        var r = GridToWindowRectNoClipped(portConnectionPoints[output]);
                        if (r.Contains(mousePos)) hoveredPort = output;
                    }
                }

                GUILayout.EndArea();
            }

            if (e.type != EventType.Layout && currentActivity == NodeActivity.DragGrid) Selection.objects = preSelection.ToArray();
            EndZoomed(position, zoom, topPadding);

            //If a change in hash is detected in the selected node, call OnValidate method. 
            //This is done through reflection because OnValidate is only relevant in editor, 
            //and thus, the code should not be included in build.
            if (nodeHash != 0) {
                if (onValidate != null && nodeHash != Selection.activeObject.GetHashCode()) onValidate.Invoke(Selection.activeObject, null);
            }
        }

        private bool ShouldBeCulled(XNode.Node node) {

            var nodePos = GridToWindowPositionNoClipped(node.position);
            if (nodePos.x / _zoom > position.width) return true; // Right
            else if (nodePos.y / _zoom > position.height) return true; // Bottom
            else if (nodeSizes.ContainsKey(node)) {
                var size = nodeSizes[node];
                if (nodePos.x + size.x < 0) return true; // Left
                else if (nodePos.y + size.y < 0) return true; // Top
            }
            return false;
        }

        private void DrawTooltip() {
            if (hoveredPort != null) {
                var type = hoveredPort.ValueType;
                var content = new GUIContent();
                content.text = type.PrettyName();
                if (hoveredPort.IsOutput) {
                    var obj = hoveredPort.node.GetValue(hoveredPort);
                    content.text += " = " + (obj != null ? obj.ToString() : "null");
                }
                var size = NodeEditorResources.styles.tooltip.CalcSize(content);
                var rect = new Rect(Event.current.mousePosition - (size), size);
                EditorGUI.LabelField(rect, content, NodeEditorResources.styles.tooltip);
                Repaint();
            }
        }
    }
}