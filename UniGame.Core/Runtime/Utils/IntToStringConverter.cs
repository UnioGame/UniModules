namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;

    public static class IntToStringConverter
    {
        private static readonly Func<int, string> Converter = MemorizeTool.Create<int,string>(x => x.ToString());

        public static string ToStringFromCache(this int value)
        {
            return Converter.Invoke(value);
        }
    }
}
