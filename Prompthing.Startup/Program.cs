using System.Text;
using Prompthing.Core.Entities;
using Prompthing.Core.Templates;
using Prompthing.Core.Templates.Basic;

var group = new ContainerNode();
group.AddChild(new TextNode("a portrait of a "));
group.AddChild(new CategoryNode(new Category("gender", new[]
{
    new Term("woman", 0.5),
    new Term("men", 0.5),
})));
group.AddChild(new TextNode(" standing in a "));
group.AddChild(new CategoryNode(new Category("location", new[]
{
    new Term("park", 0.5),
    new Term("house", 0.5),
})));

for (int i = 0; i < 10; i++)
{
    var builder = new StringBuilder();
    group.Evaluate(builder);

    Console.WriteLine(builder.ToString());
}