using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniNodeSystemEditor {
    /// <summary> Base class to derive custom Node Graph editors from. Use this to override how graphs are drawn in the editor. </summary>
    [CustomNodeGraphEditor(typeof(UniNodeSystem.NodeGraph))]
    public partial class NodeGraphEditor : UniNodeSystemEditor.NodeEditorBase<NodeGraphEditor, NodeGraphEditor.CustomNodeGraphEditorAttribute, UniNodeSystem.NodeGraph> {
        /// <summary> The position of the window in screen space. </summary>
        public Rect position;
        /// <summary> Are we currently renaming a node? </summary>
        protected bool isRenaming;

        public virtual void OnGUI() { }

        public virtual Texture2D GetGridTexture() 
        {
            return NodeEditorPreferences.GetSettings().gridTexture;
        }

        public virtual Texture2D GetSecondaryGridTexture() {
            return NodeEditorPreferences.GetSettings().crossTexture;
        }

        /// <summary> Return default settings for this graph type. This is the settings the user will load if no previous settings have been saved. </summary>
        public virtual NodeEditorPreferences.Settings GetDefaultPreferences() {
            return new NodeEditorPreferences.Settings();
        }

        /// <summary> Returns context node menu path. Null or empty strings for hidden nodes. </summary>
        public virtual string GetNodeMenuName(Type type) {
            //Check if type has the CreateNodeMenuAttribute
            UniNodeSystem.UniBaseNode.CreateNodeMenuAttribute attrib;
            if (NodeEditorUtilities.GetAttrib(type, out attrib)) // Return custom path
                return attrib.menuName;
            else // Return generated path
                return ObjectNames.NicifyVariableName(type.ToString().Replace('.', '/'));
        }

        public virtual Color GetTypeColor(Type type) {
            return NodeEditorPreferences.GetTypeColor(type);
        }

        /// <summary> Creates a copy of the original node in the graph </summary>
        public UniNodeSystem.UniBaseNode CopyNode(UniNodeSystem.UniBaseNode original) {
            UniNodeSystem.UniBaseNode node = target.CopyNode(original);
            node.name = original.name;
            AssetDatabase.AddObjectToAsset(node, target);

            if (NodeEditorPreferences.GetSettings().autoSave)
            {
                AssetDatabase.SaveAssets();
            }
            
            return node;
        }

        /// <summary> Safely remove a node and all its connections. </summary>
        public void RemoveNode(UniNodeSystem.UniBaseNode node)
        {
            var graph = node.graph;

            var sourceGraph = PrefabUtility.GetCorrespondingObjectFromSource(target);
            var sourceNode = PrefabUtility.GetCorrespondingObjectFromSource(node);

            var removedAsset = sourceNode != null ? (Object)sourceNode.gameObject : node;
            var targetGraph = sourceGraph ? sourceGraph : target;

            var targetNode = sourceNode ? sourceNode : node;
            targetGraph.RemoveNode(targetNode);

            Object.DestroyImmediate(removedAsset,true);

            AssetDatabase.SaveAssets();

            if (sourceGraph)
            {
                PrefabUtility.ApplyPrefabInstance(targetGraph.gameObject,InteractionMode.AutomatedAction);
            }
            
        }
    }
}