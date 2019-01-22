using System;
using Assets.Tools.UnityTools.Interfaces;
using XNode;

namespace UnityTools.UniVisualNodeSystem
{
    [Serializable]
    public class PortDefinition
    {
        public string Name;
        public PortIO Direction;

    }
}
