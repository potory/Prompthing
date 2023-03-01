using System.Text;
using Prompthing.Core.Entities;
using Prompthing.Core.Extensions;
using Prompthing.Core.Templates.Basic;

namespace Prompthing.Core.Templates;

public sealed class CategoryNode : BasicNode
{
    private readonly Category _category;
    private string RandomTerm => _category.Terms.OneOfWeighted().Text;

    public CategoryNode(Category category) => 
        _category = category;

    public override void Evaluate(StringBuilder output) => 
        output.Append(RandomTerm);
}