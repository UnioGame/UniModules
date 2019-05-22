using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IAssetProvider : ICommandRoutine
    {
        string[] AllAssetsNames { get; }

        #region sync loading

        //load Unity asset Objecs
        T LoadAsset<T>() where T : Object;
        List<T> LoadAssets<T>() where T : Object;
        T LoadAssetByTypeName<T>() where T : Object;
        Object LoadAssetByName(string assetName);
        T LoadAsset<T>(string assetName) where T : Object;
        
        #endregion

        void Load();

        #region async

        IEnumerator LoadAssetsAsync(Type type, Action<List<Object>> callback);

        IEnumerator LoadAssetAsync<T>(Action<T> action)
            where T : Object;

        IEnumerator LoadAssetsAsync<T>(Action<List<T>> callback)
            where T : Object;
        
        IEnumerator LoadPrefabAssetsAsync<T>(Action<List<T>> callback)
            where T : Component;

        IEnumerator LoadPrefabAssetAsync<T>(Action<T> action)
            where T : Component;

        IEnumerator LoadAssetByTypeNameAsync<T>(Action<T> callback)
            where T : Component;

        IEnumerator LoadAssetByNameAsync<T>(string assetName, Action<T> action)
            where T : Object;

        IEnumerator LoadPrefabAssetAsync<T>(string name, Action<T> callback) where T : Component;

        #endregion


    }
}