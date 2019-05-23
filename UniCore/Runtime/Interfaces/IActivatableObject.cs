namespace UniGreenModules.UniCore.Runtime.Interfaces {
    using ObjectPool.Interfaces;

    public interface IActivatableObject : IPoolable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}