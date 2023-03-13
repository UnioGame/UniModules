using UnityEditor;
using UnityEngine;

namespace UniGame.LeoEcs.Converter.Runtime
{
    using Leopotam.EcsLite;

    public static class LeoEcsConvertersData
    {
        public static EcsWorld World;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            World = null;
        }
    }
}