# UniModules

UniGame Packages Compilation

## Unity Package Installation

Add to your project manifiest by path [%UnityProject%]/Packages/manifiest.json these lines:

```json
{
  "scopedRegistries": [
    {
      "name": "Unity",
      "url": "https://package.unity.com",
      "scopes": [
        "com.unity"
      ]
    },
    {
      "name": "UniGame",
      "url": "http://package.unigame.pro:4873/",
      "scopes": [
        "com.unigame"
      ]
    }
  ],
}
```

# LiTime

# LifeTime Class Documentation

The  `LifeTime` class is used for managing the lifetime of objects and resources in a project. It ensures proper resource cleanup and prevents memory leaks. All interactions with LifeTime should be done through the `ILifeTime` interface to ensure that only the owner of the resource can destroy it. This approach allows other services to depend on the specified lifetime without having the ability to prematurely release the resources.

The key concept of `LifeTime` is single ownership of resources, acting as the owner to simplify their release.


## Features

- **Add Cleanup Actions**: Register actions to be executed when the lifetime ends.
- **Manage Disposable Objects**: Automatically dispose of objects when the lifetime ends.
- **Child Lifetimes**: Manage nested lifetimes, allowing for hierarchical cleanup.
- **Cancellation Tokens**: Provides a cancellation token that is canceled when the lifetime ends.
- **TerminatedLifetime**: Property provides terminated static lifetime

## Usage

### Creating a LifeTime Instance

To create a new `LifeTime` instance, use the static `Create` method:

```csharp
var lifeTime = LifeTime.Create();
```

### Adding Cleanup Actions

You can add cleanup actions that will be executed when the lifetime ends:

```csharp
lifeTime.AddCleanUpAction(() => {
    Debug.Log("Cleanup action executed.");
});
```

### Managing Disposable Objects

Add disposable objects to the lifetime to ensure they are disposed of when the lifetime ends:

```csharp
var disposable = new SomeDisposableObject();
lifeTime.AddDispose(disposable);
```

### Adding Child Lifetimes

You can add child lifetimes that will be terminated when the parent lifetime ends:

```csharp
var childLifeTime = LifeTime.Create();
lifeTime.AddChildLifeTime(childLifeTime);
```

### Using Cancellation Tokens

The `LifeTime` class provides a cancellation token that is canceled when the lifetime ends:

```csharp
var token = lifeTime.Token;
token.Register(() => {
    Debug.Log("Lifetime canceled.");
});


public void DoActionAsync()
{
    var assetLifeTime = this.GetAssetLifeTime();
    
    SomeActionAsync().AttachExternalCancellation(assetLifeTime.Token)
        .Forget();
    
    SomeActionAsync2(assetLifeTime.Token).Forget();
}

public async UniTask SomeActionAsync(){}

public async UniTask SomeActionAsync2(CancellationToken token){}

```

### Restarting a LifeTime

You can restart a lifetime, which will reset its state and allow it to be used again:

```csharp
lifeTime.Restart();
```

### Releasing a LifeTime

To manually end a lifetime and execute all registered cleanup actions, call the `Release` method:

```csharp
lifeTime.Release();
```

## Example

Here is a complete example demonstrating the usage of the `LifeTime` class:

```csharp
using UniModules.UniCore.Runtime.DataFlow;
using UnityEngine;

public class LifeTimeExample : MonoBehaviour
{
    private LifeTime _lifeTime;

    void Start()
    {
        _lifeTime = LifeTime.Create();
        _assetLifeTime = this.GetAssetLifeTime(); // you can use this.GetAssetLifeTime() extension

        _lifeTime.AddCleanUpAction(() => {
            Debug.Log("Cleanup action executed.");
        });

        var disposable = new SomeDisposableObject();
        _lifeTime.AddDispose(disposable);

        var childLifeTime = LifeTime.Create();
        _lifeTime.AddChildLifeTime(childLifeTime);

        var token = _lifeTime.Token;
        token.Register(() => {
            Debug.Log("Lifetime canceled.");
        });
    }

    void OnDestroy()
    {
        _lifeTime.Release();
    }
}

public class SomeDisposableObject : IDisposable
{
    public void Dispose()
    {
        Debug.Log("SomeDisposableObject disposed.");
    }
}
```

This example demonstrates how to create a `LifeTime` instance, add cleanup actions, manage disposable objects, add child lifetimes, and use the cancellation token. The `OnDestroy` method ensures that the lifetime is released when the `MonoBehaviour` is destroyed.

## Assets Life Times

The `AssetLifeTime` provides extension utilities for managing the lifecycle of assets in Unity. It includes methods for associating assets with lifetimes and cleaning up assets when their lifetimes end.

### Example Usage

```csharp

public class Example : MonoBehaviour
{
    private void Start()
    {

        var scriptableAsset = ScriptableObject.Instantiate<AddressableScriptableTest>();
        //create lifetime for scriptable asset, lifetime will be killed when asset is destroyed
        var soLifeTime = scriptableAsset.GetAssetLifeTime();

        // Get or create a lifetime for this GameObject
        var lifeTime = gameObject.GetAssetLifeTime();
        // Get or create a lifetime for Transform component, transformLifeTime == lifeTime from the same GameObject
        var transformLifeTime = transform.GetAssetLifeTime();

        // Add a cleanup action to destroy the GameObject when the lifetime ends
        lifeTime.DestroyOnCleanup(gameObject);

        // Add a disposable object to the lifetime
        var disposable = new SomeDisposable();
        lifeTime.AddDispose(disposable);

        Object.Destroy(gameObject)//now lifetime accotiated with this object will be killed
    }
}

public class AddressableScriptableTest : ScriptableObject{}

public class SomeDisposable : IDisposable
{
    public void Dispose()
    {
        // Cleanup code here
    }
}
```

### Methods

GetAssetLifeTime(Object source, bool terminateOnDisable = false) : Retrieves or creates a lifetime for the given asset.
DestroyOnCleanup(LifeTime lifeTime, GameObject gameObject) : Adds a cleanup action to destroy the GameObject when the lifetime ends.
DestroyOnCleanup(LifeTime lifeTime, Component component, bool onlyComponent = false) : Adds a cleanup action to destroy the component or its GameObject when the lifetime ends.
AddDisposable(Object gameObject, IDisposable disposable) : Adds a disposable object to the GameObject's lifetime.
AddCleanUp(Object gameObject, Action cleanupAction) : Adds a cleanup action to the GameObject's lifetime.
AddDisposable(Component component, IDisposable disposable) : Adds a disposable object to the component's GameObject's lifetime.
AddCleanUp(Component component, Action action) : Adds a cleanup action to the component's GameObject's lifetime.


## Scene LifeTime

Provides utility methods for binding resources to the scene lifetime.

### Methods

- **GetActiveSceneLifeTime()**: Gets the lifetime of the currently active scene.
- **GetSceneLifeTime(Scene scene)**: Gets the lifetime of the specified scene. If scene is not loaded when return Terminated LifeTime
- **AddTo(IDisposable disposable, Scene scene)**: Adds a disposable to the specified scene's lifetime.
- **AddToActiveScene(IDisposable disposable)**: Adds a disposable to the active scene's lifetime.
- **AddToScene(IDisposable disposable, string scenePath)**: Adds a disposable to the specified scene's lifetime by scene ID.

### Example Usage

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using UniModules.UniGame.Core.Runtime.DataFlow.Extensions;

public class ExampleUsage : MonoBehaviour
{
    private void Start()
    {
        // Initialize scene lifetime management
        SceneLifeTime.Initialize();

        // Get the lifetime of the active scene
        var activeSceneLifeTime = SceneLifeTime.GetActiveSceneLifeTime();

        // Add a disposable to the active scene's lifetime
        var disposable = new ExampleDisposable();

        disposable.AddToActiveScene();
        
        activeSceneLifeTime.AddDispose(disposable);

        // Get the lifetime of a specific scene
        var scene = SceneManager.GetSceneByName("ExampleScene");
        var sceneLifeTime = SceneLifeTime.GetSceneLifeTime(scene);

        // Add a disposable to the specific scene's lifetime
        sceneLifeTime.AddDispose(disposable);
    }
}

public class ExampleDisposable : IDisposable
{
    public void Dispose()
    {
        // Cleanup code here
    }
}
```

# Addressable API

AddressableExtensions

```csharp
/// <summary>
/// Provides extension methods for working with Unity Addressables.
/// </summary>
public static class AddressableExtensions
```

Provides extension methods for working with Unity Addressables.
This class is used for loading and unloading resources conveniently using ILifeTime,
allowing dependencies to be unloaded from memory at the appropriate time. 
Allow to spawn GameObject instances with object pooling and bind Addressable handle to Instance LifeTime

## Methods API

Methods

### Addressable Assets Loading

LoadAssetTaskAsync: Loads an asset asynchronously and bind Addressanle Handle to LifeTime

```csharp
public static async UniTask<T> LoadAssetTaskAsync<T>(
    this AssetReference assetReference, 
    ILifeTime lifeTime, 
    bool downloadDependencies = false,
    IProgress<float> progress = null)
```

LoadAssetsTaskAsync: Loads assets by resource location ids or asset references or GUID's

```csharp
public static async UniTask<IList<Object>> LoadAssetsTaskAsync(
    this string resource, 
    ILifeTime lifeTime,
    IProgress<float> progress = null)
```

```csharp
public static async UniTask<IList<T>> LoadAssetsTaskAsync<T>(
    this IEnumerable labels, 
    ILifeTime lifeTime,
    Addressables.MergeMode mode = Addressables.MergeMode.Union,
    IProgress<float> progress = null)
```


Example

```csharp

public class AddressableScriptableTest : ScriptableObject
{
    public AssetReference assetReference;

    public async UniTask LoadAssetAsync(){

        var assetLifeTime = this.GetAssetLifeTime();
        //load by asset reference
        var loadedAsset = await asset1.LoadAssetTaskAsync<GameObject>(assetLifeTime);
        //load by guid resource location
        var loadedAssetByGUID = await asset1.AssetGUID.LoadAssetTaskAsync<GameObject>(assetLifeTime);

    }
}

```

### Load Scenes from Addressable

LoadSceneTaskAsync: Loads a scene asynchronously using an AssetReference or a string reference.

```csharp

public static async UniTask<SceneInstance> LoadSceneTaskAsync(
    this string sceneReference,
    ILifeTime lifeTime,
    LoadSceneMode loadSceneMode = LoadSceneMode.Single,
    bool activateOnLoad = true,
    int priority = 100,
    IProgress<float> progress = null){}


public static async UniTask<SceneInstance> LoadSceneTaskAsync(
    this AssetReference sceneReference,
    ILifeTime lifeTime,
    LoadSceneMode loadSceneMode = LoadSceneMode.Single,
    bool activateOnLoad = true,
    int priority = 100,
    IProgress<float> progress = null){}

```

Example


```csharp

public class SceneLoader : MonoBehaviour
{
    public AssetReference sceneReference;
    private ILifeTime lifeTime;

    private async void Start()
    {
        var sceneInstance = await sceneReference.LoadSceneTaskAsync(lifeTime, LoadSceneMode.Single, true, 100);
        Debug.Log("Scene loaded: " + sceneInstance.Scene.name);
    }
}

```


### Spawn instances and release handles when it's killed

**SpawnObjectAsync: Spawns an object asynchronously.**

The SpawnObjectAsync<T> method is designed to asynchronously create an instance of an object of type T from an addressable resource. 
It loads the resource by the specified key, creates its instance, and returns it.
The method supports various parameters such as position, parent object, activation on spawn, downloading dependencies, and progress reporting. When the instance is destroyed, the addressable handle will be automatically released.

Object pooling is supported on spawn new instances

```csharp
public static async UniTask<T> SpawnObjectAsync<T>(
    this string reference,
    Vector3 position = default,
    Transform parent = null,
    ILifeTime lifeTime = null,
    bool activateOnSpawn = true,
    bool downloadDependencies = false,
    CancellationToken token = default,
    IProgress<float> progress = null)

public static UniTask<T> SpawnObjectAsync<T>(
    this AssetReferenceT<T> reference, ...)

public static UniTask<T> SpawnObjectAsync<T>(
    this AssetReference reference, ... )
```

**SpawnObjectsAsync: Spawns multiple objects asynchronously.**

The method spawns asynchronous instances and is optimized for creating multiple objects at once. It is not recommended for repeatedly spawning single objects, as an additional array will be created for the results.
The method supports object pooling. Each result increments the addressable handle by one. When the instance is destroyed, the dependency counter will decrease.


```csharp

public static async UniTask<T> SpawnObjectAsync<T>(
    this string reference,
    Vector3 position = default,
    Transform parent = null,
    ILifeTime lifeTime = null,
    bool activateOnSpawn = true,
    bool downloadDependencies = false,
    CancellationToken token = default,
    IProgress<float> progress = null)

public static UniTask<T> SpawnObjectAsync<T>(this AssetReferenceT<T> reference, ... )

public static UniTask<T> SpawnObjectAsync<T>(this AssetReference reference, ... )

```


Exmaples

```csharp

public async UniTask LoadItemsAsync(int amount)
{
    var assets = await asset1.SpawnObjectsAsync(amount,
        token: destroyCancellationToken);

    for (var i = 0; i < assets.Length; i++)
    {
        var asset = assets[i];
        if (asset == null) continue;
        
        asset.transform.position = Random.insideUnitSphere * radius;
    }
}

```


```csharp

public class GameObjectSpawner : MonoBehaviour
{
    public AssetReference assetReference;
    private ILifeTime lifeTime;

    private async void Start()
    {
        var gameObjectInstance = await assetReference.SpawnObjectAsync<GameObject>(Vector3.zero, null, lifeTime, true);
        Debug.Log("GameObject spawned: " + gameObjectInstance.name);
    }
}

```