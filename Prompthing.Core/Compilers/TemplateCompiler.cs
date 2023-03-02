using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Prompthing.Core.Abstract;
using Prompthing.Core.Entities;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Compilers;

/// <summary>
/// Compiles a template from a JObject by interpreting tokens within the template.
/// </summary>
public partial class TemplateCompiler : ICompiler<JObject, Template>
{
    /// <summary>
    /// Regular expression used to split the template into segments.
    /// </summary>
    [GeneratedRegex("({{[^}]+}})")]
    private static partial Regex SplitRegex();

    private readonly Regex _splitRegex = SplitRegex();
    private readonly TokenInterpreter _interpreter;

    /// <summary>
    /// Creates a new instance of the TemplateCompiler class.
    /// </summary>
    /// <param name="interpreter">The token interpreter used to interpret tokens within the template.</param>
    public TemplateCompiler(TokenInterpreter interpreter)
    {
        _interpreter = interpreter;
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

        if (templateToken == null)
        {
            throw new TemplateCompilationException("The 'Template' property is missing from the JObject.");
        }

        string template = templateToken.ToString();

        var container = new ContainerNode();
        var segments = ToSegments(template);

        foreach (var segment in segments)
        {
            container.AddChild(_interpreter.Interpret(segment));
        }

        return new Template(name, container);
    }

    /// <summary>
    /// Splits a template string into segments using the split regex.
    /// </summary>
    /// <param name="template">The template string to split.</param>
    /// <returns>An array of segments.</returns>
    private string[] ToSegments(string template) => 
        _splitRegex.Split(template).Where(segment => !string.IsNullOrEmpty(segment)).ToArray();

    /// <summary>
    /// Gets the name of the template from the name token.
    /// </summary>
    /// <param name="nameToken">The name token.</param>
    /// <returns>The name of the template.</returns>
    private static string GetTemplateName(JToken? nameToken) => 
        nameToken == null ? Guid.NewGuid().ToString() : nameToken.ToString();
}

public class TemplateCompilationException : Exception
{
    public TemplateCompilationException(string message) : base(message)
    {
        
    }
}