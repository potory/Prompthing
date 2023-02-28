namespace Prompthing.Core.Templates.Abstract;

public interface ITemplateRenderer
{
    string Render(ITemplate template);
}