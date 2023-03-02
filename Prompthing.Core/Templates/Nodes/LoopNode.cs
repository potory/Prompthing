using System.Text;
using Prompthing.Core.Entities;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates.Nodes;

public class LoopNode : BasicNode
{
    private readonly DelayedReference<Loop> _loop;

    public LoopNode(DelayedReference<Loop> loop)
    {
        _loop = loop;
    }

    public override void Evaluate(StringBuilder output)
    {
        for (int i = 0; i < _loop.Value.Iterations; i++)
        { 
            _loop.Value.Nodes.Evaluate(output);
        }
    }
}