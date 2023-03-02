using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Entities;

public class Loop
{
    public string Name { get; }
    public int Iterations { get; }
    public BasicNode Nodes { get; }

    public Loop(string name, int iterations, BasicNode nodes)
    {
        Name = name;
        Iterations = iterations;
        Nodes = nodes;
    }
}