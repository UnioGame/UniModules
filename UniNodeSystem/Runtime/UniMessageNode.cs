
namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using Extensions;
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public abstract class UniMessageNode<TValue> : UniNode
    {
        protected Dictionary<UniPortValue,UniPortValue> _portValueMap = new Dictionary<UniPortValue,UniPortValue>();
        protected List<UniPortValue> _messageOutputValues = new List<UniPortValue>();
        protected List<UniPortValue> _messageInputValues = new List<UniPortValue>();
        
        public List<string> PortNames = new List<string>();
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            
            yield return base.OnExecuteState(context);

            BindMessageOutputs(context);
            
            BindMessageInputs(context);
      
        }

        protected override void OnUpdatePortsCache()
        {
            
            _portValueMap.Clear();

            base.OnUpdatePortsCache();

            PortNames = GetNodeApiNames();

        }


        private void CreatePortsConnections(IContext context)
        {
                        
            var count = PortNames.Count;

            for (var i = 0; i < count; i++)
            {               
                var portName = PortNames[i];
                var ports = this.CreatePortPair(portName, false);
                
                var inputValue = ports.inputValue;
                var outputValue = ports.outputValue;

                _portValueMap[inputValue] = outputValue;
                
                _messageInputValues.Add(inputValue);
                _messageOutputValues.Add(outputValue);
                           
            }

        }

        protected void BindMessageInputs(IContext context)
        {
        
            var lifeTime = LifeTime;

            for (var i = 0; i < _messageInputValues.Count; i++)
            {
                var index = i;
                var portValue = _messageOutputValues[i];
                var disposable = portValue.UpdateValueObservable.Subscribe(x =>
                    OnInputPortUpdate(context, index));
                lifeTime.AddDispose(disposable);
            }
            
        }

        protected void BindMessageOutputs(IContext context)
        {
            var lifeTime = LifeTime;

            for (var i = 0; i < _messageOutputValues.Count; i++)
            {
                var portValue = _messageOutputValues[i];
                var disposable = context.Receive<TValue>()
                    .Subscribe(x => OnMessagePortValue(context, portValue, x));

                lifeTime.AddDispose(disposable);
            }
            
        }   
        
        protected virtual List<string> GetNodeApiNames()
        {
            return PortNames;
        }

        private void OnInputPortUpdate(IContext context,int index)
        {

            var messageData = new NodeMessageData()
            {
                Name = PortNames[index],
                Input = _messageInputValues[index],
                Output = _messageOutputValues[index],
            };
            
            var message = GetMessage(context, messageData);
            context.Publish(message);
        }
        
        protected abstract TValue GetMessage(IContext context,NodeMessageData messageData);

        protected virtual void OnMessagePortValue(IContext context,UniPortValue portValue, TValue value)
        {
            portValue.Add(value);
            portValue.Add(context);
        }
    }
}