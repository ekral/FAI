using System.Diagnostics;

namespace Utb.PizzaKosk.WebApi.Services
{
    public class MockEmailSender : IEmailSender
    {
        private static int pocet = 0;

        public MockEmailSender()
        {
            ++pocet;
            Debug.WriteLine($"Pocet EmailSenderu> {pocet}");
        }

        public void SendEmail()
        {
            Debug.WriteLine("Neposilam email, jsem jen pro testovani a vyvoj.");
        }
    }
}