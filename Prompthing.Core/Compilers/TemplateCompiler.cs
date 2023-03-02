using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Prompthing.Core.Abstract;
using Prompthing.Core.Templates;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Compilers;

public partial class TemplateCompiler : ICompiler<JObject, TemplateNode>
{
    [GeneratedRegex("({{[^}]+}})")]
    private static partial Regex SplitRegex();

    private readonly Regex _splitRegex = SplitRegex();
    private readonly ReferencePool _pool;

    public TemplateCompiler(ReferencePool pool)
    {
        _pool = pool;
    }

    public TemplateNode Compile(JObject obj)
    {
        var nameToken = obj.GetValue("name", StringComparison.OrdinalIgnoreCase);
        var templateToken = obj.GetValue("template", StringComparison.OrdinalIgnoreCase);
        
        var name = GetTemplateName(nameToken);

        if (templateToken == null)
        {
            throw new Exception("...");
        }

        string template = templateToken.ToString();
        
        var container = new ContainerNode();
        var segments = ToSegments(template);

        throw new NotImplementedException();
    }

    private string[] ToSegments(string template) => 
        _splitRegex.Split(template).Where(segment => !string.IsNullOrEmpty(segment)).ToArray();

    private static string GetTemplateName(JToken? nameToken) => 
        nameToken == null ? Guid.NewGuid().ToString() : nameToken.ToString();
}