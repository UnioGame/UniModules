using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.ProfilerTools
{
    public interface IGameLogger
    {
        void Log(string message, Object source = null);
        void LogFormatWithTrace(string template, params object[] values);
        void LogFormat(string template, Color color, params object[] values);
        void Log(string message, Color color, Object source = null);
        void LogWarning(string message, Color color, Object source = null);
        void EditorLogFormat(LogType logType,string format, params object[] objects);
        void LogWarning(string message, Object source = null);
        void LogWarningFormat(string template, params object[] values);
        void LogFormat(string template, params object[] values);
        void LogError(string message, Object source = null);
        void LogError(Exception message, Object source = null);
        void LogErrorFormat(string message, params object[] objects);
        void LogFormatRuntime(string template, params object[] values);
        void LogRuntime(string message, Color color, Object source = null);
        string GetColorTemplate(string message, Color color);
        void LogRuntime(string message, Object source = null);
    }
}