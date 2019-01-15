using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.StateMachine;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;
using UniStateMachine.Nodes;
using UnityEngine;
using XNode;

namespace UniStateMachine
{
    [Serializable]
    public abstract class UniGraphNode : Node , IContextState<IEnumerator>
    {
        public const string OutputPortName = "Output";
        
        #region private fields
        
        [NonSerialized] protected IContextData<IContext> _context;
       
        [NonSerialized] private Dictionary<string,UniPortValue> _portValuesMap;

        protected Dictionary<string, UniPortValue> PortValuesMap
        {
            get
            {
                if (_portValuesMap == null)
                {
                    _portValuesMap = new Dictionary<string, UniPortValue>();
                }

                if (_portValuesMap.Count == 0)
                {
                    _portValues.ForEach(x => _portValuesMap.Add(x.Name,x));
                }

                return _portValuesMap;
            }
        }

        [NonSerialized]
        private IContextState<IEnumerator> _state;

        #endregion
        
        #region serialized data

        [HideInInspector]
        [SerializeField]
        private List<UniPortValue> _portValues = new List<UniPortValue>();

        [SerializeField]
        private RoutineType _routineType = RoutineType.UpdateStep;

        #endregion
        
        public UniPortValue Output => GetPortValue(OutputPortName);
        
        public RoutineType RoutineType => _routineType;

        public bool IsAnyActive => _context?.Contexts.Count > 0;

        public IReadOnlyList<UniPortValue> PortValues => _portValues;

        public IReadOnlyCollection<IContext> Contexts => _context?.Contexts;
        
        #region public methods

        public void Initialize()
        {
            _state = CreateState();
            foreach (var portValue in _portValues)
            {
                portValue.Initialize();
            }
        }
        
        public virtual void UpdatePortsCache()
        {
            this.UpdatePortValue(OutputPortName, NodePort.IO.Output);
        }

        public virtual void UpdatePortsValues()
        {
            
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
            Invalidate();
        }

        public override object GetValue(NodePort port)
        {
            return GetPortValue(port);
        }

        public bool AddPortValue(UniPortValue portValue)
        {
            
            if (portValue == null)
            {
                Debug.LogErrorFormat("Try add NULL port value to {0}",this);
                return false;
            }
            
            if (PortValuesMap.ContainsKey(portValue.Name))
            {
                return false;
            }
            
            _portValues.Add(portValue);

            Invalidate();

            return true;
        }

        public void Invalidate()
        {
            
            _portValues.RemoveAll(x => GetPort(x.Name) == null);

            PortValuesMap.Clear();
            
        }
        
        public UniPortValue GetPortValue(NodePort port)
        {
            return GetPortValue(port.fieldName);
        }

        public UniPortValue GetPortValue(string portName)
        {
            PortValuesMap.TryGetValue(portName, out var value);
            return value;
        }
        
        #endregion

        #region state behaviour methods

        private void Initialize(IContextData<IContext> stateContext)
        {
            _context = stateContext;
            UpdatePortsCache();
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
