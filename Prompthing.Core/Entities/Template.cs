using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Entities;

public class Template
{
    private readonly string _name;
    private readonly BasicNode _node;

    public BasicNode Node => _node;

    public Template(string name, BasicNode node)
    {
        _name = name;
        _node = node;
    }
}