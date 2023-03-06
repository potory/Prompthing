namespace Prompthing.Core.Abstract;

public interface IReferencePool
{
    /// <summary>
    /// Creates a new delayed reference to an object of the specified type and name.
    /// </summary>
    /// <typeparam name="T">The type of object to create a reference to.</typeparam>
    /// <param name="name">The name to give to the object reference.</param>
    /// <returns>A delayed reference to the object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the name is null.</exception>
    DelayedReference<T> CreateReference<T>(string name) where T : class;

    /// <summary>
    /// Adds an object of the specified type and name to the pool.
    /// </summary>
    /// <typeparam name="T">The type of object to add.</typeparam>
    /// <param name="name">The name to give to the object.</param>
    /// <param name="obj">The object to add to the pool.</param>
    /// <exception cref="ArgumentNullException">Thrown when the name is null.</exception>
    void AddObject<T>(string name, T obj) where T : class;
}