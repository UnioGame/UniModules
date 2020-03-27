using System;

namespace UniGame.Core.Runtime.Attributes.FieldTypeDrawer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UiElementsDrawerAttribute : Attribute
    {
        public bool IsActive = true;
        public int Priority = 0;
        
        public UiElementsDrawerAttribute(int priority = 0,bool isActive = true)
        {
            this.Priority = priority;
            this.IsActive = isActive;
        }
    }
}
