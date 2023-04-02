using System.Text;
using Prompthing.Core.Abstract.Tree;

namespace Prompthing.Core.Templates.Nodes.Basic;

public abstract class BasicNode : INode<StringBuilder>
{
    public IContainer<StringBuilder> Parent { get; protected set; }

    public abstract void Evaluate(StringBuilder output);

    public virtual void SetParent(IContainer<StringBuilder> parent) => 
        Parent = parent;
}