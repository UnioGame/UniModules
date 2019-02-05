
using System;
using System.Collections.Generic;
using UnityTools.UniVisualNodeSystem;

namespace UnityTools.UniNodeEditor.Connections
{
    public class TypedModuleAdapter : NodeModuleAdapter
    {
        private Dictionary<string, Type> _portToTypes;
        private Dictionary<Type, PortDefinition> _outputMessages;
        private Dictionary<Type, PortDefinition> _inputMessages;
        
        protected override void OnInitialize()
        {
            _portToTypes = new Dictionary<string, Type>();
            _inputMessages = new Dictionary<Type, PortDefinition>();
            _outputMessages = new Dictionary<Type, PortDefinition>();
            
            
            
        }
        
        protected override List<PortDefinition> GetPorts()
        {
            var items = base.GetPorts();

            items.AddRange(_inputMessages.Values);
            items.AddRange(_outputMessages.Values);
            
            return items;
        }

        protected void BindPortType<TType>(PortDefinition port)
        {
            var type = typeof(TType);
        }
    }
}
