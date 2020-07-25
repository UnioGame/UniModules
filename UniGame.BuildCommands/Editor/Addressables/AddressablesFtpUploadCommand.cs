using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PostBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;
using UnityEditor.Build.Reporting;

namespace UniModules.UniGame.BuildCommands.Editor.Ftp
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using AddressableExtensions.Editor;
    using FluentFTP;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/UniBuild/Commands/FTP Upload", fileName = nameof(AddressablesFtpUploadCommand))]
    public class AddressablesFtpUploadCommand : UnityPostBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Auth")]
#endif
        public string ftpUrl;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Auth")]
#endif
        public string userName;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Auth")]
#endif
        public string password;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Server Info")]
#endif
        public bool overrideTargetFolder = false;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Server Info")]
        [Sirenix.OdinInspector.ShowIf("overrideTargetFolder")]
#endif
        [Space(6)]
        public string remoteDirectory = string.Empty;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Server Info")]
#endif
        public FtpRemoteExists updateMethod = FtpRemoteExists.Overwrite;

        public override void Execute(IUniBuilderConfiguration configuration, BuildReport buildReport) => Upload();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Upload()
        {
            var buildFolder           = $"[{AddressableAssetSettings.kRemoteBuildPath}]".EvaluateActiveProfileString();
            var targetUploadDirectory = remoteDirectory;
            if (!overrideTargetFolder) {
                targetUploadDirectory = Directory.Exists(buildFolder) ? 
                        Path.GetFileName(buildFolder) : 
                        Path.GetDirectoryName(buildFolder);
            }

            Debug.Log($"Upload from: {buildFolder}");
            Debug.Log($"Upload to: {targetUploadDirectory}");

            var ftpClient = new FtpClient(ftpUrl);
            ftpClient.Credentials = new NetworkCredential(userName, password, ftpUrl);
            ftpClient.Connect();

            var uploadResults = ftpClient.UploadDirectory(buildFolder,
                targetUploadDirectory,
                FtpFolderSyncMode.Update,
                updateMethod,
                FtpVerify.None, null, UploadProgress);
            var failed = uploadResults.Where(x => x.IsFailed).ToList();

            var isValidResult = failed.Count <= 0;
            if (!isValidResult) {
                Debug.LogError($"BuildCommand: {name} upload to {targetUploadDirectory} failed for:");
                foreach (var ftpResult in failed) {
                    Debug.LogError($"{ftpResult.LocalPath} {ftpResult.Size}");
                }
            }

            var uploadResult = isValidResult ? "successfully" : "failed";
            Debug.Log($"BuildCommand: {name} Upload Complete. result: {uploadResult}");
        }

        private void UploadProgress(FtpProgress progress)
        {
            var progressLog =
                $"Uploading: Source: {progress.LocalPath} Target: {progress.RemotePath}";
            Debug.Log(progressLog);
        }
    }
}