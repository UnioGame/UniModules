namespace UniGame.ECS.Mono
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    [InlineProperty]
    public class EcsSystemUpdateGroup
    {
        public PlayerLoopTiming updateType = PlayerLoopTiming.Update;

        [SerializeReference]
        public List<IEcsSystemInstance> ecsSystems = new List<IEcsSystemInstance>();
    }
}