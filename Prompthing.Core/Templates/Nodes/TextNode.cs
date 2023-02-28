using Prompthing.Core.Templates.Abstract;

namespace Prompthing.Core.Templates.Nodes;

public class TextNode : INode
{
    public string Text { get; }

    public TextNode(string text)
    {
        Text = text;
    }
}