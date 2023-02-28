using Prompthing.Core.Abstract;

namespace Prompthing.Core.Nodes;

public class ReferenceNode : INode
{
    public string Name { get; }

    public ReferenceNode(string name)
    {
        Name = name;
    }
}