namespace UniModules.UniGame.TypeConverters.Editor
{
    using System;

    public static class ObjectTypeConverterExtension
    {
        public static object TryConvert(this object source, Type target)
        {
            return ObjectTypeConverter.TypeConverters.TryConvert(source, target).result;
        }
        
        public static T TryConvert<T>(this object source)
            where T : class
        {
            return TryConvert(source, typeof(T)) as T;
        }
    }
}