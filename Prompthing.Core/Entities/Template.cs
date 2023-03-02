using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Entities;

public class Template
{
    public BasicNode Node { get; }
    public string Name { get; }
    public bool IsSnippet { get; }

    public Template(string name, BasicNode node, bool isSnippet = false)
    {
        Name = name;
        IsSnippet = isSnippet;
        Node = node;
    }
}