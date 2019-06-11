using UnityEngine;
using UniNodeSystemEditor;

namespace UnityTools.UniStateMachine.NodeEditor
{
	using UniGreenModules.UniNodeSystem.Runtime;

	[CustomNodeGraphEditor(typeof(UniGraph))]
	public class UniGraphEditor : NodeGraphEditor  {

		public override void OnEnable()
		{
			base.OnEnable();
		}
	}
}
