namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;

    public static class IntToStringConverter
    {

        private static Func<int, string> converter = MemorizeTool.Create<int,string>(x => x.ToString());

        public static string ToStringFromCache(this int value)
        {
            return converter.Invoke(value);
        }
        
    }
}
