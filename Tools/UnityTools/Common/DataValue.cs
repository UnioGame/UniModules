using Assets.Tools.Utils;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace UniTools.Common
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