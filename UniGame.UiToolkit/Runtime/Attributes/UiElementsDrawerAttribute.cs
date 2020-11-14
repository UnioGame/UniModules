using System;

namespace UniGame.Core.Runtime.Attributes.FieldTypeDrawer
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class UiElementsDrawerAttribute : Attribute, IPriorityValue
    {
        public int priority = 0;
        public Type[] types;

        public int Priority => priority;

        public Type[] Types => types;

        public UiElementsDrawerAttribute(
            int priority = 0,
            params Type[] targetTypes)
        {
            this.priority = priority;
            this.types = targetTypes;
        }
    }
}
