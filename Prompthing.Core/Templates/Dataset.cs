
using Prompthing.Core.Compilers;
using Prompthing.Core.Entities;
using Prompthing.Core.Utilities;
using SonScript.Core;

namespace Prompthing.Core.Templates;

public class Dataset
{
    private readonly FunctionFactory _factory;
    public ReferencePool ReferencePool { get; }
    public Template[] Templates { get; }

    public Dataset(FunctionFactory factory, ReferencePool referencePool, Template[] templates)
    {
        _factory = factory;

        ReferencePool = referencePool;
        Templates = templates;
    }

    public Template ResolveTemplate(string template)
    {
        var compiler = new TemplateCompiler(new TokenInterpreter(ReferencePool, _factory));
        return compiler.Compile(Guid.NewGuid().ToString(), template, false);
    }
}