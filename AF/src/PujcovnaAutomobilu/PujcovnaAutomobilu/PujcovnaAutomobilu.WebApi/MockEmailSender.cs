using System.Diagnostics;

namespace PujcovnaAutomobilu.WebApi
{
    public class MockEmailSender : IEmailSender
    {
        public void SendEmail()
        {
            Debug.WriteLine("Neposilam email majiteli pujcovny, jen testuji.");
        }
    }
}
