using System.Text;

namespace Prompthing.Core.Entities.Nodes;

public class Text : Node
{
    private readonly string _text;

    public Text(string text)
    {
        _text = text;
    }

    public override void Evaluate(StringBuilder stringBuilder) => 
        stringBuilder.Append(_text);
}