using UnityEngine;
using System;
using System.Collections.Generic;

public class SmartPanel : AbstractBehaviourNode, ISmartPanel
{
    [SerializeField]
    private double gradient = 0.0f;
    [SerializeField]
    private string closestPanel = "";

    [SerializeField]
    private float commRadius = 6f;
    [SerializeField]
    private bool isSource = false;

    private System.Random rnd;

    [SerializeField]
    private bool isEnabled;

    private GradientNode node;
    
    private bool systemAlarm;

    private List<AbstractBehaviourNode> nbrHood;

    void Awake()
    {
        systemAlarm = false;
        isEnabled = true;
        rnd = new System.Random(DateTime.Now.Millisecond);
        nbrHood = new List<AbstractBehaviourNode>();
    }

    void Start()
    {
        RecalculateNbrHood();
    }

    private void RecalculateNbrHood()
    {
        nbrHood.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, commRadius);
        foreach (Collider collider in colliders)
        {
            AbstractBehaviourNode behaviourNode = collider.gameObject.GetComponent<AbstractBehaviourNode>();
            if (behaviourNode != null) nbrHood.Add(behaviourNode);
        }
    }
    
	void Update ()
    {

        //if (rnd.NextDouble() > 1f) return;
        RecalculateNbrHood();

        gradient = (double)node.GetMolecules().GetMoleculeConcentration("data");
        
        foreach (AbstractBehaviourNode nbr in nbrHood)
        {
            ISimNode simNode = nbr.GetNode();
            if (simNode.GetType().Equals(typeof(GradientNode)))
            {
                GradientNode nbrGradientNode = simNode as GradientNode;
                if ( (double) nbrGradientNode.GetMolecules().GetMoleculeConcentration("data") < (double) node.GetMolecules().GetMoleculeConcentration("data"))
                {
                    if (systemAlarm && transform.GetComponentInChildren<DisplayScript>())
                    {
                        ClearDisplay();
                        transform.GetComponentInChildren<DisplayScript>().RenderArrow(nbrGradientNode.GetGameObject().transform.position);
                    }
                }
            }
        }
        
    }
    
    public void SwitchAlarm()
    {
        isEnabled = !isEnabled;
        node.SetMolecule("enabled", isEnabled);

    }

    public void RenderDisplay(byte[] image)
    {
        //TODO: Render Ads
    }

    public void ClearDisplay()
    {
        DisplayScript ds = transform.GetComponentInChildren<DisplayScript>();
        if (ds != null) ds.Clear();
    }

    public void ReceiveInfo(object info)
    {
        switch (info.ToString())
        {
            case "SET":
                systemAlarm = true;
                break;
            case "UNSET":
                systemAlarm = false;
                ClearDisplay();
                break;
        }
    }


    #region NODE

    public override GradientNode GetNode()
    {
        node.SetPosition(new NodePosition2D(transform.position.x, transform.position.z));
        return node;
    }

    public override void CreateNode(int id, SimNodeTypes.type type)
    {
        if (type.Equals(SimNodeTypes.type.GRADIENT))
        {
            node = new GradientNode(id, new NodePosition2D(transform.position.x, transform.position.z), this);
            node.SetMolecule("source", isSource);
            node.SetMolecule("enabled", enabled);
        }
    }

    #endregion
}
