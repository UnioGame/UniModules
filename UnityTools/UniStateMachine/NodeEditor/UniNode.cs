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

        public UniPortValue InputValue

        #endregion

        public virtual bool Validate(IContext context)
        {
            return _validator ? _validator.Validate(context) : true;
        }

        protected override void OnExit(IContext context)
        {
            var input = GetPortValue(InputPortName);
            input.RemoveContext(context);
            base.OnExit(context);
        }
    }
}