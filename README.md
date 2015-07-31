# A Watchdog Plugin for an Unity3D Android App

The idea is to have a plugin for the Unity3D app which will restart the application as soon as it becomes unresponsive. This is done for the Unity3D app running on Android, so the plugin is written in Java. 

How to build an android java plugin for an C# Unity3D application we learned from this article: 

 - https://blog.nraboy.com/2014/06/creating-an-android-java-plugin-for-unity3d/

The idea is simple: The plugin provides a `tic` method to the unity app which must be called on every update in the main event loop. This is the heartbeat of the applcation, as long as the `tic` method is called, the app is considered alive. Once the app stops looping and thereby stops sending the tics, the plugin will restart the application. 

Two classes are involved in this: 

 - `HeartbeatWatchdog.java`: the java plugin code itseld and 
 - `UnityHeartbeat.cs`: the C# class to start and call the plugin class from C#
   Unity side. 

This project combines both parts in the two subdirectories:

 - **android-heartbeat-watchdog:** The android java code to handle the restart
 - **unity-android-heartbeat-watchdog-demonstrator:** A Unity3D demo project which
   show how the android plugin is used. 

**TODO:** explain Unity3D project setup with the plugin
