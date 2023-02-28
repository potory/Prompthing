using Prompthing.Core.Dataset;
using Prompthing.Core.Templates;
using Prompthing.Core.Templates.Factories;

var terms = new Term[]
{
    new()
    {
        Key = "term1",
        Values = new[]
        {
            new TermValue
            {
                Text = "term1 value1",
                Weight = 0.5
            },
            new TermValue
            {
                Text = "term1 value2",
                Weight = 0.5
            }
        }
    },
    new()
    {
        Key = "term2",
        Values = new[]
        {
            new TermValue
            {
                Text = "term2 value1",
                Weight = 0.5
            },
            new TermValue
            {
                Text = "term2 value2",
                Weight = 0.5
            }
        }
    }
};

var compile = new TemplateCompiler(new NodeFactory());

var templates = new[]
{
    compile.CompileFrom("{{term1}} and {{term1}}", "template1"),
    compile.CompileFrom("{{ref:template1}} and {{term2}}")
};

var renderer = new TemplateRenderer(templates, terms);

var str1 = renderer.Render(templates[0]).Finish();
var str2 = renderer.Render(templates[1]).Finish();

Console.ReadKey();