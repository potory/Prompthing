using System.Text;
using Prompthing.Core.Templates.Nodes;

namespace Prompthing.Core.Templates.Evaluators;

public class TextNodeEvaluator : BaseEvaluator<TextNode>
{
    private readonly StringBuilder _stringBuilder;

    public TextNodeEvaluator(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public override void Evaluate(TextNode node) => _stringBuilder.Append(node.Text);
}