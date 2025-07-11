# FlowSynx Email Sender Plugin

The Email Sender Plugin is a pre-packaged, plug-and-play integration component for the FlowSynx engine. It enables sending emails with support for multiple recipients, CC, BCC, HTML or plain text bodies, and attachments. Designed for FlowSynx’s no-code/low-code automation workflows, this plugin simplifies email integration for various workflow scenarios.

This plugin is automatically installed by the FlowSynx engine when selected within the platform. It is not intended for manual installation or standalone developer use outside the FlowSynx environment.

---

## Purpose

The Email Sender Plugin allows FlowSynx users to:

- Send emails from a configured SMTP server.
- Support multiple recipients (To, CC, BCC).
- Send HTML or plain text email bodies.
- Include multiple attachments in emails.
- Simplify email notifications within automated workflows.

---

## Plugin Specifications

The plugin requires the following configuration:

- `Host` (string): **Required.** The SMTP server host (e.g., `smtp.flowsynx.io`).
- `Port` (int): The SMTP server port. Default: `587`.
- `UseSsl` (bool): Whether to use SSL for the SMTP connection. Default: `true`.
- `Username` (string): **Required.** The username for SMTP authentication.
- `Password` (string): **Required.** The password for SMTP authentication.
- `From` (string): **Required.** The default sender email address.

### Example Configuration

```json
{
  "Host": "smtp.flowsynx.io",
  "Port": 587,
  "UseSsl": true,
  "Username": "noreply@flowsynx.io",
  "Password": "securepassword",
  "From": "noreply@flowsynx.io"
}
```

---

## Input Parameters

Each **send** operation accepts the following parameters:

| Parameter        | Type             | Required | Description                                           |
|-------------------|------------------|----------|-------------------------------------------------------|
| `From`           | string           | No       | The sender email address. Overrides default `From`.   |
| `To`             | List&lt;string&gt; | Yes      | A list of recipient email addresses.                 |
| `Cc`             | List&lt;string&gt; | No       | A list of CC (carbon copy) recipient email addresses.|
| `Bcc`            | List&lt;string&gt; | No       | A list of BCC (blind carbon copy) recipient addresses.|
| `Subject`        | string           | Yes      | The subject of the email.                            |
| `Body`           | string           | Yes      | The body content of the email.                       |
| `IsBodyHtml`     | bool             | No       | Whether the body is HTML. Default: `false`.          |
| `Attachments`    | List&lt;object&gt; | No       | A list of attachments. See **Attachment Structure**. |

### Attachment Structure

Each attachment object includes:

| Field        | Type   | Required | Description                                |
|--------------|--------|----------|--------------------------------------------|
| `FileName`   | string | Yes      | The name of the attachment file.           |
| `Content`    | byte[] | Yes      | The binary content of the attachment file. |
| `MimeType`   | string | No       | The MIME type of the attachment. Default: `application/octet-stream`.|

### Example input (Send)

```json
{
  "Operation": "send",
  "From": "noreply@flowsynx.io",
  "To": ["user1@flowsynx.io", "user2@flowsynx.io"],
  "Cc": ["manager@flowsynx.io"],
  "Bcc": ["auditor@flowsynx.io"],
  "Subject": "Monthly Report",
  "Body": "<h1>Report Ready</h1><p>Please find the report attached.</p>",
  "IsBodyHtml": true,
  "Attachments": [
    {
      "FileName": "report.pdf",
      "Content": "BASE64_ENCODED_BYTES_HERE",
      "MimeType": "application/pdf"
    }
  ]
}
```

---

## Debugging Tips

- Verify the `Host`, `Port`, `Username`, `Password`, and `From` values are correct.
- Ensure that your SMTP server allows connections from the FlowSynx environment and supports the configured authentication method.
- If sending fails, check if the SMTP server requires application-specific passwords or IP whitelisting.
- For attachments, make sure the `Content` field contains properly encoded binary data.

---

## Email Sending Considerations

- **HTML vs Plain Text:** Use `IsBodyHtml=true` to send HTML emails. Ensure the body content is properly formatted HTML.
- **Large Attachments:** SMTP servers often impose size limits; avoid attachments larger than the server's allowed limit.
- **Security:** Credentials (`Username` and `Password`) are securely stored by the FlowSynx engine and are not exposed during workflow execution.

---

## Security Notes

- The plugin uses standard SMTP authentication to connect to the configured mail server.
- No email content or credentials are persisted outside the execution scope unless explicitly configured.
- Only authorized FlowSynx platform users can view or modify plugin configurations.

---

## License

© FlowSynx. All rights reserved.
