
using Xunit;
using Microsoft.Extensions.Options;
using SendNotificationAPI.Services;
using SendNotificationAPI.Models.Options;
using SendNotificationAPI.Models.RequestModel;

namespace Tests;

public class MailServiceTests
{
    [Fact]
    public async Task SendNotificationAsync_ShouldSendMail()
    {
        // Arrange
        var smtpOptions = Options.Create(new SMTPOptions
        {
            Host = "smtp.gmail.com",
            Port = 587,
            Username = "",
            Password = ""
        });

        var mailService = new MailService(smtpOptions);

        var request = new SendMailNotificationRequest
        {
            Subject = "xUnit Test Mail",
            Body = "<h2>Mail gönderimi başarılı!</h2>",
            Recipients = new string[]
            {
                ""
            }
        };

        // Act
        await mailService.SendNotificationAsync(request);

        // Assert
        Assert.True(true);
    }
}
