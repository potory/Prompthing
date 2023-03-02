using System.Text;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates.Nodes;

public class RandomNode : BasicNode
{
    private readonly BasicNode[] _nodes;

    public RandomNode(params BasicNode[] nodes)
    {
        _nodes = nodes;
    }

    public override void Evaluate(StringBuilder output)
    {
        var index = Random.Shared.Next(0, _nodes.Length);
        _nodes[index].Evaluate(output);
    }
}