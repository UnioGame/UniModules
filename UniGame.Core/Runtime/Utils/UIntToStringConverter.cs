namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;

    public static class UIntToStringConverter
    {
        private static readonly Func<uint, string> Converter = MemorizeTool.Create<uint, string>(x => x.ToString());

        public static string ToStringFromCache(this uint value)
        {
            return Converter.Invoke(value);
        }
    }
}