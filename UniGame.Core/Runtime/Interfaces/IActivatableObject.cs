namespace UniGreenModules.UniCore.Runtime.Interfaces {
    using ObjectPool.Runtime.Interfaces;

    public interface IActivatableObject : IPoolable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}