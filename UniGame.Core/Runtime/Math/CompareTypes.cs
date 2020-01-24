namespace UniGreenModules.UniCore.Runtime.Math
{
    using System;

    [Flags]
    public enum CompareTypes
    {
        More  = 1<<0,
        Less  = 1<<1,
        Equal = 1<<2,
        Any   = 1<<3,		
    }
}