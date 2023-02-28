using Prompthing.Core.Abstract;

namespace Prompthing.Core;

public class Template : ITemplate
{
    private readonly List<INode> _nodes;

    public IReadOnlyList<INode> Nodes => _nodes;

    public Template(List<INode> nodes)
    {
        _nodes = nodes;
    }
}