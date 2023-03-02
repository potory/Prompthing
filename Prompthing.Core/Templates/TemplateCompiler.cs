using System.Text.RegularExpressions;
using Prompthing.Core.Templates.Nodes;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates;

public partial class TemplateCompiler
{
    [GeneratedRegex("({{[^}]+}})")]
    private static partial Regex SplitRegex();

    private readonly Regex _splitRegex = SplitRegex();

    public Template Compile(string templateName, string templateString)
    {
        if (string.IsNullOrEmpty(templateName))
        {
            throw new ArgumentException("Template name cannot be null or empty.", nameof(templateName));
        }

        if (string.IsNullOrEmpty(templateString))
        {
            throw new ArgumentException("Template string cannot be null or empty.", nameof(templateString));
        }

        throw new NotImplementedException();
    }
}