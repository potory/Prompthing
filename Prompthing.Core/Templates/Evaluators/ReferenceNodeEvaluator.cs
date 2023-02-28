using Prompthing.Core.Templates.Abstract;
using Prompthing.Core.Templates.Nodes;

namespace Prompthing.Core.Templates.Evaluators;

public class ReferenceNodeEvaluator : BaseEvaluator<ReferenceNode>
{
    private readonly ITemplate[] _templates;
    private readonly TemplateRenderer _renderer;

    public ReferenceNodeEvaluator(ITemplate[] templates, TemplateRenderer renderer)
    {
        _templates = templates;
        _renderer = renderer;
    }
    public override void Evaluate(ReferenceNode node)
    {
        var template = _templates.Single(x => x.Name == node.Name);
        _renderer.Render(template);
    }
}