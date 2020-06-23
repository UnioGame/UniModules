namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class MutableChild<T> : IMutableChildBase
    {
        private static Dictionary<string, FieldInfo> _fieldInfoCache = new Dictionary<string, FieldInfo>();

        private readonly Func<T> _getter;
        protected IRemoteChangesStorage _storage;
        private readonly Dictionary<string, INotifyable> _properties;
        private Dictionary<string, IMutableChildBase> _childObjects;
        protected T Object => _getter();

        public MutableChild(Func<T> getter, string fullPath, IRemoteChangesStorage storage)
        {
            _getter = getter;
            FullPath = fullPath;
            _storage = storage;
            _properties = new Dictionary<string, INotifyable>();
            _childObjects = new Dictionary<string, IMutableChildBase>();
        }

        public string FullPath { get; private set; }

        public void UpdateChildData(string fieldName, object newValue)
        {
            _storage.AddChange(RemoteDataChange.Create(
                FullPath + fieldName,
                fieldName,
                newValue,
                ApplyChangeLocal));
        }

        private void ApplyChangeLocal(RemoteDataChange change)
        {
            var fieldInfo = GetFieldInfo(change.FieldName);
            fieldInfo.SetValue(Object, change.FieldValue);
            PropertyChanged(change.FieldName);
        }
        
        private FieldInfo GetFieldInfo(string fieldName)
        {
            if(!_fieldInfoCache.TryGetValue(fieldName, out var fieldInfo))
            {
                fieldInfo = typeof(T).GetField(fieldName);
                _fieldInfoCache.Add(fieldName, fieldInfo);
            }
            return fieldInfo;
        }

        protected MutableObjectReactiveProperty<Tvalue> CreateReactiveProperty<Tvalue>(Func<Tvalue> getter, Action<Tvalue> setter, string fieldName)
        {
            var property = new MutableObjectReactiveProperty<Tvalue>(getter, setter, this);
            _properties.Add(fieldName, property);
            return property;
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

        public void AddChange(RemoteDataChange change)
        {
            _storage.AddChange(change);
        }

        public bool IsRootLoaded()
        {
            return _storage.IsRootLoaded();
        }
    }
}
