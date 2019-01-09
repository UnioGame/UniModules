using System;
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
			
			base.OnBodyGUI();
			
			node.CleanUpValues();

			DrawOutputPorts(node);
		}

		public override GUIStyle GetBodyStyle()
		{
			return base.GetBodyStyle();
		}

		public virtual void DrawOutputPorts(UniGraphNode node)
		{
			var outputPort = node.OutputPort;
			if (outputPort == null)
			{
				node.AddInstanceOutput(UniPortValueType, Node.ConnectionType.Multiple, UniGraphNode.OutputPortName);
			}

			foreach (var portValue in node.OutputValues)
			{
				
				var port = node.GetOutputPort(portValue.Name);
				var portStyle = NodeEditorGUILayout.GetDefaultPortStyle(port);
				if (port != outputPort)
				{
					portStyle.Background = Color.red;
					portStyle.Color = Color.blue;
				}
				
				port.DrawPortField(portStyle);
			}
		}
		
	}
}
