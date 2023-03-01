namespace Prompthing.Core.Abstract;

/// <summary>
/// This interface defines the basic functionality of a node in a tree structure.
/// </summary>
/// <typeparam name="TData">The type of data associated with the node.</typeparam>
public interface INode<TData>
{
    /// <summary>
    /// Evaluates the node.
    /// </summary>
    void Evaluate();
    
    /// <summary>
    /// Gets the parent container of the node.
    /// </summary>
    /// <returns>The parent container of the node.</returns>
    IContainer<TData> Parent { get; }

    /// <summary>
    /// Sets the parent container of the node.
    /// </summary>
    /// <param name="parent">The parent container to set.</param>
    void SetParent(IContainer<TData> parent);

    /// <summary>
    /// Sets the data associated with the node.
    /// </summary>
    /// <param name="data">The data to set.</param>
    void SetData(TData data);
}