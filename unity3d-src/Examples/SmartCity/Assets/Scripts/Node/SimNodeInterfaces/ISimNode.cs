/// <summary>
/// 
/// Interface for the Unity representation of an Alchemist node
/// 
/// </summary>
public interface ISimNode
{
    /// <summary>
    /// 
    /// Returns the 2D position of the node
    /// 
    /// </summary>
    /// <returns>The position of the node</returns>
    NodePosition2D GetPosition();

    /// <summary>
    /// 
    /// Set the position of the node
    /// 
    /// </summary>
    /// <param name="pos">The new position of the node</param>
    void SetPosition(NodePosition2D pos);

    /// <summary>
    /// 
    /// Returns the ID of the node
    /// 
    /// </summary>
    /// <returns>The ID of the node</returns>
    int GetID();

    /// <summary>
    /// 
    /// Gets all molecules of the node
    /// 
    /// </summary>
    /// <returns>A GradientMoleculesMap with all molecules and concentrations of the node</returns>
    GradientMoleculesMap GetMolecules();

    /// <summary>
    /// 
    /// Updates all molecules and respective concentrations with new values
    /// 
    /// </summary>
    /// <param name="molecules">The new GradientMoleculesMap to be set</param>
    void SetMolecules(GradientMoleculesMap molecules);

    /// <summary>
    /// 
    /// Modifies the concentration of a specific molecule (if present)
    /// 
    /// </summary>
    /// <param name="mol">The molecule to modify</param>
    /// <param name="conc">The new concentration</param>
    void SetMolecule(string mol, object conc);
}
