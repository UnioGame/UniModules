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
            var sourceValidation = fromType == stringType || fromType == null;
            var destValidation = toType == stringType || toType.IsPrimitive;
            return sourceValidation && destValidation;
        }

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            var sourceType = source?.GetType();
            if (!CanConvert(sourceType, target))
                return (false, source);
            
            var sourceValue = source == null ? string.Empty : source as string;
            
            return (true,ConvertValue(sourceValue,target));
        }

        public object ConvertValue(string source, Type target)
        {
            //create default value of type
            if (string.IsNullOrEmpty(source)) {
                return Activator.CreateInstance(target);
            }
            
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