using System;

namespace UniGame.UiElements.Runtime.Attributes
{
    using Core.Runtime.Attributes.FieldTypeDrawer;

    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class UiElementsFieldDrawerAttribute : Attribute,IPriorityValue
    {
        public int Priority { get; private set; }

        public UiElementsFieldDrawerAttribute(int priority = 0)
        {
            this.Priority = priority;
        }
    }
}
