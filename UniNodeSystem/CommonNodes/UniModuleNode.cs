using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniStateMachine.Nodes;
using UnityEngine;
using UnityTools.UniNodeEditor.Connections;
using UnityTools.UniVisualNodeSystem;
using UniNodeSystem;

namespace UniStateMachine.CommonNodes
{
    public class UniModuleNode : UniNode
    {
        /// <summary>
        /// target module adapter
        /// </summary>
        [SerializeField] private NodeModuleAdapter _adapter;

        [HideInInspector] [SerializeField] private List<string> _modulePortValues = new List<string>();
        [NonSerialized] private List<PortDefinition> _inputs;
        [NonSerialized] private List<PortDefinition> _outputs;
        
        public List<string> ModulePortValues => _modulePortValues;

        public INodeModuleAdapter Adapter => _adapter;

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            UpdateModulePorts();
        }

        /// <summary>
        /// update adapter for target context
        /// </summary>
        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);

            var lifeTime = LifeTime;
            
            foreach (var value in PortValues)
            {
                var disposable = _adapter.Bind(value.Name,value,context);
                lifeTime.AddDispose(disposable);
            }
           
            while (true)
            {

                ExecuteAdapterItems(_inputs, context);
                
                ExecuteAdapterItems(_outputs, context);
                
                yield return null;

            }
        }

        private void ExecuteAdapterItems(List<PortDefinition> items,IContext context)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var value = GetPortValue(item.Name);
                _adapter.Execute(item.Name,value,context);
            }
        }
        
        private void UpdateModulePorts()
        {
            //register all module ports
            _modulePortValues.Clear();
            _inputs = new List<PortDefinition>();
            _outputs = new List<PortDefinition>();
            
            if (!_adapter)
            {
                return;
            }

            _adapter.Initialize();
            _modulePortValues.AddRange(_adapter.Ports.Select(x => x.Name));

            foreach (var port in _adapter.Ports)
            {
                if (port.Direction == PortIO.Input)
                {
                    _inputs.Add(port);
                }
                else
                {
                    _outputs.Add(port);
                }
                this.UpdatePortValue(port.Name,port.Direction);
            }
        }
    }
}