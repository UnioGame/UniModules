namespace UniGreenModules.UniGame.Core.Runtime.SerializableType
{
    using System;
    using Taktika.GameRuntime.Types;
    using UnityEngine;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.DrawWithUnity]
#endif
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

        public bool Equals(SType stype) => Type == stype.Type;
        
        public bool Equals(Type stype) => Type == stype;

        public override bool Equals(object obj)
        {
            switch (obj) {
                case Type objectType:
                    return Type == objectType;
                case SType stype:
                    return Type == stype.Type;
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
           var typeValue = Type;
           return typeValue == null ? 
               0 : 
               typeValue.GetHashCode();
        }

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
            //fullTypeName = type?.FullName;
        }

        public void OnAfterDeserialize()
        {
            type = Type.GetType(fullTypeName, false, true);
        }

        #endregion
        
        public static implicit operator Type(SType type) => type.Type;

        public static implicit operator SType(Type type) => new SType(){Type = type};

    }
}
