using System.Collections.Concurrent;

namespace Prompthing.Core;

public class ReferencePool
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, DelayedReference>> _references = new();
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _objects = new();

    public DelayedReference<T> CreateReference<T>(string name) where T: class
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        var dictionary = GetOrCreateReferenceDictionary<T>();
        var reference = GetOrCreateReference<T>(dictionary, name);

        return reference;
    }
    
    public void AddObject<T>(string name, T obj) where T: class
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        var dictionary = GetOrCreateObjectDictionary<T>();
        dictionary[name] = obj;
    }

    private ConcurrentDictionary<string, DelayedReference> GetOrCreateReferenceDictionary<T>() where T: class
    {
        var type = typeof(T);

        return _references.GetOrAdd(type, _ => new ConcurrentDictionary<string, DelayedReference>());
    }

    private DelayedReference<T> GetOrCreateReference<T>(ConcurrentDictionary<string, DelayedReference> dict, string name) where T: class
    {
        return (DelayedReference<T>) dict.GetOrAdd(name, _ => new DelayedReference<T>(() => GetOrCreateObject<T>(name)));
    }

    private T GetOrCreateObject<T>(string name) where T: class
    {
        var type = typeof(T);
        var objects = GetOrCreateObjectDictionary<T>();

        if (!objects.TryGetValue(name, out var value))
        {
            throw new DelayedReferenceException($"Object not found for {type.Name} with name {name}");
        }

        return (T)value!;
    }

    private ConcurrentDictionary<string, object> GetOrCreateObjectDictionary<T>() where T: class
    {
        var type = typeof(T);

        return _objects.GetOrAdd(type, _ => new ConcurrentDictionary<string, object>());
    }
}


public class DelayedReferenceException : Exception
{
    public DelayedReferenceException()
    {
        
    }

    public DelayedReferenceException(string message) : base(message)
    {
        
    }
}