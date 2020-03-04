namespace Taktika.MVVM.Abstracts
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IView : ILifeTimeContext, IPoolable
    {
        void SetViewModel(IViewModel vm);
        void Open();
    }
}