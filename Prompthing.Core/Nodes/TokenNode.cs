using Prompthing.Core.Abstract;

namespace Prompthing.Core.Nodes;

public class TokenNode : INode
{
    public string Key { get; }

    public TokenNode(string key)
    {
        Key = key;
    }
}