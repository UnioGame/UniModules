using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;
using UniStateMachine.Nodes;
using UnityEngine;
using XNode;


namespace UniStateMachine
{
    public class UniNode : UniGraphNode, IValidator<IContext>
    {
        public const string InputPortName = "Input";
        
        #region inspector data

        [SerializeField]
        private UniStateValidator _validator;
        
        #endregion
        
        #region ports

        public UniPortValue Input => GetPortValue(InputPortName);

        #endregion

        public override void UpdatePortsCache()
        {
            this.UpdatePortValue(InputPortName, NodePort.IO.Input);
            base.UpdatePortsCache();
        }

        public virtual bool Validate(IContext context)
        {
            return _validator ? _validator.Validate(context) : true;
        }

    }
}