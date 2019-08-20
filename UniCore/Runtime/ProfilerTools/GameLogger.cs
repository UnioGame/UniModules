namespace UniGreenModules.UniCore.Runtime.ProfilerTools
{
    using System;
    using Utils;
    using UnityEngine;
    using Debug = UnityEngine.Debug;
    using Object = UnityEngine.Object;

    public class GameLogger : IGameLogger
    {
        private const string NameTemplate = @"[{0} #{1}]:";
        private const string LogTemplate  = @"{0} :{1} {2}";

        private int  _counter;
        private bool _addTimeStamp = true;

        public bool   Enabled = true;
        public string Name;

        protected string LogPrefix => GetNamePrefix();

        public GameLogger(string name)
        {
            Name = name;
        }

        public void Log(string message, Object source = null)
        {
#if UNITY_EDITOR || GAME_LOGS_ENABLED

            LogRuntime(message, source);

#endif
        }

        public void LogFormatWithTrace(string template, params object[] values)
        {
#if UNITY_EDITOR || GAME_LOGS_ENABLED

            LogFormat(template, values);
            LogFormat("Stack Trace {0}", System.Environment.StackTrace);

#endif
        }

        public void LogFormat(string template, Color color, params object[] values)
        {
#if UNITY_EDITOR || GAME_LOGS_ENABLED

            var message = values == null || values.Length == 0 ? template : string.Format(template, values);
            Log(message, color);

#endif
        }

        public void LogFormat(string template, params object[] values)
        {
#if UNITY_EDITOR || GAME_LOGS_ENABLED
            LogFormatRuntime(template, values);
#endif
        }

        public void Log(string message, Color color, Object source = null)
        {
#if UNITY_EDITOR || GAME_LOGS_ENABLED
            LogRuntime(message, color, source);
#endif
        }

        public void LogWarning(string message, Color color, Object source = null)
        {
            if (!Enabled) return;
            var colorMessage = GetColorTemplate(message, color);
            LogWarning(colorMessage, source);
        }

        public void EditorLogFormat(LogType logType, string format, params object[] objects)
        {
            var prefix = GetNamePrefix();
            format = prefix + format;

            switch (logType) {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    LogErrorFormat(format, objects);
                    break;
                case LogType.Warning:
                    Debug.LogWarningFormat(format, objects);
                    break;
                case LogType.Log:
                    LogFormat(format, objects);
                    break;
            }
        }

        public void LogWarning(string message, Object source = null)
        {
            if (!Enabled) return;
            if (source) {
                Debug.LogWarningFormat(GetLogMessageWithPrefix(message), source);
                return;
            }

            Debug.LogWarningFormat(GetLogMessageWithPrefix(message));
        }

        public void LogWarningFormat(string template, params object[] values)
        {
            if (!Enabled) return;
            var message = string.Format(template, values);
            Debug.LogWarningFormat(GetLogMessageWithPrefix(message));
        }

        public void LogError(string message, Object source = null)
        {
            if (source) {
                Debug.LogError(message, source);
                return;
            }

            Debug.LogError(message);
        }

        public void LogError(Exception message, Object source = null)
        {
            if (source) {
                Debug.LogError(message, source);
                return;
            }

            Debug.LogError(message);
        }

        public void LogErrorFormat(string message, params object[] objects)
        {
            Debug.LogErrorFormat(message, objects);
        }

        public void LogFormatRuntime(string template, params object[] values)
        {
            var message = values == null || values.Length == 0 ? template : string.Format(template, values);
            LogRuntime(message);
        }

        public void LogRuntime(string message, Color color, Object source = null)
        {
            if (!Enabled || string.IsNullOrEmpty(message)) return;
            var colorMessage = GetColorTemplate(message, color);
            LogRuntime(colorMessage, source);
        }

        public string GetColorTemplate(string message, Color color)
        {
            var colorMessage = string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
                                             (byte) (color.r * 255f), (byte) (color.g * 255f), (byte) (color.b * 255f),
                                             message);
            return colorMessage;
        }

        public void LogRuntime(string message, Object source = null)
        {
            if (!Enabled) return;

            message = GetLogMessageWithPrefix(message);
            if (source) {
                Debug.Log(message, source);
                return;
            }

            Debug.Log(message);
        }


        private string GetNamePrefix()
        {
            return string.Format(NameTemplate, Name, _counter.ToStringFromCache());
        }

        private string GetLogMessageWithPrefix(string message)
        {
            return string.Format(LogTemplate,DateTime.Now.ToLongTimeString(), LogPrefix, message);
        }
    }
}