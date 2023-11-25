// pomoci ioc kontejneru zaregistrujste EmailSenderMock jako Singleton

namespace Utb.PizzaKosk.WebApi.Services
{
    public interface IEmailSender
    {
        void SendEmail();
    }
}