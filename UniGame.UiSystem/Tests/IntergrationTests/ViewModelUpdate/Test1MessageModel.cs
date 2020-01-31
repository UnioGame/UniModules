namespace UniGreenModules.UniUiSystem.Tests.IntergrationTests.ViewModelUpdate
{
    using System;
    using UniRx;

    [Serializable]
    public class Test1MessageModel 
    {
        public StringReactiveProperty Message = new StringReactiveProperty();
    }
}
