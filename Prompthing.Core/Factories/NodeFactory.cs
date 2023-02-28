using Prompthing.Core.Abstract;
using Prompthing.Core.Nodes;

namespace Prompthing.Core.Factories;

public enum NodeType
{
    Ref
}

public class NodeFactory : INodeFactory
{
    public INode CreateNode(string nodeType, string[] arguments)
    {
        if (!Enum.TryParse(nodeType, true, out NodeType parsedNodeType))
        {
            throw new ArgumentException($"Invalid node type: {nodeType}");
        }

        switch (parsedNodeType)
        {
            case NodeType.Ref:
                return CreateReferenceNode(arguments.Single());
            default:
                throw new NotImplementedException();
        }
    }

    private static INode CreateReferenceNode(string name) => new ReferenceNode(name);
}