using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Prompthing.Core.Abstract;
using Prompthing.Core.Entities;
using Prompthing.Core.Templates.Nodes.Basic;
using Prompthing.Core.Utilities;

namespace Prompthing.Core.Compilers;

public partial class WrapperCompiler : ICompiler<JObject, Wrapper>
{
    /// <summary>
    /// Regular expression used to split the template into segments.
    /// </summary>
    [GeneratedRegex("({{[^}]+}})")]
    private static partial Regex SplitRegex();

    private readonly Regex _splitRegex = SplitRegex();
    private readonly TokenInterpreter _interpreter;

    public WrapperCompiler(TokenInterpreter interpreter)
    {
        _interpreter = interpreter;
    }

    public Wrapper Compile(JObject obj)
    {
        var nameToken = obj.GetValue("Name", StringComparison.OrdinalIgnoreCase);
        var contentToken = obj.GetValue("Content", StringComparison.OrdinalIgnoreCase);
        var wrapperToken = obj.GetValue("Wrapper", StringComparison.OrdinalIgnoreCase);

        if (nameToken == null)
        {
            throw new CategoryCompilationException("Could not find 'Name' property in wrapper object.");
        }

        if (wrapperToken == null)
        {
            throw new CategoryCompilationException("Could not find 'Wrapper' property in wrapper object.");
        }

        if (contentToken == null)
        {
            throw new CategoryCompilationException("Could not find 'Content' property in wrapper object.");
        }

        var segments = ToSegments(wrapperToken.ToString());

        var name = nameToken.ToString();
        var content = contentToken.ToString();
        
        var preContent = new ContainerNode();

        int index = 0;

        while (true)
        {
            if (segments[index].Length > 4 && TokenInterpreter.ExtractTokenValue(segments[index]) == content)
            {
                break;
            }
    
            if (index >= segments.Length)
            {
                throw new ArgumentException();
            }
            
            preContent.AddChild(_interpreter.Interpret(segments[index]));
            index++;
        }

        index++;

        var postContent = new ContainerNode();

        while (index < segments.Length)
        {
            postContent.AddChild(_interpreter.Interpret(segments[index]));
            index++;
        }
        
        return new Wrapper(name, preContent, postContent);
    }
    
    /// <summary>
    /// Splits a template string into segments using the split regex.
    /// </summary>
    /// <param name="template">The template string to split.</param>
    /// <returns>An array of segments.</returns>
    private string[] ToSegments(string template) => 
        _splitRegex.Split(template).Where(segment => !string.IsNullOrEmpty(segment)).ToArray();
}