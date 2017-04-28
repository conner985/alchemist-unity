# alchemist-unity

GOAL OF THIS BRANCH: create a generic node (that contains a generic collection of pairs <molecule,concentration>, molecule as string and concentration as generic object) and be able to serialize it as a Json, using the JsonUtility that Unity provides.
MAIN PROBLEM: JsonUtility doesn't know how to serialize a generic object, while Gson in Java knows that - How to standardize the communication between Unity and Java?

an integration of the Unity3D game engine with Alchemist simulator

- Alchemist: https://github.com/AlchemistSimulator/Alchemist
- Unity3D: https://unity3d.com/

What is done:
- Unity-Alchemist communication through REST, sending JSON files containing node's positions
- Alchemist-Unity communication through REST, sending JSON files containing updates for node's molecules

TODO:
- Tests: reactivity, performance
- Updates timing calibration (Unity sends asynchronous updates every second and waits another second before asking Alchemist for updates)

Directories:
- java-src/alchemist-unity : contains the Eclipse project of the REST server that handles the communication between Unity and Alchemist
- unity3d-src/ISSAC_Project : contains the entire Unity3D project folder
