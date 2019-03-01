using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniStateMachine.Nodes;
using UnityEngine;
using UniNodeSystem;


namespace UniStateMachine
{
    public class UniNode : UniGraphNode, IValidator<IContext>
    {
        public const string InputTriggerPrefix = "[in]";

        public const string InputPortName = "Input";

        #region ports

        public UniPortValue Input => GetPortValue(InputPortName);

        #endregion

        public virtual bool Validate(IContext context)
        {
            return true;
        }

        public static string GetFormatedInputName(string portName)
        {
            portName = string.Format($"{UniNode.InputTriggerPrefix}{portName}");
            return portName;
        }
        
        
        protected override void OnUpdatePortsCache()
        {
            this.UpdatePortValue(InputPortName, PortIO.Input);
            base.OnUpdatePortsCache();
        }
    }
}