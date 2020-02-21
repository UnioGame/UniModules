namespace UniGreenModules.UniCore.Runtime
{
    using System;
    using UnityEngine;

    public abstract class ScriptableID : ScriptableObject, IEquatable<ScriptableID>
    {
        public bool Equals(ScriptableID other)
        {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return name.Equals(other.name);
        }

        public override bool Equals(object obj)
        {
            return obj is ScriptableID other && Equals(other);
        }

        public override int GetHashCode() => name.GetHashCode();
    }
}