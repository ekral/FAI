using System.Diagnostics;

namespace Utb.PizzaKosk.WebApi.Services
{
    public class MockEmailSender : IEmailSender
    {
        public void SendEmail()
        {
            Debug.WriteLine("Neposilam sms");
        }
    }
}