using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;
using XNode;

namespace UniStateMachine.CommonNodes
{
    public class CombineNode : UniNode
    {
        
        private const string _itemTemplate = "{0}{1}";
        
        [SerializeField]
        private string _inputPortName = "CombineItem";
        [SerializeField]
        private int _inputPortsCount;
        [NonSerialized]
        private List<UniPortValue> _inputValues;
        
        protected override IEnumerator ExecuteState(IContext context)
        {
            
            while (IsActive(context))
            {
                var isCombined = true;
                
                for (var i = 0; i < _inputValues.Count; i++)
                {
                    var value = _inputValues[i];
                    
                    if (value.HasContext(context)) continue;
                    
                    var output = GetPortValue(OutputPortName);
                    output.RemoveContext(context);
                    
                    isCombined = false;
                    
                    break;
                }
               
                if(isCombined)
                    yield return base.ExecuteState(context);
                
                yield return null;
            }
            
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            _inputValues = new List<UniPortValue>();
            
            for (int i = 0; i < _inputPortsCount; i++)
            {
                var inputName = string.Format(_itemTemplate, _inputPortName, i + 1);
                var value = this.UpdatePortValue(inputName, PortIO.Input);
                _inputValues.Add(value.value);
            }
        }
    }
}
