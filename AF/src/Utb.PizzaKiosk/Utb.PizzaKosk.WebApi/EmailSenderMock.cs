using System.Diagnostics;

public class EmailSenderMock : IEmailSender
{
    public void SendEmail()
    {
        Debug.WriteLine("Posilam sms");
    }
}