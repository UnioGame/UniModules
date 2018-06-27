using System.Collections;
using Assets.Scripts.ProfilerTools;
using UnityEngine;

namespace AssetBundlesModule
{

    public class AssetBundleWwwRequest : AssetBundleRequest
    {

        protected WWW _wwwRequest;

        protected override IEnumerator MoveNext() {

            yield return WaitLoad(_wwwRequest);
            
            if (string.IsNullOrEmpty(_wwwRequest.error) == false)
            {
                Error = _wwwRequest.error;
                GameLog.LogErrorFormat("WWW Error : {0}", Error);
            }

        }

        private IEnumerator WaitLoad(WWW www) {
            
            if (_wwwRequest == null)
            {
                GameLog.LogErrorFormat("WWW of resource [{0}] is NULL", Resource);
                yield break;
            }

            yield return www;

        }

        protected override void OnComplete() {
            base.OnComplete();
            
            if(_wwwRequest!=null)
                _wwwRequest.Dispose();

            if(BundleResource!=null)          
                BundleResource = new LoadedAssetBundle(_wwwRequest.assetBundle);

            _wwwRequest = null;
        }

        protected override void OnInitialize()
        {
            if (string.IsNullOrEmpty(Resource))
            {
                Error = "Load Bundle from local file request ERROR : Resource is NULL";
                GameLog.LogError(Error);
                return;
            }
            if(_wwwRequest==null)
                _wwwRequest = new WWW(Resource);
        }
    }


}
