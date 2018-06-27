namespace AssetBundlesModule
{
    public abstract class AssetBundleRequest : AsyncRequestOperation, IAssetBundleRequest {

        public IAssetBundleResource BundleResource { get; protected set; }

        public string Resource { get; protected set; }

        public string BundleName { get; protected set; }

        public void Initialize(string assetBundleName,string resource) {
            BundleName = assetBundleName;
            Resource = resource;
        }

        protected override bool OnValidate() {
            return base.OnValidate() 
                && BundleResource == null;
        }

        protected override void OnReset()
        {
            base.OnReset();
            BundleResource = null;
            Resource = string.Empty;
        }
    }
}
