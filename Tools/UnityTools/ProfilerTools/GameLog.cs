using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Assets.Scripts.ProfilerTools
{
    public static class GameLog
    {
        private const string TraceTemplate = @"{0} / {1}";
        private static Dictionary<string, string> _traces =
            new Dictionary<string, string>();

        public static bool Enabled = true;

        [Conditional("LOGS_ENABLED")]
        public static void Log(string message, Object source = null)
        {
            LogRuntime(message, source);
        }

        
        [Conditional("LOGS_ENABLED")]
        public static void LogFormatWithTrace(string template, params object[] values)
        {
            LogFormat(template,values);
            LogFormat("Stack Trace {0}", System.Environment.StackTrace);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogFormat(string template, Color color, params object[] values)
        {
            var message = values == null || values.Length == 0 ? template :
                string.Format(template, values);
            Log(message, color);
        }

        [Conditional("LOGS_ENABLED")]
        public static void Log(string message, Color color, Object source = null)
        {
            if (!Enabled || string.IsNullOrEmpty(message)) return;
            var colorMessage = GetColorTemplate(message, color);
            Log(colorMessage, source);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogWarning(string message, Color color, Object source = null)
        {
            if (!Enabled) return;
            var colorMessage = GetColorTemplate(message, color);
            LogWarning(colorMessage, source);
        }
  

        [Conditional("RESOURCES_LOG_ENABLED")]
        public static void LogResource(string message)
        {
            Debug.Log(message);
        }


        [Conditional("LOG_GAME_STATE")]
        public static void LogGameState(string message)
        {
            Debug.Log(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void EditorLogFormat(LogType logType,string format, params object[] objects) {
            switch (logType) {

                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    LogErrorFormat(format,objects);
                    break;
                case LogType.Warning:
                    Debug.LogWarningFormat(format,objects);
                    break;
                case LogType.Log:
                    Debug.LogFormat(format,objects);
                    break;
            }
        }
        
        
        [Conditional("LOGS_ENABLED")]
        public static void LogWarning(string message, Object source = null)
        {
            if (!Enabled) return;
            if (source)
            {
                Debug.LogWarning(message, source);
                return;
            }
            Debug.LogWarning(message);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogWarningFormat(string template, params object[] values)
        {
            if (!Enabled) return;
            Debug.LogWarningFormat(template,values);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogFormat(string template, params object[] values)
        {
            LogFormatRuntime(template, values);
        }

        public static void LogError(string message, Object source = null)
        {
            if (!Enabled) return;
            if (source)
            {
                Debug.LogError(message, source);
                return;
            }
            Debug.LogError(message);
        }

        public static void LogError(Exception message, Object source = null)
        {
            if (!Enabled) return;
            if (source)
            {
                Debug.LogError(message, source);
                return;
            }
            Debug.LogError(message);
        }

        public static void LogErrorFormat(string message, params object[] objects)
        {
            if (!Enabled) return;
            Debug.LogErrorFormat(message, objects);
        }

        public static void LogFormatRuntime(string template, params object[] values)
        {
            var message = values == null || values.Length == 0 ? template :
                string.Format(template, values);
            LogRuntime(message);
        }

        public static string GetColorTemplate(string message, Color color)
        {
            var colorMessage = string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
                (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f),
                message);
            return colorMessage;
        }
        
              
        public static void LogRuntime(string message, Object source = null)
        {
            if (!Enabled) return;
            if (source)
            {
                Debug.Log(message, source);
                return;
            }
            Debug.Log(message);
        }

        #region extensions

        [Conditional("LOGS_ENABLED")]
        public static void Log(this MonoBehaviour behaviour, string message,
            Color color)
        {
            if (!behaviour || !Enabled) return;
            Log(message, color, behaviour);
        }

        [Conditional("LOGS_ENABLED")]
        public static void Log(this MonoBehaviour behaviour, string message)
        {
            if (!behaviour || !Enabled) return;
            Log(message, behaviour);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogFormat(this MonoBehaviour behaviour,
            string message, params object[] values)
        {
            if (!behaviour || !Enabled) return;
            LogFormat(message, values);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogFormat(this MonoBehaviour behaviour,
            string message, Color color, params object[] values)
        {
            if (!behaviour || !Enabled) return;
            LogFormat(message, color, values);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogTrace(string traceKey, object traceNode)
        {
            if (!Enabled) return;
            var message = SetTrace(traceKey, traceNode);
            if (string.IsNullOrEmpty(message)) return;
            Log(message);
        }

        [Conditional("LOGS_ENABLED")]
        public static void LogTrace(string traceKey, object traceNode, Color color)
        {
            if (!Enabled) return;
            var message = SetTrace(traceKey, traceNode);
            if (string.IsNullOrEmpty(message)) return;
            Log(message, color);
        }

        private static string SetTrace(string traceKey, object traceNode)
        {
            if (traceNode == null)
            {
                return null;
            }
            var message = _traces.ContainsKey(traceKey)
                ? string.Format(TraceTemplate, _traces[traceKey], traceNode)
                : traceNode.ToString();
            _traces[traceKey] = message;
            return message;
        }

        #endregion
    }
}
