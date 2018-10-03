using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Common
{
    public class DataValue<TData> : IDataValue<TData>
    {
        protected bool _isReleased;

        public TData Value { get; protected set; }

        public void SetValue(TData value)
        {
            _isReleased = false;
            Value = value;
        }

        public void Release()
        {
            if (_isReleased)
                return;

            Value = default(TData);
            _isReleased = true;
        }

        public void Dispose()
        {
            Release();
            this.Despawn();
        }
    }
}