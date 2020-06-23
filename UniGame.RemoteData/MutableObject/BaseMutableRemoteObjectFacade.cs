namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RemoteData;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;

    public class BaseMutableRemoteObjectFacade<T> : IRemoteChangesStorage where T : class
    {
        protected RemoteObjectHandler<T> _objectHandler;

        private ConcurrentStack<RemoteDataChange> _pendingChanges;

        private Dictionary<string, INotifyable> _properties;

        private Dictionary<string, IMutableChildBase> _childObjects;

        public BaseMutableRemoteObjectFacade(RemoteObjectHandler<T> objectHandler)
        {
            _objectHandler = objectHandler;
            _pendingChanges = new ConcurrentStack<RemoteDataChange>();
            _properties = new Dictionary<string, INotifyable>();
            _childObjects = new Dictionary<string, IMutableChildBase>();
        }

        /// <summary>
        /// Loads remote data. if not exits sets initialValue
        /// </summary>
        /// <param name="initialDataProvider"></param>
        /// <returns></returns>
        public async Task LoadRootData(bool keepSynched = false, Func<T> initialDataProvider = null)
        {

            await _objectHandler.LoadData(keepSynched, initialDataProvider);
            AllPropertiesChanged();
        }

        public string GetId()
        {
            return _objectHandler.GetDataId();
        }

        public void UpdateChildData(string childName, object newData)
        {
            var change = _objectHandler.CreateChange(childName, newData);
            change.ApplyCallback = ApplyChangeOnLocalHandler;
            AddChange(change);
        }

        public void AddChange(RemoteDataChange change)
        {
            ChangeApplied(change);
            _pendingChanges.Push(change);
        }

        /// <summary>
        /// Отправляет все локально записанные изменения на сервер.
        /// ВАЖНО: все операции в рамках одной комманды не должны прерыватья вызовом
        /// метода
        /// </summary>
        /// <returns></returns>
        public async Task CommitChanges()
        {
            var updateTasks = ClassPool.SpawnOrCreate<List<Task>>(()=>new List<Task>());
            List<RemoteDataChange> changes;
            lock (_pendingChanges)
            {
                changes = _pendingChanges.ToList();
                changes.Reverse();
                _pendingChanges.Clear();
            }

            foreach (var change in changes)
                updateTasks.Add(_objectHandler.ApplyChange(change));
            await Task.WhenAll(updateTasks.ToArray());
            changes.ForEach((ch) => ch.Dispose());
            changes.Clear();
        }

        /// <summary>
        /// Возвращает список всех локальных изменений и обнуляет его внутри объекта
        /// применимо для работы с BatchUpdater
        /// </summary>
        /// <returns></returns>
        public List<RemoteDataChange> FlushChanges()
        {
            var result = _pendingChanges.ToList();
            _pendingChanges.Clear();
            return result;
        }

        /// <summary>
        /// Создает Reactive Property для работы с оборачиваемыми данными
        /// </summary>
        /// <typeparam name="Tvalue">Тип обрабатываемого поля</typeparam>
        /// <param name="getter"></param>
        /// <param name="setter"></param>
        /// <param name="fieldName">Имя поля</param>
        /// <returns></returns>
        public MutableObjectReactiveProperty<Tvalue> CreateReactiveProperty<Tvalue>(Func<Tvalue> getter, Action<Tvalue> setter, string fieldName)
        {
            var property = new MutableObjectReactiveProperty<Tvalue>(getter, setter, this);
            _properties.Add(fieldName, property);
            return property;
        }

        public void RegisterMutableChild(string childName, IMutableChildBase child)
        {
            _childObjects.Add(childName, child);
        }

        public bool IsRootLoaded()
        {
            return _objectHandler.Object != null;
        }

        protected void PropertyChanged(string name)
        {
            if (_properties.ContainsKey(name))
                _properties[name].Notify();
        }

        protected void AllPropertiesChanged()
        {
            foreach (var property in _properties.Values)
                property.Notify();
        }

        private void ChangeApplied(RemoteDataChange change)
        {
            change.ApplyCallback?.Invoke(change);
        }

        private void ApplyChangeOnLocalHandler(RemoteDataChange change)
        {
            _objectHandler.ApplyChangeLocal(change);
            PropertyChanged(change.FieldName);
        }
    }
}
