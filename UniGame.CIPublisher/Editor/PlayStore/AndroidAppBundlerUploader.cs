namespace ConsoleGPlayAPITool
{
    using System;
    using System.IO;
    using Google.Apis.AndroidPublisher.v3;
    using Google.Apis.AndroidPublisher.v3.Data;
    using Google.Apis.Upload;
    using UnityEngine;

    [Serializable]
    public class AndroidAppBundlerUploader : IAndroidArtifactUploader
    {
        public ResumableUpload Upload(
            IAndroidDistributionSettings configs, 
            AndroidPublisherService androidPublisherService, 
            AppEdit edit)
        {
            Debug.Log("Upload started for apk: " + Path.GetFileName(configs.ArtifactPath));
            var upload = androidPublisherService.Edits.Bundles.Upload(
                configs.PackageName,
                edit.Id,
                new FileStream(configs.ArtifactPath, FileMode.Open),
                "application/octet-stream"
            );
            
            upload.ResponseReceived += x => OnUploadResponce(x, androidPublisherService, configs, edit);

            return upload;
        }

        public void OnUploadResponce(Bundle bundle, AndroidPublisherService androidPublisherService,IAndroidDistributionSettings configs,AppEdit edit)
        {
            if (bundle == null)
                return;
            
            var track = androidPublisherService.LoadTrackBranch(configs, edit);

            track.UpdateTrackInformation(bundle.VersionCode, configs);

            var updatedTrack = androidPublisherService.Edits.Tracks
                .Update(track, configs.PackageName, edit.Id, track.TrackValue).Execute();
            Debug.Log("Track " + updatedTrack.TrackValue + " has been updated.");
        }
        

    }
}