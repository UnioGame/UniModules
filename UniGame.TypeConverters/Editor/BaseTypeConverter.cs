namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.TypeConverters
{
    using System;
    using Abstract;
    using UnityEngine;

    [Serializable]
    public abstract class BaseTypeConverter : ScriptableObject,ITypeConverter
    {
        public abstract bool CanConvert(Type fromType, Type toType);

        
        public abstract (bool isValid, object result) TryConvert(object source, Type target);
    }
}