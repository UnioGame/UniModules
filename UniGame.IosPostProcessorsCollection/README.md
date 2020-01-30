# iOSPostprocessCollection
A number of Unity3d iOS post-processes wrapped in a single post-process, configured by Scriptable Object.
This plugin shouldn't be confused with graphical post-process. Also these post-process supports only iOS, and contains no post-processes for other platform. But on not-iOS platforms all the modifications won't affect building pipeline.

A lot of people expirience a bunch of troubles while making their project run on iOS, event if Android version works fine. Most of these troubles come from setting som custom actions on XCode project while building your game. These troubles are solved by Unity's post-process system. Searching through the web you can find a lot of different iOS post-process scripts. Each of this scripts will do only a small amount of needed job - solve concrete prolem. The bigger your project is - more post-process script (operation) it requires.

This package is an attempt to make a codeless convenient solution for integration a lot of post-process actions.
What actions does this package can help you to do with the XCode project:
1. Adding native frameworks (such as GameKit, SystemConfiguration, UIKit, Foundation, CoreTelephony, CoreLocation, CoreGraphics, AdSupport, Security, GameKit, SafariServices, etc)
2. Adding Liker flags to project's settins (-Objc, -lxml2, etc)
3. Adding custom values to Info.plist file (essential for lot of SDK, as Google, Facebook, Firebase)
4. Copy files from Unity3D project to "Copy Bundle Resources" section in XCode project (files with any extension, except .meta, which are ignored automatically).
5. Add Entitlements file to project (project capabilities)
6. Replace Xcode's app delegate file to your custom one (rarely required but sometimes saves your life. Essential in situations when you need to modify app's delegate, but one or several 3rd party plugins are already doing it).

## Integration
This plugin is distributed via source code. You can simply download PostProcessCollection.unitypackage from this repo. Then follow these steps:
1. Import PostProcessCollection.unitypackage in your project
2. RMB click in you Project view in Unity and select Create -> XCode Modification or select Assets -> Create -> XCode Modification
3. A new instance of Scriptable Object with New_XCode_Modification name will be created.
4. You can name this SO as you wish. You can now enter needed values to this SO. This will describe modifications which will be performed on XCode project.
5. Locate and select script called PostProcessCollectionExecutor in inspector.
6. Drag & Drop your XCode Modification SO file into PostProcessCollectionExecutor inspector's field called "Xcode Modifications Scriptable Object"

When the steps below are done, set up is complete.
