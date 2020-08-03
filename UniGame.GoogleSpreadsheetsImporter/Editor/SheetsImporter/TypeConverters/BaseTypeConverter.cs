namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;

    [Serializable]
    public abstract class BaseTypeConverter : ITypeConverter
    {
        public abstract bool CanConvert(Type fromType, Type toType);

        public abstract object ConvertValue(object source, Type target);
    }
}