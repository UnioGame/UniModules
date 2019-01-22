using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;

namespace XNodeEditor {
    /// <summary> xNode-specific version of <see cref="EditorGUILayout"/> </summary>
    public static class NodeEditorGUILayout {

        private static readonly Dictionary<UnityEngine.Object, Dictionary<string, ReorderableList>> reorderableListCache = new Dictionary<UnityEngine.Object, Dictionary<string, ReorderableList>>();
        private static int reorderableListIndex = -1;
        /// <summary> Make a field for a serialized property. Automatically displays relevant node port. </summary>
        public static void PropertyField(SerializedProperty property, bool includeChildren = true, params GUILayoutOption[] options) {
            PropertyField(property, (GUIContent) null, includeChildren, options);
        }

        /// <summary> Make a field for a serialized property. Automatically displays relevant node port. </summary>
        public static void PropertyField(SerializedProperty property, GUIContent label, bool includeChildren = true, params GUILayoutOption[] options) {
            if (property == null) throw new NullReferenceException();
            var node = property.serializedObject.targetObject as XNode.Node;
            var port = node.GetPort(property.name);
            PropertyField(property, label, port, includeChildren);
        }

        /// <summary> Make a field for a serialized property. Manual node port override. </summary>
        public static void PropertyField(SerializedProperty property, XNode.NodePort port, bool includeChildren = true, params GUILayoutOption[] options) {
            PropertyField(property, null, port, includeChildren, options);
        }

        /// <summary> Make a field for a serialized property. Manual node port override. </summary>
        public static void PropertyField(SerializedProperty property, GUIContent label, XNode.NodePort port, bool includeChildren = true, params GUILayoutOption[] options) {
            if (property == null) throw new NullReferenceException();

            // If property is not a port, display a regular property field
            if (port == null) EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(30));
            else {
                var rect = new Rect();

                // If property is an input, display a regular property field and put a port handle on the left side
                if (port.direction == XNode.PortIO.Input) {
                    // Get data from [Input] attribute
                    var showBacking = XNode.Node.ShowBackingValue.Unconnected;
                    XNode.Node.InputAttribute inputAttribute;
                    var instancePortList = false;
                    if (NodeEditorUtilities.GetAttrib(port.node.GetType(), property.name, out inputAttribute)) {
                        instancePortList = inputAttribute.instancePortList;
                        showBacking = inputAttribute.backingValue;
                    }

                    if (instancePortList) {
                        var type = GetType(property);
                        var connectionType = inputAttribute != null ? inputAttribute.connectionType : XNode.Node.ConnectionType.Multiple;
                        InstancePortList(property.name, type, property.serializedObject, port.direction, connectionType);
                        return;
                    }
                    switch (showBacking) {
                        case XNode.Node.ShowBackingValue.Unconnected:
                            // Display a label if port is connected
                            if (port.IsConnected) EditorGUILayout.LabelField(label != null ? label : new GUIContent(property.displayName));
                            // Display an editable property field if port is not connected
                            else EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(30));
                            break;
                        case XNode.Node.ShowBackingValue.Never:
                            // Display a label
                            EditorGUILayout.LabelField(label != null ? label : new GUIContent(property.displayName));
                            break;
                        case XNode.Node.ShowBackingValue.Always:
                            // Display an editable property field
                            EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(30));
                            break;
                    }

                    rect = GUILayoutUtility.GetLastRect();
                    rect.position = rect.position - new Vector2(16, 0);
                    // If property is an output, display a text label and put a port handle on the right side
                } else if (port.direction == XNode.PortIO.Output) {
                    // Get data from [Output] attribute
                    var showBacking = XNode.Node.ShowBackingValue.Unconnected;
                    XNode.Node.OutputAttribute outputAttribute;
                    var instancePortList = false;
                    if (NodeEditorUtilities.GetAttrib(port.node.GetType(), property.name, out outputAttribute)) {
                        instancePortList = outputAttribute.instancePortList;
                        showBacking = outputAttribute.backingValue;
                    }

                    if (instancePortList) {
                        var type = GetType(property);
                        var connectionType = outputAttribute != null ? outputAttribute.connectionType : XNode.Node.ConnectionType.Multiple;
                        InstancePortList(property.name, type, property.serializedObject, port.direction, connectionType);
                        return;
                    }
                    switch (showBacking) {
                        case XNode.Node.ShowBackingValue.Unconnected:
                            // Display a label if port is connected
                            if (port.IsConnected) EditorGUILayout.LabelField(label != null ? label : new GUIContent(property.displayName), NodeEditorResources.OutputPort, GUILayout.MinWidth(30));
                            // Display an editable property field if port is not connected
                            else EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(30));
                            break;
                        case XNode.Node.ShowBackingValue.Never:
                            // Display a label
                            EditorGUILayout.LabelField(label != null ? label : new GUIContent(property.displayName), NodeEditorResources.OutputPort, GUILayout.MinWidth(30));
                            break;
                        case XNode.Node.ShowBackingValue.Always:
                            // Display an editable property field
                            EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(30));
                            break;
                    }

                    rect = GUILayoutUtility.GetLastRect();
                    rect.position = rect.position + new Vector2(rect.width, 0);
                }

                rect.size = new Vector2(16, 16);

                Color backgroundColor = new Color32(90, 97, 105, 255);
                Color tint;
                if (NodeEditorWindow.nodeTint.TryGetValue(port.node.GetType(), out tint)) backgroundColor *= tint;
                var col = NodeEditorWindow.current.graphEditor.GetTypeColor(port.ValueType);
                DrawPortHandle(rect, backgroundColor, col);

                // Register the handle position
                var portPos = rect.center;
                if (NodeEditor.portPositions.ContainsKey(port)) NodeEditor.portPositions[port] = portPos;
                else NodeEditor.portPositions.Add(port, portPos);
            }
        }

        private static System.Type GetType(SerializedProperty property) {
            var parentType = property.serializedObject.targetObject.GetType();
            var fi = parentType.GetField(property.propertyPath);
            return fi.FieldType;
        }

        /// <summary> Make a simple port field. </summary>
        public static void PortField(NodePort port, params GUILayoutOption[] options) {
            PortField(null, port, options);
        }

        /// <summary> Make a simple port field. </summary>
        public static void PortField(GUIContent label, XNode.NodePort port, params GUILayoutOption[] layoutOptions) 
        {
            if (port == null) return;
            
            var defaultStyle = GetDefaultPortStyle(port);
            defaultStyle.Label = label;
            
            if (layoutOptions != null) 
                defaultStyle.Options = layoutOptions;
            
            PortField(port,defaultStyle);
        }
        
        public static void PortField(XNode.NodePort port, NodeGuiLayoutStyle portStyle) 
        {
            
            if (port == null) return;
            
            if (portStyle.Options == null) 
                portStyle.Options = new GUILayoutOption[] { GUILayout.MinWidth(30) };
            
            Vector2 position = Vector3.zero;
            portStyle.Label = portStyle.Label != null ? portStyle.Label
                : string.IsNullOrEmpty(portStyle.Name) ? new GUIContent(ObjectNames.NicifyVariableName(port.fieldName)) :
                new GUIContent(portStyle.Name);

            // If property is an input, display a regular property field and put a port handle on the left side
            if (port.direction == XNode.PortIO.Input) {
                // Display a label
                EditorGUILayout.LabelField(portStyle.Label, portStyle.Options);

                var rect = GUILayoutUtility.GetLastRect();
                position = rect.position - new Vector2(16, 0);

            }
            // If property is an output, display a text label and put a port handle on the right side
            else if (port.direction == XNode.PortIO.Output) {
                
                // Display a label
                EditorGUILayout.LabelField(portStyle.Label, NodeEditorResources.OutputPort, portStyle.Options);
                var rect = GUILayoutUtility.GetLastRect();
                position = rect.position + new Vector2(rect.width, 0);
                
            }
            
            PortField(position, port,portStyle.Color,portStyle.Background);
        }



        /// <summary> Make a simple port field. </summary>
        public static void PortField(Vector2 position, NodePort port) {
            
            if (port == null) return;

            var col = GetMainPortColor(port);

            PortField(position, port,col);

        }

        public static NodeGuiLayoutStyle GetDefaultPortStyle(NodePort port)
        {
            var name = port == null ? string.Empty : port.fieldName;
            var label = port == null ? new GUIContent(string.Empty) : new GUIContent(port.fieldName);
            
            var style = new NodeGuiLayoutStyle()
            {
                Color = GetMainPortColor(port),
                Background = GetBackgroundPortColor(port),
                Options = new GUILayoutOption[] { GUILayout.MinWidth(30)},
                Label = label,
                Name = name,
            };
            
            return style;
        }
        
        public static Color GetMainPortColor(NodePort port)
        {
            if(port ==null)
                return Color.magenta;
            
            return NodeEditorWindow.current.graphEditor.GetTypeColor(port.ValueType);
        }

        public static Color GetBackgroundPortColor(NodePort port)
        {
            
            Color backgroundColor = new Color32(90, 97, 105, 255);
            if (port == null)
                return backgroundColor;
            
            if (NodeEditorWindow.nodeTint.TryGetValue(port.node.GetType(), out var tint)) backgroundColor *= tint;
            return backgroundColor;
            
        }
        
        public static void PortField(Vector2 position, XNode.NodePort port,Color color) {
            
            if (port == null) return;
            
            var backgroundColor = GetBackgroundPortColor(port);

            PortField(position, port, color, backgroundColor);
        }
        
        public static void PortField(Vector2 position, XNode.NodePort port,Color color, Color backgroundColor) {
            
            if (port == null) return;

            var rect = new Rect(position, new Vector2(16, 16));

            DrawPortHandle(rect, backgroundColor, color);

            // Register the handle position
            var portPos = rect.center;
            if (NodeEditor.portPositions.ContainsKey(port)) NodeEditor.portPositions[port] = portPos;
            else NodeEditor.portPositions.Add(port, portPos);
        }

        /// <summary> Add a port field to previous layout element. </summary>
        public static void AddPortField(XNode.NodePort port) {
            if (port == null) return;
            var rect = new Rect();

            // If property is an input, display a regular property field and put a port handle on the left side
            if (port.direction == XNode.PortIO.Input) {
                rect = GUILayoutUtility.GetLastRect();
                rect.position = rect.position - new Vector2(16, 0);
                // If property is an output, display a text label and put a port handle on the right side
            } else if (port.direction == XNode.PortIO.Output) {
                rect = GUILayoutUtility.GetLastRect();
                rect.position = rect.position + new Vector2(rect.width, 0);
            }

            rect.size = new Vector2(16, 16);

            Color backgroundColor = new Color32(90, 97, 105, 255);
            Color tint;
            if (NodeEditorWindow.nodeTint.TryGetValue(port.node.GetType(), out tint)) backgroundColor *= tint;
            var col = NodeEditorWindow.current.graphEditor.GetTypeColor(port.ValueType);
            DrawPortHandle(rect, backgroundColor, col);

            // Register the handle position
            var portPos = rect.center;
            if (NodeEditor.portPositions.ContainsKey(port)) NodeEditor.portPositions[port] = portPos;
            else NodeEditor.portPositions.Add(port, portPos);
        }

        /// <summary> Draws an input and an output port on the same line </summary>
        public static void PortPair(XNode.NodePort input, XNode.NodePort output) {
            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(input, GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(output, GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();
        }

        public static void PortPair(XNode.NodePort input, XNode.NodePort output,
            NodeGuiLayoutStyle intputStyle,NodeGuiLayoutStyle outputStyle) {
            GUILayout.BeginHorizontal();
            PortField(input, intputStyle);
            PortField(output,outputStyle);
            GUILayout.EndHorizontal();
        }
        
        public static void DrawPortHandle(Rect rect, Color backgroundColor, Color typeColor) {
            var col = GUI.color;
            GUI.color = backgroundColor;
            GUI.DrawTexture(rect, NodeEditorResources.dotOuter);
            GUI.color = typeColor;
            GUI.DrawTexture(rect, NodeEditorResources.dot);
            GUI.color = col;
        }

        [Obsolete("Use InstancePortList(string, Type, SerializedObject, NodePort.IO, Node.ConnectionType) instead")]
        public static void InstancePortList(string fieldName, Type type, SerializedObject serializedObject, XNode.Node.ConnectionType connectionType = XNode.Node.ConnectionType.Multiple) {
            InstancePortList(fieldName, type, serializedObject, XNode.PortIO.Output, connectionType);
        }

        /// <summary> Draw an editable list of instance ports. Port names are named as "[fieldName] [index]" </summary>
        /// <param name="fieldName">Supply a list for editable values</param>
        /// <param name="type">Value type of added instance ports</param>
        /// <param name="serializedObject">The serializedObject of the node</param>
        /// <param name="connectionType">Connection type of added instance ports</param>
        public static void InstancePortList(string fieldName, Type type, SerializedObject serializedObject, XNode.PortIO io, XNode.Node.ConnectionType connectionType = XNode.Node.ConnectionType.Multiple) {
            var node = serializedObject.targetObject as XNode.Node;
            var arrayData = serializedObject.FindProperty(fieldName);

            Predicate<string> isMatchingInstancePort =
                x => {
                    var split = x.Split(' ');
                    if (split != null && split.Length == 2) return split[0] == fieldName;
                    else return false;
                };
            var instancePorts = node.InstancePorts.Where(x => isMatchingInstancePort(x.fieldName)).OrderBy(x => x.fieldName).ToList();

            ReorderableList list = null;
            Dictionary<string, ReorderableList> rlc;
            if (reorderableListCache.TryGetValue(serializedObject.targetObject, out rlc)) {
                if (!rlc.TryGetValue(fieldName, out list)) list = null;
            }
            // If a ReorderableList isn't cached for this array, do so.
            if (list == null) {
                var label = serializedObject.FindProperty(fieldName).displayName;
                list = CreateReorderableList(instancePorts, arrayData, type, serializedObject, io, label, connectionType);
                if (reorderableListCache.TryGetValue(serializedObject.targetObject, out rlc)) rlc.Add(fieldName, list);
                else reorderableListCache.Add(serializedObject.targetObject, new Dictionary<string, ReorderableList>() { { fieldName, list } });
            }
            list.list = instancePorts;
            list.DoLayoutList();
        }

        private static ReorderableList CreateReorderableList(List<XNode.NodePort> instancePorts, SerializedProperty arrayData, Type type, SerializedObject serializedObject, XNode.PortIO io, string label, XNode.Node.ConnectionType connectionType = XNode.Node.ConnectionType.Multiple) {
            var hasArrayData = arrayData != null && arrayData.isArray;
            var arraySize = hasArrayData ? arrayData.arraySize : 0;
            var node = serializedObject.targetObject as XNode.Node;
            var list = new ReorderableList(instancePorts, null, true, true, true, true);

            list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var port = node.GetPort(arrayData.name + " " + index);
                    if (hasArrayData) {
                        var itemData = arrayData.GetArrayElementAtIndex(index);
                        EditorGUI.PropertyField(rect, itemData);
                    } else EditorGUI.LabelField(rect, port.fieldName);
                    var pos = rect.position + (port.IsOutput?new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));
                    NodeEditorGUILayout.PortField(pos, port);
                };
            list.elementHeightCallback =
                (int index) => {
                    if (hasArrayData) {
                        var itemData = arrayData.GetArrayElementAtIndex(index);
                        return EditorGUI.GetPropertyHeight(itemData);
                    } else return EditorGUIUtility.singleLineHeight;
                };
            list.drawHeaderCallback =
                (Rect rect) => {
                    EditorGUI.LabelField(rect, label);
                };
            list.onSelectCallback =
                (ReorderableList rl) => {
                    reorderableListIndex = rl.index;
                };
            list.onReorderCallback =
                (ReorderableList rl) => {

                    // Move up
                    if (rl.index > reorderableListIndex) {
                        for (var i = reorderableListIndex; i < rl.index; ++i) {
                            var port = node.GetPort(arrayData.name + " " + i);
                            var nextPort = node.GetPort(arrayData.name + " " + (i + 1));
                            port.SwapConnections(nextPort);

                            // Swap cached positions to mitigate twitching
                            var rect = NodeEditorWindow.current.portConnectionPoints[port];
                            NodeEditorWindow.current.portConnectionPoints[port] = NodeEditorWindow.current.portConnectionPoints[nextPort];
                            NodeEditorWindow.current.portConnectionPoints[nextPort] = rect;
                        }
                    }
                    // Move down
                    else {
                        for (var i = reorderableListIndex; i > rl.index; --i) {
                            var port = node.GetPort(arrayData.name + " " + i);
                            var nextPort = node.GetPort(arrayData.name + " " + (i - 1));
                            port.SwapConnections(nextPort);

                            // Swap cached positions to mitigate twitching
                            var rect = NodeEditorWindow.current.portConnectionPoints[port];
                            NodeEditorWindow.current.portConnectionPoints[port] = NodeEditorWindow.current.portConnectionPoints[nextPort];
                            NodeEditorWindow.current.portConnectionPoints[nextPort] = rect;
                        }
                    }
                    // Apply changes
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();

                    // Move array data if there is any
                    if (hasArrayData) {
                        var arrayDataOriginal = arrayData.Copy();
                        arrayData.MoveArrayElement(reorderableListIndex, rl.index);
                    }

                    // Apply changes
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    NodeEditorWindow.current.Repaint();
                    EditorApplication.delayCall += NodeEditorWindow.current.Repaint;
                };
            list.onAddCallback =
                (ReorderableList rl) => {
                    // Add instance port postfixed with an index number
                    var newName = arrayData.name + " 0";
                    var i = 0;
                    while (node.HasPort(newName)) newName = arrayData.name + " " + (++i);

                    if (io == XNode.PortIO.Output) node.AddInstanceOutput(type, connectionType, newName);
                    else node.AddInstanceInput(type, connectionType, newName);
                    serializedObject.Update();
                    EditorUtility.SetDirty(node);
                    if (hasArrayData) arrayData.InsertArrayElementAtIndex(arraySize);
                    serializedObject.ApplyModifiedProperties();
                };
            list.onRemoveCallback =
                (ReorderableList rl) => {
                    var index = rl.index;
                    // Clear the removed ports connections
                    instancePorts[index].ClearConnections();
                    // Move following connections one step up to replace the missing connection
                    for (var k = index + 1; k < instancePorts.Count(); k++) {
                        for (var j = 0; j < instancePorts[k].ConnectionCount; j++) {
                            var other = instancePorts[k].GetConnection(j);
                            instancePorts[k].Disconnect(other);
                            instancePorts[k - 1].Connect(other);
                        }
                    }
                    // Remove the last instance port, to avoid messing up the indexing
                    node.RemoveInstancePort(instancePorts[instancePorts.Count() - 1].fieldName);
                    serializedObject.Update();
                    EditorUtility.SetDirty(node);
                    if (hasArrayData) {
                        arrayData.DeleteArrayElementAtIndex(index);
                        arraySize--;
                        // Error handling. If the following happens too often, file a bug report at https://github.com/Siccity/xNode/issues
                        if (instancePorts.Count <= arraySize) {
                            while (instancePorts.Count <= arraySize) {
                                arrayData.DeleteArrayElementAtIndex(--arraySize);
                            }
                            Debug.LogWarning("Array size exceeded instance ports size. Excess items removed.");
                        }
                        serializedObject.ApplyModifiedProperties();
                        serializedObject.Update();
                    }

                };

            if (hasArrayData) {
                var instancePortCount = instancePorts.Count;
                while (instancePortCount < arraySize) {
                    // Add instance port postfixed with an index number
                    var newName = arrayData.name + " 0";
                    var i = 0;
                    while (node.HasPort(newName)) newName = arrayData.name + " " + (++i);
                    if (io == XNode.PortIO.Output) node.AddInstanceOutput(type, connectionType, newName);
                    else node.AddInstanceInput(type, connectionType, newName);
                    EditorUtility.SetDirty(node);
                    instancePortCount++;
                }
                while (arraySize < instancePortCount) {
                    arrayData.InsertArrayElementAtIndex(arraySize);
                    arraySize++;
                }
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            return list;
        }
    }
}