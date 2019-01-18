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

        public IContextData<IContext> Value;

        public void Bind(IContextData<IContext> value)
        {
            Value = value;
        }
    }
}
