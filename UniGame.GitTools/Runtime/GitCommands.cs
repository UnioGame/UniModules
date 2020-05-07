using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UniModules.UniGame.GitTools.Runtime
{
    public static class GitCommands
    {
        public const string BranchParameters = "rev-parse --abbrev-ref HEAD";
    
        public static string GetGitBranch()
        {
            var result = string.Empty;

            try {
                using (var process = StartGitProcess(BranchParameters)) {
                    process.Start();
                    var output = process.StandardOutput.ReadLine();
                    result = RemoveSpecialCharacters(output);
                }
            }
            catch (Exception e) {
                Debug.LogException(e);
            }

            return result;
        }

        public static Process StartGitProcess(string parameters)
        {
            var startinfo = new ProcessStartInfo("git") {
                UseShellExecute        = false,
                WorkingDirectory       = Application.dataPath,
                RedirectStandardInput  = true,
                RedirectStandardOutput = true,
                Arguments              = parameters,
            };
            var process = new Process() {
                StartInfo = startinfo
            };
            return process;
        }
    
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }
}
