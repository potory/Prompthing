using Newtonsoft.Json.Linq;
using Prompthing.Core.Abstract;
using Prompthing.Core.Entities;
using Prompthing.Core.Templates.Nodes.Basic;
using Prompthing.Core.Utilities;
using SonScript.Core;

namespace Prompthing.Core.Compilers;

/// <summary>
/// Compiles a template from a JObject by interpreting tokens within the template.
/// </summary>
public class TemplateCompiler : ICompiler<JObject, Template>
{
    private readonly TokenInterpreter _interpreter;
    private readonly SegmentHandler _segmentHandler;

    /// <summary>
    /// Creates a new instance of the TemplateCompiler class.
    /// </summary>
    /// <param name="interpreter">The token interpreter used to interpret tokens within the template.</param>
    public TemplateCompiler(TokenInterpreter interpreter)
    {
        _interpreter = interpreter;
        _segmentHandler = new SegmentHandler();
    }

    /// <summary>
    /// Compiles a template from a JObject.
    /// </summary>
    /// <param name="obj">The JObject to compile.</param>
    /// <returns>The compiled template.</returns>
    public Template Compile(JObject obj)
    {
        var name = GetTemplateName(obj.GetValue("Name", StringComparison.OrdinalIgnoreCase));
        var templateToken = obj.GetValue("Template", StringComparison.OrdinalIgnoreCase);
        var isSnippetToken = obj.GetValue("IsSnippet", StringComparison.OrdinalIgnoreCase);

        if (templateToken == null)
        {
            throw new TemplateCompilationException("The 'Template' property is missing from the JObject.");
        }

        bool isSnippet = false;

        if (isSnippetToken != null)
        {
            isSnippet = ((JValue)isSnippetToken).Value<bool>();
        }

        string template = templateToken.ToString();

        return Compile(name, template, isSnippet);
    }

    public Template Compile(string name, string template, bool isSnippet)
    {
        var container = new ContainerNode();
        var segments = _segmentHandler.GetSegments(template);

        foreach (var segment in segments)
        {
            container.AddChild(_interpreter.Interpret(segment));
        }

        return new Template(name, container, isSnippet);
    }

    /// <summary>
    /// Gets the name of the template from the name token.
    /// </summary>
    /// <param name="nameToken">The name token.</param>
    /// <returns>The name of the template.</returns>
    private static string GetTemplateName(JToken nameToken) => 
        nameToken == null ? Guid.NewGuid().ToString() : nameToken.ToString();
}

public class TemplateCompilationException : Exception
{
    public TemplateCompilationException(string message) : base(message)
    {
        
    }
}