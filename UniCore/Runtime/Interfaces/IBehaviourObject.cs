namespace UniGreenModules.UniCore.Runtime.Interfaces {
    using ObjectPool.Interfaces;

    public interface IBehaviourObject : IPoolable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}