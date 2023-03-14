using System.Collections.Concurrent;
using Prompthing.Core.Abstract;

namespace Prompthing.Core;

/// <summary>
/// A thread-safe pool of delayed references to objects.
/// </summary>
public class ReferencePool : IReferencePool
{
    // Dictionary of dictionaries that store delayed references to objects of a given type
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, DelayedReference>> _references = new();

    // Dictionary of dictionaries that store actual objects of a given type
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _objects = new();

    /// <summary>
    /// Creates a new delayed reference to an object of the specified type and name.
    /// </summary>
    /// <typeparam name="T">The type of object to create a reference to.</typeparam>
    /// <param name="name">The name to give to the object reference.</param>
    /// <returns>A delayed reference to the object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the name is null.</exception>
    public DelayedReference<T> CreateReference<T>(string name) where T : class
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        var dictionary = GetOrCreateReferenceDictionary<T>();
        var reference = GetOrCreateReference<T>(dictionary, name);

        return reference;
    }

    /// <summary>
    /// Adds an object of the specified type and name to the pool.
    /// </summary>
    /// <typeparam name="T">The type of object to add.</typeparam>
    /// <param name="name">The name to give to the object.</param>
    /// <param name="obj">The object to add to the pool.</param>
    /// <exception cref="ArgumentNullException">Thrown when the name is null.</exception>
    public void AddObject<T>(string name, T obj) where T : class
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        var dictionary = GetOrCreateObjectDictionary<T>();
        dictionary[name] = obj;
    }

    /// <summary>
    /// Gets or creates the dictionary that stores delayed references to objects of a given type.
    /// </summary>
    /// <typeparam name="T">The type of object to get or create the reference dictionary for.</typeparam>
    /// <returns>The dictionary that stores delayed references to objects of the specified type.</returns>
    private ConcurrentDictionary<string, DelayedReference> GetOrCreateReferenceDictionary<T>() where T : class
    {
        var type = typeof(T);

        return _references.GetOrAdd(type, _ => new ConcurrentDictionary<string, DelayedReference>());
    }

    /// <summary>
    /// Gets or creates a delayed reference to an object of the specified type and name.
    /// </summary>
    /// <typeparam name="T">The type of object to get or create a reference to.</typeparam>
    /// <param name="dict">The dictionary to get or create the reference in.</param>
    /// <param name="name">The name to give to the object reference.</param>
    /// <returns>A delayed reference to the object.</returns>
    private DelayedReference<T> GetOrCreateReference<T>(ConcurrentDictionary<string, DelayedReference> dict, string name) where T : class
    {
        return (DelayedReference<T>)dict.GetOrAdd(name, _ => new DelayedReference<T>(() => GetOrCreateObject<T>(name)));
    }

    /// <summary>
    /// Gets an object with the specified name and type from the object dictionary, or creates it if it does not exist.
    /// </summary>
    /// <typeparam name="T">The type of object to get or create.</typeparam>
    /// <param name="name">The name of the object to get or create.</param>
    /// <returns>The object with the specified name and type.</returns>
    /// <exception cref="DelayedReferenceException">Thrown if an object with the specified name and type does not exist.</exception>
    private T GetOrCreateObject<T>(string name) where T : class
    {
        var type = typeof(T);
        var objects = GetOrCreateObjectDictionary<T>();

        if (!objects.TryGetValue(name, out var value))
        {
            throw new DelayedReferenceException($"Object not found for {type.Name} with name {name}");
        }

        return (T)value;
    }

    /// <summary>
    /// Returns or creates a dictionary for holding objects of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of object to create the dictionary for.</typeparam>
    /// <returns>A dictionary that holds objects of the specified type.</returns>
    private ConcurrentDictionary<string, object> GetOrCreateObjectDictionary<T>() where T : class
    {
        var type = typeof(T);

        return _objects.GetOrAdd(type, _ => new ConcurrentDictionary<string, object>());
    }

    public void Clear()
    {
        _references.Clear();
        _objects.Clear();
    }
}


/// <summary>
/// Represents an exception that is thrown when a delayed reference cannot find an object with the specified name and type.
/// </summary>
public class DelayedReferenceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the DelayedReferenceException class.
    /// </summary>
    public DelayedReferenceException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the DelayedReferenceException class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DelayedReferenceException(string message) : base(message)
    {
    }
}