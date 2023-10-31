using System.Diagnostics;

public class MockEmailSender : IEmailSender
{
    public void SendEmail()
    {
        Debug.WriteLine("Neposilam sms");
    }
}
