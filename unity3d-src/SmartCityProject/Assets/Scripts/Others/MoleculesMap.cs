using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoleculesMap
{
    [SerializeField]
    private List<string> keys = new List<string>();
    [SerializeField]
    private List<bool> values = new List<bool>();

    public void AddMolecule(string mol, bool conc)
    {
        if (keys.Contains(mol))
        {
            int index = keys.IndexOf(mol);
            values[index] = conc;
        }
        else
        {
            keys.Add(mol);
            values.Add(conc);
        }
    }

    public bool GetConcentration(string mol)
    {
        if (keys.Contains(mol))
        {
            return values[keys.IndexOf(mol)];
        }
        else
        {
            return false;
        }
    }
}
