using Prompthing.Core.Abstract;

namespace Prompthing.Core.Nodes;

public class TextNode : INode
{
    public string Text { get; }

    public TextNode(string text)
    {
        Text = text;
    }
}