using System;
using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Connections;
using Modules.UniTools.UnityTools.Extension;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniRx;
using UniStateMachine.Nodes;

namespace UniStateMachine.CommonNodes
{
    public abstract class UniMessageNode<TValue> : UniNode
    {
        protected Dictionary<UniPortValue,UniPortValue> _portValueMap = new Dictionary<UniPortValue,UniPortValue>();
        protected List<BroadcastActionContextData<IContext>> _portActions = new List<BroadcastActionContextData<IContext>>();
        protected List<UniPortValue> _messageOutputValues = new List<UniPortValue>();

        public List<string> PortNames = new List<string>();
        
        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);

            var lifeTime = GetLifeTime(context);
            
            foreach (var portValuePair in _portValueMap)
            {
                var outputPortValue = portValuePair.Value;
                var disposable = MessageBroker.Default.Receive<TValue>().
                    Subscribe(x => OnMessagePortValue(context,outputPortValue,x));

                lifeTime.AddDispose(disposable);
            }
            
        }

        protected override void OnUpdatePortsCache()
        {
            
            base.OnUpdatePortsCache();

            _portValueMap.Clear();
            _portActions.DisposeItems();

            PortNames = GetNodeApiNames();
            
            var count = PortNames.Count;

            for (var i = 0; i < count; i++)
            {               
                var portName = PortNames[i];
                var ports = this.CreatePortPair(portName, false);
                var inputValue = ports.inputValue;
                var outputValue = ports.outputValue;

                _portValueMap[inputValue] = outputValue;
                _messageOutputValues.Add(outputValue);
                
                BindPorts(ports.inputValue, ports.outputValue, i);                
            }
            
        }
        
        protected virtual List<string> GetNodeApiNames()
        {
            return PortNames;
        }

        protected virtual void BindPorts(UniPortValue input, UniPortValue output, int index)
        {
            var broadCastAction = ClassPool.Spawn<BroadcastActionContextData<IContext>>();
            broadCastAction.Initialize(x => OnInputPortUpdate(x,output,index));

            input.Add(broadCastAction);
        }

        private void OnInputPortUpdate(IContext context,UniPortValue output,int index)
        {
            var messageName = PortNames[index];
            var message = GetMessage(index, messageName);
            MessageBroker.Default.Publish(message);
        }
        
        protected abstract TValue GetMessage(int id, string messageName);

        protected virtual void OnMessagePortValue(IContext context,UniPortValue portValue, TValue value)
        {
            portValue.UpdateValue(context,value);
        }
    }
    
}