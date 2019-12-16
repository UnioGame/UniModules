namespace UniGreenModules.UniNodeSystem.Inspector.Editor
{
	using System.IO;
	using System.Linq;
	using Boo.Lang;
	using Runtime;
	using Runtime.Core;
	using UniCore.EditorTools.Editor.AssetOperations;
	using UniCore.EditorTools.Editor.Utility;
	using UniNodeSystem.Nodes;
	using UnityEditor;
	using UnityEngine;

	public class UniGraphOperations 
	{

		[MenuItem("Assets/UniGraph/Cleanup UniGraph")]
		public static void CleanUpSelectedUniGraph()
		{
			var selections = Selection.objects;
			foreach (var selection in selections)
			{
				if(selection is UniGraph graph)
					CleanUpUniGraph(graph);
			}
		}

		[MenuItem("Assets/UniGraph/Create UniGraph")]
		public static void CreateGraph()
		{
			
			var graph = new GameObject().AddComponent<UniGraph>();
			
			//add main root node
			var root = graph.AddNode<UniPortNode>("input");
			root.name = "input";
			root.direction = PortIO.Input;
			
			var activePath = AssetDatabase.GetAssetPath(Selection.activeObject);
			
			var assetFolder = Directory.Exists(activePath) ? activePath :
				Path.GetDirectoryName(activePath);
			
			AssetEditorTools.SaveAsset(graph.gameObject, "UniGraph", assetFolder);
		}
		
		[MenuItem("Assets/UniGraph/Stop UniGraph")]
		public static void StopUniGraph()
		{
			var selections = Selection.objects;
			foreach (var selection in selections)
			{
				if(selection is UniGraph graph)
					graph.Dispose();
			}
		}

		public static void CleanUpUniGraph(UniGraph graph)
		{

			var assetPath = AssetDatabase.GetAssetPath(graph);
			if (string.IsNullOrEmpty(assetPath))
				return;

			var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath).ToList();
			var nodes = graph.nodes;
			
			assets.Remove(graph);

			var itemsToRemove = new List<UniBaseNode>();
			
			foreach (var node in nodes)
			{
				if (!node)
					itemsToRemove.Add(node);
			}

			foreach (var node in itemsToRemove)
			{
				nodes.Remove(node);
			}
			
			foreach (var asset in assets)
			{
				if (!asset || !nodes.Contains(asset))
				{
					nodes.Remove(asset as UniBaseNode);
					asset.DestroyNestedAsset();
					continue;
				}
				
				Debug.LogFormat("FIND ASSET {0}",asset.name);
			}
			
			EditorUtility.SetDirty(graph);
		}
		
	}
}
