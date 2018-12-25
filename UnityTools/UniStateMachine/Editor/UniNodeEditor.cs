using UniEditorTools;
using UniStateMachine;
using UnityEngine;

namespace SubModules.Scripts.UniStateMachine.NodeEditor
{
	[CustomNodeEditor(typeof(UniGraphNode))]
	public class UniNodeEditor : XNodeEditor.NodeEditor
	{
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
			foreach (var portValue in node.GetOutputValues())
			{
				var port = node.GetOutputPort(portValue.Name);
				port.DrawPortField(port.fieldName);
			}
		}
		
	}
}
