/// <summary>
/// 
/// An interface representing the structure of a generic molecule set
/// 
/// </summary>
public interface IMoleculesMap
{
    /// <summary>
    /// 
    /// Method to set a new concentration to a molecule (if present)
    /// 
    /// </summary>
    /// <param name="mol">The molecule to set</param>
    /// <param name="conc">The concentration to set</param>
    void SetMolecule(string mol, object conc);
    /// <summary>
    /// 
    /// Method that allowes to get the concentration of a particular molecule (if present)
    /// 
    /// </summary>
    /// <param name="mol">The molecule to modify</param>
    /// <returns></returns>
    object GetMoleculeConcentration(string mol);
    /// <summary>
    /// 
    /// Set all concentrations with new values
    /// 
    /// </summary>
    /// <param name="molecules">The map with new concentrations</param>
    void SetMolecules(IMoleculesMap molecules);
    /// <summary>
    /// 
    /// Adds a new molecule to the set
    /// 
    /// </summary>
    /// <param name="mol">The name of the molecule</param>
    /// <param name="conc">he concentration</param>
    void AddMolecule(string mol, object conc);
}
