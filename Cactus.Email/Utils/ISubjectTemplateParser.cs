namespace Cactus.Email.Core.Utils
{
    public interface ISubjectTemplateParser
    {
        string Parse(string template, string senderEmail, string recipientsEmails, string createdDateTime);
    }
}
