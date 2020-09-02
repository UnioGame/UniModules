namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PrimitiveTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);

        public override bool CanConvert(Type fromType, Type toType)
        {
            if (fromType == toType || toType == stringType)
                return true;
            var result =
                (toType.IsPrimitive && toType.IsPrimitive) ||
                (fromType == stringType && toType.IsPrimitive);

            return result;
        }

        public override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null) {
                return ConvertToDefault(target);
            }

            var sourceType = source.GetType();
            var canConvert = CanConvert(sourceType, target);
            return (canConvert, canConvert ? ConvertValue(source, target) : source);
        }

        public object ConvertValue(object source, Type target)
        {
            return CanConvert(source.GetType(), target) ? 
                Convert.ChangeType(source, target) : 
                source;
        }

        private (bool isValid, object result) ConvertToDefault(Type target)
        {
            switch (target) {
                case Type to when to.IsPrimitive:
                    return (true,Activator.CreateInstance(target));   
                case Type to when to == typeof(string):
                    return  (true,string.Empty);   
                default:
                    return (false,null);
            }
        }
    }
}