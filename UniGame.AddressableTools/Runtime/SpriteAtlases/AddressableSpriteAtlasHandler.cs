using UnityEngine;

namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Runtime.ScriptableObjects;
    using SerializableContext.Runtime.Addressables;
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
        
        public List<AssetReferenceSpriteAtlas> _unloadableAtlases = new List<AssetReferenceSpriteAtlas>();

        public AddressbleAtlasesTagsMap _atlasesTagsMap = new AddressbleAtlasesTagsMap();
        
        #endregion

        private LifeTimeDefinition _atlasesLifetime;

        public IDisposable Execute()
        {
            Observable.FromEvent(
                    x => SpriteAtlasManager.atlasRequested += OnSpriteAtlasRequested,
                    x => SpriteAtlasManager.atlasRequested -= OnSpriteAtlasRequested).
                Subscribe().
                AddTo(LifeTime);
            return this;
        }

        public void Set(IReadOnlyList<AssetReferenceSpriteAtlas> atlases)
        {
            _atlasesTagsMap.Clear();

            foreach (var atlasRef in atlases) {
                var atlas = atlasRef.editorAsset;
                _atlasesTagsMap[atlas.tag] = atlasRef;
            }
        }

        public void Unload() => _atlasesLifetime.Release();

        private async void OnSpriteAtlasRequested(string tag, Action<SpriteAtlas> atlasAction)
        {
            if (_atlasesTagsMap.TryGetValue(tag, out var atlasReference) == false)
                return;
            
            GameLog.Log($"OnSpriteAtlasRequested : TAG {tag}", Color.blue);

            var isUnloadable = _unloadableAtlases.
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
    }
}
