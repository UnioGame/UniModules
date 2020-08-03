namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;

    [Serializable]
    public class PrimitiveTypeConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);

        public override bool CanConvert(Type fromType, Type toType)
        {
            if (toType == stringType)
                return true;
            var result =
                (toType.IsPrimitive && toType.IsPrimitive) ||
                (fromType == stringType && toType.IsPrimitive);

            return result;
        }

        public override object ConvertValue(object source, Type target)
        {
            if (CanConvert(source.GetType(), target)) {
                return Convert.ChangeType(source, target);
            }

            return source;
        }
    }
}