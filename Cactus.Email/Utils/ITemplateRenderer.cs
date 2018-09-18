namespace Cactus.Email.Core.Utils
{
    public interface ITemplateRenderer
    {
        string Render<TModel>(string template, TModel model);
        string Render(string template);
    }
}
