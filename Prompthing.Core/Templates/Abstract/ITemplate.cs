namespace Prompthing.Core.Templates.Abstract;

public interface ITemplate
{
    string Name { get; }
    IReadOnlyList<INode> Nodes { get; }
}