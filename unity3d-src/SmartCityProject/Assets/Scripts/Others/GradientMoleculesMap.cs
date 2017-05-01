using System;
using UnityEngine;

[Serializable]
public class GradientMoleculesMap
{
    [SerializeField]
    private bool enabled = true;
    [SerializeField]
    private bool source = false;
    [SerializeField]
    private double data = 0.0f;

    public object GetMoleculeConcentration(string mol)
    {
        switch (mol)
        {
            case "enabled":
                return enabled;
            case "source":
                return source;
            case "data":
                return data;
            default:
                return null;
        }
    }

    public void SetMolecule(string mol, object conc)
    {
        switch(mol)
        {
            case "enabled":
                if (conc.GetType().Equals(typeof(bool)))
                {
                    enabled = (bool)conc;
                }
                else
                {
                    Debug.Log("concentration of enabled must be a bool type!");
                }
                break;
            case "source":
                if (conc.GetType().Equals(typeof(bool)))
                {
                    source = (bool)conc;
                }
                else
                {
                    Debug.Log("concentration of source must be a bool type!");
                }
                break;
            case "data":
                if (conc.GetType().Equals(typeof(double)))
                {
                    data = (double)conc;
                }
                else
                {
                    Debug.Log("concentration of data must be a double type!");
                }
                break;
        }
    }

    public void SetMolecules(GradientMoleculesMap newMolecules)
    {
        enabled = (bool)newMolecules.GetMoleculeConcentration("enabled");
        source = (bool)newMolecules.GetMoleculeConcentration("source");
        data = (double)newMolecules.GetMoleculeConcentration("data");
    }
}
