using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Entities;

public class Template
{
    public BasicNode Node { get; }
    public string Name { get; }

    public Template(string name, BasicNode node)
    {
        Name = name;
        Node = node;
    }
}