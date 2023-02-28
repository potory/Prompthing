using System.Text;

namespace Prompthing.Core.Entities.Nodes;

public abstract class Node
{
    protected readonly List<Node> Children = new();

    public void Add(Node node) => 
        Children.Add(node);

    public abstract void Evaluate(StringBuilder stringBuilder);
}