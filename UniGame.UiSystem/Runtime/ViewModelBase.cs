namespace Taktika.UI.Runtime
{
    using MVVM.Abstracts;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniRx;

    public class ViewModelBase : IViewModel
    {
        private LifeTimeDefinition  lifeTimeDefinition = new LifeTimeDefinition();
        
        public  ILifeTime LifeTime => lifeTimeDefinition.LifeTime;
        
        public IReadOnlyReactiveProperty<bool> IsActive { get; }
        
        public void Dispose()
        {
            lifeTimeDefinition.Terminate();
        }

    }
}