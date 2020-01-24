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
    

        public static T GetRandom()
        {
            var index = Random.Range(0, Values.Count);
            return Values[index];
        }

    }

}
