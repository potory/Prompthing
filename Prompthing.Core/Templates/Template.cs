using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates;

public class Template : ContainerNode
{
    public string Name { get; }

    public Template(string name) => 
        Name = name;
}