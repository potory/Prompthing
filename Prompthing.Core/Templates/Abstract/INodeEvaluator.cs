namespace Prompthing.Core.Templates.Abstract;

public interface INodeEvaluator
{
    public void Evaluate(object node);
}

public interface INodeEvaluator<in TNode> : INodeEvaluator where TNode: INode
{
    public void Evaluate(TNode node);
}