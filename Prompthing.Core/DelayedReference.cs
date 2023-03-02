namespace Prompthing.Core;

/// <summary>
/// Represents a delayed reference to an object.
/// </summary>
public class DelayedReference
{
    private readonly Func<object> _source;

    private object? _value;

    /// <summary>
    /// Initializes a new instance of the DelayedReference class with the specified source function and optional initial value.
    /// </summary>
    /// <param name="source">The function that returns the referenced object.</param>
    /// <param name="value">The initial value of the reference, if available.</param>
    public DelayedReference(Func<object> source, object? value = null)
    {
        _source = source;
        _value = value;
    }

    /// <summary>
    /// Gets the value of the referenced object. If the value has not been resolved yet, it is resolved using the source function.
    /// </summary>
    public object Value => _value ??= _source();

    /// <summary>
    /// Forces the reference to resolve the value using the source function, even if it has already been resolved.
    /// </summary>
    public void ForceResolve() => _value = _source();
}

/// <summary>
/// Represents a delayed reference to an object of a specific type.
/// </summary>
/// <typeparam name="T">The type of object referenced by the delayed reference.</typeparam>
public class DelayedReference<T> : DelayedReference where T : class
{
    /// <summary>
    /// Initializes a new instance of the DelayedReference class with the specified source function and optional initial value.
    /// </summary>
    /// <param name="source">The function that returns the referenced object.</param>
    /// <param name="value">The initial value of the reference, if available.</param>
    public DelayedReference(Func<T> source, T? value = null) : base(source, value)
    {
    }

    /// <summary>
    /// Gets the value of the referenced object. If the value has not been resolved yet, it is resolved using the source function.
    /// </summary>
    public new T Value => (T) base.Value;
}
