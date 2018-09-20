using Cactus.Email.Core.Utils;

namespace Cactus.Email.Templates.Razor
{
    public interface ITemplateRazorRenderer : ITemplateRenderer
    {
        string Render<TModel>(string template, TModel model, string key);
        string Render(string template, string key);
    }
}
