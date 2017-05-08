# alchemist-unity

GOAL OF THIS BRANCH: create a generic node (that contains a generic collection of pairs <molecule,concentration>, molecule as string and concentration as generic object) and be able to serialize it as a Json, using the JsonUtility that Unity provides.

MAIN PROBLEM: JsonUtility doesn't know how to serialize a generic object, while Gson in Java knows that - How to standardize the communication between Unity and Java?

For other information read the README.md into tha master branch
