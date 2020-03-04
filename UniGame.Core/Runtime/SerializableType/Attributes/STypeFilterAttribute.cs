using UnityEngine;

namespace Taktika.GameRuntime.Types.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class STypeFilterAttribute : PropertyAttribute
    {
        public readonly Type Type;
        public readonly string FieldName;

        public STypeFilterAttribute(Type type,string fieldName = "type")
        {
            this.Type = type;
            this.FieldName = fieldName;
        }
    }
}
