namespace UniModules.UniGame.Core.Editor.Tools
{
    using System;
    using System.Diagnostics;
    using UnityEngine;
    using Debug = UnityEngine.Debug;

    public static class UniEditorProfiler 
    {
        public static void LogTime(Action action)
        {
            var name = action?.Method.Name;
            name = string.IsNullOrEmpty(name) ? StackTraceUtility.ExtractStackTrace() : name;
            LogTime(name, action);
        }
        
        public static void LogTime(string name,Action action)
        {
            var timer     = new Stopwatch();
            try
            {
                timer.Start();
                action?.Invoke();
                var startTime = timer.ElapsedMilliseconds;
                Debug.Log($"UniEditorProfiler : {name} : {timer.ElapsedMilliseconds} ms");
            }
            finally{
                timer.Stop();
            }
        }
        
    }
}
