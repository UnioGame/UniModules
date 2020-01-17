using UniGreenModules.UniNodeSystem.Runtime;

namespace UniGreenModules.UniNodes.Examples.TypeNodes
{
    using UniNodeSystem.Runtime.Core;
    using UnityEngine;

    [CreateNodeMenu("Examples/IntPortDemoNode")]
    public class IntPortDemoNode : UniNode
    {
        [SerializeField]
        private int input;

        [SerializeField]
        private UniPortValue inputPort;
    }
}
