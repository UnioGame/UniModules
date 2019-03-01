
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.ResourceSystem
{
    [Serializable]
    public class ResourceItem {

        [HideInInspector]
        [SerializeField]
        private string assetPath;
        
        [HideInInspector]
        [SerializeField]
        private string guid;

        [SerializeField]
        private Object _asset;

        #region public methods

        public T GetResource<T>()
            where T : Object
        {
            return null;
        }
        
        public void Update(Object asset) {

        }

        public void Update() {

        }

        #endregion

    }
}
