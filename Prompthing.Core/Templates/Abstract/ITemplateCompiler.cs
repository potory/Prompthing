namespace Prompthing.Core.Templates.Abstract;

public interface ITemplateCompiler
{
    ITemplate CompileFrom(string input);
}