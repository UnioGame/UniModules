namespace UniGreenModules.UniNodeSystem.Inspector.Editor
{
	using BaseEditor;
	using Runtime;
	using UniNodeSystem.Nodes;

	[CustomNodeGraphEditor(typeof(UniGraph))]
	public class UniGraphEditor : NodeGraphEditor
	{

		private UniGraph graph;
		
		public override void OnEnable()
		{
			base.OnEnable();
			graph = target as UniGraph;
			
		}
		
	}
}
