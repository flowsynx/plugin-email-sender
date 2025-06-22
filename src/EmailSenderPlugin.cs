using FlowSynx.PluginCore.Helpers;
using FlowSynx.PluginCore;
using FlowSynx.PluginCore.Extensions;
using FlowSynx.Plugins.Email.Sender.Models;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace FlowSynx.Plugins.Email.Sender;

public class EmailSenderPlugin : IPlugin
{
    private IPluginLogger? _logger;
    private EmailSenderPluginSpecifications _emailSenderSpecifications = null!;
    private bool _isInitialized;

    public PluginMetadata Metadata
    {
        get
        {
            return new PluginMetadata
            {
                Id = Guid.Parse("b68a5dad-9a0c-4dd3-8474-1d76407be54e"),
                Name = "Email.Sender",
                CompanyName = "FlowSynx",
                Description = Resources.PluginDescription,
                Version = new PluginVersion(1, 0, 0),
                Namespace = PluginNamespace.Connectors,
                Authors = new List<string> { "FlowSynx" },
                Copyright = "© FlowSynx. All rights reserved.",
                Icon = "flowsynx.png",
                ReadMe = "README.md",
                RepositoryUrl = "https://github.com/flowsynx/plugin-email-sender",
                ProjectUrl = "https://flowsynx.io",
                Tags = new List<string>() { "flowSynx", "email", "email-sender", "communication", "collaboration" },
                Category = PluginCategories.Communication
            };
        }
    }

    public PluginSpecifications? Specifications { get; set; }

    public Type SpecificationsType => typeof(EmailSenderPluginSpecifications);

    public IReadOnlyCollection<string> SupportedOperations => new List<string>();

    public Task Initialize(IPluginLogger logger)
    {
        if (ReflectionHelper.IsCalledViaReflection())
            throw new InvalidOperationException(Resources.ReflectionBasedAccessIsNotAllowed);

        ArgumentNullException.ThrowIfNull(logger);
        _emailSenderSpecifications = Specifications.ToObject<EmailSenderPluginSpecifications>();
        _logger = logger;
        _isInitialized = true;
        return Task.CompletedTask;
    }

    public async Task<object?> ExecuteAsync(PluginParameters parameters, CancellationToken cancellationToken)
    {
        if (ReflectionHelper.IsCalledViaReflection())
            throw new InvalidOperationException(Resources.ReflectionBasedAccessIsNotAllowed);

        if (!_isInitialized)
            throw new InvalidOperationException($"Plugin '{Metadata.Name}' v{Metadata.Version} is not initialized.");

        var emailMessageParameters = parameters.ToObject<EmailSenderMessageParameters>();
        var message = new MimeMessage();

        message.From.Add(MailboxAddress.Parse(emailMessageParameters.From));

        foreach (var to in emailMessageParameters.To)
            message.To.Add(MailboxAddress.Parse(to));

        foreach (var cc in emailMessageParameters.Cc)
            message.Cc.Add(MailboxAddress.Parse(cc));

        foreach (var bcc in emailMessageParameters.Bcc)
            message.Bcc.Add(MailboxAddress.Parse(bcc));

        message.Subject = emailMessageParameters.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = emailMessageParameters.IsBodyHtml ? emailMessageParameters.Body : null,
            TextBody = !emailMessageParameters.IsBodyHtml ? emailMessageParameters.Body : null
        };

        foreach (var attachment in emailMessageParameters.Attachments)
        {
            builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.MimeType));
        }

        message.Body = builder.ToMessageBody();

        var smtpHost = _emailSenderSpecifications.Host;
        var smtpPort = _emailSenderSpecifications.Port;
        var useSsl = _emailSenderSpecifications.UseSsl;
        var userName = _emailSenderSpecifications.Username;
        var password = _emailSenderSpecifications.Password;

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpHost, smtpPort, useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto, cancellationToken);
        await client.AuthenticateAsync(userName, password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);

        return Task.CompletedTask;
    }
}