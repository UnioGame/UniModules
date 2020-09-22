namespace ConsoleGPlayAPITool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Google.Apis.AndroidPublisher.v3;
    using Google.Apis.AndroidPublisher.v3.Data;
    using Google.Apis.Upload;
    using UnityEngine;

    [Serializable]
    public class AndroidApkUploader : IAndroidArtifactUploader
    {
        
        public ResumableUpload Upload(
            IAndroidDistributionSettings configs,
            AndroidPublisherService androidPublisherService,
            AppEdit edit)
        {
            
            Debug.Log("Upload started for : " + Path.GetFileName(configs.ArtifactPath));
            var upload = androidPublisherService.Edits.Apks.Upload(
                configs.PackageName,
                edit.Id,
                new FileStream(configs.ArtifactPath, FileMode.Open),
                "application/vnd.android.package-archive"
            );

            upload.ResponseReceived += x => OnUploadResponce(x, androidPublisherService, configs, edit);
            return upload;
        }

        
        private void OnUploadResponce(Apk apk, AndroidPublisherService androidPublisherService,IAndroidDistributionSettings configs,AppEdit edit)
        {
            if (apk == null)
                return;
            
            var track = androidPublisherService.LoadTrackBranch(configs, edit);

            track.UpdateTrackInformation(apk.VersionCode,configs);

            //Verify if exist any obb
            var needUploadExtensionsFiles = CheckIfNeedProcessObb(configs, out string[] obbs);

            if (needUploadExtensionsFiles)
            {
                UploadObbFiles(androidPublisherService, edit, apk, configs, obbs);
            }
           
            var updatedTrack = androidPublisherService.Edits.Tracks
                .Update(track, configs.PackageName, edit.Id, track.TrackValue).Execute();
            Debug.Log("Track " + updatedTrack.TrackValue + " has been updated.");
        }

        private bool CheckIfNeedProcessObb(IAndroidDistributionSettings configs, out string[] f)
        {
            var apkFolder = Directory.GetParent(configs.ArtifactPath);
            Debug.Log($"Trying find obb on Path: {apkFolder}");
            var boolNeedProcessObb = false;
            var tempList           = new List<string>();

            var files = apkFolder.GetFiles();
            foreach (var fileInfo in files)
            {
                if (fileInfo.Extension == ".obb")
                {
                    boolNeedProcessObb = true;
                    tempList.Add(fileInfo.FullName);
                }
            }

            f = tempList.ToArray();
            Debug.Log($"Need Upload Obb:{boolNeedProcessObb}");
            return boolNeedProcessObb;
        }

        private void UploadObbFiles(AndroidPublisherService service, AppEdit edit, Apk apk,
            IAndroidDistributionSettings configs, string[] obbs)
        {
            foreach (var obbPath in obbs)
            {
                var upload = service.Edits.Expansionfiles.Upload(
                    configs.PackageName,
                    edit.Id,
                    apk.VersionCode.Value,
                    EditsResource.ExpansionfilesResource.UploadMediaUpload.ExpansionFileTypeEnum.Main,
                    new FileStream(obbPath, FileMode.Open),
                    "application/octet-stream"
                );
                Debug.Log($"Starting Uploading Obb:{obbPath}");
                upload.ResponseReceived += response =>
                {
                    if (response == null)
                    {
                        throw new Exception("Failed Upload " + obbPath);
                    }
                    else
                    {
                        Debug.Log("Success Upload " + obbPath);
                    }
                };
                var result = upload.Upload();
                if (result.Exception != null)
                {
                    throw new Exception("Error: " + result.Exception.Message);
                }

                if (result.Status != UploadStatus.Completed)
                {
                    throw new Exception("Obb not uploaded");
                }
                Debug.Log($"Finish Uploading Obb:{obbPath}");
            }
        }

        private AppEdit CreateAppEdit(
            AndroidPublisherService androidPublisherService,
            IAndroidDistributionSettings configs)
        {
            var edit = androidPublisherService.Edits
                .Insert(null /** no content */, configs.PackageName)
                .Execute();
            Debug.Log("Created edit with id: " +
                      edit.Id +
                      " (valid for " + edit.ExpiryTimeSeconds + " seconds)");
            return edit;
        }

    }
}