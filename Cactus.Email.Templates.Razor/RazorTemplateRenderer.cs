using System;
using Cactus.Email.Core.Utils;
using Cactus.Email.Templates.Razor.Logging;
using RazorEngine;
using RazorEngine.Templating;

namespace Cactus.Email.Templates.Razor
{
    public class RazorTemplateRenderer : ITemplateRenderer
    {
        private static readonly ILog Logger = LogProvider.GetLogger(typeof(RazorTemplateRenderer));

        public string Render<TModel>(string template, TModel model)
        {
            try
            {
                return Engine.Razor.RunCompile(template, template, typeof(TModel), model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to render of razor template");
                throw;
            }
        }

        public string Render(string template)
        {
            try
            {
                return Engine.Razor.RunCompile(template, template);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to render of razor template");
                throw;
            }
        }
    }
}
