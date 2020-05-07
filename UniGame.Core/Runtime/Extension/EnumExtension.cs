namespace UniGreenModules.UniCore.Runtime.Extension
{
    using System;

    public static class EnumFlagExtension  {

        public static bool IsFlagSet(this long value, long flag) 
        {
            return (value & flag) > 0;
        }
	
        public static bool IsFlagInSet(this long value, long[] flags) 
        {
            for (int i = 0; i < flags.Length; i++) {
                var flag = flags[i];
                if ((value & flag) > 0)
                    return true;
            }

            return false;
        }
	
    }
}
