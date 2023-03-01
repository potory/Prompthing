namespace Prompthing.Core.Entities;

public class Category
{
    public string Name { get; }
    public IReadOnlyList<Term> Terms { get; }

    public Category(string name, Term[] terms)
    {
        Name = name;
        Terms = terms;
    }
}