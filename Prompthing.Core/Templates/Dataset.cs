
using Prompthing.Core.Compilers;
using Prompthing.Core.Entities;
using Prompthing.Core.Utilities;

namespace Prompthing.Core.Templates;

public class Dataset
{
    public ReferencePool ReferencePool { get; }
    public Template[] Templates { get; }

    public Dataset(ReferencePool referencePool, Template[] templates)
    {
        ReferencePool = referencePool;
        Templates = templates;
    }

    public Template ResolveTemplate(string template)
    {
        var compiler = new TemplateCompiler(new TokenInterpreter(ReferencePool));
        return compiler.Compile(Guid.NewGuid().ToString(), template, false);
    }
}