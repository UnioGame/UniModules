using System;

namespace Tools.ActorModel
{
    public class EntityObject : IEntity, IDisposable
    {
        protected float _deltaTime;

        public int Id { get; protected set; }
        public bool IsActive { get; protected set; }

        #region public methods

        public void SetState(bool state)
        {
            if (IsActive == state)
                return;
            if (state)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }

            IsActive = state;
        }

        public void Dispose()
        {
            if (IsActive)
                SetState(false);
            Id = -1;
            OnDispose();
        }

        public void Update(float time)
        {
            if (IsActive == false)
                return;

            _deltaTime = time;
            OnUpdate(time);
        }

        public virtual TData GetContext<TData>() where TData : class 
        {
            var result = this as TData;
            return result;
        }
        
        #endregion

        #region private methods

        protected virtual void Activate()
        {
        }

        protected virtual void Deactivate()
        {
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnUpdate(float time)
        {
        }

        #endregion
    }
}