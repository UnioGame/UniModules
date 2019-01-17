using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UnityTools.UniNodeEditor.Connections
{
    public class NodeModuleAdapter : ScriptableObject
    {
        protected Dictionary<string, IContextData<IContext>> _values;
        protected List<string> _ports;
        
        public bool IsReady { get; protected set; }

        public IReadOnlyList<string> Ports => _ports;

        public void Initialize()
        {
            if (IsReady)
                return;
            
            _ports = GetPorts();
            _values = new Dictionary<string, IContextData<IContext>>();

            OnInitialize();
            
            IsReady = true;
        }

        public void BindValue(string key,IContextData<IContext> value)
        {
            OnBindValue(key,value); 
        }
        
        public void Bind(IContext context)
        {
            foreach (var data in _values)
            {
                OnBindAction(context, data.Key,data.Value);              
            }
        }

        public void Update( IContext context)
        {
            foreach (var data in _values)
            {
                OnUpdate(context,data.Key, data.Value);
            }
        }

        #region module methods
        
        public virtual void Release(IContext context)
        {
            
        }
        
        protected virtual void OnBindAction(IContext context,string key, IContextData<IContext> value)
        {
            
        }

        protected virtual void OnBindValue(string key, IContextData<IContext> value)
        {
            
        }

        protected virtual void OnUpdate(IContext context,string key, IContextData<IContext> value)
        {
            
        }

        protected virtual void OnInitialize(){}

        /// <summary>
        /// Get registered port names
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetPorts()
        {
            return new List<string>();
        }
                    
        #endregion

    }
}
