using System;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Interfaces {
    public interface IBehaviourObject : IPoolable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}