namespace Submission
{
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
}