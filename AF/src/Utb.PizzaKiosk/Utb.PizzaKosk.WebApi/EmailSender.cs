using System.Diagnostics;

public class EmailSender : IEmailSender
{
    public void SendEmail()
    {
        Debug.WriteLine("Opravdu posilam sms");
    }
}