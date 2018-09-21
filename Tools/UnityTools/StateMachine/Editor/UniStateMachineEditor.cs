using System;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using Assets.UI.Windows.Tools.Editor;
using LevelEditor;
using Tools.ReflectionUtils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniStateMachine
{
	[CustomEditor(typeof(DataContextStateMachine),true)]
	public class UniStateMachineEditor : Editor
	{

		private Vector2 _nodesScroll = Vector2.zero;
		private ReorderableList _nodesList;
		private DataContextStateMachine _stateMachine;
		
		private static List<Type> _validators;
		private static List<Type> _behaviours;
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			if (_behaviours == null || GUILayout.Button("Update Nodes"))
			{
				UpdateScriptsCache();
			}

			_stateMachine = target as DataContextStateMachine;
			var nodeSelector = _stateMachine.StateSelector;

			if (!nodeSelector)
			{
				if (CreateNodeSelector(_stateMachine) == false)
				{
					return;
				}
			}
			
			if (nodeSelector == null)
				return;

			_nodesScroll = EditorDrawerUtils.DrawScroll(() => DrawNodes(nodeSelector.Nodes),_nodesScroll);

			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(_stateMachine);
			
		}

		private void UpdateScriptsCache()
		{
			
			var validators = ReflectionTools.GetAllChildTypes(typeof(UniNodeValidator));
			var behaviours = ReflectionTools.GetAllChildTypes(typeof(UniStateBehaviour));
			
			_behaviours = new List<Type>(behaviours);
			_validators = new List<Type>(validators);
			
		}
		
		private void DrawNodes(List<UniSelectorNode> nodes)
		{

			CreateNodeList(nodes);
			
			EditorGUILayout.BeginVertical();

			//draw the list using GUILayout, you can of course specify your own position and label
			_nodesList.DoLayoutList();
			for (int i = 0; i < nodes.Count; i++)
			{
				
			}
			
			EditorGUILayout.EndVertical();
		}


		private void CreateNodeList(List<UniSelectorNode> nodes)
		{
			
			if (_nodesList == null)
			{
				_nodesList = new ReorderableList(nodes,typeof(UniSelectorNode),true,true,true,true);
				_nodesList.onAddCallback = AddStateNode;
			}
			
		}

		private void AddStateNode(ReorderableList list)
		{
			var node = AssetEditorTools.SaveAssetAsNested<UniSelectorNode>(_stateMachine.StateSelector);
			list.list[list.index] = node;
		}
		
		private bool CreateNodeSelector(DataContextStateMachine stateMachine)
		{
			var result = AssetEditorTools.SaveAssetAsNested<DataContextStateSelector>(stateMachine);
			if (result)
			{
				stateMachine.SetSelector(result);
				Save();
			}
			
			return result;
		}

		private void Save()
		{
			serializedObject.Update();
			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}
	}
	
	

}
