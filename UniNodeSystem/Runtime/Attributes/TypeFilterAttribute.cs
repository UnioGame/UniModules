using System;

namespace Modules.UniTools.UniNodeSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class TypeFilterAttribute : Attribute
    {

        public Type[] Types;
        
        public TypeFilterAttribute(params Type[] types)
        {
            Types = types;
        }
        
    }
}
