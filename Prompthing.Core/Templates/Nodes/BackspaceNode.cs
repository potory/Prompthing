using System.Text;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates.Nodes;

public class BackspaceNode : BasicNode
{
    private readonly int _count;

    public BackspaceNode(int count)
    {
        _count = count;
    }
    public override void Evaluate(StringBuilder output) => 
        output.Length -= _count;
}