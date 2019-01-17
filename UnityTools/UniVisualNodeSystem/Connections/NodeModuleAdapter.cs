using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UnityTools.UniNodeEditor.Connections
{
    public abstract class NodeModuleAdapter : ScriptableObject, INodeModuleAdapter
    {
        protected Dictionary<string, IContextData<IContext>> _values;

        public IReadOnlyCollection<string> Ports => GetPorts();

        public void Initialize()
        {

            _values = new Dictionary<string, IContextData<IContext>>();

            OnInitialize();
            
        }

        public void BindValue(string key,IContextData<IContext> value)
        {
            _values[key] = value;
        }

        public abstract void Bind(IContext context, ILifeTime timeline);

        public abstract void Update(IContext context, ILifeTime lifeTime);

        #region module methods

        protected virtual void OnInitialize(){}

        /// <summary>
        /// Get registered port names
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyCollection<string> GetPorts()
        {
            return new List<string>();
        }

        protected IContextData<IContext> GetConnection(string key)
        {
            _values.TryGetValue(key, out var data);
            return data;
        }
                    
        #endregion

    }
}
