namespace UniModules.UniGame.RemoteData.RemoteData
{
    using System;
    using System.Threading.Tasks;
    using MutableObject;

    public abstract class RemoteObjectHandler<T> : IDisposable
    {
        public T Object { get; protected set; }

        public abstract void Dispose();

        public abstract Task<RemoteObjectHandler<T>> LoadData(bool keepSynched = false, Func<T> initialDataProvider = null);

        //public abstract Task UpdateRemoteData(T newData);

        //public abstract event Action<RemoteObjectHandler<T>> ValueChanged;

        //public abstract RemoteObjectHandler<TChild> GetChild<TChild>(string path) where TChild : class;

        public abstract RemoteDataChange CreateChange(string fieldName, object fieldValue);

        public async Task ApplyChange(RemoteDataChange change)
        {
            await ApplyChangeRemote(change);
        }

        public void ApplyChangeLocal(RemoteDataChange change)
        {
            var fieldInfo = typeof(T).GetField(change.FieldName);
            fieldInfo.SetValue(Object, change.FieldValue);
        }

        public abstract string GetDataId();

        public abstract string GetFullPath();

        //public abstract Task<TResult> PerformTransaction<TResult>(UpdateFunc<TResult, T> updateFunc) where TResult : struct;

        //public delegate TRes UpdateFunc<TRes, Tin>(object oldValue, out Tin newValue) where TRes : struct;

        protected abstract Task ApplyChangeRemote(RemoteDataChange change);
    }
}
