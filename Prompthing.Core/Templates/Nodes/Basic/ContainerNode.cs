using System.Text;
using Prompthing.Core.Abstract.Tree;

namespace Prompthing.Core.Templates.Nodes.Basic;

public class ContainerNode : BasicNode, IContainer<StringBuilder>
{
    private readonly List<INode<StringBuilder>> _children = new();
    public int ChildCount => _children.Count;

    public override void Evaluate(StringBuilder output)
    {
        for (int i = 0; i < ChildCount; i++)
        {
            _children[i].Evaluate(output);
        }
    
        EvaluateSelf(output);
    }

    public void AddChild(INode<StringBuilder> child)
    {
        _children.Add(child);
        child.SetParent(this);
    }
    public void AddChildren(IReadOnlyList<INode<StringBuilder>> children)
    {
        _children.AddRange(children);

        foreach (var child in _children)
        {
            child.SetParent(this);
        }
    }

    public void RemoveChild(INode<StringBuilder> child)
    {
        _children.Remove(child);
        child.SetParent(null);
    }

    public void ClearChildren()
    {
        foreach (var child in _children)
        {
            child.SetParent(null);
        }
        
        _children.Clear();
    }

    public IEnumerable<INode<StringBuilder>> GetChildren() => _children;

    public bool HasChild(INode<StringBuilder> child) => _children.Contains(child);

    protected virtual void EvaluateSelf(StringBuilder output)
    {
        // Here is custom implementation of self-evaluation
    }
}