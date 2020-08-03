namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class StringToPrimitiveTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);

        public override bool CanConvert(Type fromType, Type toType)
        {
            return fromType == stringType && toType.IsPrimitive;
        }

        public override object ConvertValue(object source, Type target)
        {
            var typeConverter = TypeDescriptor.GetConverter(target);
            var propValue     = typeConverter.ConvertFromString((string) source);
            return propValue;
        }
    }
}