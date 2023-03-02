using Prompthing.Core;
using Prompthing.Core.Templates;

string content = File.ReadAllText("Example.json");

var templates = new DatasetCompiler().Compile(content);

Console.ReadKey();