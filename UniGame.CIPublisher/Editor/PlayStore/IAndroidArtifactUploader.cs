namespace ConsoleGPlayAPITool
{
    using Google.Apis.AndroidPublisher.v3;
    using Google.Apis.AndroidPublisher.v3.Data;
    using Google.Apis.Upload;

    public interface IAndroidArtifactUploader
    {
        ResumableUpload Upload(
            IAndroidDistributionSettings configs,
            AndroidPublisherService androidPublisherService,
            AppEdit edit);
    }
}