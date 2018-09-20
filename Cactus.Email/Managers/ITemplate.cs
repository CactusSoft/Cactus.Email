using System.Text;

namespace Cactus.Email.Core.Managers
{
    public interface ITemplate<TKey>
    {
        TKey Id { get; set; }

        string Name { get; set; }

        string SubjectTemplate { get; set; }

        string HtmlBodyTemplate { get; set; }

        string PlainBody { get; set; }

        string Language { get; set; }

        Encoding HtmlBodyEncoding { get; set; }

        Encoding PlainBodyEncoding { get; set; }
    }
}
