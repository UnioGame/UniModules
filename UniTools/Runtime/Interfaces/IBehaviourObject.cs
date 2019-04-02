using UniModule.UnityTools.UniPool.Scripts;

namespace UniModule.UnityTools.Interfaces {
    public interface IBehaviourObject : IPoolable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}