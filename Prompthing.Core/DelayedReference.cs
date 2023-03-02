namespace Prompthing.Core;

public class DelayedReference
{
    private readonly Func<object> _source;

    private object? _value;

    public DelayedReference(Func<object> source, object? value = null)
    {
        _source = source;
        _value = value;
    }

    public object Value => _value ??= _source();

    public void ForceResolve() => _value = _source();
}

public class DelayedReference<T> : DelayedReference where T: class
{
    public DelayedReference(Func<T> source, T? value = null) : base(source, value)
    {
        
    }

    public new T Value => (T) base.Value;
}