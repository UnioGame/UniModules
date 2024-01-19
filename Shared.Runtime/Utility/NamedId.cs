namespace UniGame.Shared.Runtime.Utility
{
    using System;
    using Core.Runtime;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
#if ODIN_INSPECTOR
    [InlineProperty]
#endif
    [Serializable]
    public struct NamedId : IUnique, IEquatable<int>,IEquatable<string>
    {
        public const string IdGroup = "Id";
        
#if ODIN_INSPECTOR
        [BoxGroup(IdGroup)]
        [OnValueChanged(nameof(RecalculateId))]
#endif
        public string name;
        
#if ODIN_INSPECTOR
        [BoxGroup(IdGroup)]
        [ReadOnly]
#endif
        public int id;
        
        public int Id => id;
        
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RecalculateId();
            }
        }
        
#if ODIN_INSPECTOR
        [OnInspectorInit]
#endif
        public void RecalculateId()
        {
            id = string.IsNullOrEmpty(name) ? 0 : name.GetHashCode();
        }
        
        public static implicit operator int(NamedId v)=> v.id;

        public static explicit operator NamedId(string v)
        {
            var idValue = new NamedId { name = v };
            idValue.RecalculateId();
            return idValue;
        }

        public override string ToString() => name;

        public override int GetHashCode() => id;

        public NamedId FromString(string data) => (NamedId)data;

        public bool Equals(int other) => id == other;

        public bool Equals(string other) => name == other;

        public override bool Equals(object obj)
        {
            if (obj is NamedId mask)
                return mask.id == id;
            return false;
        }
        
    }
}