namespace Prompthing.Core.Templates.Abstract;

public interface INodeFactory
{
    INode CreateNode(string nodeType, string[] arguments);
}