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

        [NonSerialized]
        private NodePort _inputPort;
        public NodePort InputPort
        {
            get
            {
                if (_inputPort == null)
                {
                    _inputPort = GetPort(InputPortName);
                }

                return _inputPort;
            }
        }
        
        public UniPortValue Input;      
        
        #endregion

        public virtual bool Validate(IContext context)
        {
            return _validator ? _validator.Validate(context) : true;
        }

        protected override void OnExit(IContext context)
        {
            Input.RemoveContext(context);
            base.OnExit(context);
        }
    }
}