using System.Text;
using Prompthing.Core.Templates;

string content = File.ReadAllText("Example.json");

var stringBuilder = new StringBuilder();
var templates = new DatasetCompiler().Compile(content);

for (int i = 0; i < 10; i++)
{
    templates.Templates[0].Node.Evaluate(stringBuilder);
    Console.WriteLine(stringBuilder.ToString());
    stringBuilder.Clear();
}

Console.ReadKey();