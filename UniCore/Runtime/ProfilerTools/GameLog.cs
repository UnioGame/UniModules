using System;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.ProfilerTools
{
    public static class GameLog
    {

        private static IGameLogger _logger;
        public static IGameLogger Logger
        {
            get
            {
                if(_logger == null)
                    _logger = new GameLogger("GameLog",true);
                return _logger;
            }

        }
        
        private static IGameLogger _messageLogger;
        public static IGameLogger MessageLogger
        {
            get
            {
                if(_messageLogger == null)
                    _messageLogger = new GameLogger("MESSAGE",true);
                return _messageLogger;
            }

        }
        
        [Conditional("LOGS_ENABLED")]
        public static void Log(string message, Object source = null)
        {
            Logger.LogRuntime(message, source);
        }
        
        [Conditional("LOGS_ENABLED")]
        public static void LogFormatWithTrace(string template, params object[] values)
        {
            Logger.LogFormatWithTrace(template, values);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogFormat(string template, Color color, params object[] values)
        {
            Logger.LogFormat(template,color,values);
        }

        [Conditional("LOGS_ENABLED")]
        public static void Log(string message, Color color, Object source = null) {
            Logger.LogRuntime(message, color, source);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogWarning(string message, Color color, Object source = null)
        {
            Logger.LogWarning(message,color, source);
        }
  
        [Conditional("RESOURCES_LOG_ENABLED")]
        public static void LogResource(string message)
        {
            Logger.Log(message);
        }


        [Conditional("LOG_GAME_STATE")]
        public static void LogGameState(string message)
        {
            Logger.Log(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void EditorLogFormat(LogType logType,string format, params object[] objects)
        {
            Logger.EditorLogFormat(logType, format, objects);
        }
        
        
        [Conditional("LOGS_ENABLED")]
        public static void LogWarning(string message, Object source = null)
        {
            Logger.LogWarning(message, source);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogWarningFormat(string template, params object[] values)
        {
            Logger.LogWarningFormat(template,values);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogFormat(string template, params object[] values)
        {
            Logger.LogFormatRuntime(template, values);
        }

        [Conditional("ENABLE_MESSAGE_LOG")]
        public static void LogMessage(string template, params object[] values)
        {
            MessageLogger.LogFormat(template,values);
        }

        public static void LogError(string message, Object source = null)
        {
            Logger.LogError(message, source);
        }

        public static void LogError(Exception message, Object source = null)
        {
            Logger.LogError(message, source);
        }

        public static void LogErrorFormat(string message, params object[] objects)
        {
            Logger.LogErrorFormat(message, objects);
        }

        public static void LogFormatRuntime(string template, params object[] values)
        {
            Logger.LogFormatRuntime(template,values);
        }
        
        public static void LogRuntime(string message, Color color, Object source = null)
        {
            Logger.LogRuntime(message,color, source);
        }

        public static string GetColorTemplate(string message, Color color)
        {
            return Logger.GetColorTemplate(message, color);
        }
        
        public static void LogRuntime(string message, Object source = null)
        {
            Logger.Log(message,source);
        }

    }
}
