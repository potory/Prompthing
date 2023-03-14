using SonScript.Core.Attributes;
using SonScript.Core.Functions;

namespace Prompthing.Core.Functions;

[AllowFunctionCaching]
public class BackspaceFunction : Function
{
    public override object Evaluate(List<object> arguments) => 
        GetInt(arguments[0]);
}