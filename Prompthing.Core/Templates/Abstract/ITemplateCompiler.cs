namespace Prompthing.Core.Templates.Abstract;

public interface ITemplateCompiler
{
    ITemplate CompileFrom(string input, string? name = null);
}