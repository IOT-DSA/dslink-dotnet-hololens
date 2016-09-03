# HoloLens DSLink
## Requirements
- Visual Studio 2015 Update 3 - https://developer.microsoft.com/en-us/windows/downloads
- HoloLens Emulator - http://go.microsoft.com/fwlink/?LinkID=823018
- Unity HoloLens Technical Preview - https://unity3d.com/partners/windows/hololens

## Getting Started
1. Clone this repository locally.
2. Open the project in Unity HoloLens Technical Preview.
3. Open scene /Assets/Scenes/Main.unity
4. File -> Build Settings...
  - Click "Add Open Scenes" button
  - Ensure Scenes/Main is checked
  - Select Windows Store
  - SDK = Universal 10
  - UWP Build Type = D3D
  - Build and Run on = Local Machine
  - Unity C# Projects = true
5. Click Build, create a new folder in the repository root named App, select that folder.
6. After building, open the folder that it shows, and open the solution.
7. In the DSHoloLens project, replace the contents of App.cs with the contents of App.cs in the root of the repository.
8. In Visual Studio, open the configuration manager and ensure that DSHoloLens has both "Build" and "Deploy" checked.
9. Select Release and x86 as Configuration and Platform.
10. Change your run target to HoloLens Emulator 10.0.XXXXX.
11. Click the run button.

## Workflow
### Updated a scene in Unity
1. Save your scene and any other changes you made.
2. File -> Build Settings...
3. Click Build and select the same folder you chose in the Getting Started guide.
4. Switch back to your Visual Studio instance, and you should be given a window that allows you to Reload All.
5. Hit Run to test your new scene.

### Unity complains about DSLink reference not found
Due to the way this project must be implemented to properly use .NET 4.5 and the UWP platform, you must encapsulate all of your DSLink-specific code with:
```
#if WINDOWS_UWP
// Your code here...
#endif WINDOWS_UWP
```

## Voice Commands
### remove
Removes the object that the player is currently looking at, as long as it implements the OnVoiceRemove method.

### add switch
Adds a switch to the scene, in front of the camera.

### add light
Adds a light to the scene, in front of the camera.

### move
Moves the object that the player is looking at, as long as it contains the Placeable component.

### place
Places an object that the player is currently holding.

### activate
Activates the object that the player is currently looking at, as long as it implements the OnVoiceActivate method.

### deactivate
Deactivates the object that the player is currently looking at, as long as it implements the OnVoiceDeactivate method.

