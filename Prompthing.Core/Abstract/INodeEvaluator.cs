namespace Prompthing.Core.Abstract;

public interface INodeEvaluator<in TNode> where TNode: INode
{
    public void Evaluate(TNode node);
}