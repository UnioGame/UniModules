using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.ProfilerTools;
using Debug = UnityEngine.Debug;

namespace UniStateMachine
{
    public static class StateLogger
    {

        [Conditional("STATE_MACHINE_LOG")]
        public static void LogState(string message, params object[] values)
        {
            GameLog.LogFormatRuntime(message,values);
        }

    }
}