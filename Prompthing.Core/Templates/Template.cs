using Prompthing.Core.Templates.Abstract;

namespace Prompthing.Core.Templates;

public class Template : ITemplate
{
    public string? Name { get; }
    public IReadOnlyList<INode> Nodes { get; }

    public Template(string? name, List<INode> nodes)
    {
        Name = name;
        Nodes = nodes;
    }
}