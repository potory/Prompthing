using System.Text.RegularExpressions;
using Prompthing.Core.Abstract;
using Prompthing.Core.Nodes;

namespace Prompthing.Core;

public partial class TemplateCompiler
{
    [GeneratedRegex("({{[^}]+}})")]
    private static partial Regex SplitRegex();

    [GeneratedRegex("({{(?<function>\\w+):?((?<argument>[^}]+))?}})")]
    private static partial Regex GetFullRegex();
    
    private readonly Regex _splitRegex = SplitRegex();
    private readonly Regex _fullRegex = GetFullRegex();
    private readonly INodeFactory _nodeFactory;

    public TemplateCompiler(INodeFactory nodeFactory)
    {
        _nodeFactory = nodeFactory;
    }

    public ITemplate CompileFrom(string input)
    {
        var inputTokens = InputTokens(input);
        var nodes = new List<INode>(inputTokens.Length);

        foreach (var token in inputTokens)
        {
            nodes.Add(
                _fullRegex.IsMatch(token) 
                    ? CreateNode(token) 
                    : new TextNode(token));
        }

        return new Template(nodes);
    }

    private INode CreateNode(string token)
    {
        var match = _fullRegex.Match(token);

        bool isFunction = IsFunction(match);
        string name = NameGroup(match);

        return !isFunction 
            ? new TokenNode(name) 
            : _nodeFactory.CreateNode(name, Arguments(match));
    }

    private static string[] Arguments(Match match) => 
        match.Groups[2].Value.Split(';');

    private static string NameGroup(Match match) => 
        match.Groups[3].Value;

    private static bool IsFunction(Match match) => 
        !string.IsNullOrWhiteSpace(match.Groups[2].Value);

    private string[] InputTokens(string input) => 
        _splitRegex.Split(input).Where(x => !string.IsNullOrEmpty(x)).ToArray();
}