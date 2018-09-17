namespace Cactus.Email.Smtp.Configurations
{
    public interface ISmtpConfiguration
    {
        string SmtpServer { get; }

        int? SmtpServerPort { get; }

        int? SmtpServerTimeout { get; }

        string SmtpAccount { get; }

        string SmtpAccountPassword { get; }

        bool? EnableSsl { get; }
    }
}
