using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.ActorEntityModel;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniRoutine;
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
