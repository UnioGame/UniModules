using System;
using UniModule.UnityTools.ActorEntityModel;
using UniModule.UnityTools.UniRoutine;
using UniStateMachine.Nodes;
using UnityEngine;

public class GraphLauncher : MonoBehaviour
{
    private EntityObject _context;
    private IDisposable _disposable;
    
    [SerializeField]
    private UniNodesGraph _graph;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (!_graph)
            return;
        _context = new EntityObject();
        _disposable = _graph.Execute(_context).RunWithSubRoutines();
        
    }

    // Update is called once per frame
    private void OnDisable()
    {
        
        if (!_graph) return;
        _graph.Exit(_context);
        _disposable.Dispose();
        
    }
}
