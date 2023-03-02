using System.Text;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates.Nodes;

public class LoopNode : BasicNode
{
    private readonly int _iterationsMin;
    private readonly int _iterationsMax;
    private readonly BasicNode _node;

    public LoopNode(int iterationsMin, int iterationsMax, BasicNode node)
    {
        _iterationsMin = iterationsMin;
        _iterationsMax = iterationsMax;
        _node = node;
    }

    public override void Evaluate(StringBuilder output)
    {
        var iterations = Random.Shared.Next(_iterationsMin, _iterationsMax+1);
        
        for (int i = 0; i < iterations; i++)
        { 
            _node.Evaluate(output);
        }
    }
}