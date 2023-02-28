namespace Prompthing.Core.Abstract;

public interface INodeFactory
{
    INode CreateNode(string nodeType, string[] arguments);
}