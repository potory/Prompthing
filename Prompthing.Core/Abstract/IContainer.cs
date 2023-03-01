namespace Prompthing.Core.Abstract;

/// <summary>
/// This interface defines the functionality of a container node in a tree structure.
/// </summary>
/// <typeparam name="TData">The type of data associated with the container.</typeparam>
public interface IContainer<TData> : INode<TData>
{
    /// <summary>
    /// Adds a child node to the container.
    /// </summary>
    /// <param name="child">The child node to add.</param>
    void AddChild(INode<TData> child);

    /// <summary>
    /// Removes a child node from the container.
    /// </summary>
    /// <param name="child">The child node to remove.</param>
    void RemoveChild(INode<TData> child);

    /// <summary>
    /// Returns an enumeration of the child nodes in the container.
    /// </summary>
    /// <returns>An enumeration of the child nodes in the container.</returns>
    IEnumerable<INode<TData>> GetChildren();

    /// <summary>
    /// Clears all child nodes from the container.
    /// </summary>
    void ClearChildren();

    /// <summary>
    /// Determines if a child node is in the container.
    /// </summary>
    /// <param name="child">The child node to search for.</param>
    /// <returns>True if the child node is in the container, false otherwise.</returns>
    bool HasChild(INode<TData> child);
    
    /// <summary>
    /// Gets the number of child nodes in the container.
    /// </summary>
    int ChildCount { get; }
}