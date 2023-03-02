using Prompthing.Core.Entities;

var categories = new[]
{
    new Category("gender", new[]
    {
        new Term("woman", 0.5),
        new Term("men", 0.5),
    }),
    new Category("adjective", new[]
    {
        new Term("beautiful", 2),
        new Term("scary", 0.5),
    }),
    new Category("location", new[]
    {
        new Term("park", 0.5),
        new Term("house", 0.5),
    })
};