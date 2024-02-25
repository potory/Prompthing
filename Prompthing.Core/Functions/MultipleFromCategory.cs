using System.Text;
using Prompthing.Core.Entities;
using SonScript.Core.Extensions;
using SonScript.Core.Functions;

namespace Prompthing.Core.Functions;

public class MultipleFromCategory : Function
{
    private readonly ReferencePool _referencePool;
    private readonly List<int> _indexes;
    private readonly StringBuilder _sb;

    public MultipleFromCategory(ReferencePool referencePool)
    {
        _referencePool = referencePool;
        _indexes = new List<int>();
        _sb = new StringBuilder();
    }

    public override object Evaluate(List<object> arguments)
    {
        var name = arguments[0].ToString();
        var category = _referencePool.CreateReference<Category>(name).Value;

        int minArg = arguments.Count > 1 ? Convert.ToInt32(arguments[1]) : 1;
        int maxArg = arguments.Count > 2 ? Convert.ToInt32(arguments[2]) : category.Terms.Count;
        
        _sb.Clear();

        int count = Random.Shared.Next(minArg, maxArg + 1);

        for (int i = 0; i < category.Terms.Count; i++)
        {
            _indexes.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            var index = _indexes.OneOf();
            _sb.Append($"{category.Terms[index].Text}");
            _indexes.Remove(index);

            if (i < count - 1)
            {
                _sb.Append(", ");
            }
        }

        return _sb.ToString();
    }
}