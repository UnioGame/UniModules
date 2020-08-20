namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;

    public static class ObjectTypeConverterExtension
    {
        public static object TryConvert(this object source, Type target)
        {
            return ObjectTypeConverter.TypeConverters.TryConvert(source, target).result;
        }
    }
}