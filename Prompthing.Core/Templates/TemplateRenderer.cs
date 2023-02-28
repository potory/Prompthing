using System.Text;
using Prompthing.Core.Dataset;
using Prompthing.Core.Templates.Abstract;
using Prompthing.Core.Templates.Evaluators;
using Prompthing.Core.Templates.Nodes;

namespace Prompthing.Core.Templates;

public class TemplateRenderer : ITemplateRenderer
{
    private readonly StringBuilder _stringBuilder = new();
    private readonly Dictionary<Type, INodeEvaluator> _evaluators = new();

    public TemplateRenderer(Term[] terms)
    {
        _evaluators.Add(typeof(TextNode), new TextNodeEvaluator(_stringBuilder));
        _evaluators.Add(typeof(TokenNode), new TokenNodeEvaluator(_stringBuilder, terms));
    }

    public string Render(ITemplate template)
    {
        foreach (var node in template.Nodes)
        {
            var evaluator = _evaluators[node.GetType()];
            evaluator.Evaluate(node);
        }

        return _stringBuilder.ToString();
    }
}