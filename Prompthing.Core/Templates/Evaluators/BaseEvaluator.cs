using Prompthing.Core.Templates.Abstract;

namespace Prompthing.Core.Templates.Evaluators;

public abstract class BaseEvaluator<TNode> : INodeEvaluator<TNode> where TNode: INode 
{
    public void Evaluate(object node)
    {
        if (node is not TNode instance)
        {
            throw new ArgumentException();
        }

        Evaluate(instance);
    }

    public abstract void Evaluate(TNode node);
}