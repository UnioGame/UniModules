using Assets.Tools.UnityTools.ActorEntityModel.Interfaces;
using Assets.Tools.UnityTools.Common;

namespace Assets.Tools.UnityTools.ActorEntityModel
{
    public class EntityObject : IEntity
    {
        private ContextData _contextData = new ContextData();

        #region public properties
        
        public int Id { get; protected set; }

        #endregion

        #region public methods

        public void Dispose()
        {
            OnDispose();
            Release();
        }

        public virtual TData Get<TData>()
        {
            return _contextData.Get<TData>();
        }

        public bool Remove<TData>()
        {
            return _contextData.Remove<TData>();
        }

        public void Add<TData>(TData data)
        {
            _contextData.Add(data);

        }

        public void Release()
        {
            _contextData.Release();
            Id = -1;
        }

        #endregion

        #region private methods

        protected virtual void OnDispose()
        {
        }

        #endregion

 
    }
}