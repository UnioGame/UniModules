namespace UniModule.UnityTools.AssetBundleManager.Interfaces {

	public class AssetBundleCacheItem {

		public string Name { get; set; }
		public int Referencies { get; set; }
		public IAssetBundleRequest Request { get; set; }
		public AssetBundleSourceType SourceType { get; set; }

	}

}