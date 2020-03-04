using UnityEngine;

namespace Taktika.UI.Runtime
{
    using Abstracts;
    using MVVM.Abstracts;
    using UniRx.Async;

    public class GameUiManager : MonoBehaviour, IUIController
    {
        public async UniTask<T> Open<T>(IViewModel viewModel) 
            where T : UiView<>
        {
            return null;
        }
    }
}
