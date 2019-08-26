using UniGreenModules.UniNodeSystem.Runtime;
using UniGreenModules.UniNodeSystem.Runtime.Interfaces;

namespace UniGreenModules.UniNodeSystem.Nodes.Commands
{
    using System.Collections;
    using Runtime.Runtime;
    using UniStateMachine.Runtime.Interfaces;
    using UniTools.UniRoutine.Runtime;
    using Runtime.Extensions;

    public class FlowCommands : INodeCommand
    {
        private string input;
        private string output;
        private RoutineType routineType; 
        
        private IContextState<IEnumerator> behaviourState;

        public void Initialize(
            string inputPort, 
            string outputPort,
            RoutineType routine)
        {
            input = inputPort;
            output = outputPort;
            routineType = routine;
        }

        public IPortValue Input { get; protected set; }

        public IPortValue Output { get; protected set; }

        public void AttachToNode(IUniNode targetNode)
        {
            var inputPort = targetNode.UpdatePortValue(input, PortIO.Input);
            var outputPort = targetNode.UpdatePortValue(output, PortIO.Output);
            
            Input = inputPort.value;
            Output = outputPort.value;
        }

        public void Execute()
        {
            
        }

    }
}
