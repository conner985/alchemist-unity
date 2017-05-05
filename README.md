## Alchemist-Unity Integration

### What is this Project:
an integration of the Unity3D game engine with Alchemist simulator

* Alchemist: https://github.com/AlchemistSimulator/Alchemist
* Unity3D: https://unity3d.com/

We aim to use the Protelis engine, written in Java, to calculate computational fields and take the results to Unity, through C# scripts, to be able to animate a full simulation exploiting at full all the built-in features that Unity offers like collision avoidance, physics, animations, etc...

Since Alchemist and Unity are written in different languages, it was realized a communication using HTTP protocol and REST messages.

### How to create a new project:

* download the file `alchemist-unity.unitypackage` present in the `Bundles` folder of this project


* create a new unity project


* import the bundle into the project hierarchy of Unity using drag&drop or through the menu: `Assets -> Import Package -> Custom Package...`


* make sure that everything is selected and press `import`


* now you should have two new folders in the Project hierarchy: "Prefabs" and "Scripts"


* The core of the communication is the prefab `"CentralSystem"` inside the `Prefabs` folder: drag&drop it into the scene


* now in the Scene Hierarchy you will see a new game object called CentralSystem and if you click on it, the inspector will show the components attached: ComUtil let you use synchronous and asynchronous HTTP communications through REST messages; "GradientNodesCollector" is responsible for gathering all the game objects that have attached a component that extends "AbstractBehaviourNode" and send them, using ComUtil functionalities, to Alchemist.


* what you want to do now is populate your scene with nodes that the collector can recognize: you can, for example, place a cube in the scene and add the component "SmartPanel" that extends AbstractBehaviourNode. From now on that cube will be collected by the CentralSystem and sent to Alchemist. If you place another object in the scene with the SmartPanel component attached, then the CentralSystem will collect both of them.

**Note 1:** SmartPanel is just a script example to show how to extend AbstractBehaviourNode and won't actually work in just any game object. If you want to see the SmartPanel script in action, it is suggested to take a look at the "SmartCity" project inside the "Examples" folder.

**Note 2:** When you build a scene remember to use the same unit measures the Unity's physic engine use: 1 unit = 1 meter; velocity [m/s]; mass [kg]. As reference the basic Cube is 1m x 1m x 1m. This is basically to have a more realistic simulation and avoid incoherency problems with Alchemist.
 
### How Communication Works:

* The communication is based on the HTTP protocol and will use REST messages. It is synchronous for the first two messages that are used for initialization (one POST to send Alchemist the number of nodes you have and the name of the program you want to run, a GET to request ids of the Alchemist's nodes to assign them to unity's ones). Every following communication will be completely asynchronous


* After the server has sent back the previous GET response and after every "post2getTime" milliseconds (this time is tweakable from the inspection of the CentralSystem), the GradientNodesCollector present in the CentralSystem will collect every node present in the scene that has a script attached that extends AbstractBehaviourNode (e.g. SmartPanel), then it will retrieve all GradientNode within them (that are the Unity version of the Alchemist node), serialize through JsonUtility and then send them, with a POST request, to the server


* After the server has sent back the POST response, a second timer will start ("get2postTime", also tweakable) and, when this is over, GradientNodesCollector will send a GET request to retrieve the concentration of the gradient for every node. The GET response will be a Json string that respect the NodesDescriptor structure, since the same exact class is present in the server, so that JsonUtility can deserialize automatically the response into a C# object

### How to extend the project:

Due to time, the project has been made with a very specific implementation that permits only a communication with nodes that have exactly 3 molecules (see the script "GradientNode") and will use only the Alchemist simulation described in the YAML file "gradient2.yml".

This has been done due to the limitation of the JsonUtility used for the serialization/deserialization of the messages since it doesn't know how to convert a generic C# object into the Json representation of it's real instance: if you create, for example, a List of objects called "alist" `(List\<object\> aList)` and then you populate aList with integers, booleans, etc..., the utility will see every element of the list as an object and won't be able to transform aList into a Json string.

In order to generalize the communication, a generic node should be realized (instead of the GradientNode) that contains a collection of pairs `\<molecule,concentration\>` where "molecule" is a string and "concentration" is of a type T that JsonUtility knows how to serialize correctly.

Alchemist side, the same generic node must be created to maintain consistency in the communication and instead of manually set the concentration of every single molecule, according to the ones arrived from Unity, iterate the collection and automatically set the concentration of the molecule with the same name of the one present in the collection.

**Side note:** to implement the server we used the NanoHTTPD library but it has a problem with the deletion of temp files it creates so, if you run the server for a while it will accumulates a bunch of these files and won't be able to delete them. What it can be done is to try another library (e.g. Gretty) and re-implement the server.
