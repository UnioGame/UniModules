namespace UniGreenModules.UniNodes.CommonNodes
{
    using UniNodeSystem.Runtime;
    using UniNodeSystem.Runtime.Extensions;

    public class PortsHubNode : UniNode
    {
        private const string portHubTemplate = "port{0}";

        public int PortsCount;

        protected override void OnRegisterPorts()
        {
            base.OnRegisterPorts();

            for (var i = 0; i < PortsCount; i++)
            {

                var portName = string.Format(portHubTemplate, i);
                this.CreatePortPair(portName,true);

            }
            
        }
    }
}
