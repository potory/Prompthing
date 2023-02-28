namespace Prompthing.Core.Templates.Abstract;

public interface ITemplate
{
    IReadOnlyList<INode> Nodes { get; }
}