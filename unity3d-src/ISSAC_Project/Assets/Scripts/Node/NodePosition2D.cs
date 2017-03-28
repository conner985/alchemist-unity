using UnityEngine;

[System.Serializable]
public class NodePosition2D
{

    [SerializeField]
    private float posx, posz;

    public NodePosition2D(float posx, float posz)
    {
        this.posx = posx;
        this.posz = posz;
    }

    public float GetPosx()
    {
        return posx;
    }

    public float GetPosz()
    {
        return posz;
    }

    public void SetPosx(float posx)
    {
        this.posx = posx;
    }

    public void SetPosz(float posz)
    {
        this.posz = posz;
    }

}
