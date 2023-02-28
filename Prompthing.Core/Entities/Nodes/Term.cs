using System.Text;
using Prompthing.Core.Extensions;

namespace Prompthing.Core.Entities.Nodes;

public class Term : Node
{
    private readonly Value[] _values;

    public Term(Value[] values)
    {
        _values = values;
    }

    public override void Evaluate(StringBuilder stringBuilder) => 
        stringBuilder.Append(_values.OneOfWeighted().Text);
}