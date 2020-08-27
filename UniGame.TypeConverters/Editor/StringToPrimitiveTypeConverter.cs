namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using UnityEngine;

    [Serializable]
    public class StringToPrimitiveTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);
        private static Type floatType = typeof(float);
        private const string globalizationSeparator = ",";

        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            return fromType == stringType && toType.IsPrimitive;
        }

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source is string stringValue) {
                return string.IsNullOrEmpty(stringValue) ? 
                    (false, source) : 
                    (true, ConvertValue(stringValue, target));
            }

            return (false, source);
        }

        public object ConvertValue(string source, Type target)
        {
            if (target == typeof(float)) {
                var floatSource = source.Replace(globalizationSeparator, ".");
                var style   = NumberStyles.Any;
                var culture = CultureInfo.InvariantCulture;
                float.TryParse(floatSource,style,culture,out var resultFloat);
                return resultFloat;
            }
            var typeConverter = TypeDescriptor.GetConverter(target);
            var propValue     = typeConverter.ConvertFromString((string) source);
            return propValue;
        }
    }
}