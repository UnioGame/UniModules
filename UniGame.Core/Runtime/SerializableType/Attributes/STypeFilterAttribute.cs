namespace UniGreenModules.UniGame.Core.Runtime.SerializableType.Attributes
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field)]
    public class STypeFilterAttribute : PropertyAttribute
    {
        public readonly Type Type;
        public readonly string FieldName;

        public STypeFilterAttribute(Type type,string fieldName = nameof(SType.fullTypeName))
        {
            this.Type = type;
            this.FieldName = fieldName;
        }
    }
}
