using System;
using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniStateMachine.Nodes;
using UnityEngine;
using UniNodeSystem;
using UniStateMachine.Runtime;

namespace UniStateMachine
{
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Extensions;
    using UniTools.UniRoutine.Runtime;

    [Serializable]
    public abstract class UniGraphNode : UniBaseNode , IValidator<IContext>, IContextState<IEnumerator>
    {
        /// <summary>
        /// output port name
        /// </summary>
        public const string OutputPortName = "Output";
        
        /// <summary>
        /// input port name
        /// </summary>
        public const string InputPortName = "Input";

        #region private fields

        [NonSerialized] private bool _isInitialized;
        
        [NonSerialized] protected IContext _context;
       
        [NonSerialized] private Dictionary<string,UniPortValue> _portValuesMap;

        [NonSerialized] private IContextState<IEnumerator> _state;

        [NonSerialized] private List<UniPortValue> _portValues;

        #endregion
        
        #region serialized data

        [SerializeField] private RoutineType routineType = RoutineType.UpdateStep;

        #endregion
        
        #region public properties
        
        public UniPortValue Input => GetPortValue(InputPortName);

        public UniPortValue Output => GetPortValue(OutputPortName);
        
        public RoutineType RoutineType => routineType;

        public IReadOnlyList<UniPortValue> PortValues => _portValues;

        public bool IsActive => _state!=null  && _state.IsActive;

        public ILifeTime LifeTime => _state?.LifeTime;
        
        #endregion
        
        #region public methods

        public void Initialize()
        {           
            _state = CreateState();
            
            if (Application.isPlaying && _isInitialized)
                return;
            
            _portValues = new List<UniPortValue>();
            _portValuesMap = new Dictionary<string, UniPortValue>();           
            
            UpdatePortsCache();

            foreach (var portValue in _portValues)
            {
                portValue.Initialize();
            }

            _isInitialized = true;
        }
        
        public virtual bool Validate(IContext context) => true;
        
        public void UpdatePortsCache()
        {
            OnUpdatePortsCache();
            
            var removedPorts = ClassPool.Spawn<List<NodePort>>();
            
            foreach (var port in Ports)
            {
                if(port.IsStatic) continue;
                
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
        
        public void Exit() => _state?.Exit();

        public IEnumerator Execute(IContext context)
        {
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1} CONTEXT {2}", 
                name, GetType().Name, context), this);
            
            Initialize();
            
            yield return _state.Execute(context);
            
        }
        
        public void Release() => Exit();

        #region Node Ports operations
 
        public override object GetValue(NodePort port) => GetPortValue(port);
        
        public UniPortValue GetPortValue(NodePort port) => GetPortValue(port.fieldName);

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
            
            if (_portValuesMap.ContainsKey(portValue.name))
            {
                return false;
            }

            _portValuesMap[portValue.name] = portValue;
            _portValues.Add(portValue);
            
            return true;
        }

        #endregion

        #endregion

        protected virtual void OnUpdatePortsCache()
        {
            this.UpdatePortValue(OutputPortName, PortIO.Output);
            this.UpdatePortValue(InputPortName, PortIO.Input);
        }
        
        #region state behaviour methods

        private void Initialize(IContext stateContext)
        {
            _context = stateContext;
            
            LifeTime.AddCleanUpAction(CleanUpAction);
            
            OnInitialize(stateContext);
        }

        /// <summary>
        /// base logic realization
        /// transfer context data to output port value
        /// </summary>
        protected virtual IEnumerator ExecuteState(IContext context)
        {
            var output = Output;
            output.Add(context);
            yield break;
        }

        protected virtual void OnExit(IContext context) => CleanUpAction();

        protected virtual void OnInitialize(IContext context){}
        
        protected virtual void OnPostExecute(IContext context){}

        protected virtual IContextState<IEnumerator> CreateState()
        {
            if (_state != null)
                return _state;
            
            var behaviour = ClassPool.Spawn<ProxyState>();
            behaviour.Initialize(ExecuteState, Initialize, OnExit, OnPostExecute);
            behaviour.LifeTime.AddCleanUpAction(() => behaviour.Despawn());
            
            return behaviour;
        }

        private void CleanUpAction()
        {
            for (var i = 0; i < PortValues.Count; i++)
            {
                var portValue = PortValues[i];
                portValue.CleanUp();
            }

            _context = null;
            _state = null;
        }
        
        #endregion

    }
}
