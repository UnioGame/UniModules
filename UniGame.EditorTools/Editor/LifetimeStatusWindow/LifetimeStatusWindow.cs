#if ODIN_INSPECTOR

using Sirenix.OdinInspector.Editor;

namespace UniModules.UniGame.EditorTools.Editor.LifetimeStatusWindow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Runtime.DataFlow.Interfaces;
    using Core.Runtime.ScriptableObjects;
    using Sirenix.OdinInspector;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEditor;
    using UnityEngine;

    public class LifetimeStatusWindow : OdinEditorWindow 
    {

        [MenuItem("UniGame/Tools/LifeTimeStatus")]
        private static void OpenWindow()
        {
            var window = GetWindow<LifetimeStatusWindow>();
            window.InitializeWindow();
            window.Show();
        }

        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private List<LifeTimeEditorItem> _lifeTimesObjects = new List<LifeTimeEditorItem>();

        
        [OnValueChanged("OnSearchChanged")]
        [HideLabel]
        public string searchField;

        [OnValueChanged("OnSearchChanged")]
        public bool hideTerminated = false;
        
        [Space]
        [InlineProperty]
        [HideLabel]
        public List<LifeTimeEditorItem> _filteredlifeTimesObjects = new List<LifeTimeEditorItem>();
        
        public void InitializeWindow()
        {
            foreach (var lifeTime in LifetimeObjectData.LifeTimes) {
                lifeTime.TryGetTarget(out var item);
                Add(item);
            }
            
            LifetimeObjectData.LifeTimes.
                ObserveAdd().
                Subscribe(x => Add(x.Value)).
                AddTo(_lifeTime);
            
            // LifetimeObjectData.LifeTimes.
            //     ObserveRemove().
            //     Subscribe(x => Remove(x.Value)).
            //     AddTo(_lifeTime);
            UpdateFilteredItems();
        }

        public void UpdateFilteredItems()
        {
            _filteredlifeTimesObjects.Clear();
            _filteredlifeTimesObjects.AddRange(_lifeTimesObjects.Where(Validate));
        }

        private bool Validate(LifeTimeEditorItem lifeTimeItem)
        {
            if (hideTerminated && lifeTimeItem.IsTerminated)
                return false;
            if (string.IsNullOrEmpty(searchField) || lifeTimeItem.Name.IndexOf(searchField, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            return false;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _lifeTime.Terminate();
        }

        private void Add(WeakReference<ILifeTime> lifetimeReference)
        {
            lifetimeReference.TryGetTarget(out var lifeTime);
            Add(lifeTime);
        }
        
        private void Add(ILifeTime lifetime)
        {
            if (lifetime is LifetimeScriptableObject lifetimeScriptableObject) {
                _lifeTimesObjects.Add(Create(lifetimeScriptableObject));
            }
        }
        
        private void Remove(LifetimeScriptableObject lifetime)
        {
            var item = _lifeTimesObjects.FirstOrDefault(x => x.LifeTime == lifetime);
            _lifeTimesObjects.Remove(item);
        }
        
        private LifeTimeEditorItem Create(LifetimeScriptableObject lifetimeScriptableObject)
        {
            return new LifeTimeEditorItem(lifetimeScriptableObject);
        }

        private void OnSearchChanged()
        {
            UpdateFilteredItems();
        }
    }
}



#endif