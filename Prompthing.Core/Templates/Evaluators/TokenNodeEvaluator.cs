using System.Text;
using Prompthing.Core.Dataset;
using Prompthing.Core.Templates.Nodes;

namespace Prompthing.Core.Templates.Evaluators;

public class TokenNodeEvaluator : BaseEvaluator<TokenNode>
{
    private readonly StringBuilder _stringBuilder;
    private readonly Term[] _terms;

    public TokenNodeEvaluator(StringBuilder stringBuilder, Term[] terms)
    {
        _stringBuilder = stringBuilder;
        _terms = terms;
    }

    public override void Evaluate(TokenNode node)
    {
        var term = _terms.First(term => term.Key == node.Key);
        var value = term.Values[Random.Shared.Next(0, term.Values.Length)];

        _stringBuilder.Append(value.Text);
    }
}