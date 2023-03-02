using System.Text;
using Prompthing.Core.Entities;
using Prompthing.Core.Extensions;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core.Templates.Nodes;

public sealed class CategoryNode : BasicNode
{
    private readonly DelayedReference<Category> _category;
    private string RandomTerm => _category.Value.Terms.OneOfWeighted().Text;

    public CategoryNode(DelayedReference<Category> category) => 
        _category = category;

    public override void Evaluate(StringBuilder output) => 
        output.Append(RandomTerm);
}