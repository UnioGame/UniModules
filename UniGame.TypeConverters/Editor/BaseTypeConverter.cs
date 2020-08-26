namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using Abstract;
    using UnityEngine;

    [Serializable]
    public abstract class BaseTypeConverter : ITypeConverter
    {
        public abstract bool CanConvert(Type fromType, Type toType);

        public abstract (bool isValid, object result) TryConvert(object source, Type target);
    }
}