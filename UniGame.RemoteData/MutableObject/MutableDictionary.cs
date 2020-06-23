namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;

    public interface IObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IObservable<TKey> { };

    public class MutableDictionary<TValue> : MutableChild<Dictionary<string, TValue>>, IObservableDictionary<string, TValue>
    {
        private ReactiveCommand<string> _itemChangedCommand;

        public MutableDictionary(Func<Dictionary<string, TValue>> getter, string fullPath, IRemoteChangesStorage storage) : base(getter, fullPath, storage)
        {
            _itemChangedCommand = new ReactiveCommand<string>();
        }

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Object.GetEnumerator();
        }

        private void ClearApply(RemoteDataChange change)
        {
            var keys = Object.Keys.ToArray();
            Object.Clear();
            foreach (var key in keys)
                OnItemChanged(key);
        }

        private void OnItemChanged(string key)
        {
            _itemChangedCommand.Execute(key);
        }

        private void AddRemoveChange(string key)
        {
            var remove = RemoteDataChange.Create(FullPath + key, key, null, RemoveApply);
            AddChange(remove);
        }

        private void RemoveApply(RemoteDataChange change)
        {
            Object.Remove(change.FieldName);
            OnItemChanged(change.FieldName);
        }

        private void AddUpdateChange(string key, TValue value)
        {
            var update = RemoteDataChange.Create(FullPath + key, key, value, UpdateApply);
            AddChange(update);
        }

        private void UpdateApply(RemoteDataChange change)
        {
            Object[change.FieldName] = (TValue)change.FieldValue;
            OnItemChanged(change.FieldName);
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            return _itemChangedCommand.Subscribe(observer);
        }
    }
}
