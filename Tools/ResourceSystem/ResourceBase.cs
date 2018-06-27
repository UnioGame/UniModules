using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ME;
using UnityEngine;
using UnityEngine.UI.Windows;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif


public interface IUnloadableItem
{

    void Unload();

};

public enum StreamingAssetsPathType
{

    StreamingAssetsPathStandalone = 0,
    StreamingAssetsPathStandaloneWindows,
    StreamingAssetsPathStandaloneLinux,
    StreamingAssetsPathStandaloneOSX,
    StreamingAssetsPathIOS,
    StreamingAssetsPathAndroid,
    StreamingAssetsPathPS4,
    StreamingAssetsPathXBOXONE,
    StreamingAssetsPathSwitch,
    StreamingAssetsPathCommon,

}

[Flags]
public enum StreamingAssetsPathIsMovie
{
    StreamingAssetsPathNone = 0,
    StreamingAssetsPathStandalone = 1,
    StreamingAssetsPathStandaloneWindows = 1 << 1,
    StreamingAssetsPathStandaloneLinux = 1 << 2,
    StreamingAssetsPathStandaloneOSX = 1 << 3,
    StreamingAssetsPathIOS = 1 << 4,
    StreamingAssetsPathAndroid = 1 << 5,
    StreamingAssetsPathPS4 = 1 << 6,
    StreamingAssetsPathXBOXONE = 1 << 7,
    StreamingAssetsPathSwitch = 1 << 8,
    StreamingAssetsPathCommon = 1 << 9,

}

[Flags]
public enum ResourceSource
{

    Direct = 0,
    StreamingAssets = 1,
    Resources = 1 << 1,
    AssetBundles = 1 << 2,
    Web = 1 << 3,

}

[Serializable]
public class ResourceBase
{

    public static Dictionary<Platform, StreamingAssetsPathType> StreamingPathMap = new Dictionary<Platform, StreamingAssetsPathType>() {
            {Platform.Common,StreamingAssetsPathType.StreamingAssetsPathCommon },
            {Platform.Android,StreamingAssetsPathType.StreamingAssetsPathAndroid },
            {Platform.PS4,StreamingAssetsPathType.StreamingAssetsPathPS4 },
            {Platform.Standalone,StreamingAssetsPathType.StreamingAssetsPathStandalone },
            {Platform.StandaloneLinux,StreamingAssetsPathType.StreamingAssetsPathStandaloneLinux },
            {Platform.StandaloneOSX,StreamingAssetsPathType.StreamingAssetsPathStandaloneOSX },
            {Platform.StandaloneWindows,StreamingAssetsPathType.StreamingAssetsPathStandaloneWindows },
            {Platform.Switch,StreamingAssetsPathType.StreamingAssetsPathSwitch },
            {Platform.XBOXONE,StreamingAssetsPathType.StreamingAssetsPathXBOXONE },
            {Platform.iOS,StreamingAssetsPathType.StreamingAssetsPathIOS },
        };

    public static Dictionary<StreamingAssetsPathType, StreamingAssetsPathIsMovie> StreamingIsMoviePathMap = new Dictionary<StreamingAssetsPathType, StreamingAssetsPathIsMovie>() {
            {StreamingAssetsPathType.StreamingAssetsPathCommon,StreamingAssetsPathIsMovie.StreamingAssetsPathCommon },
            {StreamingAssetsPathType.StreamingAssetsPathAndroid,StreamingAssetsPathIsMovie.StreamingAssetsPathAndroid },
            {StreamingAssetsPathType.StreamingAssetsPathPS4,StreamingAssetsPathIsMovie.StreamingAssetsPathPS4 },
            {StreamingAssetsPathType.StreamingAssetsPathStandalone,StreamingAssetsPathIsMovie.StreamingAssetsPathStandalone },
            {StreamingAssetsPathType.StreamingAssetsPathStandaloneLinux,StreamingAssetsPathIsMovie.StreamingAssetsPathStandaloneLinux },
            {StreamingAssetsPathType.StreamingAssetsPathStandaloneOSX,StreamingAssetsPathIsMovie.StreamingAssetsPathStandaloneOSX },
            {StreamingAssetsPathType.StreamingAssetsPathStandaloneWindows,StreamingAssetsPathIsMovie.StreamingAssetsPathStandaloneWindows },
            {StreamingAssetsPathType.StreamingAssetsPathSwitch,StreamingAssetsPathIsMovie.StreamingAssetsPathSwitch },
            {StreamingAssetsPathType.StreamingAssetsPathXBOXONE,StreamingAssetsPathIsMovie.StreamingAssetsPathXBOXONE },
            {StreamingAssetsPathType.StreamingAssetsPathIOS,StreamingAssetsPathIsMovie.StreamingAssetsPathIOS },
        };

    [NonSerialized]
    protected Object objectCacheResource;

    public enum Platform : byte
    {
        Common,
        Standalone,
        StandaloneWindows,
        StandaloneLinux,
        StandaloneOSX,
        iOS,
        Android,
        PS4,
        XBOXONE,
        Switch,
    };

    #region constructor

    public ResourceBase() { }

    public ResourceBase(bool async)
    {

        this.async = async;

    }

    public ResourceBase(ResourceBase other)
    {

        this.resourceIds = other.resourceIds;
        this.async = other.async;
        this.isMono = other.isMono;

        this.assetPath = other.assetPath;
        this.assetBundleName = other.assetBundleName;
        this.resourcesPath = other.resourcesPath;
        this.webPath = other.webPath;
        this.resourceName = other.resourceName;

        this.loadableWeb = other.loadableWeb;
        this.loadableResource = other.loadableResource;
        this.loadableStream = other.loadableStream;
        this.loadableAssetBundle = other.loadableAssetBundle;

        this.multiObjects = other.multiObjects;
        this.objectIndex = other.objectIndex;

        this.multiObjectsAssetBundle = other.multiObjectsAssetBundle;
        this.objectIndexAssetBundle = other.objectIndexAssetBundle;

        this.customResourcePath = other.customResourcePath;
        this.readableTexture = other.readableTexture;
        this.id = other.id;
        this.cacheVersion = other.cacheVersion;

        this.directRef = other.directRef;

        this.StreamingAssetsPathIsMovie = other.StreamingAssetsPathIsMovie;

        var length = other.StreamingAssetsPaths.Length;
        this.StreamingAssetsPaths = new string[length];
        for (int i = 0; i < length; i++)
        {
            this.StreamingAssetsPaths[i] = other.StreamingAssetsPaths[i];
        }

        length = other.StreamingAssetsMovieAudioPaths.Length;
        this.StreamingAssetsMovieAudioPaths = new string[length];
        for (int i = 0; i < length; i++)
        {
            this.StreamingAssetsMovieAudioPaths[i] = other.StreamingAssetsMovieAudioPaths[i];
        }

        this.canBeUnloaded = other.canBeUnloaded;

    }

    #endregion

    public bool isInitialized = false;

    public bool async = true;

    [NonSerialized]
    public bool loadResourceAsync = false;

    public Object tempObject
    {
        get
        {
            if (Application.isEditor == true && Application.isPlaying == false)
                return GetDirectRef<Object>();
            return this.objectCacheResource;
        }
        set
        {
            this.objectCacheResource = value;
            if (Application.isPlaying == false && Application.isEditor && value != null)
            {
                Validate(this.objectCacheResource);
            }
        }
    }


    public Object EditorAssetReferece
    {
        get
        {
#if UNITY_EDITOR
            GetEditorDirectReference<Object>();
#endif
            return null;
        }
    }

    public T GetEditorDirectReference<T>() where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        AssetDatabase.LoadAssetAtPath<T>(this.assetPath);
#endif
        return null;
    }


    public long[] resourceIds;

    public bool isMono;

    public string resourceName;
    public string assetPath;

    public string assetBundleName;
    public string resourcesPath;
    public string webPath;

    public bool loadableWeb
    {
        get { return IsSupporterSource(ResourceSource.Web); }
        set { UpdateSupporterSource(ResourceSource.Web, value); }
    }

    public bool loadableResource
    {
        get { return IsSupporterSource(ResourceSource.Resources); }
        set { UpdateSupporterSource(ResourceSource.Resources, value); }
    }
    public bool loadableStream
    {
        get { return IsSupporterSource(ResourceSource.StreamingAssets); }
        set { UpdateSupporterSource(ResourceSource.StreamingAssets, value); }
    }
    public bool loadableAssetBundle
    {
        get { return IsSupporterSource(ResourceSource.AssetBundles); }
        set { UpdateSupporterSource(ResourceSource.AssetBundles, value); }
    }

    [EnumFlagsAttribute]
    public ResourceSource ResourceSource;

    public bool multiObjects;
    public int objectIndex;
    public bool multiObjectsAssetBundle;
    public int objectIndexAssetBundle;
    public string customResourcePath;
    public bool readableTexture;
    public int id;
    public int cacheVersion;
    public bool canBeUnloaded;

    public string[] StreamingAssetsPaths = new string[Enum.GetValues(typeof(StreamingAssetsPathType)).Length];
    public string[] StreamingAssetsMovieAudioPaths = new string[Enum.GetValues(typeof(StreamingAssetsPathType)).Length];

    [EnumFlagsAttribute]
    public StreamingAssetsPathIsMovie StreamingAssetsPathIsMovie;

    public Object directRef;
    public Texture2D directRefTexture;
    public Sprite directRefSprite;

    public object loadedObject
    {

        get
        {
            if (this.objectCacheResource == true)
                return objectCacheResource;
            return ResourcesProvider.GetResourceObjectById(this);
        }

    }

    public bool loaded
    {

        get
        {

            return this.objectCacheResource || ResourcesProvider.IsResourceObjectLoaded(this);

        }

    }

    public bool IsLoadable(bool checkDirectRef = true)
    {

        return
            this.loadableAssetBundle == true ||
            this.loadableResource == true ||
            this.loadableStream == true ||
            this.loadableWeb == true ||
            (checkDirectRef == true && this.directRef != null);
    }

    private bool IsAsync()
    {
        return
            this.loadableAssetBundle == true ||
            this.loadableResource == true ||
            this.loadableStream == true ||
            this.loadableWeb == true;
    }

    public bool HasDirectRef()
    {

        return this.directRef != null;

    }

    public bool IsType(Type type)
    {

        var key = (long)TypeUtils.GetJavaHash(type.FullName);
        for (int i = 0; i < resourceIds.Length; i++)
        {
            var typeId = resourceIds[i];
            if (typeId == key)
                return true;
        }

        return false;
    }

    public T GetDirectRef<T>() where T : Object
    {

        if (Application.isEditor && Application.isPlaying == false)
        {
            return GetEditorDirectReference<T>();
        }

        var bufferRessource = this.objectCacheResource as T;
        if (bufferRessource != null)
            return bufferRessource;
        bufferRessource = GetObjectReferences<T>();
        this.objectCacheResource = bufferRessource;
        return bufferRessource;
    }

    public int GetId()
    {

        return this.id;

    }

    private T GetRemoteDirectRefRuntime<T>() where T : Object
    {

        if (this.directRef != null) return this.directRef as T;
        T result = null;

        if (this.loadableAssetBundle)
        {
            result = this.loadResourceAsync ?
                ResourcesProvider.LoadBundleResource<T>(this.assetBundleName, this.resourcesPath) :
                ResourcesProvider.LoadBundleResourceSync<T>(this.assetBundleName, this.resourcesPath);
        }
        else if (this.loadableResource)
        {
            result = ResourcesProvider.Load<T>(this.resourcesPath);
        }
        //todo add web and stream resources loading
        return result;
    }

    private void UnloadObject_INTERNAL(Object item)
    {

        if (item == null) return;

        if (this.loadableResource == true)
        {

            if (this.canBeUnloaded == true)
            {

                Resources.UnloadAsset(item);

            }

        }
        else if (this.loadableStream == true)
        {

        }
        else if (this.loadableAssetBundle == true)
        {

            //WindowSystemResources.UnloadBundleResource(assetBundleName);
            //Object.DestroyImmediate(item, true);

        }

    }


    public void Unload()
    {

        if (this.IsLoadable() == true)
        {

            this.UnloadObject_INTERNAL(this.loadedObject as Object);

        }

    }

    public IEnumerator<byte> LoadAudioClip(System.Action<AudioClip> callback)
    {

        #region Load Resource
        if (this.loadableResource == true)
        {

            if (this.async == true)
            {

                var request = Resources.LoadAsync<AudioClip>(this.resourcesPath);
                while (request.isDone == false)
                {

                    yield return 0;

                }

                callback.Invoke(request.asset as AudioClip);

            }
            else
            {

                callback.Invoke(UnityEngine.UI.Windows.ResourcesProvider.Load<AudioClip>(this.resourcesPath));

            }

            yield break;

        }
        #endregion

        #region Load Stream
        if (this.loadableStream == true)
        {

            var path = this.GetStreamPath(withFile: true);
            //if (UnityEngine.UI.Windows.Constants.LOGS_ENABLED == true) UnityEngine.Debug.Log("Loading: " + path);
            var www = new WWW(path);
            while (www.isDone == false)
            {

                yield return 0;

            }

            if (string.IsNullOrEmpty(www.error) == true)
            {
#if UNITY_5_6_OR_NEWER
                var clip = www.GetAudioClip();
#else
					var clip = www.audioClip;
#endif
                //if (UnityEngine.UI.Windows.Constants.LOGS_ENABLED == true) UnityEngine.Debug.Log("Callback!");
                callback.Invoke(clip);

            }
            else
            {

                //if (UnityEngine.UI.Windows.Constants.LOGS_ENABLED == true) UnityEngine.Debug.Log("Callback!");
                callback.Invoke(null);

            }

            www.Dispose();
            www = null;

            yield break;

        }
        #endregion

        callback.Invoke(null);

    }

    public string GetAssetBundleRelativePath()
    {
        return this.assetBundleName;
    }

    public bool IsMovie(StreamingAssetsPathIsMovie isMovie)
    {
        var result = (this.StreamingAssetsPathIsMovie & isMovie) > 0;
        return result;
    }

    public bool IsSupporterSource(ResourceSource source)
    {
        var result = (this.ResourceSource & source) > 0;
        return result;
    }

    public void UpdateSupporterSource(ResourceSource source, bool supported)
    {

        var result = supported ? this.ResourceSource | source : this.ResourceSource & ~source;
        ResourceSource = result;

    }

    public string GetPlatformStreamingPath(StreamingAssetsPathType platform)
    {
        return this.StreamingAssetsPaths[(int)platform];
    }

    public void SetPlatformStreamingPath(StreamingAssetsPathType platform, string path)
    {
        this.StreamingAssetsPaths[(int)platform] = path;
    }

    public string GetPlatformMovieStreamingPath(StreamingAssetsPathType platform)
    {
        return this.StreamingAssetsMovieAudioPaths[(int)platform];
    }

    public bool IsMovie(string streamingPath)
    {

        var fileExt = System.IO.Path.GetExtension(streamingPath).Trim('.').ToLower();
        return (fileExt == "avi" || fileExt == "mp4" || fileExt == "ogv" || fileExt == "mov" || fileExt == "webm");

    }

    public string GetStreamPath(bool withFile = false)
    {

        var combine = true;
        var path = string.Empty;
        var prefix = string.Empty;
        var prefixPath = string.Empty;
        var result = string.Empty;
#if UNITY_STANDALONE || UNITY_EDITOR
        {

#if UNITY_STANDALONE_OSX
				path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathStandaloneOSX);
#endif
        }
        {
#if UNITY_STANDALONE_LINUX
                path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathStandaloneLinux);
#endif
        }
        {
#if UNITY_STANDALONE_WIN
            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathStandaloneWindows);
#endif
        }
        if (string.IsNullOrEmpty(path) == true)
            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathStandalone);
        /*if (path.Contains("://") == false) {

            prefix = "file:///";

        }*/
#elif UNITY_IPHONE || UNITY_TVOS
            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathIOS);
			combine = true;
			/*if (withFile == true) {
				
				prefixPath = "Data/Raw/";

			}*/
#elif UNITY_ANDROID
            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathAndroid);
#elif UNITY_PS4
            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathPS4);
			combine = false;
			prefix = "streamingAssets/";
#elif UNITY_XBOXONE
            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathXBOXONE);
#elif UNITY_SWITCH
            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathSwitch);
#else
            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathCommon);
			if (path.Contains("://") == false) {

				prefix = "file:///";

			}
#endif

        if (string.IsNullOrEmpty(path) == true)
        {

            path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathCommon);
        }

        if (string.IsNullOrEmpty(path) == false)
        {

            path = path.Replace("\\", "/");

            if (combine == true)
            {

                result = prefix + System.IO.Path.Combine(Application.streamingAssetsPath, prefixPath + path);

            }
            else
            {

                result = prefix + prefixPath + path;

            }

        }

        if (withFile == true && string.IsNullOrEmpty(result) == false)
        {

            if (result.Contains("://") == false)
            {

                result = "file:///" + result;

            }

        }

        return result;

    }

    private T GetObjectReferences<T>() where T : Object
    {

        if (this.directRef == null)
        {
            return GetRemoteDirectRefRuntime<T>();
        }

        return LoadDataFromDirectRef<T>();
    }

    public static int GetJavaHash(string s)
    {

        if (string.IsNullOrEmpty(s) == true) return 0;

        int hash = 0;
        foreach (char c in s)
        {

            hash = 31 * hash + c;

        }

        return hash;

    }

    public static long GetKey(int val1, int val2)
    {

        return (long)(((long)val1 << 32) | ((long)val2 & 0xffffffff));

    }


    public virtual bool Validate()
    {
#if UNITY_EDITOR

        if (string.IsNullOrEmpty(assetPath) == false && !File.Exists(assetPath) && assetPath.Contains(@"/Resources/"))
        {
            var path = assetPath.Replace(@"/Resources/", @"/");
            if (File.Exists(path))
            {
                assetPath = path;
            }
        }
        if (this.directRef != null)
        {
            return Validate(this.directRef);
        }
        if (!string.IsNullOrEmpty(assetPath))
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (asset)
            {
                return Validate(asset);
            }
        }
#endif
        return false;
    }

    private T LoadDataFromDirectRef<T>() where T : Object
    {

        if (this.directRef != null)
        {

            if (typeof(T) == typeof(Sprite) && this.directRef is Texture2D)
            {

                return (T)(object)this.directRefSprite;
            }

            if (typeof(T) == typeof(Texture2D) && this.directRef is Sprite)
            {

                return (T)(object)this.directRefTexture;

            }

            if (this.directRef is GameObject)
            {
                return (T)(object)((this.directRef as GameObject).GetComponent(typeof(T)));
            }

            return (T)(object)this.directRef;

        }

        return default(T);
    }

    #region Unity Editor


    public virtual bool Validate(Object item)
    {

#if UNITY_EDITOR
        if (item == null)
        {
            ResetToDefault();
            return false;
        }

        var itemPath = AssetDatabase.GetAssetPath(item);

        var previousPath = this.assetPath;

        var isPathChanged = EditorUtilities.SetValueIfDirty(ref this.assetPath, itemPath);

        var dirty = isPathChanged;

        dirty = this.UpdateResourcesAsset() || dirty;

        if (isPathChanged || isInitialized == false)
        {
            this.UpdateSteamingAssetResource();
        }

        dirty = this.UpdateAssetBundleResource(this.assetPath) || dirty;

        var uniquePath = (this.multiObjects == true) ?
            (string.Format("{0}#{1}", this.GetAssetPath(), this.objectIndex)) : this.GetAssetPath();
        dirty = EditorUtilities.SetValueIfDirty(ref this.id, ResourceBase.GetJavaHash(uniquePath)) || dirty;
        dirty = EditorUtilities.SetValueIfDirty(ref this.canBeUnloaded, ((item is GameObject) == false && (item is Component) == false)) || dirty;

        this.UpdateLoadableResource(item);

        //update path
        var path = AssetDatabase.GetAssetPath(item);
        this.assetPath = path;
        this.resourceName = item.name;
        dirty = EditorUtilities.SetValueIfDirty(ref this.resourceName, item.name) || dirty;
        var type = item.GetType();
        this.resourceIds = type.GetTypeIds();
        this.isMono = item is GameObject;
        this.objectCacheResource = item;
        return dirty;
#endif
        return false;
    }


#if UNITY_EDITOR

    public virtual void ResetToDefault()
    {

        this.assetPath = null;
        this.customResourcePath = null;
        this.resourcesPath = null;
        this.assetBundleName = null;

        this.objectIndex = 0;
        this.objectIndexAssetBundle = 0;
        this.id = 0;

        this.multiObjectsAssetBundle = false;
        this.loadableResource = false;
        this.loadableStream = false;
        this.loadableAssetBundle = false;
        this.loadableWeb = false;

    }

    public static string GetResourcePathFromAssetPath(string assetPath)
    {

        if (assetPath.Contains("/Resources/") == true)
        {

            var splitted = assetPath.Split(new string[] { "/Resources/" }, System.StringSplitOptions.None);
            var joined = new List<string>();
            for (int i = 1; i < splitted.Length; ++i)
            {

                joined.Add(splitted[i]);
                if (i < splitted.Length - 1) joined.Add("Resources");

            }

            var resourcePath = string.Join("/", joined.ToArray());
            var ext = System.IO.Path.GetExtension(resourcePath);
            return resourcePath.Substring(0, resourcePath.Length - ext.Length);

        }

        return string.Empty;

    }

    //public Object tempRef;

    private void UpdateLoadableResource(Object item)
    {

        var isDirectResource = this.IsAsync() == false;
        if (isDirectResource)
        {
            EditorUtilities.SetObjectIfDirty(ref this.directRef, item);
            this.directRefTexture = item as Texture2D;
            this.directRefSprite = item as Sprite;
            if (item is Texture)
            {

                this.directRefSprite = AssetDatabase.LoadAllAssetsAtPath(this.assetPath).OfType<Sprite>().FirstOrDefault<Sprite>();

            }

            if (item is Sprite)
            {

                this.directRefTexture = (item as Sprite).texture;

            }

        }
        else
        {

            ME.EditorUtilities.SetObjectIfDirty(ref this.directRef, null);
            var obj = (Object)this.directRefSprite;
            ME.EditorUtilities.SetObjectIfDirty(ref obj, null);
            obj = (Object)this.directRefTexture;
            ME.EditorUtilities.SetObjectIfDirty(ref obj, null);
            this.directRefSprite = null;
            this.directRefTexture = null;
        }

        async = isDirectResource;
    }

    /// <summary>
    /// update database item if loaded from bundle
    /// </summary>
    private bool UpdateAssetBundleResource(string budnleResourcePath)
    {
        var assetImporter = AssetImporter.GetAtPath(budnleResourcePath);
        if (assetImporter == false)
        {
            return ResetBundleData();
        }
        var temp = this.loadableAssetBundle;
        var bundleName = GetBundleName(budnleResourcePath);
        var isInBundle = (string.IsNullOrEmpty(bundleName) == false);

        var dirty = EditorUtilities.SetValueIfDirty(ref this.assetBundleName, bundleName);
        dirty = EditorUtilities.SetValueIfDirty(ref temp, isInBundle) || dirty;

        this.loadableAssetBundle = temp;

        if (this.loadableAssetBundle == true)
        {
            var assetName = assetImporter.assetPath;
            assetName = Path.GetFileNameWithoutExtension(assetImporter.assetPath);
            dirty = EditorUtilities.SetValueIfDirty(ref this.resourcesPath, assetName) || dirty;
            this.directRefTexture = null;
            this.directRefSprite = null;
            this.resourcesPath = assetName;
        }
        else
        {
            this.assetBundleName = string.Empty;
            dirty = EditorUtilities.SetValueIfDirty(ref this.assetBundleName, string.Empty) || dirty;
        }

        return dirty;
    }

    private bool ResetBundleData()
    {

        if (this.loadableAssetBundle == false) return false;
        this.loadableAssetBundle = false;
        this.assetBundleName = string.Empty;
        return true;
    }

    public string GetBundleName(string path)
    {
        if (string.IsNullOrEmpty(path) == true) return string.Empty;
        var abName = UnityEditor.AssetDatabase.GetImplicitAssetBundleName(path);
        return string.IsNullOrEmpty(abName) ? string.Empty : abName;
    }

    public void UpdateSteamingAssetResource()
    {

        var isStreaming = this.assetPath.Contains("/StreamingAssets/") == true;

        var streamingAssetsPath = (isStreaming ? this.assetPath.Split(new string[] { "/StreamingAssets/" }, System.StringSplitOptions.None)[1] : string.Empty);

        UpdateStreamingPaths(streamingAssetsPath);

        this.loadableStream = (string.IsNullOrEmpty(streamingAssetsPath) == false);

    }

    public void UpdateStreamingPaths(string streamingAssetsPath)
    {

        var platformSplit = streamingAssetsPath.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

        var platformTypes = Enum.GetValues(typeof(StreamingAssetsPathType));
        this.StreamingAssetsPaths = new string[platformTypes.Length];
        this.StreamingAssetsMovieAudioPaths = new string[platformTypes.Length];
        this.StreamingAssetsPathIsMovie = StreamingAssetsPathIsMovie.StreamingAssetsPathNone;

        if (platformSplit.Length == 0) return;

        var localPath = string.Join("/", platformSplit, 1, platformSplit.Length - 1);
        var localDir = System.IO.Path.GetDirectoryName(localPath);
        var filenameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(streamingAssetsPath);

        foreach (var pathData in ResourceBase.StreamingPathMap)
        {

            var path = this.GetPlatformDirectory(pathData.Key, localDir, filenameWithoutExt);
            var index = (int)pathData.Value;
            this.StreamingAssetsPaths[index] = path;
            var moviePath = this.GetMovieAudio(path);
            this.StreamingAssetsMovieAudioPaths[index] = moviePath;
            var isMovie = this.IsMovie(path);
            this.StreamingAssetsPathIsMovie |= isMovie
                ? ResourceBase.StreamingIsMoviePathMap[pathData.Value]
                : StreamingAssetsPathIsMovie.StreamingAssetsPathNone;

        }

    }

    private bool UpdateResourcesAsset()
    {
        var assetResourcePath = (this.assetPath.Contains("/Resources/") == true
            ? this.assetPath.Split(new string[] { "/Resources/" }, System.StringSplitOptions.None)[1]
            : string.Empty);

        var ext = System.IO.Path.GetExtension(assetResourcePath);
        var dirty = EditorUtilities.SetValueIfDirty(ref this.resourcesPath, assetResourcePath.Substring(0, assetResourcePath.Length - ext.Length));

        var isLoadableResource = (string.IsNullOrEmpty(this.resourcesPath) == false);
        dirty = dirty || (isLoadableResource != this.loadableResource);

        this.loadableResource = isLoadableResource;
        return dirty;
    }

    private string GetAssetPath()
    {

        var path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathIOS);

        if (string.IsNullOrEmpty(path) == true) path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathAndroid);
        if (string.IsNullOrEmpty(path) == true) path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathStandalone);
        if (string.IsNullOrEmpty(path) == true) path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathPS4);
        if (string.IsNullOrEmpty(path) == true) path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathXBOXONE);
        if (string.IsNullOrEmpty(path) == true) path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathSwitch);
        if (string.IsNullOrEmpty(path) == true) path = GetPlatformStreamingPath(StreamingAssetsPathType.StreamingAssetsPathCommon);
        if (string.IsNullOrEmpty(path) == true) path = this.assetPath;

        return path;

    }

    private string GetMovieAudio(string streamingPath)
    {

        if (string.IsNullOrEmpty(streamingPath) == true)
        {

            return string.Empty;

        }

        var path = System.IO.Path.Combine(Application.streamingAssetsPath, System.IO.Path.GetDirectoryName(streamingPath));
        if (System.IO.Directory.Exists(path) == true)
        {

            var filename = System.IO.Path.GetFileNameWithoutExtension(streamingPath);
            var filenameWithExt = System.IO.Path.GetFileName(streamingPath);
            var files = System.IO.Directory.GetFiles(path);
            var found = string.Empty;
            foreach (var file in files)
            {

                if (filenameWithExt == System.IO.Path.GetFileName(file)) continue;

                var curFilename = System.IO.Path.GetFileNameWithoutExtension(file);
                if (curFilename == filename)
                {

                    found = System.IO.Path.GetFileName(file);
                    break;

                }

            }

            if (string.IsNullOrEmpty(found) == false)
            {

                return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(streamingPath), found);

            }

        }

        return string.Empty;

    }

    private string GetPlatformDirectory(Platform platform, string localDir, string filenameWithoutExt)
    {

        var platformDir = platform.ToString() + "/";

        var sPath = System.IO.Path.Combine(Application.streamingAssetsPath, platformDir + localDir);
        if (System.IO.Directory.Exists(sPath) == true)
        {

            var files = System.IO.Directory.GetFiles(sPath);
            foreach (var file in files)
            {

                var ext = System.IO.Path.GetExtension(file).ToLower();
                if (ext == ".meta") continue;

                var curFilename = System.IO.Path.GetFileNameWithoutExtension(file);
                if (filenameWithoutExt == curFilename)
                {

                    ext = System.IO.Path.GetExtension(file);
                    var combinedPath = System.IO.Path.Combine(platformDir + localDir, filenameWithoutExt + ext);
                    combinedPath = combinedPath.Replace('\\', '/');
                    return combinedPath;

                }

            }

        }

        return string.Empty;

    }
#endif
    #endregion
};
