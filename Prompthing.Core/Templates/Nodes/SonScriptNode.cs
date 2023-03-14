using System.Text;
using Prompthing.Core.Templates.Nodes.Basic;
using SonScript.Core.Nodes;

namespace Prompthing.Core.Templates.Nodes;

public class SonScriptNode : BasicNode
{
    private readonly FunctionNode _node;

    public SonScriptNode(FunctionNode node)
    {
        _node = node;
    }
    public override void Evaluate(StringBuilder output) => 
        output.Append(_node.Evaluate());
}