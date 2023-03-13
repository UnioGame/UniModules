using System;

namespace UniGame.LeoEcs.ViewSystem.Components
{
    [Serializable]
    public struct CloseViewByTypeRequest
    {
        public Type Type;
    }
}