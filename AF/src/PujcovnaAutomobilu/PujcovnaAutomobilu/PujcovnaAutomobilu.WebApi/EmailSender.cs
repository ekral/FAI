using System.Diagnostics;

namespace PujcovnaAutomobilu.WebApi
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail()
        {
            Debug.WriteLine("Opravdu Posilam email majiteli pujcovny.");
        }
    }
}
