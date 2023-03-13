namespace UniGame.Shared.Runtime.Utility
{
    using System;
    using Core.Runtime;
    using Sirenix.OdinInspector;

    [InlineProperty]
    [Serializable]
    public struct NamedId : IUnique, IEquatable<int>,IEquatable<string>
    {
        public const string IdGroup = "Id";
        
        [BoxGroup(IdGroup)]
        [OnValueChanged(nameof(RecalculateId))]
        public string name;
        
        [BoxGroup(IdGroup)]
        [ReadOnly]
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
        
        [OnInspectorInit]
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