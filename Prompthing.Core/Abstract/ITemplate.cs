namespace Prompthing.Core.Abstract;

public interface ITemplate
{
    IReadOnlyList<INode> Nodes { get; }
}