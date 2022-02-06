namespace UniGame.ECS.Mono
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class EcsSystemConfiguration
    {
        [SerializeReference]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Searchable]
#endif
        public List<EcsSystemUpdateGroup> systems = new List<EcsSystemUpdateGroup>();
    }
}
