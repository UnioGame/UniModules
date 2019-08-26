using UniGreenModules.UniNodeSystem.Runtime;

namespace UniGreenModules.UniNodeSystem.Nodes.DebugTools
{
    using System.Collections.Generic;
    using Commands;
    using Runtime.Extensions;
    using Runtime.Runtime;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;

    public class LogNode : UniNode
    {
        private const string logPortName = "log";
        
        public LogMode mode = LogMode.Log;
        
        public string message = "LogNode";
        
        protected override void OnExecute()
        {
            PrintLog(GetMessage(), mode);
        }

        protected override void UpdateNodeCommands(List<ILifeTimeCommand> nodeCommands)
        {
            base.UpdateNodeCommands(nodeCommands);
            
            var inputMessagePort = this.UpdatePortValue(logPortName, PortIO.Input);
            
            nodeCommands.Add(new NodeDataActionCommand(inputMessagePort.value,() => PrintLog(message, mode)));
            
            nodeCommands.Add(new PortActionCommand<string>(x => PrintLog(x, mode),inputMessagePort.value));
        }

        protected virtual string GetMessage()
        {
            return message;
        }

        private void PrintLog(string messageData, LogMode logMode)
        {
            switch (logMode) {
                case LogMode.Runtime:
                    GameLog.LogRuntime(messageData);
                    break;
                case LogMode.Log:
                    GameLog.Log(messageData);
                    break;
                case LogMode.Warning:
                    GameLog.LogWarning(messageData);
                    break;
                case LogMode.Error:
                    GameLog.LogError(messageData);
                    break;
                case LogMode.Exception:
                    GameLog.LogError(messageData);
                    break;
            }
        }
    }
}
