// Klientsky kod
static class Test
{
    public static void SendAll(INotificationSender[] senders, string recipient, string message)
    {
        foreach(var sender in senders)
        {
            sender.Send(recipient, message);
        }
    }
}

class Solution 
{
    public static void Run(int n)
    {
        EmailSender emailSender = new EmailSender();
        SmsSender smsSender = new SmsSender();

        Test.SendAll([emailSender, smsSender], "user@example.com", "Hello, World!");
    }
}

// TestConsole.WriteLine($"Sent email to {recipient}: {message}");
// TestConsole.WriteLine($"Sent SMS to {recipient}: {message}");

interface INotificationSender
{
    void Send(string recipient, string message);
}

class EmailSender : INotificationSender
{
    public void Send(string recipient, string message)
    {
        TestConsole.WriteLine($"Sent email to {recipient}: {message}");
    }
}

class SmsSender : INotificationSender
{
    public void Send(string recipient, string message)
    {
        TestConsole.WriteLine($"Sent SMS to {recipient}: {message}");
    }
}
