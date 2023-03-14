using System.Text;
using Prompthing.Core.Templates;
using SonScript.Core.Extensions;
using SonScript.Core.Functions;

namespace Prompthing.Core.Functions;

public class PrompthingFunction : Function
{
    private readonly Dictionary<string, Dataset> _cache = new();
    private readonly StringBuilder _sb = new();

    private readonly DatasetCompiler _compiler;

    public PrompthingFunction(DatasetCompiler compiler)
    {
        _compiler = compiler;
    }
    public override object Evaluate(List<object> arguments)
    {
        string path = (string)arguments.Single();
        
        if (!_cache.TryGetValue(path, out var dataset))
        {
            dataset = ReadDataset(path);
            _cache.Add(path, dataset);
        }

        var template = dataset.Templates.OneOf();
        template.Node.Evaluate(_sb);

        return _sb.ToString();
    }

    private Dataset ReadDataset(string path) => 
        _compiler.Compile(File.ReadAllText(path));
}