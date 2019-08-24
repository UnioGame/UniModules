using UniGreenModules.UniNodeSystem.Runtime;

namespace UniGreenModules.UniNodeSystem.Nodes.DebugTools
{
    using UniCore.Runtime.ProfilerTools;

    public class LogNode : UniNode
    {
        public LogMode mode = LogMode.Log;

        public string message = "LogNode";
        
        protected override void OnExecute()
        {
            switch (mode) {
                case LogMode.Runtime:
                    GameLog.LogRuntime(GetMessage());
                    break;
                case LogMode.Log:
                    GameLog.Log(GetMessage());
                    break;
                case LogMode.Warning:
                    GameLog.LogWarning(GetMessage());
                    break;
                case LogMode.Error:
                    GameLog.LogError(GetMessage());
                    break;
                case LogMode.Exception:
                    GameLog.LogError(GetMessage());
                    break;
            }
        }

        protected virtual string GetMessage()
        {
            return message;
        }
    }
}
