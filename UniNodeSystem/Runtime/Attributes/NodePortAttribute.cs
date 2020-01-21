using System;

namespace UniGreenModules.UniNodeSystem.Runtime.Attributes
{
    using Core;

    public class NodePortAttribute : Attribute
    {
        public string Name;
        public PortIO Direction;

        public NodePortAttribute(string name, PortIO direction = PortIO.Input)
        {
            this.Name = name;
            this.Direction = direction;
        }
        
    }
}
