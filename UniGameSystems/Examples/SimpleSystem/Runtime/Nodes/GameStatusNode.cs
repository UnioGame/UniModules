using UniGreenModules.UniGameSystem.Nodes;
using UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime;

namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Nodes
{
    [CreateNodeMenu("Examples/DemoSystem/GameStatusNode")]
    public class GameStatusNode : GameServiceNode<DemoSystemStatusService> {}
}
