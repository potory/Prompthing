using SonScript.Core;
using SonScript.Core.Attributes;
using SonScript.Core.Functions;

namespace Prompthing.Core.Functions;

[AllowFunctionCaching]
public class BackspaceFunction : Function
{
    private readonly FunctionContext _context;

    public BackspaceFunction(FunctionContext context) => 
        _context = context;

    public override object Evaluate(List<object> arguments)
    {
        var length = GetInt(arguments.Single());

        return string.Empty;
    }
}