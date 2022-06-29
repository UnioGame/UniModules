namespace UniGame.Utils.Editor.ShellCommands
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Text;
    using UniModules.Editor;
    using UnityEditor;
    using UnityEngine;
    using Application = UnityEngine.Application;
    using Debug = UnityEngine.Debug;

    public static class ShellRunner
    {
        private static StringBuilder stringBuilder = new StringBuilder();

        private const string argsFormateTemplate = "{0} {1} ";
        public const  string progressTitle       = "Run Shell Progress";
        public const  string windowsRunner       = "powershell.exe";
        public const  string linuxRunner         = "sh";
        public const  string osxRunner           = "sh";
        
        private const string DefaultScriptPathWin   = "../Tools/Firebase/NodeScripts/firebase-win.ps1";
        private const string DefaultScriptPathOsx   = "../Tools/Firebase/NodeScripts/firebase-macos.sh";
        private const string DefaultScriptPathLinux = "../Tools/Firebase/NodeScripts/firebase-macos.sh"; 
        
        public static IEnumerator RunShellScript(ShellArgument[] args, Action<string> result = null,int timeoutMs = 10000)
        {
            yield return RunShellScript(null,args,timeoutMs,result);
        }
        
        public static IEnumerator RunShellScript(ShellArgument[] args)
        {
            yield return RunShellScript(null,args);
        }

        public static IEnumerator RunShellScript(string shellFilePath, ShellArgument[] args,int timeoutMs = 5000,Action<string> result = null)
        {
            var runner = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    runner        =   linuxRunner;
                    shellFilePath = (shellFilePath ?? DefaultScriptPathLinux).ToAbsoluteProjectPath();
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    runner        = windowsRunner;
                    shellFilePath = (shellFilePath ?? DefaultScriptPathWin).ToAbsoluteProjectPath();
                    break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    runner        = osxRunner;
                    shellFilePath = (shellFilePath ?? DefaultScriptPathOsx).ToAbsoluteProjectPath();
                    break;
            }

            var parameters = CreateParameters(args);

            yield return RunProcess(runner, $"\"{shellFilePath}\" {parameters}", timeoutMs,result);
        }


        public static IEnumerator RunProcess(string processName, string parameters,int timeout, Action<string>  resultAction = null)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName               = processName,
                Arguments              = parameters,
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false,
                CreateNoWindow         = true
            };

            var process = new Process {StartInfo = startInfo};

            var progressInfo = $"{nameof(ShellRunner)} START PROCESS : {processName} {parameters}";
            
            Debug.Log(progressInfo);
            
            process.Start();

            EditorUtility.DisplayProgressBar(progressTitle, progressInfo, 0);
            
            var startTime  = DateTime.Now;
            var timePassed = 0;

            while (!process.HasExited && timePassed < timeout)
            {
                timePassed = (int)(DateTime.Now - startTime).TotalMilliseconds;
                EditorUtility.DisplayProgressBar(progressTitle, progressInfo, timeout <= 0 ? 1 : timePassed / (float)timeout);
                yield return null;
            }
            
            EditorUtility.ClearProgressBar();
            
            if (!process.HasExited)
            {
                Debug.Log($"{nameof(ShellRunner)} STOP PROCESS BY TIMEOUT: {processName} {parameters}");
                process.Dispose();
                yield break;
            }
                
            var output = process.StandardOutput.ReadToEnd();
            var errors = process.StandardError.ReadToEnd();

            Debug.Log($"{nameof(ShellRunner)}: {processName} {parameters} \nOUTPUT : \n{output}");

            if (!string.IsNullOrEmpty(errors))
                Debug.LogError(errors);
            
            resultAction?.Invoke(output);
        }

        public static string CreateParameters(ShellArgument[] args)
        {
            foreach (var argument in args)
            {
                var name  = string.IsNullOrEmpty(argument.Name) ? string.Empty : argument.Name;
                var value = argument.Value == null ? string.Empty : argument.Value.ToString();
                stringBuilder.AppendFormat(argsFormateTemplate, name, value);
            }

            var result = stringBuilder.ToString();
            stringBuilder.Clear();
            return result;
        }
    }

    public struct ShellArgument
    {
        public string Name;
        public object Value;
    }
}