namespace UniGame.LeoEcs.Shared.Components
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Component with single transform data 
    /// </summary>
    [Serializable]
    public struct TransformComponent
    {
        public Transform Value;
    }
}