using Prompthing.Core.Templates.Abstract;

namespace Prompthing.Core.Templates.Nodes;

public class TokenNode : INode
{
    public string Key { get; }

    public TokenNode(string key)
    {
        Key = key;
    }
}