# csharp_doodles
Hello,

Here are some of the classes I worked on. Feel free to use them in your projects!

List:

-ECSHelper:
Extension class that enhances the use of Unity's entity component system, still leveraging it's speed, yet allows to use the entities in similar fashion to individual GameObjects.

-ArithmeticHelper:
Hey, have you ever wanted to write Vector3<float> or Vector3<int> instead of creating individual classes for them (Vector3f, Vector3i etc.). If yes, then u most likely stumbled upon the inability to distinguish between numeric T an others. Arithmethic helper leverages checks if the given T is of numeric type, and then, through the power of reflections and and method caching it creates optimized "operators" for you to easily create all the Vector3<T>, Matrix4x4<T> and other numeric classes!

-EventLogic:
Attaching to event in unity or csharp means that you have to get the reference to the object containing the event, which defeats the purpose because it also creates spaghetti code. With my Events you are able to create Event classes that contain any type of parameters you prefer, and easily connect ot them via attribute. Example usage in the file!

-Save:
Advanced save system for Unity (and, with a slight path modification, csharp). It allows you to easili store zipped data of any serializable type. Data is divided by it's use (eg. Global, User, Snapshot). Global standss for all your global data - mostly settings, User is your player's data unrelated to current save file - keybinds, icon, playtime etc., and Snapshot is all the current moment data, like level they are on, or enemies that were spawned. 
