using UnityEngine;

[System.Serializable]
public class NodePosition2D
{

    [SerializeField]
    private double posx, posz;

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
