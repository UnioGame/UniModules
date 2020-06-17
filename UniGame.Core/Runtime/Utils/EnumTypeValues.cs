namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Random = UnityEngine.Random;

    public static class EnumValue <T>
    {
        
        public static readonly IReadOnlyList<T> Values =
            Enum.GetValues(typeof(T)).
                OfType<T>().ToList();
    
        public static readonly IReadOnlyList<string> Names =
            Enum.GetNames(typeof(T)).ToList();

        public static string GetName(T value)
        {
            for (var i = 0; i < Values.Count; i++) {
                var enumValue = Values[i];
                if (EqualityComparer<T>.Default.Equals(value,enumValue)) {
                    return Names[i];
                }
            }
            
            return String.Empty;
        }

        public static T GetRandom()
        {
            var index = Random.Range(0, Values.Count);
            return Values[index];
        }

    }

}
