namespace UniGreenModules.UniGame.Core.Runtime.SerializableType
{
    using System;
    using Taktika.GameRuntime.Types;
    using UnityEngine;

    [Serializable]
    public class SType : ISerializationCallbackReceiver, IReadOnlyType
    {
        public string fullTypeName;

        public Type type;

        public Type Type {
            get => GetItemType();
            set {
                type = value;
                #if UNITY_EDITOR
                fullTypeName = type.AssemblyQualifiedName;
                #endif
            }
        }

        public string TypeName {
            get => fullTypeName;
            set => type = Type.GetType(value, false, true);
        }
        
		
        public Type GetItemType()
        {
            if (type != null) return type;
            if (string.IsNullOrEmpty(fullTypeName)) return type;
            type = Type.GetType(fullTypeName, false, false);
            return type;
        }
		
        #region ISerializationCallbackReceiver
        
        public void OnBeforeSerialize() {}

        public void OnAfterDeserialize()
        {
            type = Type.GetType(fullTypeName, false, true);
        }

        
        #endregion


        public static implicit operator Type(SType type) => type.Type;
        
        public static implicit operator SType(Type type) => new SType(){Type = type};

    }
}
