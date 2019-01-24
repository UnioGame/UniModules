using System;
using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.Extensions;
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
        private string _combinedOutputName = "Combined"; 
        [SerializeField]
        private string _inputPortName = "Item";
        [SerializeField]
        private int _inputPortsCount;
       
        [NonSerialized]
        private List<UniPortValue> _inputValues;
        [NonSerialized]
        private List<UniPortValue> _outputValues;
        
        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);

            var combinedPort = GetPortValue(_combinedOutputName);
            
            while (IsActive(context))
            {
                var isCombined = IsCombined(context);

                if (isCombined)
                {
                    combinedPort.UpdateValue(context,context);
                }
                else
                {
                    combinedPort.RemoveContext(context);
                }

                UpdatePorts(isCombined, context);
                
                yield return null;
            }
            
        }

        private void UpdatePorts(bool isCombined,IContext context)
        {
            for (int i = 0; i < _inputPortsCount; i++)
            {
                var outputPort = _outputValues[i];
                var inputPort = _inputValues[i];
                
                if (isCombined)
                {
                    outputPort.RemoveContext(context);
                }
                else if(inputPort.HasContext(context))
                {
                    inputPort.CopyTo(outputPort);
                }
                else
                {
                    outputPort.RemoveContext(context);
                }
            }
        }
        
        private bool IsCombined(IContext context)
        {
  
            for (var i = 0; i < _inputPortsCount; i++)
            {
                var value = _inputValues[i];
                    
                if (!value.HasContext(context)) 
                    return false;
            }

            return true;
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            _inputValues = new List<UniPortValue>();
            _outputValues = new List<UniPortValue>();
            
            for (int i = 0; i < _inputPortsCount; i++)
            {
                var outputName = string.Format(_itemTemplate, _inputPortName, i + 1);
                var inputName = GetFormatedInputName(outputName);
                
                var value = this.UpdatePortValue(inputName, PortIO.Input);
                _inputValues.Add(value.value);
                value = this.UpdatePortValue(outputName, PortIO.Output);
                _outputValues.Add(value.value);
            }

            this.UpdatePortValue(_combinedOutputName, PortIO.Output);
        }
    }
}
