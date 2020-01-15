using UniGreenModules.UniNodeSystem.Runtime;

namespace UniGreenModules.UniNodeSystem.Nodes.DebugTools
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Runtime.Core;
    using Runtime.Extensions;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniNodes.Runtime.Commands;

    [Serializable]
    public class LogNode : UniNode
    {
        private const string logPortName = "log";
        
        public LogMode mode = LogMode.Log;
        
        public string message = "LogNode";
        
        protected override void OnExecute()
        {
            PrintLog(GetMessage(), mode);
        }

        protected override void UpdateCommands(List<ILifeTimeCommand> nodeCommands)
        {
            base.UpdateCommands(nodeCommands);
            
            var inputMessagePort = this.UpdatePortValue(logPortName, PortIO.Input);
            nodeCommands.Add(new ContextBroadCastCommand<object>(x => 
                PrintLog($"{x.GetType().Name} : {x}", mode),inputMessagePort.value));
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
