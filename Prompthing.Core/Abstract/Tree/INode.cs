namespace Prompthing.Core.Abstract.Tree;

/// <summary>
/// This interface defines the basic functionality of a node in a tree structure.
/// </summary>
/// <typeparam name="TOutput">The type of output associated with the node system.</typeparam>
public interface INode<TOutput>
{
    /// <summary>
    /// Evaluates the node.
    /// </summary>
    void Evaluate(TOutput output);
    
    /// <summary>
    /// Gets the parent container of the node.
    /// </summary>
    /// <returns>The parent container of the node.</returns>
    IContainer<TOutput>? Parent { get; }

    /// <summary>
    /// Sets the parent container of the node.
    /// </summary>
    /// <param name="parent">The parent container to set.</param>
    void SetParent(IContainer<TOutput>? parent);
}