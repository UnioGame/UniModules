using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.ProfilerTools
{
    public class GameLogger : IGameLogger
    {
        
        private const string TraceTemplate = @"{0} / {1}";
        private const string NameTemplate = @"[{0} #{1}]:";
        private const string LogTemplate = @"{0} {1}";
        
        private long _counter;
        private bool _includeLoggerName;
        
        private Dictionary<string, string> _traces =
            new Dictionary<string, string>();

        public bool Enabled = true;
        public string Name;

        public GameLogger(string name, bool includeLoggerName)
        {
            Name = name;
            _includeLoggerName = includeLoggerName;
        }
        
        public void Log(string message, Object source = null)
        {
            LogRuntime(message, source);
        }
        
        public void LogFormatWithTrace(string template, params object[] values)
        {
            LogFormat(template,values);
            LogFormat("Stack Trace {0}", System.Environment.StackTrace);
        }

        public void LogFormat(string template, Color color, params object[] values)
        {
            var message = values == null || values.Length == 0 ? template :
                string.Format(template, values);
            Log(message, color);
        }

        public void Log(string message, Color color, Object source = null) {
            LogRuntime(message, color, source);
        }

        public void LogWarning(string message, Color color, Object source = null)
        {
            if (!Enabled) return;
            var colorMessage = GetColorTemplate(message, color);
            LogWarning(colorMessage, source);
        }

        public void EditorLogFormat(LogType logType,string format, params object[] objects)
        {

            var prefix = GetNamePrefix();
            format = prefix + format;
            
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
        
        public void LogWarning(string message, Object source = null)
        {
            if (!Enabled) return;
            if (source)
            {
                Debug.LogWarningFormat(LogTemplate,GetNamePrefix(),message, source);
                return;
            }
            Debug.LogWarningFormat(LogTemplate,GetNamePrefix(),message);
        }

        public void LogWarningFormat(string template, params object[] values)
        {
            if (!Enabled) return;
            var message = string.Format(template, values);
            Debug.LogWarningFormat(LogTemplate,GetNamePrefix(),message);
        }

        public void LogFormat(string template, params object[] values)
        {
            LogFormatRuntime(template, values);
        }

        public void LogError(string message, Object source = null)
        {
            if (source)
            {
                Debug.LogError(message, source);
                return;
            }
            Debug.LogError(message);
        }

        public void LogError(Exception message, Object source = null)
        {
            if (source)
            {
                Debug.LogError(message, source);
                return;
            }
            Debug.LogError(message);
        }

        public void LogErrorFormat(string message, params object[] objects)
        {
            if (!Enabled) return;
            Debug.LogErrorFormat(message, objects);
        }

        public void LogFormatRuntime(string template, params object[] values)
        {
            var message = values == null || values.Length == 0 ? template :
                string.Format(template, values);
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
                (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f),
                message);
            return colorMessage;
        }
        
              
        public void LogRuntime(string message, Object source = null)
        {
            if (!Enabled) return;
            message = string.Format(LogTemplate, GetNamePrefix(), message);
            if (source)
            {
                Debug.Log(message, source);
                return;
            }
            Debug.Log(message);
        }


        private string GetNamePrefix()
        {
            return string.Format(NameTemplate, Name, ++_counter);
        }
    }
}
