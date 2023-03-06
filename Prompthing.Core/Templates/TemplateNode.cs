using System.Text;
using Prompthing.Core.Entities;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates;

public class TemplateNode : BasicNode
{
    private readonly DelayedReference<Template> _template;

    public Template Template => _template.Value;
    
    public TemplateNode(DelayedReference<Template> template)
    {
        _template = template;
    }

    public override void Evaluate(StringBuilder output) => 
        _template.Value.Node.Evaluate(output);
}