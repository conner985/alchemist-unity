using UnityEngine;
using System;
using System.Collections.Generic;

public class SmartPanel : AbstractBehaviourNode, ISmartPanel
{

    [SerializeField]
    private float commRadius = 6f;
    [SerializeField]
    private bool isSource = false;

    private System.Random rnd;

    private bool isEnabled;

    private GenericNode node;
    
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, commRadius);
        foreach (Collider collider in colliders)
        {
            AbstractBehaviourNode behaviourNode = collider.gameObject.GetComponent<AbstractBehaviourNode>();
            if (behaviourNode != null) nbrHood.Add(behaviourNode);
        }
    }
    
	void Update ()
    {

        if (rnd.NextDouble() > 0.4f) return;
        if (rnd.NextDouble() < 0.1f) RecalculateNbrHood();

        GenericNode myGradientNode = node as GenericNode;
        foreach (AbstractBehaviourNode nbr in nbrHood)
        {
            ISimNode simNode = nbr.GetNode();
            if (simNode.GetType().Equals(typeof(GenericNode)))
            {
                GenericNode gradientNode = simNode as GenericNode;
                if (Convert.ToDouble(gradientNode.GetMolecules().GetConcentration("data")) < Convert.ToDouble(myGradientNode.GetMolecules().GetConcentration("data")))
                {
                    if (systemAlarm && transform.GetComponentInChildren<DisplayScript>())
                    {
                        transform.GetComponentInChildren<DisplayScript>().RenderArrow(GetComponent<Collider>().transform.position);
                    }
                }
            }
        }
        
    }
    
    public void SwitchAlarm()
    {
        isEnabled = !isEnabled;
        node.AddMolecule("enabled", isEnabled);

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

    public override GenericNode GetNode()
    {
        node.SetPosition(new NodePosition2D(transform.position.x, transform.position.z));
        return node;
    }

    public override void CreateNode(int id, SimNodeTypes.type type)
    {
        if (type.Equals(SimNodeTypes.type.GRADIENT))
        {
            node = new GenericNode(id, new NodePosition2D(transform.position.x, transform.position.z));
            node.AddMolecule("source", isSource);
            node.AddMolecule("enabled", enabled);
        }
    }

    #endregion
}
