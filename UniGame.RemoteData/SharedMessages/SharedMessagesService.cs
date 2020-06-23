namespace UniModules.UniGame.RemoteData.SharedMessages
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Authorization;
    using MessageData;
    using MutableObject;
    using UnityEngine;

    public class SharedMessagesService
    {
        private Dictionary<Type, ISharedMessageProcessor> _processors = new Dictionary<Type, ISharedMessageProcessor>();

        private IAuthModule _authModule;
        private BaseSharedMessagesStorage _storage;

        public void Init(IAuthModule authModule, BaseSharedMessagesStorage storage)
        {
            _authModule = authModule;
            _storage = storage;
        }

        public void Run()
        {
            _storage.MessageAdded += SelfMessagesAdded;
            _storage.StartListen();
        }

        private void SelfMessagesAdded(AbstractSharedMessage obj)
        {
            NotifyListeners(obj);
        }

        private void NotifyListeners(AbstractSharedMessage message)
        {
            var type = message.GetType();
            if (_processors.ContainsKey(type))
                _processors[type].ProcessMessage(message);
            else
                Debug.LogWarning("No processors registered for message type :: " + type.Name);
        }

        public async Task PushMessage(string userId, AbstractSharedMessage message)
        {
            await _storage.CommitMessage(userId, message);
        }

        public RemoteDataChange CreateMarkProcededChange(AbstractSharedMessage message)
        {
            return RemoteDataChange.Create(
                string.Format("{0}/{1}", message.FullPath, nameof(message.Proceeded)),
                nameof(message.Proceeded),
                true,
                null);
        }

        public RemoteDataChange CreateRemoveChange(AbstractSharedMessage message)
        {
            return RemoteDataChange.Create(
                message.FullPath,
                string.Empty,
                null,
                null);
        }

        public void RegisterProcessor<T>(ISharedMessageProcessor processor) where T : AbstractSharedMessage
        {
            if (_processors.ContainsValue(processor))
                throw new InvalidOperationException("Repeated processor registration for type :: " + processor.GetType().FullName);

            _processors.Add(typeof(T), processor);
        }

        public void UnregisterProcessor(ISharedMessageProcessor processor)
        {
            Type keyToRemove = null;
            foreach (var kvp in _processors)
            {
                if (kvp.Value == processor)
                {
                    keyToRemove = kvp.Key;
                    break;
                }
            }
            _processors.Remove(keyToRemove);
        }
    }
}
