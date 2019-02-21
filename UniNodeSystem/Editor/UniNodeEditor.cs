using System;
using System.Collections.Generic;
using UniEditorTools;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace SubModules.Scripts.UniStateMachine.NodeEditor
{
	[CustomNodeEditor(typeof(UniGraphNode))]
	public class UniNodeEditor : XNodeEditor.NodeEditor
	{
		
		private static Dictionary<string,NodePort> _drawedPorts = new Dictionary<string,NodePort>();

		public static Type UniPortValueType = typeof(UniPortValue);
		
		public bool IsSelected()
		{
			var node = target as UniGraphNode;
			if (!node)
				return false;
			return node.IsAnyActive;
		}
		
		private static GUIStyle SelectedHeaderStyle = 
			new GUIStyle()
		{
			alignment = TextAnchor.MiddleCenter,
			fontStyle = FontStyle.Bold,		
		};

		public override void OnHeaderGUI()
		{
			if (IsSelected())
			{
				EditorDrawerUtils.DrawWithContentColor(Color.red, base.OnHeaderGUI);
				return;
			}
			base.OnHeaderGUI();
		}

		public override void OnBodyGUI()
		{
			var node = target as UniGraphNode;
			node.Initialize();

			UpdateData(node);
			
			base.OnBodyGUI();
			
			DrawPorts(node);
					
			serializedObject.ApplyModifiedPropertiesWithoutUndo();

		}

		public virtual void UpdateData(UniGraphNode node)
		{
			node.UpdatePortsCache();
		}

		public override GUIStyle GetBodyStyle()
		{
			return base.GetBodyStyle();
		}

		public void DrawPorts(UniGraphNode node)
		{
			_drawedPorts.Clear();

			var inputPort = node.GetPort(UniNode.InputPortName);
			var outputPort = node.GetPort(UniGraphNode.OutputPortName);
			DrawPortPair(inputPort,outputPort,_drawedPorts);
			
			foreach (var portValue in node.PortValues)
			{
				var portName = portValue.Name;
				var formatedName = UniNode.GetFormatedInputName(portName);

				DrawPortPair(node,portName,formatedName,_drawedPorts);

			}

			foreach (var portValue in node.PortValues)
			{
				var portName = portValue.Name;
				if(_drawedPorts.ContainsKey(portName))
					continue;
			
				var port = node.GetPort(portValue.Name);
				var portStyle = GetPortStyle(port);
				
				port.DrawPortField(portStyle);
			}
			
		}

		public void DrawPortPair(UniGraphNode node,string inputPortName, string outputPortName,Dictionary<string,NodePort> ports)
		{
			if(_drawedPorts.ContainsKey(inputPortName))
				return;	
			
			var outputPort = node.GetPort(inputPortName);
			var inputPort = node.GetPort(outputPortName);

			DrawPortPair(inputPort, outputPort,ports);
		}
		
		public void DrawPortPair(NodePort inputPort, NodePort outputPort,Dictionary<string,NodePort> ports)
		{
			if (outputPort == null || inputPort == null)
			{
				return;
			}
			
			var inputStyle = GetPortStyle(inputPort);
			var outputStyle = GetPortStyle(outputPort);
				
			_drawedPorts[inputPort.fieldName] = inputPort;
			_drawedPorts[outputPort.fieldName] = outputPort;

			inputPort.DrawPortField(outputPort, inputStyle, outputStyle);
		}

		private NodeGuiLayoutStyle GetPortStyle(NodePort port)
		{
            var portStyle = NodeEditorGUILayout.GetDefaultPortStyle(port);

			if (port == null)
				return portStyle;

			var uniNode = port.node as UniGraphNode;
			var portValue = uniNode.GetPortValue(port.fieldName);
			var hasData = portValue != null && portValue.Count > 0;
			
  			if (port.fieldName == UniNode.OutputPortName || port.fieldName == UniNode.InputPortName)
			{
				portStyle.Background = Color.blue;
				portStyle.Color = hasData ? Color.red : Color.white;
				return portStyle;
			}
			
			if (port.IsDynamic)
			{
				portStyle.Name = port.fieldName;
				portStyle.Background = Color.red;
				portStyle.Color = port.direction  == PortIO.Input ?
					hasData ? new Color(128,128,0) : Color.green : 
					hasData ? new Color(128,128,0) : Color.blue;
			}
			
			return portStyle;
		}
		
	}
}
