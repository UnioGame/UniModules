namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using System;
    using System.Collections.Generic;
    using Taktika.MVVM.Abstracts;
    using Taktika.UI.Abstracts;
    using UniRx.Async;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Addressables = UnityEngine.AddressableAssets.Addressables;

    public class UIController : MonoBehaviour, IUIController
    {
        [SerializeField]
        private Canvas WindowCanvas;
        
        private List<IView> views = new List<IView>();

        private void Awake()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        // HACK
        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            foreach (var view in views)
            {
                //if (view != null)
                    //view.Close();
            }
            views.Clear();
        }

        public async UniTask<T> Open<T>(IViewModel viewModel) 
            where T :class, IView
        {
            var address = GetResourceAddress(typeof(T));
            var handler = Addressables.LoadAssetAsync<GameObject>(address);
            var asset = await handler.Task;
            var window = GameObject.Instantiate(asset, WindowCanvas.transform);
            var windowView = window.GetComponent<T>();
            windowView.SetViewModel(viewModel);
            views.Add(windowView);
            return windowView;
        }

        private string GetResourceAddress(Type type)
        {
            // TO DO map вместо рефлексии

            var attribute = (ResourceAttribute)Attribute.GetCustomAttribute(type, typeof(ResourceAttribute));
            return attribute.Address;
        }


        public void Dispose()
        {
            
        }
    }
}
