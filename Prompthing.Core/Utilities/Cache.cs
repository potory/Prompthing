namespace Prompthing.Core.Utilities;

public class Cache
{
    private readonly Dictionary<Type, Dictionary<string, object>> _data = new();

    public void Add<T>(T entity, string name)
    {
        var type = typeof(T);
        
        if (!_data.ContainsKey(type))
        {
            _data.Add(type, new Dictionary<string, object>());
        }
        
        _data[type].Add(name, entity);
    }

    public bool Contains<T>(string name)
    {
        var type = typeof(T);

        return _data.ContainsKey(type) && _data[type].ContainsKey(name);
    }

    public bool TryGetByName<T>(string name, out T? obj)
    {
        var type = typeof(T);
        obj = default;

        if (!_data.ContainsKey(type) || !_data[type].TryGetValue(name, out var content))
        {
            return false;
        }

        obj = (T)content;
        return true;
    }

    public T GetByName<T>(string name) => 
        (T) _data[typeof(T)][name];
}