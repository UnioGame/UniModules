using UnityEngine;

namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using AssetReferencies;
    using Core.Runtime.ScriptableObjects;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniRx;
    using UnityEngine.U2D;

    [CreateAssetMenu(menuName = "UniGame/Addressables/SpriteAtlasManager",fileName = nameof(AddressableSpriteAtlasHandler))]
    public class AddressableSpriteAtlasHandler : DisposableScriptableObject, IAddressableSpriteAtlasHandler
    {
        #region inspector
        
        public List<AssetReferenceSpriteAtlas> _immortalAtlases = new List<AssetReferenceSpriteAtlas>();

        public AddressbleAtlasesTagsMap _atlasesTagsMap = new AddressbleAtlasesTagsMap();

        public bool _preloadunImmortalAtlases = true;
        
        #endregion

        private LifeTimeDefinition _atlasesLifetime;

        public IDisposable Execute()
        {
            Observable.FromEvent(
                    x => SpriteAtlasManager.atlasRequested += OnSpriteAtlasRequested,
                    x => SpriteAtlasManager.atlasRequested -= OnSpriteAtlasRequested).
                Subscribe().
                AddTo(LifeTime);

            if (_preloadunImmortalAtlases) {
                //load unloadable immisiate
                foreach (var referenceSpriteAtlas in _immortalAtlases) {
                    referenceSpriteAtlas.LoadAssetTaskAsync(LifeTime);
                }
            }

            return this;
        }
        
        public void Unload() => _atlasesLifetime.Release();

        [ContextMenu("Validate")]
        public void Validate() => OnValidate();

        private async void OnSpriteAtlasRequested(string tag, Action<SpriteAtlas> atlasAction)
        {
            if (_atlasesTagsMap.TryGetValue(tag, out var atlasReference) == false)
                return;
            
            GameLog.Log($"OnSpriteAtlasRequested : TAG {tag}", Color.blue);

            var isUnloadable = _immortalAtlases.
                FirstOrDefault(x => x.AssetGUID == atlasReference.AssetGUID) != null;

            var lifetime = isUnloadable ? LifeTime : _atlasesLifetime;
            var result = await atlasReference.LoadAssetTaskAsync(lifetime);
            if (result == null) {
                GameLog.LogError($"Null Atlas Result by TAG {tag}");
                return;
            }
            atlasAction(result);
        }

        protected override void OnDispose() => _atlasesLifetime.Release();

        protected override void OnActivate()
        {
            _atlasesLifetime = new LifeTimeDefinition();
            _lifeTimeDefinition.AddCleanUpAction(() => _atlasesLifetime.Terminate());
        }


        protected void OnValidate()
        {
#if UNITY_EDITOR
            _immortalAtlases.RemoveAll(x => x == null || x.editorAsset == null);

            var keys = _atlasesTagsMap.Keys.ToList();
            foreach (var key in keys) {
                var reference = _atlasesTagsMap[key];
                if (reference == null || reference.editorAsset == null)
                    _atlasesTagsMap.Remove(key);
            }
#endif
        }
    }
}
