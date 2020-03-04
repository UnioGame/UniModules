namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using Abstracts;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
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