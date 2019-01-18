using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;
using UnityTools.UniVisualNodeSystem;

namespace UnityTools.UniNodeEditor.Connections
{
    public abstract class NodeModuleAdapter : ScriptableObject, INodeModuleAdapter
    {
        #region inspector

        [SerializeField]
        private List<PortDefinition> _portDefinitions;
        
        #endregion
        
        protected Dictionary<string, PortDefinition> _values;

        public IReadOnlyCollection<PortDefinition> Ports { get; protected set; }

        public void Initialize()
        {

            _values = new Dictionary<string, PortDefinition>();
            
            OnInitialize();
            
            _portDefinitions = GetPorts();

            foreach (var definition in _portDefinitions)
            {
                _values[definition.Name] = definition;
            }

            Ports = _portDefinitions;
            
            
        }

        public abstract void Bind(IContext context, ILifeTime timeline);

        public abstract void Execute(IContext context, ILifeTime lifeTime);

        #region module methods

        protected abstract void OnInitialize();

        /// <summary>
        /// Get registered port names
        /// </summary>
        /// <returns></returns>
        protected virtual List<PortDefinition> GetPorts()
        {
            return new List<PortDefinition>();
        }

        protected PortDefinition GetConnection(string key)
        {
            _values.TryGetValue(key, out var data);
            return data;
        }
                    
        #endregion

    }
}
