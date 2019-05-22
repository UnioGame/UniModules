namespace UniGreenModules.UniCore.Runtime.Attributes
{
    using System;

    [AttributeUsage (AttributeTargets.Field,AllowMultiple = true,Inherited = true)]
    public class TargetTypeAttribute : Attribute
    {

        public Type TargetType;

        public TargetTypeAttribute(Type targetType)
        {
            TargetType = targetType;
        }
        
    }
}
