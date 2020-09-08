using System;
using System.Linq;
using UniGreenModules.UniGame.Core.Runtime.Extension;
using UniModules.UniGame.Core.EditorTools.Editor.AssetOperations;
using UnityEngine;

namespace UniModules.UniGame.TypeConverters.Editor
{
    using Newtonsoft.Json;
    using UniGreenModules.UniCore.Runtime.ReflectionUtils;

    [Serializable]
    public class JsonSerializableClassConverter : BaseTypeConverter
    {
        private static Type stringType = typeof(string);

        public sealed override bool CanConvert(Type fromType, Type toType)
        {
            if (fromType != stringType && toType != stringType)
                return false;
            if (fromType.IsRegularType() && toType.IsRegularType())
                return false;

            if (fromType == stringType && toType.HasAttribute<SerializableAttribute>())
                return true;
            
            if (toType == stringType && fromType.HasAttribute<SerializableAttribute>())
                return true;

            return false;
        }

        public sealed override (bool isValid, object result) TryConvert(object source, Type target)
        {
            if (source == null || !CanConvert(source.GetType(), target)) {
                return (false, source);
            }
            
            if (target == stringType) {
                var textValue = JsonConvert.SerializeObject(source);
                return (true,textValue);
            }

            if (source is string value) {
                var serializedData = JsonConvert.DeserializeObject(value, target);
                return (true,serializedData);
            }
            
            return (false, source);
        }

    }
}