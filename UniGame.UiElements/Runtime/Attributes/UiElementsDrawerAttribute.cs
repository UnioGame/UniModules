using System;

namespace UniGame.Core.Runtime.Attributes.FieldTypeDrawer
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class UiElementsDrawerAttribute : Attribute
    {
        public int Priority = 0;
        public Type[] Types;
        
        public UiElementsDrawerAttribute(
            int priority = 0,
            params Type[] targetTypes)
        {
            this.Priority = priority;
            this.Types = targetTypes;
        }
    }
}
