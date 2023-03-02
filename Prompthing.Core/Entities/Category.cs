namespace Prompthing.Core.Entities;

public class Category
{
    public string Name { get; }
    public IReadOnlyList<Term> Terms { get; }

    public Category(string name, IReadOnlyList<Term> terms)
    {
        Name = name;
        Terms = terms;
    }
}