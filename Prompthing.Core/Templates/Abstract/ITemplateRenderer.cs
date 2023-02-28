namespace Prompthing.Core.Templates.Abstract;

public interface ITemplateRenderer
{
    ITemplateRenderer Render(ITemplate template);
    string Finish();
}