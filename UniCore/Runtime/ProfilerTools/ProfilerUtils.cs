namespace UniGreenModules.UniCore.Runtime.ProfilerTools
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class ProfilerUtils
    {

        public static long GetMemorySize(object target)
        {

            long size = 0;
            using (var s = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                try {
                    formatter.Serialize(s, target);
                }
                catch (Exception) {}
                size = s.Length;
            }

            return size;

        }

    }
}
