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
    private IContext _context;
    
    public UniNodesGraph Graph;
	
	// Use this for initialization
	void Start ()
	{
		_context = new EntityObject();
	    _disposableItem = Graph.Execute(_context).RunWithSubRoutines();
	}

	private void OnDisable()
	{
	    _disposableItem?.Dispose();
	    _disposableItem = null;
        Graph.Dispose();
        _context.Release();
	}
}
