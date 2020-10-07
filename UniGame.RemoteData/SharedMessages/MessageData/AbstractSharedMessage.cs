namespace UniModules.UniGame.RemoteData.SharedMessages.MessageData
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using UniModules.UniCore.Runtime.ReflectionUtils;
    using UnityEngine;

    [Serializable]
    public abstract class AbstractSharedMessage
    {
        /// <summary>
        /// User id отправителя
        /// </summary>
        [SerializeField]
        public string MessageOwner;

        [SerializeField]
        public string MessageType;

        [SerializeField]
        public bool Proceeded;

        public string FullPath { get; private set; }

        /// <summary>
        /// TODO вместо ииспользования этого метода надо настроить
        /// Newtonsoft.Json чтобы он сериализовал инфу о типе в валидные поля
        /// </summary>
        public void AssureType()
        {
            MessageType = GetType().Name;
            FullPath = null;
        }

        public void SetPath(string path)
        {
            FullPath = path;
        }
        
        private static Dictionary<string, Type> cacheTypes = new Dictionary<string, Type>();

        public static AbstractSharedMessage FromJson(string typeShortName, string data)
        {
            Type GetTypeForDeserialization()
            {
                if (cacheTypes.TryGetValue(typeShortName, out var type)) {
                    return type;
                }
                var allImplementations = ReflectionTools.FindAllImplementations(typeof(AbstractSharedMessage));
                foreach(var candidate in allImplementations)
                {
                    if (candidate.Name == typeShortName)
                        type = candidate;
                    cacheTypes[candidate.Name] = type;
                }

                return type;
            }
            return (AbstractSharedMessage) JsonConvert.DeserializeObject(data, GetTypeForDeserialization());
        }
    }
}
