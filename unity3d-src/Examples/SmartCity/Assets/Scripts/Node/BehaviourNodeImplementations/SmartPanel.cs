using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// 
/// Class representing an implementation of a node that lives within Unity and it will contain all necessary information for a Unity
/// node, including a GradientNode, which is a Unity representation of an Alchemist node and it will be sent to Alchemist.
/// 
/// 
/// </summary>
public class SmartPanel : AbstractBehaviourNode, ISmartPanel
{
    [SerializeField]
    private float commRadius = 6f;
    [SerializeField]
    private bool isSource = false;
    [SerializeField]
    private bool isEnabled;

    private System.Random rnd;
    private GradientNode node;
    private List<AbstractBehaviourNode> nbrHood;
    private bool systemAlarm;

    private Collider[] colliderBuffer = new Collider[1000];

    void Awake()
    {
        systemAlarm = false;
        isEnabled = true;
        rnd = new System.Random(DateTime.Now.Millisecond);
        nbrHood = new List<AbstractBehaviourNode>();
    }

    /// <summary>
    /// It will recalculate all AbstractBehaviourNode within a certain range
    /// </summary>
    void Start()
    {
        RecalculateNbrHood();
    }

    private void RecalculateNbrHood()
    {
        nbrHood.Clear();
        int buffSize = Physics.OverlapSphereNonAlloc(transform.position, commRadius, colliderBuffer);
        for (int i = 0; i < buffSize; i++)
        {
            AbstractBehaviourNode behaviourNode = colliderBuffer[i].gameObject.GetComponent<AbstractBehaviourNode>();
            if (behaviourNode != null) nbrHood.Add(behaviourNode);
        }
    }
    
    /// <summary>
    /// 
    /// It will periodically recalculate the gradient value (and render the arrow) using gradient molecules and concentrations from 
    /// the neighbourhood which are updated by Alchemist
    /// 
    /// </summary>
	void Update ()
    {

        //if (rnd.NextDouble() < 0.8f) return;
        if (rnd.NextDouble() < 0.1f) RecalculateNbrHood();
        
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
