namespace UniGreenModules.UniCore.Runtime.Extension
{
    using System;

    public static class EnumFlagExtension  {

        public static bool IsFlagSet<TEnum>(this TEnum value, TEnum flag)
            where TEnum : Enum
        {
            var source = Convert.ToInt64(value); 
            var flagValue = Convert.ToInt64(flag); 
            
            return IsFlagSet(source,flagValue);
        }
        
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
