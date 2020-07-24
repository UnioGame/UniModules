using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PostBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;
using UnityEditor.Build.Reporting;

namespace UniModules.UniGame.BuildCommands.Editor.Ftp
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using FluentFTP;
    using UniGreenModules.UniCore.EditorTools.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UnityEditor;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/UniBuild/Commands/FTP Upload", fileName = nameof(FtpUploadCommand))]
    public class FtpUploadCommand : UnityPostBuildCommand
    {
        public string ftpUrl;
        public string userName;
        public string password;

        [Space(8)]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public List<string> uploadFolders = new List<string>();

        [Space(6)]
        public string remoteUrl = string.Empty;
        
        public override void Execute(IUniBuilderConfiguration configuration, BuildReport buildReport)
        {
        
            Upload();

        }

        public void Upload()
        {
            
            var addressableAssetSettings = AssetEditorTools.GetAsset<AddressableAssetSettings>();
            if (!addressableAssetSettings) {
                Debug.LogWarning($"Missing AddressableAssetSettings File",this);
                return;
            }

            var ftpClient = new FtpClient(ftpUrl);

            ftpClient.Credentials = new NetworkCredential(userName,password);
            ftpClient.Connect();

            var isValidResult = true;
            var folders = uploadFolders.Where(Directory.Exists);
            try {
                foreach (var folder in folders) {
                    var uploadResults = ftpClient.UploadDirectory(folder,
                        remoteUrl,
                        FtpFolderSyncMode.Update,
                        FtpRemoteExists.Overwrite,
                        FtpVerify.None, null, UploadProgress);
                    var failed = uploadResults.
                        Where(x => x.IsFailed).
                        ToList();
                    if(failed.Count <= 0)
                        continue;
                    
                    isValidResult = false;
                    Debug.LogError($"BuildCommand: {name} upload to {remoteUrl} failed for:");
                    foreach (var ftpResult in failed) {
                        Debug.LogError($"{ftpResult.LocalPath} {ftpResult.Size}");
                    }
                }
            }
            finally{
                EditorUtility.ClearProgressBar();
            }

            var uploadResult = isValidResult ? "successfully" : "failed";
            Debug.Log($"BuildCommand: {name} Upload Complete. result: {uploadResult}");
        }
        
        private void UploadProgress(FtpProgress progress)
        {
            var progressData = new ProgressData() {
                Content = $"{progress.FileIndex} | {progress.FileIndex} | {progress.TransferredBytes} bytes {progress.LocalPath}",
                Title = $"{progress.RemotePath}",
                Progress = (float)progress.Progress,
            };
            AssetEditorTools.ShowProgress(progressData);
        }
        
    }
}
