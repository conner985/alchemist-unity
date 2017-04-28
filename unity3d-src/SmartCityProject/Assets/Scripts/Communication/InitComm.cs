using UnityEngine;

[System.Serializable]
class InitComm
{
    [SerializeField]
    private string type;
    [SerializeField]
    private int nNodes;
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
