using UnityEngine;

namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
#if UNITY_EDITOR
    using System.Reflection;
#endif
    using AssetReferencies;
    using Core.Runtime.ScriptableObjects;
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniRx;
    using UnityEngine.U2D;

    [CreateAssetMenu(menuName = "UniGame/Addressables/SpriteAtlasConfiguration",fileName = nameof(AddressableSpriteAtlasConfiguration))]
    public class AddressableSpriteAtlasConfiguration : 
        DisposableScriptableObject, IAddressableSpriteAtlasHandler
    {
        #region inspector
        
        [SerializeField]
        public List<AssetReferenceSpriteAtlas> immortalAtlases = new List<AssetReferenceSpriteAtlas>();
        [SerializeField]
        public AddressblesAtlasesTagsMap atlasesTagsMap = new AddressblesAtlasesTagsMap();
        [SerializeField]
        public bool preloadImmortalAtlases = true;
        
        [SerializeField]
        [ReadOnlyValue]
        public bool isFastMode;

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
                //load immortal immediate
                foreach (var referenceSpriteAtlas in immortalAtlases) {
                    referenceSpriteAtlas.LoadAssetTaskAsync(LifeTime);
                }
            }
            
#if UNITY_EDITOR
            if(isFastMode) {
                foreach (var atlasPair in atlasesTagsMap) {
                    RegisterAtlas(atlasPair.Value.editorAsset);
                }
            }
#endif

            return this;
        }
        
        public void Unload() => _atlasesLifetime.Release();

        public async UniTask<bool> RequestSpriteAtlas(string guid)
        {
#if UNITY_EDITOR
            var atlas = await LoadAtlas(guid);
            if (atlas == null)
                return false;
            RegisterAtlas(atlas);
            return true; 
#endif
            return false;
        }

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

            var atlas = await LoadAtlas(atlasReference.AssetGUID);
            if (atlas == null)
                return;
            
            atlasAction(atlas);
        }

        private async UniTask<SpriteAtlas> LoadAtlas(string guid)
        {
            var atlasReferencePair = atlasesTagsMap.
                FirstOrDefault(x => x.Value.AssetGUID == guid);
            if (atlasReferencePair.Value == null)
                return null;
            
            var tag            = atlasReferencePair.Key;
            var atlasReference = atlasReferencePair.Value;
            
            GameLog.Log($"ATLAS: OnSpriteAtlasRequested : TAG {tag} GUID {atlasReference.AssetGUID}", Color.blue);

            var isImmortal = immortalAtlases.
                FirstOrDefault(x => x.AssetGUID == guid) != null;

            var lifetime = isImmortal ? LifeTime : _atlasesLifetime;
            var result   = await atlasReference.LoadAssetTaskAsync(lifetime);
            if (result == null) {
                GameLog.LogError($"ATLAS: Null Atlas Result by TAG {tag}");
            }
            else {
                lifetime.AddCleanUpAction(() => GameLog.Log($"ATLAS: LifeTime Finished : {tag}"));
                GameLog.Log($"ATLAS: Register NEW TAG : {tag}");
            }

            return result;
        }

        protected override void OnDispose() => _lifeTimeDefinition.Release();

        protected override void OnActivate()
        {
            _atlasesLifetime = new LifeTimeDefinition();
            _lifeTimeDefinition.AddCleanUpAction(() => _atlasesLifetime.Terminate());
        }

#if UNITY_EDITOR
        
        private static MethodInfo _registerMethod;
        private static MethodInfo RegisterMethod => (_registerMethod = _registerMethod == null ?
            typeof(SpriteAtlasManager).
                GetMethod("Register", BindingFlags.Static | BindingFlags.NonPublic) : _registerMethod);

        [Conditional("UNITY_EDITOR")]
        private void RegisterAtlas(SpriteAtlas atlas) => RegisterMethod?.Invoke(null, new object[] {atlas});
        
#endif
    }
}
