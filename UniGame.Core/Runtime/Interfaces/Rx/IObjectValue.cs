namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using System;

    public interface IReadonlyObjectValue
    {
        Type Type { get; }
        object GetValue();
    }
    
    public interface IObjectValue
    {
        void SetObjectValue(object value);
    }
}