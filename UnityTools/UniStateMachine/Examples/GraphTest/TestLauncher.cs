using System.Collections;
using System.Collections.Generic;
using UnityTools.ActorEntityModel;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;
using UniStateMachine.Nodes;
using UnityEngine;

public class TestLauncher : MonoBehaviour
{
    private IDisposableItem _disposableItem;
	public UniNodesGraph Graph;
	
	// Use this for initialization
	void Start () 
	{
	    _disposableItem = Graph.Execute(new EntityObject()).RunWithSubRoutines();
	}

	private void OnDisable()
	{
	    _disposableItem?.Dispose();
	    _disposableItem = null;
        Graph.Dispose();
	}
}
