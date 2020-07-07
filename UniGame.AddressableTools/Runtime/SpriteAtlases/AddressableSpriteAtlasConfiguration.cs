using UnityEngine;

namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssetReferencies;
    using Core.Runtime.ScriptableObjects;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniRx;
    using UnityEngine.U2D;

    [CreateAssetMenu(menuName = "UniGame/Addressables/SpriteAtlasConfiguration",fileName = nameof(AddressableSpriteAtlasConfiguration))]
    public class AddressableSpriteAtlasConfiguration : DisposableScriptableObject, IAddressableSpriteAtlasHandler
    {
        #region inspector
        
        [SerializeField]
        public List<AssetReferenceSpriteAtlas> immortalAtlases = new List<AssetReferenceSpriteAtlas>();
        [SerializeField]
        public AddressblesAtlasesTagsMap atlasesTagsMap = new AddressblesAtlasesTagsMap();
        [SerializeField]
        public bool preloadImmortalAtlases = true;
        
        #endregion

        private LifeTimeDefinition _atlasesLifetime;

        public IDisposable Execute()
        {
            Observable.FromEvent(
                    x => SpriteAtlasManager.atlasRequested += OnSpriteAtlasRequested,
                    x => SpriteAtlasManager.atlasRequested -= OnSpriteAtlasRequested).
                Subscribe().
                AddTo(LifeTime);

            if (preloadImmortalAtlases) {
                //load unloadable immisiate
                foreach (var referenceSpriteAtlas in immortalAtlases) {
                    referenceSpriteAtlas.LoadAssetTaskAsync(LifeTime);
                }
            }

            return this;
        }
        
        public void Unload() => _atlasesLifetime.Release();

        [ContextMenu("Validate")]
        public void Validate()
        {
#if UNITY_EDITOR
            immortalAtlases.RemoveAll(x => x == null || x.editorAsset == null);

            var keys = atlasesTagsMap.Keys.ToList();
            foreach (var key in keys) {
                var reference = atlasesTagsMap[key];
                if (reference == null || reference.editorAsset == null)
                    atlasesTagsMap.Remove(key);
            }
#endif
        }

        private async void OnSpriteAtlasRequested(string tag, Action<SpriteAtlas> atlasAction)
        {
            if (atlasesTagsMap.TryGetValue(tag, out var atlasReference) == false)
                return;
            
            GameLog.Log($"OnSpriteAtlasRequested : TAG {tag}", Color.blue);

            var isImmortal = immortalAtlases.
                FirstOrDefault(x => x.AssetGUID == atlasReference.AssetGUID) != null;

            var lifetime = isImmortal ? LifeTime : _atlasesLifetime;
            var result = await atlasReference.LoadAssetTaskAsync(lifetime);
            if (result == null) {
                GameLog.LogError($"Null Atlas Result by TAG {tag}");
                return;
            }
            atlasAction(result);
        }

        protected override void OnDispose() => _lifeTimeDefinition.Release();

        protected override void OnActivate()
        {
            _atlasesLifetime = new LifeTimeDefinition();
            _lifeTimeDefinition.AddCleanUpAction(() => _atlasesLifetime.Terminate());
        }

    }
}
