using System.Text;
using Prompthing.Core.Functions;
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
    public override void Evaluate(StringBuilder output)
    {
        if (IsBackspaceFunction(_node))
        {
            EvaluateBackspace(output);
            return;
        }

        output.Append(_node.Evaluate());
    }

    private void EvaluateBackspace(StringBuilder output) => 
        output.Length -= (int)_node.Evaluate();

    private static bool IsBackspaceFunction(FunctionNode node) => 
        node is FunctionCallNode callNode && callNode.Function is BackspaceFunction;
}