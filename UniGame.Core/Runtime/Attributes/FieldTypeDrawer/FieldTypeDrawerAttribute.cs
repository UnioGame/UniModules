using System;

namespace UniGreenModules.UniGame.Core.Runtime.Attributes.FieldTypeDrawer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FieldTypeDrawerAttribute : Attribute
    {
        public Type fieldType;

        /// <summary>
        /// Register a custom view for a type in the FieldFactory class
        /// </summary>
        /// <param name="fieldType"></param>
        public FieldTypeDrawerAttribute(Type fieldType)
        {
            this.fieldType = fieldType;
        }
    }
}
