using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PostBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;
using UnityEditor.Build.Reporting;

namespace UniModules.UniGame.BuildCommands.Editor.Ftp
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using AddressableExtensions.Editor;
    using Core.EditorTools.Editor.Tools;
    using FluentFTP;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    [Serializable]
    public class AddressablesFtpUploadPostCommand : UnitySerializablePostBuildCommand
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
        [Sirenix.OdinInspector.InfoBox("You can use addressables variables [BuildPath] e.t.c")]
#endif
        [Space(6)]
        public string remoteDirectory = string.Empty;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Server Info")]
#endif
        public FtpRemoteExists updateMethod = FtpRemoteExists.Overwrite;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Server Info")]
#endif
        public FtpFolderSyncMode folderSyncMode = FtpFolderSyncMode.Update;

        public override void Execute(IUniBuilderConfiguration configuration, BuildReport buildReport) => Upload();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Upload()
        {
            var buildFolder           = $"[{AddressableAssetSettings.kRemoteBuildPath}]".EvaluateActiveProfileString();
            var targetUploadDirectory = remoteDirectory.EvaluateActiveProfileString();

            if (!overrideTargetFolder) {
                targetUploadDirectory = Directory.Exists(buildFolder) ? Path.GetFileName(buildFolder) : Path.GetDirectoryName(buildFolder);
            }

            Debug.Log($"Upload from: {buildFolder}");
            Debug.Log($"Upload to: {targetUploadDirectory}");

            var ftpClient = new FtpClient(ftpUrl);
            ftpClient.Credentials = new NetworkCredential(userName, password, ftpUrl);
            ftpClient.Connect();

            CreateMissingDirectories(targetUploadDirectory, ftpClient);

            var uploadResults = ftpClient.UploadDirectory(buildFolder,
                targetUploadDirectory,
                folderSyncMode,
                updateMethod,
                FtpVerify.None, null, UploadProgress);

            var failed = uploadResults.Where(x => x.IsFailed).ToList();

            var isValidResult = failed.Count <= 0;
            if (!isValidResult) {
                Debug.LogError($"BuildCommand: {Name} upload to {targetUploadDirectory} failed for:");
                foreach (var ftpResult in failed) {
                    Debug.LogError($"{ftpResult.LocalPath} {ftpResult.Size}");
                }
            }

            var uploadResult = isValidResult ? "successfully" : "failed";

            Debug.Log($"BuildCommand: {Name} Upload Complete. result: {uploadResult}");
        }

        private void CreateMissingDirectories(string serverPath, IFtpClient client)
        {
            var pathItems      = serverPath.SplitPath();
            var serverLocation = string.Empty;
            foreach (var pathItem in pathItems) {
                serverLocation = serverLocation.CombinePath(pathItem);
                var exists = client.DirectoryExists(serverLocation);
                if (exists) {
                    continue;
                }
                var creationResult = client.CreateDirectory(serverLocation);
                if (creationResult == false) {
                    Debug.LogError($"Can't create Remote Folder {serverLocation}");
                }
            }
        }

        private void UploadProgress(FtpProgress progress)
        {
            var progressLog =
                $"Uploading: Source: {progress.LocalPath} Target: {progress.RemotePath}";
            Debug.Log(progressLog);
        }
    }
}