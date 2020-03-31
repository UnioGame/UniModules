using System;

namespace UniGame.UiElements.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class UiElementsFieldDrawerAttribute : Attribute
    {
        public int Priority { get; private set; }

        public UiElementsFieldDrawerAttribute(int priority = 0)
        {
            this.Priority = priority;
        }
    }
}
