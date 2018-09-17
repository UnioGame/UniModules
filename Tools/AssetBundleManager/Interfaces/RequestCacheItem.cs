using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using UnityEngine;

namespace AssetBundlesModule {

	public class RequestCacheItem {

		public string Name { get; set; }
		public int References { get; set; }
		public IAssetBundleRequest Request { get; set; }
		public AssetBundleSourceType SourceType { get; set; }

	}

}