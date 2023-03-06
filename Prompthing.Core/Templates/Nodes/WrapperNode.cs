using System.Text;
using Prompthing.Core.Entities;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates.Nodes;

public class WrapperNode : BasicNode
{
    private readonly BasicNode _content;
    private readonly DelayedReference<Wrapper> _wrapper;

    public Wrapper Wrapper => _wrapper.Value;

    public WrapperNode(BasicNode content, DelayedReference<Wrapper> wrapper)
    {
        _content = content;
        _wrapper = wrapper;
    }

    public override void Evaluate(StringBuilder output)
    {
        var wrapper = _wrapper.Value;
        wrapper.PreContent.Evaluate(output);
        _content.Evaluate(output);
        wrapper.PostContent.Evaluate(output);
    }
}