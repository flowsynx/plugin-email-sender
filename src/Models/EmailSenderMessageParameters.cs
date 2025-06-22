namespace FlowSynx.Plugins.Email.Sender.Models;

internal class EmailSenderMessageParameters
{
    public string From { get; set; } = string.Empty;
    public List<string> To { get; set; } = new();
    public List<string> Cc { get; set; } = new();
    public List<string> Bcc { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsBodyHtml { get; set; } = false;
    public List<EmailSenderAttachment> Attachments { get; set; } = new();
}

internal class EmailSenderAttachment
{
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string MimeType { get; set; } = "application/octet-stream";
}