using System;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using UniRx;
using UniStateMachine.Nodes;
using UnityEngine;
using UnityTools.UniVisualNodeSystem;

namespace UnityTools.UniNodeEditor.Connections
{
    public abstract class NodeModuleAdapter : ScriptableObject, INodeModuleAdapter
    {

        [SerializeField]
        private List<PortDefinition> _portDefinitions;
        [NonSerialized]
        private Dictionary<string, Func<IContextData<IContext>, IContext, IDisposable>> _bindActions;
        private Dictionary<string, Action<string,IContextData<IContext>, IContext>> _runtimeActions;
        
        protected Dictionary<string, PortDefinition> _values;

        public IReadOnlyCollection<PortDefinition> Ports { get; protected set; }

        public void Initialize()
        {
            
            _values = new Dictionary<string, PortDefinition>();
            _bindActions = new Dictionary<string, Func<IContextData<IContext>, IContext, IDisposable>>();
            _runtimeActions = new Dictionary<string, Action<string,IContextData<IContext>, IContext>>();
            
            OnInitialize();
            
            _portDefinitions = GetPorts();

            foreach (var definition in _portDefinitions)
            {
                _values[definition.Name] = definition;
            }

            Ports = _portDefinitions;

        }

        public IDisposable Bind(string portName,IContextData<IContext> portValue,IContext context)
        {
            var binding = GetBindAction(portName);
            if (binding == null)
                return Disposable.Empty;

            var disposable = binding(portValue, context);
            
            return disposable;
        }


        public void Execute(string portName, IContextData<IContext> portValue, IContext context)
        {
            var action = GetRuntimeAction(portName);
            action?.Invoke(portName,portValue, context);
        }

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

        protected void RegisterBindAction(string port, Func<IContextData<IContext>, IContext, IDisposable> action)
        {
            _bindActions[port] = action;
        }

        protected Func<IContextData<IContext>, IContext, IDisposable> GetBindAction(string portName)
        {
            _bindActions.TryGetValue(portName, out var action);
            return action;
        }

        protected void RegisterRuntimeAction(string port, Action<string,IContextData<IContext>, IContext> action)
        {
            _runtimeActions[port] = action;
        }

        protected Action<string,IContextData<IContext>, IContext> GetRuntimeAction(string portName)
        {
            _runtimeActions.TryGetValue(portName, out var action);
            return action;
        }

        
        #endregion

    }
}
