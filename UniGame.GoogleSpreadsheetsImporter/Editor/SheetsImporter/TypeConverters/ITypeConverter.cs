namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;

    public interface ITypeConverter
    {
        bool CanConvert(Type fromType,Type toType);
        
        object ConvertValue(object source, Type target);
        
    }
}