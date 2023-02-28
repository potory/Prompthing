using Prompthing.Core.Templates.Abstract;

namespace Prompthing.Core.Templates.Nodes;

public class ReferenceNode : INode
{
    public string Name { get; }

    public ReferenceNode(string name)
    {
        Name = name;
    }
}