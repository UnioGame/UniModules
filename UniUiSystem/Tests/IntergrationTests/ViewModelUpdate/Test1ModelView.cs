using UniGreenModules.UniUiSystem.Runtime;

namespace UniGreenModules.UniUiSystem.Tests.IntergrationTests.ViewModelUpdate
{
    using System.Collections;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine;

    public class Test1ModelView : UiView<Test1MessageModel>
    {
        protected override void OnInitialize(Test1MessageModel model)
        {
            model.Message.Subscribe(x => UpdateView()).AddTo(LifeTime);
        }

        protected override IEnumerator OnUpdateView()
        {
            Debug.Log($"VIEW update value : {ModelValue.Message}");
            
            yield break;
        }
    }
}
