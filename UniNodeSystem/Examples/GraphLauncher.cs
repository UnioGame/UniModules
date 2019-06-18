using System;
using UniGreenModules.UniContextData.Runtime.Entities;
using UnityEngine;
using UniTools.UniRoutine.Runtime;

namespace UniGreenModules.UniNodeSystem.Examples
{
    using Nodes;
    using Runtime;

    public class GraphLauncher : MonoBehaviour
    {
        private EntityContext _context;
        private IDisposable   _disposable;
    
        [SerializeField]
        private UniGraph _graph;
    
        // Start is called before the first frame update
        private void Start()
        {
            if (!_graph)
                return;
            
            _context    = new EntityContext();
            _disposable = _graph.Execute(_context).RunWithSubRoutines();
        
        }

        // Update is called once per frame
        private void OnDisable()
        {
        
            if (!_graph) return;
            _graph.Exit();
            _disposable.Dispose();
        
        }
    }
}
