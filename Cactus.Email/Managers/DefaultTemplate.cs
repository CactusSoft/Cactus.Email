using System.Text;

namespace Cactus.Email.Core.Managers
{
    public class DefaultTemplate<TKey> : ITemplate<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }

        public string SubjectTemplate { get; set; }

        public string HtmlBodyTemplate { get; set; }

        public string PlainBody { get; set; }

        public string Language { get; set; }

        public Encoding HtmlBodyEncoding { get; set; }

        public Encoding PlainBodyEncoding { get; set; }
    }
}
