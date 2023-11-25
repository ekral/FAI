namespace Utb.PizzaKosk.WebApi.Services
{
    public class MyService
    {
        public IEmailSender Sender { get; }

        public MyService(IEmailSender sender)
        {
            Sender = sender;
        }



    }
}
