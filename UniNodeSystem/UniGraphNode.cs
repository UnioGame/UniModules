using System;
using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UniModule.UnityTools.UniRoutine;
using UniModule.UnityTools.UniStateMachine;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniStateMachine.Nodes;
using UnityEngine;
using UniNodeSystem;

namespace UniStateMachine
{
    [Serializable]
    public abstract class UniGraphNode : UniBaseNode , IContextState<IEnumerator>
    {
        public const string OutputPortName = "Output";
        
        #region private fields

        [NonSerialized]
        private bool _isInitialized;
        
        [NonSerialized] protected IContextData<IContext> _context;
       
        [NonSerialized] private Dictionary<string,UniPortValue> _portValuesMap;

        [NonSerialized]
        private IContextState<IEnumerator> _state;

        [NonSerialized]
        private List<UniPortValue> _portValues;

        #endregion
        
        #region serialized data

        [SerializeField]
        private RoutineType _routineType = RoutineType.UpdateStep;

        #endregion
        
        public UniPortValue Output => GetPortValue(OutputPortName);
        
        public RoutineType RoutineType => _routineType;

        public bool IsAnyActive => _context?.Contexts.Count > 0;

        public IReadOnlyList<UniPortValue> PortValues => _portValues;

        public IReadOnlyList<IContext> Contexts => _context?.Contexts;
        
        #region public methods

        public void Initialize()
        {
            if (Application.isPlaying && _isInitialized)
                return;
            
            _portValues = new List<UniPortValue>();
            _portValuesMap = new Dictionary<string, UniPortValue>();           
            _state = CreateState();
            
            UpdatePortsCache();

            foreach (var portValue in _portValues)
            {
                portValue.Initialize();
            }

            _isInitialized = true;
        }
        
        public void UpdatePortsCache()
        {
            
            OnUpdatePortsCache();
            
            var removedPorts = ClassPool.Spawn<List<NodePort>>();
            
            foreach (var port in Ports)
            {
                if(port.IsStatic)
                    continue;
                var value = GetPortValue(port.fieldName);
                if (value == null)
                {
                    removedPorts.Add(port);
                }
            }

            foreach (var port in removedPorts)
            {
                RemoveInstancePort(port);
            }

            removedPorts.DespawnCollection();
        }
        
        public bool IsActive(IContext context)
        {
  
            return _state.IsActive(context);

        }

        public ILifeTime GetLifeTime(IContext context)
        {
            return _state.GetLifeTime(context);
        }
 
        public void Exit(IContext context)
        {
            _state.Exit(context);
            foreach (var output in PortValues)
            {
                output.RemoveContext(context);
            }
        }
        
        public IEnumerator Execute(IContext context)
        {
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1} CONTEXT {2}", 
                name, GetType().Name, context), this);

            yield return _state.Execute(context);
            
        }
        
        /// <summary>
        /// stop ay execution of state
        /// release all resources
        /// </summary>
        public virtual void Dispose()
        {
            foreach (var outputValue in PortValues)
            {
                outputValue.Release();
            }
            _state?.Dispose();
        }
        
        public override object GetValue(NodePort port)
        {
            return GetPortValue(port);
        }
        

        public UniPortValue GetPortValue(NodePort port)
        {
            return GetPortValue(port.fieldName);
        }

        public UniPortValue GetPortValue(string portName)
        {
            _portValuesMap.TryGetValue(portName, out var value);
            return value;
        }

        public bool AddPortValue(UniPortValue portValue)
        {
            
            if (portValue == null)
            {
                Debug.LogErrorFormat("Try add NULL port value to {0}",this);
                return false;
            }
            
            if (_portValuesMap.ContainsKey(portValue.Name))
            {
                return false;
            }

            _portValuesMap[portValue.Name] = portValue;
            _portValues.Add(portValue);

            return true;
        }

        #endregion

        protected virtual void OnUpdatePortsCache()
        {
            this.UpdatePortValue(OutputPortName, PortIO.Output);
        }

        #region state behaviour methods

        private void Initialize(IContextData<IContext> stateContext)
        {
            _context = stateContext;
            OnInitialize(stateContext);
        }

        protected virtual IEnumerator ExecuteState(IContext context)
        {
            var output = GetPortValue(OutputPortName);
            output.UpdateValue(context, context);
            yield break;
        }

        protected virtual void OnExit(IContext context)
        {
            for (int i = 0; i < _portValues.Count; i++)
            {
                var portValue = _portValues[i];
                portValue.RemoveContext(context);
            }
            _context?.RemoveContext(context);
        }

        protected virtual void OnInitialize(IContextData<IContext> localContext)
        {
            
        }
        
        protected virtual void OnPostExecute(IContext context){}

        protected virtual IContextState<IEnumerator> CreateState()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, Initialize, OnExit, OnPostExecute);
            return behaviour;
        }
        
        #endregion

    }
}
