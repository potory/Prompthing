using System.Text;
using Prompthing.Core.Entities;
using SonScript.Core.Attributes;
using SonScript.Core.Functions;

namespace Prompthing.Core.Functions;

public sealed class TemplateFunction : Function
{
    private readonly StringBuilder _stringBuilder;
    private readonly ReferencePool _referencePool;

    public TemplateFunction(ReferencePool referencePool)
    {
        _referencePool = referencePool;
        _stringBuilder = new StringBuilder();
    }

    public override object Evaluate(List<object> arguments)
    {
        _stringBuilder.Clear();

        var name = arguments[0].ToString();
        var template = _referencePool.CreateReference<Template>(name);
        template.Value.Node.Evaluate(_stringBuilder);

        return _stringBuilder.ToString();
    }
}