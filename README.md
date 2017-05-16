## Alchemist-Unity Integration

### What is this Project:
an integration of the Unity3D game engine with Alchemist simulator

* Alchemist: [LINK](https://github.com/AlchemistSimulator/Alchemist)
* Unity3D: [LINK](https://unity3d.com/)

We aim to use the Protelis engine, written in Java, to calculate computational fields and take the results to Unity, through C# scripts, to be able to animate a full simulation exploiting at full all the built-in features that Unity offers like collision avoidance, physics, animations, etc...

Since Alchemist and Unity are written in different languages, it was realized a communication using HTTP protocol and REST messages.

### How to create a new project:

* download the file `alchemist-unity.unitypackage` present in the `Bundles` folder of this project


* create a new unity project


* import the bundle into the project hierarchy of Unity using drag&drop or through the menu: `Assets -> Import Package -> Custom Package...`


* make sure that everything is selected and press `import`


* now you should have two new folders in the Project hierarchy: `Prefabs` and `Scripts`


* The core of the communication is the prefab `CentralSystem` inside the `Prefabs` folder: drag&drop it into the scene


* now in the Scene Hierarchy you will see a new game object called `CentralSystem` and if you click on it, the inspector will show the components attached: `ComUtil` let you use synchronous and asynchronous HTTP communications through REST messages; `GradientNodesCollector` is responsible for gathering all the game objects that have attached a component that extends `AbstractBehaviourNode` and send them, using `ComUtil` functionalities, to Alchemist.


* what you want to do now is populate your scene with nodes that the collector can recognize: you can, for example, place a cube in the scene and add the component `SmartPanel` that extends `AbstractBehaviourNode`. From now on that cube will be collected by the `CentralSystem` and sent to Alchemist. If you place another object in the scene with the `SmartPanel` component attached, then the `CentralSystem` will collect both of them.

**Note 1:** `SmartPanel` is just a script example to show how to extend `AbstractBehaviourNode` and won't actually work in just any game object. If you want to see the SmartPanel script in action, it is suggested to take a look at the `SmartCity` project inside the `Examples` folder.

**Note 2:** When you build a scene remember to use the same unit measures the Unity's physic engine use: 1 unit = 1 meter; velocity [m/s]; mass [kg]. As reference the basic Cube is `1m x 1m x 1m`. This is basically to have a more realistic simulation and avoid incoherency problems with Alchemist.

### NanoHTTPD server and Alchemist simulation setup
Java side, some prelimiary steps are required in order to properly start the simulation

* Download the Alchemist project and follow the instructions to import the project on your IDE (e.g. Eclipse) correctly.

* Download the alchemist-unity project and copy the `alchemist-unity` folder, present inside the java-src folder, into the one chosen to contain the Alchemist repository, more specifically into the \Alchemist\alchemist folder.

* Modify the `build.gradle` file within the `alchemist-unity` project in order to add all dependencies you need from Alchemist and the  runtime section for all the external library required

* **IMPORTANT**: add this line into the \Alchemist\alchemist\setting.gradle at the end of the include: 'alchemist-unity'

* import the `alchemist-unity` project in your IDE  together with the Alchemist folders

Now the server is ready to work, following a description of the setup of a new Alchemist simulation:

* if a POST request arrives and the Json string is of type 'init' and progType GRADIENT (currently the only one supported), a new Alchemist simulation has to be started (without graphical interface) as follows:
   ```
   final String pathYaml = "/gradient2.yml";
   final Loader loader = new YamlLoader(NanoServer.class.getResourceAsStream(pathYaml));
   final Environment<Object> env = loader.getWith(Collections.emptyMap());
   final Simulation<Object> sim = new Engine<Object>(env, DoubleTime.INFINITE_TIME);
   sim.addOutputMonitor(new OutputMonitor<Object>() { ... });
   new Thread(sim).start();
   ```
   So, the necessary steps are: loading a YAML configuration file, create a new `Environment<Object>` from that YAML, create a  new `Simulation` as new Engine and start the simulation on a different thread, then we would be able to control the simulation via commands like `sim.play()`, `sim.stop()`, and so on; moreover, to communicate with the simulation environment we could use an `OutputMonitor` to be attached to the simulation object before starting the Thread: it provides 3 method that could be exploited to know when the Simulation starts (`initialized` method called), ends (`finished` method called) or completes a step (`stepDone` method called) and inside the last one we build the NodesDescriptor collecting all nodes and all molecules/concentrations and ready to be sent to Unity.

IMPORTANT: `sim.play();` must be called after the `init` phase in order to start the simulation. 


### How Communication Works (Unity side):

* The communication is based on the HTTP protocol and will use REST messages. It is synchronous for the first two messages that are used for initialization purpose (Unity side: one POST to send to Alchemist the number of nodes you have and the name of the program you want to run, then a GET to request all IDs of the Alchemist's nodes to assign them to Unity's ones). During this phase the communication type to be written in the json string must be 'init', while every following communication will be completely asynchronous


* After the server has sent back the previous GET response and after every `"post2getTime"` milliseconds (this time is tweakable from the inspection of the `CentralSystem`), the `GradientNodesCollector` present in the `CentralSystem` will collect every node present in the scene that has a script attached that extends `AbstractBehaviourNode` (e.g. `SmartPanel`), then it will retrieve all `GradientNode` within them (that are the Unity version of the Alchemist node), serialize through JsonUtility and then send them, with a POST request, to the server: from now on, the communication type to be written on the json string must be 'step', as expected by the server.


* After the server has sent back the POST response, a second timer will start (`"get2postTime"`, also tweakable) and, when this is over, `GradientNodesCollector` will send a GET request to retrieve the concentration of the gradient for every node. The GET response will be a Json string that respect the `NodesDescriptor` structure, since the same exact class is present in the server, so that JsonUtility can deserialize automatically the response into a C# object

### How Communication Works (NanoHTTPD/Java side):

GET and POST requests are made in order to begin and carry on the communication, it follows the explanation of steps required to the Alchemist configuration:

* a **POST** request is made to the server with a specific json object called `init`, this object carries the number of nodes Unity needs to create into Alchemist, so the server will start a new Alchemist simulation (the number of nodes is written in the YAML file)


* a **GET** request is made by Unity to the server in order to retrieve all the IDs of every node in the scene (it is a normal GET, it will provide a json object with all nodes with every information in them)


* Unity periodically sends via POST the new positions of nodes, then it retrieves via GET all updated informations (like gradient) of all nodes previously computated by Alchemist.

### How to extend the project:

Due to time, the project has been made with a very specific implementation that permits only a communication with nodes that have exactly 3 molecules (see the `GradientNode` script) and it will use only the Alchemist simulation environment described in the YAML file "gradient2.yml".

This has been done due to the limitation of the JsonUtility used for the serialization/deserialization of the messages since it doesn't know how to convert a generic C# object into the Json representation of it's real instance: if you create, for example, a List of objects called "aList" `(List<object> aList)` and then you populate "aList" with integers, booleans, etc..., the utility will see every element of the list as an object and won't be able to transform it into a Json string.

In order to generalize the communication, a generic node should be realized (instead of the `GradientNode`) that contains a collection of pairs `<molecule,concentration>` where "molecule" is a string and "concentration" is of a type T that JsonUtility knows how to serialize correctly.

Alchemist side, the same generic node must be created to maintain consistency in the communication and instead of manually set the concentration of every single molecule, according to the ones arrived from Unity, iterate the collection and automatically set the concentration of the molecule with the same name of the one present in the collection.

Moreover, the creation of Alchemist's nodes is done exploiting the `init(jsonObj)` function, which will use nodes and molecules described in the YAML file "gradient2.yml", so they will be exactly in the number and with the molecules described in that file (once known the number of nodes and molecules created in Unity, the same number and molecules must be written in the YAML file).
In order to exploit the cloning functionality it was implemented another function, `initWithClone(jsonString)`: it has to be used loading the YAML file called "gradient.yml" and it will create the number of nodes received from Unity's first POST cloning the node already present in the Environment thanks to the YAML file; it is not currently supported in the main Alchemist project.

**Side note:** to implement the server we used the NanoHTTPD library but it has a problem with the deletion of temp files it creates so, if you run the server for a while it will accumulates a bunch of these files in the temp folder and won't be able to delete them. What it can be done is to try another library (e.g. Gretty) and re-implement the server.

So, basically, next improvements will be:
- [ ] replace NanoHTTPD server with a new one (e.g. [Gretty](https://github.com/akhikhl/gretty))
- [ ] proper use of node's cloning into Alchemist (so the YAML file will contain only 1 node)
- [ ] generic molecules and nodes in Unity
- [ ] generated YAML file
- [ ] ...
