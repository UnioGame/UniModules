using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityTools.ProfilerTools;
using Object = UnityEngine.Object;

public class EmptyLogger : IGameLogger
{
    public void Log(string message, Object source = null)
    {
    }

    public void LogFormatWithTrace(string template, params object[] values)
    {
    }

    public void LogFormat(string template, Color color, params object[] values)
    {
    }

    public void Log(string message, Color color, Object source = null)
    {
    }

    public void LogWarning(string message, Color color, Object source = null)
    {
    }

    public void EditorLogFormat(LogType logType, string format, params object[] objects)
    {
    }

    public void LogWarning(string message, Object source = null)
    {
    }

    public void LogWarningFormat(string template, params object[] values)
    {
    }

    public void LogFormat(string template, params object[] values)
    {
    }

    public void LogError(string message, Object source = null)
    {
    }

    public void LogError(Exception message, Object source = null)
    {
    }

    public void LogErrorFormat(string message, params object[] objects)
    {
    }

    public void LogFormatRuntime(string template, params object[] values)
    {
    }

    public void LogRuntime(string message, Color color, Object source = null)
    {
    }

    public string GetColorTemplate(string message, Color color)
    {
        return string.Empty;
    }

    public void LogRuntime(string message, Object source = null)
    {
    }
}