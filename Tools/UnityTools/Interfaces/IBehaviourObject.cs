using System;

namespace Assets.Tools.UnityTools.Interfaces {
    public interface IBehaviourObject : IDisposable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}