using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.ContextStateMachine
{
    public abstract class ContextStateBehaviour : IContextState<IEnumerator>
    {
        private Dictionary<IContext, LifeTimeDefinition> _lifeTimes;

        private bool _initialized = false;

        /// <summary>
        /// state local context data
        /// </summary>
        protected ContextData<IContext> _contextData;

        #region public methods

        public IEnumerator Execute(IContext context)
        {
            Initialize();

            //if state already active - wait
            if (IsActive(context))
            {
                yield return this.WaitWhile(() => IsActive(context));
                yield break;
            }

            _contextData.UpdateValue(context,true);
            
            yield return ExecuteState(context);

            OnPostExecute(context);
            
        }

        public void Exit(IContext context)
        {
            if(!IsActive(context))
                return;
            
            OnExit(context);
            
            //remove all local state data
            _contextData?.RemoveContext(context);
            
            var lifeTime = GetLifeTime(context);

            if (lifeTime != null)
            {
                lifeTime.Despawn();
                _lifeTimes.Remove(context);
            }
            
        }

        public virtual void Dispose()
        {
            var contexts = _contextData?.Contexts;
            if (contexts != null) {
                foreach (var context in contexts) {
                    Exit(context);
                }
            }
            _contextData?.Release();
        }

        public virtual bool IsActive(IContext context)
        {
            return _contextData?.HasContext(context) ?? false;
        }

        public ILifeTime GetLifeTime(IContext context)
        {
            if(_lifeTimes== null)
                _lifeTimes = new Dictionary<IContext, LifeTimeDefinition>();
            
            if (!_lifeTimes.TryGetValue(context, out var value))
            {
                value = ClassPool.Spawn<LifeTimeDefinition>();
                _lifeTimes[context] = value;
            }
            return value.LifeTime;
        }

        #endregion

        protected void Initialize()
        {
            if (_initialized != false) return;
            
            _initialized = true;
            _contextData = new ContextData<IContext>();
            _lifeTimes = new Dictionary<IContext, LifeTimeDefinition>();
            OnInitialize(_contextData);
        }
        
        protected virtual void OnInitialize(IContextData<IContext> stateContext) { }

        protected virtual void OnExit(IContext context){}

        protected virtual void OnPostExecute(IContext context){}

        protected abstract IEnumerator ExecuteState(IContext context);

    }
}
