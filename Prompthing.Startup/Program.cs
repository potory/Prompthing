using Prompthing.Core;
using Prompthing.Core.Factories;

var compile = new TemplateCompiler(new NodeFactory());
var template = compile.CompileFrom("{{if:category1 > 5}} and {{category2}}");

Console.ReadKey();