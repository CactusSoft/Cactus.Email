namespace Cactus.Email.Smtp.Configurations
{
    public class SmtpConfiguration : ISmtpConfiguration
    {
        public SmtpConfiguration(string smtpServer, int? smtpServerPort, string smtpAccount, string smtpAccountPassword, int? smtpServerTimeout = null, bool? enableSsl = null)
        {
            SmtpServer = smtpServer;
            SmtpServerPort = smtpServerPort;
            SmtpServerTimeout = smtpServerTimeout;
            SmtpAccount = smtpAccount;
            SmtpAccountPassword = smtpAccountPassword;
            EnableSsl = enableSsl;
        }

        public string SmtpServer { get; }
        public int? SmtpServerPort { get; }
        public int? SmtpServerTimeout { get; }

        public string SmtpAccount { get; }
        public string SmtpAccountPassword { get; }

        public bool? EnableSsl { get; }
    }
}
