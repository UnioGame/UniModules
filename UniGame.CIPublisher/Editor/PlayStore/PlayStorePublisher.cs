using System;
using System.IO;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.AndroidPublisher.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;

namespace ConsoleGPlayAPITool
{
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    /// <summary>
    /// publish your android artifact by configuration to your target branch
    /// </summary>
    [Serializable]
    public class PlayStorePublisher : IPlayStorePublisher
    {
        public int UploadSecondsTimeout = 600;
        public int UpdalodAwaitTimeout  = 200;

        public async void Publish(IAndroidDistributionSettings configs)
        {
            if (configs == null)
            {
                throw new Exception("Cannot load a valid BundleConfig");
            }

            //Create publisherService
            using (var androidPublisherService = CreateGoogleConsoleAPIService(configs))
            {
                var appEdit                 = CreateAppEdit(androidPublisherService, configs);
                await UploadAtrifact(androidPublisherService, configs, appEdit);
            }
        }

        private async Task UploadAtrifact(AndroidPublisherService androidPublisherService,IAndroidDistributionSettings configs,AppEdit appEdit)
        {
            var uploader = configs.IsAppBundle ? 
                new AndroidAppBundlerUploader() : 
                new AndroidApkUploader() as IAndroidArtifactUploader;

            // Upload new apk to developer console
            var upload       = uploader.Upload(configs, androidPublisherService, appEdit);
            var artifactName = Path.GetFileName(configs.ArtifactPath);
                
            upload.ProgressChanged += x =>  OnUploadProgressChanged(artifactName, x);
                
            upload.UploadAsync();

            var uploadProgress = upload.GetProgress();
                
            while (uploadProgress.Status != UploadStatus.Completed && uploadProgress.Status != UploadStatus.Failed)
            {
                uploadProgress = upload.GetProgress();
                if (uploadProgress != null)
                {
                    OnUploadProgressChanged(artifactName, uploadProgress);
                }
                
                Thread.Sleep(UpdalodAwaitTimeout);
            }

            if (uploadProgress.Exception != null)
            {
                throw new Exception(uploadProgress.Exception.Message);
            }

            if (uploadProgress.Status != UploadStatus.Completed)
            {
                throw new Exception("File upload failed. Reason: unknown :(");
            }

            if (uploadProgress.Status == UploadStatus.Completed)
            {
                CommitChangesToGooglePlay(androidPublisherService, configs, appEdit);
            }
        }

        private void OnUploadProgressChanged(string artifactName,IUploadProgress upload)
        {
            Debug.Log($"Upload {artifactName} {upload.Status.ToString()} : {upload.BytesSent} bytes ");
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

        private void CommitChangesToGooglePlay(
            AndroidPublisherService androidPublisherService,
            IAndroidDistributionSettings configs,
            AppEdit edit)
        {
            var commitRequest = androidPublisherService.Edits.Commit(configs.PackageName, edit.Id);
            var appEdit       = commitRequest.Execute();
            Debug.Log("App edit with id " + appEdit.Id + " has been comitted");
        }

        private AndroidPublisherService CreateGoogleConsoleAPIService(IAndroidDistributionSettings configs)
        {
            var cred = GoogleCredential.FromJson(File.ReadAllText(configs.JsonKeyPath));
            cred = cred.CreateScoped(new[] {AndroidPublisherService.Scope.Androidpublisher});

            // Create the AndroidPublisherService.
            var androidPublisherService = new AndroidPublisherService(new BaseClientService.Initializer
                {HttpClientInitializer = cred});
            androidPublisherService.HttpClient.Timeout = TimeSpan.FromSeconds(UploadSecondsTimeout);
            return androidPublisherService;
        }
    }
}