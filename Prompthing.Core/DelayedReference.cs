namespace Prompthing.Core;

/// <summary>
/// A delayed reference to an object. The object is not created until the Value property is accessed.
/// </summary>
public class DelayedReference
{
    private readonly Func<object> _source;

    private readonly bool _useCache;

    private object? _value;

    /// <summary>
    /// Initializes a new instance of the DelayedReference class.
    /// </summary>
    /// <param name="source">The function that creates the referenced object.</param>
    /// <param name="useCache">Whether to cache the referenced object once it is created. Default is true.</param>
    /// <param name="value">A pre-created value for the referenced object. If this is not null, the source function will not be called.</param>
    public DelayedReference(Func<object> source, bool useCache = true, object? value = null)
    {
        _source = source;
        _useCache = useCache;
        _value = value;
    }

    /// <summary>
    /// Gets the referenced object. If the object has not been created yet, it will be created now.
    /// </summary>
    public object Value 
    {
        get 
        {
            if (_useCache && _value != null)
            {
                return _value;
            }
            else
            {
                _value = _source();
                return _value;
            }
        }
    }

    /// <summary>
    /// Forces the referenced object to be created, even if it has already been created.
    /// </summary>
    public void ForceResolve() 
    {
        _value = _source();
    }
}

/// <summary>
/// A strongly-typed delayed reference to an object of type T.
/// </summary>
/// <typeparam name="T">The type of the referenced object.</typeparam>
public class DelayedReference<T> : DelayedReference where T: class
{
    /// <summary>
    /// Initializes a new instance of the DelayedReference class.
    /// </summary>
    /// <param name="source">The function that creates the referenced object.</param>
    /// <param name="useCache">Whether to cache the referenced object once it is created. Default is true.</param>
    /// <param name="value">A pre-created value for the referenced object. If this is not null, the source function will not be called.</param>
    public DelayedReference(Func<T> source, bool useCache = true, T? value = null) : base(source, useCache, value)
    {
        
    }

    /// <summary>
    /// Gets the referenced object of type T. If the object has not been created yet, it will be created now.
    /// </summary>
    public new T Value => (T) base.Value;
}
