namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;

    public class MutableDictionary<TValue> : MutableChild<Dictionary<string, TValue>>, IReactiveDictionary<string, TValue>
    {
        public MutableDictionary(Func<Dictionary<string, TValue>> getter, string fullPath, IRemoteChangesStorage storage) : base(getter, fullPath, storage)
        {

        }

        private Subject<DictionaryAddEvent<string, TValue>> _addObserver = new Subject<DictionaryAddEvent<string, TValue>>();
        private Subject<DictionaryReplaceEvent<string, TValue>> _replaceObserver = new Subject<DictionaryReplaceEvent<string, TValue>>();
        private Subject<DictionaryRemoveEvent<string, TValue>> _removeObserver = new Subject<DictionaryRemoveEvent<string, TValue>>();
        private ReactiveProperty<int> _reactiveCount = new ReactiveProperty<int>();

        public TValue this[string key] { get => Object[key]; set => AddUpdateChange(key, value); }

        public ICollection<string> Keys => Object.Keys;

        public ICollection<TValue> Values => Object.Values;

        public int Count => Object.Count;

        public bool IsReadOnly => false;

        public void Add(string key, TValue value)
        {
            if (Object.ContainsKey(key))
                throw new InvalidOperationException(string.Format("Element with key :: {0} exists in Dictionary", key));
            AddUpdateChange(key, value);
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            var clearChange = RemoteDataChange.Create(FullPath, string.Empty, null, ClearApply);
            AddChange(clearChange);
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return Object.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return Object.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return Object.GetEnumerator() as IEnumerator<KeyValuePair<string, TValue>>;
        }

        public bool Remove(string key)
        {
            AddRemoveChange(key);
            return true;
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            if (Object.Contains(item))
            {
                AddRemoveChange(item.Key);
                return true;
            }
            return false;
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return Object.TryGetValue(key, out value);
        }

        public IObservable<DictionaryAddEvent<string, TValue>> ObserveAdd() => _addObserver;

        public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false) => _reactiveCount; // TODO убрать первую нотификацию по notifyCurrentCount == true

        public IObservable<DictionaryRemoveEvent<string, TValue>> ObserveRemove() => throw new NotImplementedException();

        public IObservable<DictionaryReplaceEvent<string, TValue>> ObserveReplace() => throw new NotImplementedException();

        public IObservable<Unit> ObserveReset() => throw new NotImplementedException(); //TODO

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Object.GetEnumerator();
        }

        private void AddRemoveChange(string key)
        {
            var remove = RemoteDataChange.Create(FullPath + key, key, null, RemoveApply);
            AddChange(remove);
        }

        private void AddUpdateChange(string key, TValue value)
        {
            var update = RemoteDataChange.Create(FullPath + key, key, value, UpdateApply);
            AddChange(update);
        }

        private void ClearApply(RemoteDataChange change)
        {
            Object.Clear();
        }

        private void RemoveApply(RemoteDataChange change)
        {
            var val = Object[change.FieldName];
            var e = new DictionaryRemoveEvent<string, TValue>(change.FieldName, val);
            Object.Remove(change.FieldName);
            _removeObserver.OnNext(e);
        }

        private void UpdateApply(RemoteDataChange change)
        {
            Object[change.FieldName] = (TValue)change.FieldValue;
        }

    }
}
