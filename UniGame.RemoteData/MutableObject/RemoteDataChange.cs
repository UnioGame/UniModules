namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;

    public class RemoteDataChange : IDisposable
    {
        public string FieldName;
        public string FullPath;
        public object FieldValue;
        public Action<RemoteDataChange> ApplyCallback;

        protected RemoteDataChange() { }

        public static RemoteDataChange Create(string FullPath,
                                                    string FieldName,
                                                    object FieldValue,
                                                    Action<RemoteDataChange> ApplyCallback)
        {
            var change = ClassPool.SpawnOrCreate(() => new RemoteDataChange());
            change.FullPath = FullPath;
            change.FieldName = FieldName;
            change.FieldValue = FieldValue;
            change.ApplyCallback = ApplyCallback;
            return change;
        }

        public void Dispose()
        {
            ClassPool.Despawn(this, null);
        }

    }
}
