namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniRx;

    public class MutableList<T> : MutableChild<List<T>>, IReactiveCollection<T>
    {
        private ReactiveCommand<CollectionReplaceEvent<T>> _replaceObservable;
        private ReactiveCommand<CollectionAddEvent<T>> _addObservable;

        public MutableList(Func<List<T>> getter, string fullPath, IRemoteChangesStorage storage) : base(getter, fullPath, storage)
        {
            _replaceObservable = new ReactiveCommand<CollectionReplaceEvent<T>>();
            _addObservable = new ReactiveCommand<CollectionAddEvent<T>>();
        }

        public T this[int index] { get => Object[index]; set => AddUpdateChange(index, value); }

        T IReadOnlyReactiveCollection<T>.this[int index] => Object[index];

        public int Count => Object.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return Object.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Object.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Object.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return Object.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void Move(int oldIndex, int newIndex)
        {
            throw new NotImplementedException();
        }

        public IObservable<CollectionAddEvent<T>> ObserveAdd()
        {
            return _addObservable;
        }

        public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false)
        {
            throw new NotImplementedException();
        }

        public IObservable<CollectionMoveEvent<T>> ObserveMove()
        {
            throw new NotImplementedException();
        }

        public IObservable<CollectionRemoveEvent<T>> ObserveRemove()
        {
            throw new NotImplementedException();
        }

        public IObservable<CollectionReplaceEvent<T>> ObserveReplace()
        {
            return _replaceObservable;
        }

        public IObservable<Unit> ObserveReset()
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private void AddUpdateChange(int index, T value)
        {
            var update = new ListRemoteDataChange()
            {
                FullPath = FullPath + index.ToString(),
                FieldName = index.ToString(),
                FieldValue = value,
                Index = index,
                ApplyCallback = UpdateApply
            };
            AddChange(update);
        }

        private void AddRemoveChange(int index)
        {
            var remove = new ListRemoteDataChange()
            {
                FullPath = FullPath + index.ToString(),
                FieldName = index.ToString(),
                Index = index,
                ApplyCallback = RemoveApply
            };
            AddChange(remove);
        }

        private void UpdateApply(RemoteDataChange change)
        {
            var listChange = change as ListRemoteDataChange;
            var replaceEvent = new CollectionReplaceEvent<T>(listChange.Index, Object[listChange.Index], (T)listChange.FieldValue);
            Object[listChange.Index] = (T)listChange.FieldValue;
            _replaceObservable.Execute(replaceEvent);
        }

        private void RemoveApply(RemoteDataChange change)
        {
            // TO DO
        }
    }

    internal class ListRemoteDataChange : RemoteDataChange
    {
        public int Index;
    }
}
