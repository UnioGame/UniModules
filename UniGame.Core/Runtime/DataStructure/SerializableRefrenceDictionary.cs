namespace UniGreenModules.UniGame.Core.Runtime.DataStructure
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class SerializableRefrenceDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
        ISerializationCallbackReceiver
    {
        [SerializeField]     protected List<TKey>   keys   = new List<TKey>();
        [SerializeReference] protected List<TValue> values = new List<TValue>();

        #region construcotr

        public SerializableRefrenceDictionary(int capacity) : base(capacity) { }

        public SerializableRefrenceDictionary() : base() { }

        #endregion
        
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in this) {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception("there are " + keys.Count + " keys and " + values.Count +
                                           " values after deserialization. Make sure that both key and value types are serializable.");

            for (var i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }
    }
}