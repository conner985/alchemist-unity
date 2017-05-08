using UnityEngine;

/// <summary>
/// 
/// Class representing the structure of the 'init' step of the communication with Alchemist: it will contain the type
/// 
/// </summary>
[System.Serializable]
class InitComm
{
    /// <summary>
    /// The type of the communication (SHOULD BE 'init')
    /// </summary>
    [SerializeField]
    private string type;
    /// <summary>
    /// The number of the nodes that Alchemist must create
    /// </summary>
    [SerializeField]
    private int nNodes;
    /// <summary>
    /// The program type tha Alchemist has to load
    /// </summary>
    [SerializeField]
    private string progType;
    
    public InitComm(string type, int nNodes, string progType)
    {
        this.type = type;
        this.nNodes = nNodes;
        this.progType = progType;
    }

    public string GetCommType()
    {
        return type;
    }

    public int GetNumNodes()
    {
        return nNodes;
    }

    public string GetProgType()
    {
        return progType;
    }
}
