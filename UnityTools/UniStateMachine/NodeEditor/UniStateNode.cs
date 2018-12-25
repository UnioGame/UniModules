using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniStateMachine.Nodes;
using UnityEngine;
using XNode;

namespace UniStateMachine
{
    [Serializable]
    public abstract class UniGraphNode : Node, IContextState<IEnumerator>
    {
        public const string OutputPortName = "Output";
        
        [HideInInspector]
        [SerializeField]
        private List<UniPortValue> _outputs = new List<UniPortValue>();
        
        [NonSerialized]
        private IContextState<IEnumerator> _state;
        
        [NonSerialized]
        protected IContextData<IContext> _context;

        #region ports
        
        [Output(ShowBackingValue.Always)]
        public UniPortValue Output = new UniPortValue(){Name = OutputPortName};
                
        [NonSerialized]
        private NodePort _outputPort;

        public NodePort OutputPort
        {
            get
            {
                if (_outputPort == null)
                {
                    _outputPort = GetPort(OutputPortName);
                }

                return _outputPort;
            }
        }
        
        #endregion
        
        
        [NonSerialized]
        private Dictionary<string, UniPortValue> _portValues;
        public IReadOnlyDictionary<string, UniPortValue> PortValues
        {
            get
            {
                if (_portValues == null)
                {
                    _portValues = new Dictionary<string, UniPortValue>();
                    foreach (var value in GetOutputValues())
                    {
                        _portValues[value.Name] = value;
                    }
                }

                return _portValues;
            }
        }
        
        #region public methods

        public bool IsAnyActive => _context?.Contexts.Count > 0;

        public bool IsActive(IContext context)
        {
            var state = GetBehaviour();
            return state.IsActive(context);
        }

        public ILifeTime GetLifeTime(IContext context)
        {
            return _state.GetLifeTime(context);
        }

        public void Exit(IContext context)
        {
            var behaviour = GetBehaviour();
            behaviour.Exit(context);
            foreach (var output in GetOutputValues())
            {
                output.Value.RemoveContext(context);
            }
        }
        
        /// <summary>
        /// stop ay execution of state
        /// release all resources
        /// </summary>
        public virtual void Dispose()
        {
            _context?.Release();
            _state?.Dispose();
        }

        public IEnumerator Execute(IContext context)
        {
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1} CONTEXT {2}", 
                name, GetType().Name, context), this);

            var state = GetBehaviour();
            yield return state.Execute(context);
        }

        #endregion

        #region state behaviour methods

        private void Initialize(IContextData<IContext> stateContext)
        {
            _context = stateContext;
        }

        protected virtual IEnumerator ExecuteState(IContext context)
        {
            Output.Value.AddValue(context, context);
            yield break;
        }

        protected virtual void OnExit(IContext context)
        {
            Output.Value.RemoveContext(context);
            _context?.RemoveContext(context);
        }

        protected virtual void OnPostExecute(IContext context){}
        
        #endregion

        private IContextState<IEnumerator> Create()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, Initialize, OnExit, OnPostExecute);
            return behaviour;
        }

        private IContextState<IEnumerator> GetBehaviour()
        { 
            if (_state == null)
            {
                _state = Create();
            }
            return _state;
        }

                
        public IEnumerable<UniPortValue> GetOutputValues()
        {
            yield return Output;
            for (var i = 0; i < _outputs.Count; i++)
            {
                yield return _outputs[i];    
            }
        }

        public void AddPortValue(UniPortValue portValue)
        {
            if (portValue == null)
            {
                Debug.LogErrorFormat("Try add NULL port value to {0}",this);
                return;
            }
            if (PortValues.ContainsKey(portValue.Name))
            {
                Debug.LogErrorFormat("Port Value connected to port woth ID {0} already exists",portValue.PortId);
                return;
            }
            _outputs.Add(portValue);
            _portValues = null;
        }

        public void CleanUpValues()
        {
            var ports = PortValues;
            _outputs.RemoveAll(x => ports.ContainsKey(x.Name) == false);
        }
        
        public UniPortValue GetPortValue(NodePort port)
        {
            PortValues.TryGetValue(port.fieldName, out var value);
            if (value == null && port.fieldName == OutputPortName)
            {
                _portValues[port.fieldName] = Output;
                value = Output;
            }
            return value;
        }
    }
}
