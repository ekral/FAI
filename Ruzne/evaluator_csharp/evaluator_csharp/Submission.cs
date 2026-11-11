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

    class Application(INotificationSender sender)
    {
        private INotificationSender sender = sender;

        void Method(string recipient, string message)
        {
            sender.Send(recipient, message);
        }
    }
}