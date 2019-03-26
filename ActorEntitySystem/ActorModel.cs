using System;
using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.ActorEntityModel
{
    [Serializable]
    public class ActorModel<TSource> : IActorModel
        where TSource : class
    {
        protected TSource _sourceData;

        #region public methods

        public void Initialize(TSource source)
        {
            _sourceData = source;
        }

        public virtual void Release()
        {
            _sourceData = null;          
        }

        public virtual void Register(IContext context)
        {
            context.Add(this);
        }

        #endregion
    }
}