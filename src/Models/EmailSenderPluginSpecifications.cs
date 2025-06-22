using FlowSynx.PluginCore;

namespace FlowSynx.Plugins.Email.Sender.Models;

public class EmailSenderPluginSpecifications: PluginSpecifications
{
    [RequiredMember]
    public string SmtpHost { get; set; } = string.Empty;

    public int SmtpPort { get; set; } = 587;

    public bool UseSsl { get; set; } = true;
    
    [RequiredMember]
    public string Username { get; set; } = string.Empty;
    
    [RequiredMember]
    public string Password { get; set; } = string.Empty;

    [RequiredMember]
    public string From { get; set; } = string.Empty;
}