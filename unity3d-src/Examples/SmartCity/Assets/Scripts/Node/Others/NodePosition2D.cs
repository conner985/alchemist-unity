using UnityEngine;

/// <summary>
/// 
/// Class containing the Unity 2D position of a GradientNode, to be serialized and sent to Alchemist
/// 
/// </summary>
[System.Serializable]
public class NodePosition2D
{

    [SerializeField]
    private double posx, posz;

    /// <summary>
    /// 
    /// Constructor: it will build a NodePosition2D with the specified x and z coordinates (y are not used, obviously)
    /// 
    /// </summary>
    /// <param name="posx">X position</param>
    /// <param name="posz">Y position</param>
    public NodePosition2D(double posx, double posz)
    {
        this.posx = posx;
        this.posz = posz;
    }

    public double GetPosx()
    {
        return posx;
    }

    public double GetPosz()
    {
        return posz;
    }

    public void SetPosx(double posx)
    {
        this.posx = posx;
    }

    public void SetPosz(double posz)
    {
        this.posz = posz;
    }

}
