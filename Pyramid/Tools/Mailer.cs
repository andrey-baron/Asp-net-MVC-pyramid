using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Pyramid.Tools
{
    public class Mailer
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public class MailerMessage
        {
            public string SenderName { get; set; }
            public List<string> To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }

            public MailerMessage(string senderName, List<string> to, string subject, string body)
            {
                SenderName = senderName;
                To = to;
                Subject = subject;
                Body = body;
            }

            public MailerMessage()
            {

            }
        }

        public static void Send(MailerMessage message)
        {
            new System.Threading.Thread((obj) =>
            {
                var info = obj as MailerMessage;
                var client = new SmtpClient();
                var fromAddress = client.Credentials.GetCredential("", 0, "").UserName;
                var mailMessage = new MailMessage(fromAddress, string.Join(",", info.To), info.Subject, info.Body);
                mailMessage.IsBodyHtml = true;
                mailMessage.Sender = mailMessage.From = new MailAddress(fromAddress, info.SenderName);
                try
                {
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    //Logger.Log(LogLevel.Error, ex, $"Ошибка при отправке письма: {ex}");
                }
            }).Start(message);
        }
    }
}