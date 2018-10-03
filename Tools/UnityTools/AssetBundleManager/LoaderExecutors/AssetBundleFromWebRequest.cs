using System;
using System.Collections;
using UnityEngine.Networking;

namespace Assets.Tools.UnityTools.AssetBundleManager.LoaderExecutors
{

    public class AssetBundleFromWebRequest : AssetBundleRequest
    {
        protected UnityWebRequest _webRequest;

        protected override IEnumerator MoveNext()
        {
            throw new NotImplementedException();
        }

        protected override void OnReset()
        {

            base.OnReset();

            if (_webRequest != null)
                _webRequest.Dispose();

        }

    }


}


