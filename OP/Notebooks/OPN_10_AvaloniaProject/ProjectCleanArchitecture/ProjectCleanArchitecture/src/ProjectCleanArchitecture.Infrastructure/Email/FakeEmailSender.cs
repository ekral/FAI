using Microsoft.Extensions.Logging;
using ProjectCleanArchitecture.Core.Interfaces;

namespace ProjectCleanArchitecture.Infrastructure.Email;
public class FakeEmailSender(ILogger<FakeEmailSender> _logger) : IEmailSender
{
  public Task SendEmailAsync(string to, string from, string subject, string body)
  {
    _logger.LogInformation("Not actually sending an email to {to} from {from} with subject {subject}", to, from, subject);
    return Task.CompletedTask;
  }
}
