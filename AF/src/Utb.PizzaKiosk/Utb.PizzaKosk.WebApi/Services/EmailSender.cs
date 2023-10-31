using System.Diagnostics;

namespace Utb.PizzaKosk.WebApi.Services
{
    public class EmailSender : IEmailSender
    {
        
        public void SendEmail()
        {
            Debug.WriteLine("Opravdu posilam email");
        }
    }
}