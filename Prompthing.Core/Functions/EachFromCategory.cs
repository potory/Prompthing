using Prompthing.Core.Entities;
using SonScript.Core.Functions;

namespace Prompthing.Core.Functions;

public class EachFromCategory : Function
{
    private readonly ReferencePool _referencePool;
    private readonly Dictionary<string, int> _indexes = new();

    public EachFromCategory(ReferencePool referencePool)
    {
        _referencePool = referencePool;
    }
    
    public override object Evaluate(List<object> arguments)
    {
        var name = arguments[0].ToString();
        
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException(nameof(name));
        }
        
        var category = _referencePool.CreateReference<Category>(name).Value;

        _indexes.TryAdd(name, 0);
        var value = category.Terms[_indexes[name]].Text;
        _indexes[name]++;

        if (_indexes[name] >= category.Terms.Count)
        {
            _indexes[name] = 0;
        }

        return value;
    }
}