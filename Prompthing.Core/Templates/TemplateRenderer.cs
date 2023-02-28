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

    public TemplateRenderer(ITemplate[] templates, Term[] terms)
    {
        _evaluators.Add(typeof(TextNode), new TextNodeEvaluator(_stringBuilder));
        _evaluators.Add(typeof(TokenNode), new TokenNodeEvaluator(_stringBuilder, terms));
        _evaluators.Add(typeof(ReferenceNode), new ReferenceNodeEvaluator(templates, this));
    }

    public ITemplateRenderer Render(ITemplate template)
    {
        foreach (var node in template.Nodes)
        {
            GetEvaluator(node).Evaluate(node);
        }

        return this;
    }

    public string Finish()
    {
        var result = _stringBuilder.ToString();
        _stringBuilder.Clear();
        return result;
    }

    private INodeEvaluator GetEvaluator(INode node)
    {
        var isEvaluatorExist = _evaluators.TryGetValue(node.GetType(), out var evaluator);

        if (!isEvaluatorExist)
        {
            throw new RenderingException($"Missing evaluator for node type '{node.GetType().Name}'.");
        }

        return evaluator!;
    }
}

public class RenderingException : Exception
{
    public RenderingException()
    {
        
    }

    public RenderingException(string message) : base(message)
    {
        
    }
}