using System.Text;
using Prompthing.Core.Templates.Basic;

namespace Prompthing.Core.Templates;

public sealed class TextNode : BasicNode
{
    private readonly string _text;

    public TextNode(string text) => 
        _text = text;

    public override void Evaluate(StringBuilder output) => 
        output.Append(_text);
}