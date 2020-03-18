namespace UniModules.UniGame.AddressableTools.Runtime.AssetReferencies
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class AssetReferenceParticleSystem : AssetReferenceT<ParticleSystem>
    {
        public AssetReferenceParticleSystem(string guid) : base(guid)
        {
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(ParticleSystem));
            return prefab != null;
#else
            return false;
#endif
        }
    }
}