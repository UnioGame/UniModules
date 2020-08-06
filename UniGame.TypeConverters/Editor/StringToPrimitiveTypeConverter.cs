namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.TypeConverters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using UnityEngine;

    [Serializable]
    [CreateAssetMenu(menuName = "UniGame/TypeConverters/StringToPrimitiveTypeConverter",fileName = nameof(StringToPrimitiveTypeConverter))]
    public class StringToPrimitiveTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);
        private static Type floatType = typeof(float);

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
                var style   = NumberStyles.Float;
                var culture = CultureInfo.InvariantCulture.NumberFormat;
                float.TryParse(source,style,culture,out var resultFloat);
                return resultFloat;
            }
            var typeConverter = TypeDescriptor.GetConverter(target);
            var propValue     = typeConverter.ConvertFromString((string) source);
            return propValue;
        }
    }
}