using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Entities;

public class Wrapper
{
    public string Name { get; }
    public BasicNode PreContent { get; }
    public BasicNode PostContent { get; }

    public Wrapper(string name, BasicNode preContent, BasicNode postContent)
    {
        Name = name;
        PreContent = preContent;
        PostContent = postContent;
    }
}