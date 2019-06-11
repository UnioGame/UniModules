using System;
using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniNodeSystem.Runtime;
    using UniGreenModules.UniNodeSystem.Runtime.Extensions;
    using UniGreenModules.UniNodeSystem.Runtime.Runtime;

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
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            yield return base.OnExecuteState(context);

            var combinedPort = GetPortValue(_combinedOutputName);
            
            while (IsActive)
            {
                var isCombined = IsCombined(context);

                if (isCombined)
                {
                    combinedPort.Add(context);
                }
                else
                {
                    combinedPort.Remove<IContext>();
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
                    outputPort.Remove<IContext>();
                }
                else if(inputPort.HasValue())
                {
                    outputPort.Add(context);
                }
                else
                {
                    outputPort.Remove<IContext>();
                }
            }
        }
        
        private bool IsCombined(IContext context)
        {
  
            for (var i = 0; i < _inputPortsCount; i++)
            {
                var value = _inputValues[i];
                    
                if (!value.HasValue()) 
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
