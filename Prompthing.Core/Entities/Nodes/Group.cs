using System.Text;

namespace Prompthing.Core.Entities.Nodes;

public sealed class Group : Node
{
    public override void Evaluate(StringBuilder stringBuilder)
    {
        foreach (var child in Children)
        {
            child.Evaluate(stringBuilder);
        }
    }
}