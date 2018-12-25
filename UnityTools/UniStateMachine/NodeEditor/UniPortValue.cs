using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;
using XNode;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue
    {
        public ulong PortId;

        public string Name;
        
        private ContextDataProvider<IContext> _dataProvider = new ContextDataProvider<IContext>();
        
        public IReadOnlyCollection<IContext> Contexts => _dataProvider.Contexts;

        public IContextData<IContext> Value => _dataProvider;

        public void ConnectToPort(NodePort port)
        {
            PortId = port.Id;
            Name = port.fieldName;
        }
    }
}