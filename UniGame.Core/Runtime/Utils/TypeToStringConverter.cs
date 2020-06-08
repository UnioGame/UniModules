namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;

    public static class TypeToStringConverter
    {
        private static readonly Func<int, string> IntConverter = MemorizeTool.Create<int,string>(x => x.ToString());
        private static readonly Func<uint, string> UIntConverter = MemorizeTool.Create<uint, string>(x => x.ToString());

        public static string ToStringFromCache(this int value)
        {
            return IntConverter.Invoke(value);
        }
        
        public static string ToStringFromCache(this uint value)
        {
            return UIntConverter.Invoke(value);
        }
    }
}
