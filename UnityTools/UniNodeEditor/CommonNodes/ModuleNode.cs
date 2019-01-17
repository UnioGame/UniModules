using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using UniStateMachine.Nodes;
using UnityEngine;
using UnityTools.UniNodeEditor.Connections;

namespace UniStateMachine.CommonNodes
{
    public class ModuleNode : UniNode
    {
        /// <summary>
        /// target module adapter
        /// </summary>
        [SerializeField]
        private NodeModuleAdapter _adapter;

        public override void UpdatePortsCache()
        {
            base.UpdatePortsCache();

            //register all module ports

            if (!_adapter)
                return;
            
            _adapter.Initialize();

            var modulePorts = _adapter.Ports;
            
            foreach (var port in modulePorts)
            {
                var item = this.UpdatePortValue(port);
                _adapter.BindValue(port,item.value);
            }

        }

        /// <summary>
        /// update adapter for target context
        /// </summary>
        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);

            BindModule(context);
            
            while (IsActive(context))
            {
                yield return null;             
                _adapter.Update(context);
            }
        }    

        /// <summary>
        /// bind adapter to portvalues
        /// </summary>
        /// <param name="context">bind module to current context</param>
        private void BindModule(IContext context)
        {
            
            _adapter.Bind(context);
            
            var lifeTime = GetLifeTime(context);
            lifeTime.AddCleanUpAction(() => _adapter.Release(context));

        }
    }
}
