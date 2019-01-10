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
        private IContextState<IEnumerator> Behaviour
        {
            get
            {
                if (_state == null)
                {
                    var behaviour = new ProxyStateBehaviour();
                    behaviour.Initialize(ExecuteState, Initialize, OnExit, OnPostExecute);
                    _state = behaviour;
                }

                return _state;
            }
        }

        #endregion
        
        #region serialized data

        [HideInInspector]
        [SerializeField]
        private List<UniPortValue> _portValues = new List<UniPortValue>();

        [SerializeField]
        private RoutineType _routineType = RoutineType.UpdateStep;

        #endregion
              
        public RoutineType RoutineType => _routineType;

        public bool IsAnyActive => _context?.Contexts.Count > 0;

        public IReadOnlyList<UniPortValue> PortValues => _portValues;
        
        
        #region public methods
        
        public bool IsActive(IContext context)
        {
            return Behaviour.IsActive(context);
        }

        public ILifeTime GetLifeTime(IContext context)
        {
            return Behaviour.GetLifeTime(context);
        }
 
        public void Exit(IContext context)
        {
            Behaviour.Exit(context);
            foreach (var output in PortValues)
            {
                output.RemoveContext(context);
            }
        }
        
        public IEnumerator Execute(IContext context)
        {
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1} CONTEXT {2}", 
                name, GetType().Name, context), this);

            yield return Behaviour.Execute(context);
            
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
            Behaviour?.Dispose();
            Invalidate();
        }

        public override object GetValue(NodePort port)
        {
            return GetPortValue(port);
        }

        public void AddPortValue(UniPortValue portValue)
        {
            
            if (portValue == null)
            {
                Debug.LogErrorFormat("Try add NULL port value to {0}",this);
                return;
            }
            
            if (PortValuesMap.ContainsKey(portValue.Name))
            {
                return;
            }
            
            _portValues.Add(portValue);

            Invalidate();
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
        }

        protected virtual IEnumerator ExecuteState(IContext context)
        {
            var output = GetPortValue(OutputPortName);
            output.UpdateValue(context, context);
            yield break;
        }

        protected virtual void OnExit(IContext context)
        {
            var output = GetPortValue(OutputPortName);
            output.RemoveContext(context);
            _context?.RemoveContext(context);
        }

        protected virtual void OnPostExecute(IContext context){}
        
        #endregion

    }
}
