namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using System;

    public interface IObjectValue
    {
        Type Type { get; }
        object GetValue();
    }
}