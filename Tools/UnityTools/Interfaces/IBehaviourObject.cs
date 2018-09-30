using System;

namespace UnityToolsModule.Tools.UnityTools.ActorEntityModel {
    public interface IBehaviourObject : IDisposable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}