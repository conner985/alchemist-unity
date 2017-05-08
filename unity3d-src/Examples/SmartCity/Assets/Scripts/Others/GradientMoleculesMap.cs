using System;
using UnityEngine;

/// <summary>
/// 
/// An class representing the structure of a GradientNode molecule set
/// 
/// </summary>
[Serializable]
public class GradientMoleculesMap
{
    [SerializeField]
    private bool enabled = true;
    [SerializeField]
    private bool source = false;
    [SerializeField]
    private double data = 0.0f;

    /// <summary>
    /// 
    /// Method that allowes to get the concentration of a particular molecule (if present)
    /// 
    /// </summary>
    /// <param name="mol">The molecule name</param>
    /// <returns>The concentration requested</returns>
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
                return default(object);
        }
    }

    /// <summary>
    /// 
    /// Method to set a new concentration to a molecule (if present)
    /// 
    /// </summary>
    /// <param name="mol">The molecule to modify</param>
    /// <param name="conc">The concentration of be set</param>
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

    /// <summary>
    /// 
    /// Method used to change all molecules concentration with new ones (used for updating purpose)
    /// 
    /// </summary>
    /// <param name="newMolecules">The new GradientMoleculesMap to be set</param>
    public void SetMolecules(GradientMoleculesMap newMolecules)
    {
        enabled = (bool)newMolecules.GetMoleculeConcentration("enabled");
        source = (bool)newMolecules.GetMoleculeConcentration("source");
        data = (double)newMolecules.GetMoleculeConcentration("data");
    }
}
