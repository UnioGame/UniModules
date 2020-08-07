namespace UniModules.UniGame.TypeConverters.Editor.Abstract
{
    using System;

    public interface ITypeConverter
    {
        bool CanConvert(Type fromType,Type toType);
        
        (bool isValid,object result) TryConvert(object source, Type target);

    }
}