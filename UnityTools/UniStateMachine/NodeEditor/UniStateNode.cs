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
        
        [HideInInspector]
        [SerializeField]
        private List<UniPortValue> _outputs = new List<UniPortValue>();

        [NonSerialized]
        protected IContextData<IContext> _context;
       
        [SerializeField]
        private RoutineType _routineType = RoutineType.UpdateStep;
        
        #region ports
        
        [HideInInspector]
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
        
        public RoutineType RoutineType => _routineType;

        [NonSerialized]
        private Dictionary<string, UniPortValue> _portValues;
        
        public IReadOnlyDictionary<string, UniPortValue> PortValues
        {
            get
            {
                if (_portValues == null)
                {
                    _portValues = new Dictionary<string, UniPortValue>();
                    _portValues[OutputPortName] = Output;
                    foreach (var value in OutputValues)
                    {
                        _portValues[value.Name] = value;
                    }
                }

                return _portValues;
            }
        }
        
        #region public methods

                
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
        
        public bool IsAnyActive => _context?.Contexts.Count > 0;

        [NonSerialized]
        private List<UniPortValue> _outputValues;
        public IReadOnlyList<UniPortValue> OutputValues
        {
            get
            {
                if (_outputValues == null)
                {
                    _outputValues = new List<UniPortValue>();
                    _outputValues.Add(Output);
                    _outputValues.AddRange(_outputs);
                }

                return _outputValues;
            }
        }
        
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
            foreach (var output in OutputValues)
            {
                output.Value.RemoveContext(context);
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
            foreach (var outputValue in OutputValues)
            {
                outputValue.Value.Release();
            }
            Behaviour?.Dispose();
            _context?.Release();
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

    }
}
